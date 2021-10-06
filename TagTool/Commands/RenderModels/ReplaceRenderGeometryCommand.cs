using Assimp;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Commands.Common;
using TagTool.Geometry;
using TagTool.Tags.Definitions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace TagTool.Commands.RenderModels
{
	class ReplaceRenderGeometryCommand : Command
	{
		private GameCache Cache { get; }
		private CachedTag Tag { get; }
		private RenderModel Definition { get; }

		public ReplaceRenderGeometryCommand(GameCache cache, CachedTag tag, RenderModel definition) :
			base(false,

				name:
				"ReplaceRenderGeometry",
				
				description:
				"Replaces the render_geometry of the current render_model tag.",

				usage:
				"ReplaceRenderGeometry <COLLADA Scene> [IndexBufferFormat]",

				examples:
				"ReplaceRenderGeometry d:\\model.dae trianglestrip",

				helpMessage:
				"- Replaces the render_geometry of the current render_model tag with geometry compiled from a COLLADA scene file (.DAE).\n" +
                "- Your DAE file must contain a single mesh for every permutation.\n" +
                "- Name your meshes as {region}:{permutation} (e.g. hull:base).\n" +
				"- IndexBufferFormat is TriangleList unless TriangleStrip specified.")
		{
			Cache = cache;
			Tag = tag;
			Definition = definition;
		}

		public override object Execute(List<string> args)
		{
			Console.WriteLine();
			
			if (args.Count < 1 || args.Count > 2)
				return new TagToolError(CommandError.ArgCount);

			if (!Cache.TagCache.TryGetTag<Shader>(@"shaders\invalid", out var defaultShaderTag))
            {
                new TagToolWarning("shaders\\invalid.shader' not found!\n"
                    + "You will have to assign material shaders manually.");
            }

			var stringIdCount = Cache.StringTable.Count;

			var sceneFile = new FileInfo(args[0]);

			var indexBufferFormat = args.Count > 1 && args[1].ToLower() == "trianglestrip" ? IndexBufferFormat.TriangleStrip : IndexBufferFormat.TriangleList;
			var showTriangleStripWarning = false;
			
			if (!sceneFile.Exists)
				return new TagToolError(CommandError.FileNotFound);

			if (sceneFile.Extension.ToLower() != ".dae")
				return new TagToolError(CommandError.FileType, $"\"{sceneFile.FullName}\"");

			Scene scene;

			using (var importer = new AssimpContext())
			{
                //importer.SetConfig(new Assimp.Configs.IntegerPropertyConfig("AI_CONFIG_PP_SLM_VERTEX_LIMIT", 65536));
                scene = importer.ImportFile(sceneFile.FullName,
					PostProcessSteps.CalculateTangentSpace |
					PostProcessSteps.GenerateNormals |
					PostProcessSteps.SortByPrimitiveType |
					PostProcessSteps.Triangulate |
                    PostProcessSteps.JoinIdenticalVertices);
			}

			var builder = new RenderModelBuilder(Cache);
			var nodes = new Dictionary<string, sbyte>();
			var materialIndices = new Dictionary<string, short>();
			bool usePerMeshNodeMapping = true;

			foreach (var oldNode in Definition.Nodes)
			{
				var name = Cache.StringTable.GetString(oldNode.Name);
				nodes.Add(name,builder.AddNode(oldNode));
			}

			foreach (var region in Definition.Regions)
			{
				builder.BeginRegion(region.Name);

				string[] altconventions = { ":", "-" };

				var regionName = Cache.StringTable.GetString(region.Name);

				foreach (var permutation in region.Permutations)
				{
					if (permutation.MeshCount > 1)
						throw new NotSupportedException("multiple permutation meshes");

					if (permutation.MeshIndex == -1)
						continue;

					var permName = Cache.StringTable.GetString(permutation.Name);

					var permMeshes = scene.Meshes.Where(i => i.Name == $"{regionName}FBXASC058{permName}Mesh").ToList();
					foreach (string entry in altconventions)
					{
						//var tempMeshesA = scene.Meshes.Where(i => i.Name == $"{regionName}{entry}{permName}Mesh").ToList();
						var tempMeshes = scene.Meshes.Where(i => i.Name.Contains($"{regionName}{entry}{permName}")).ToList();
						//permMeshes.AddRange(tempMeshesA);
						permMeshes.AddRange(tempMeshes);
					}

					if (permMeshes.Count == 0)
					{
						//quick fix to allow simple import of single mesh objects
						if (scene.Meshes.Count == 1 && Definition.Regions.Count == 1 && Definition.Regions[0].Permutations.Count == 1)
						{
							Console.WriteLine($"Importing single mesh for {regionName}|{permName}...\n");
							permMeshes.Add(scene.Meshes[0]);
						}
						else
						{
							return new TagToolError(CommandError.CustomError, $"No mesh(es) found for region '{regionName}' permutation '{permName}'!");
						}
					}

					permMeshes.Sort((a, b) => a.MaterialIndex.CompareTo(b.MaterialIndex));

					// Build a multipart mesh from the model data,
					// with each model mesh mapping to a part of one large mesh and having its own material
					ushort partStartVertex = 0;
					ushort partStartIndex = 0;

					var rigidVertices = new List<RigidVertex>();
					var skinnedVertices = new List<SkinnedVertex>();

					var indices = new List<ushort>();
					var meshNodeIndices = new List<byte>();

					var vertexType = Definition.Geometry.Meshes[permutation.MeshIndex].Type;
					var rigidNode = Definition.Geometry.Meshes[permutation.MeshIndex].RigidNodeIndex;

					builder.BeginPermutation(permutation.Name);
					builder.BeginMesh();

					var MeshIndexCountOG = 0;
					var MeshIndexCountNew = 0;
					Console.Write($"   [{regionName}:{permName}]({permMeshes.Count()}) ");
					foreach (var part in permMeshes)
					{
						for (var i = 0; i < part.VertexCount; i++)
						{
							var position = part.Vertices[i];
							var normal = part.Normals[i];

							Vector3D uv;

							try
							{
								uv = part.TextureCoordinateChannels[0][i];
							}
							catch
							{
								new TagToolWarning($"Missing texture coordinate for vertex {i} in '{regionName}:{permName}'");
								uv = new Vector3D();
							}

							var tangent = part.Tangents.Count != 0 ? part.Tangents[i] : new Vector3D();
							var bitangent = part.BiTangents.Count != 0 ? part.BiTangents[i] : new Vector3D();

							if (usePerMeshNodeMapping && part.Bones.Count > 0)
							{
								// generate the list of node indices used by this mesh
								foreach (var bone in part.Bones)
								{
									string bonefix = FixBoneName(bone.Name);

									if (!nodes.ContainsKey(bonefix))
									{
										new TagToolWarning($"There is no node {bonefix} to match bone {bone.Name}");
									}
									else
									{
										byte nodeIndex = (byte)nodes[bonefix];
										int meshNodeIndex = meshNodeIndices.IndexOf(nodeIndex);
										if (meshNodeIndex == -1)
										{
											meshNodeIndex = meshNodeIndices.Count;
											meshNodeIndices.Add(nodeIndex);
										}
									}
								}
							}

							if (vertexType == VertexType.Skinned)
							{
								var blendIndicesList = new List<byte>();
								var blendWeightsList = new List<float>();

								foreach (var bone in part.Bones)
								{
									foreach (var vertexInfo in bone.VertexWeights)
									{
										if (vertexInfo.VertexID == i)
										{
											string bonefix = FixBoneName(bone.Name);

											if (!nodes.ContainsKey(bonefix))
											{
												new TagToolError(CommandError.CustomError, $"There is no node {bonefix} to match bone {bone.Name}");
												return false;
											}

											byte nodeIndex = (byte)nodes[bonefix];

											if (usePerMeshNodeMapping && part.Bones.Count > 0)
												blendIndicesList.Add((byte)meshNodeIndices.IndexOf(nodeIndex));
											else
												blendIndicesList.Add((byte)nodeIndex);

											blendWeightsList.Add(vertexInfo.Weight);
										}
									}
								}

								var blendIndices = new byte[4];
								var blendWeights = new float[4];

								for (int j = 0; j < blendIndicesList.Count; j++)
								{
									if (j < 4)
										blendIndices[j] = blendIndicesList[j];
								}

								for (int j = 0; j < blendWeightsList.Count; j++)
								{
									if (j < 4)
										blendWeights[j] = blendWeightsList[j];
								}

								skinnedVertices.Add(new SkinnedVertex
								{
									Position = new RealQuaternion(position.X * 0.01f, position.Y * 0.01f, position.Z * 0.01f, 1),
									Texcoord = new RealVector2d(uv.X, 1.0f - uv.Y),
									Normal = new RealVector3d(normal.X, normal.Y, normal.Z),
									Tangent = new RealQuaternion(tangent.X, tangent.Y, tangent.Z, 1),
									Binormal = new RealVector3d(bitangent.X, bitangent.Y, bitangent.Z),
									BlendIndices = blendIndices,
									BlendWeights = blendWeights
								});
							}
							else
							{
								rigidVertices.Add(new RigidVertex
								{
									Position = new RealQuaternion(position.X * 0.01f, position.Y * 0.01f, position.Z * 0.01f, 1),
									Texcoord = new RealVector2d(uv.X, 1.0f - uv.Y),
									Normal = new RealVector3d(normal.X, normal.Y, normal.Z),
									Tangent = new RealQuaternion(tangent.X, tangent.Y, tangent.Z, 1),
									Binormal = new RealVector3d(bitangent.X, bitangent.Y, bitangent.Z),
								});
							}
						}

						// Build the index buffer
						ushort[] meshIndices = part.GetIndices().Select(i => (ushort)(i + partStartVertex)).ToArray();
						MeshIndexCountOG += meshIndices.Count();
						if (indexBufferFormat == IndexBufferFormat.TriangleList)
						{
							Console.ForegroundColor = ConsoleColor.DarkGray;
							Console.Write($"{meshIndices.Count()} ");
							Console.ResetColor();
						}
						else
						{
							// Optimize and stripify indices 
							var indicesUint = meshIndices.Select(i => (uint)i).ToArray();
							var indicesOptimized = new uint[indicesUint.Length];
							MeshOptimizer.OptimizeVertexCacheStrip(indicesOptimized, indicesUint, indicesUint.Count(), 65536);
							var indicesStripped = new uint[MeshOptimizer.StripifyBound(indicesOptimized.Length)];
							uint indicesStrippedCount = MeshOptimizer.Stripify(indicesStripped, indicesOptimized, indicesOptimized.Length, 65536, 0);
							meshIndices = indicesStripped.Take((int)indicesStrippedCount).Select(i => (ushort)i).ToArray();
							
							bool badResult = indicesStrippedCount > indicesUint.Count();

							Console.ForegroundColor = badResult ? ConsoleColor.DarkYellow : ConsoleColor.DarkGray;
							Console.Write($"{indicesStrippedCount} ");
							Console.ResetColor();

							if (badResult) showTriangleStripWarning = true;
						}
						indices.AddRange(meshIndices);
						MeshIndexCountNew += meshIndices.Count();

						// Define a material and part for this mesh
						var meshMaterial = scene.Materials[part.MaterialIndex];

						short materialIndex = 0;

						if (materialIndices.ContainsKey(meshMaterial.Name))
							materialIndex = materialIndices[meshMaterial.Name];
						else
						{
							materialIndices.Add(meshMaterial.Name, builder.AddMaterial(new RenderMaterial
							{
								RenderMethod = defaultShaderTag,
							}));
							materialIndex = materialIndices[meshMaterial.Name];
						}

						builder.BeginPart(materialIndex, partStartIndex, (ushort)meshIndices.Length, (ushort)part.VertexCount);
						builder.DefineSubPart(partStartIndex, (ushort)meshIndices.Length, (ushort)part.VertexCount);
						builder.EndPart();

						// Move to the next part
						partStartVertex += (ushort)part.VertexCount;
						partStartIndex += (ushort)meshIndices.Length;
					}

					// Bind the vertex and index buffers
					if (vertexType == VertexType.Skinned)
						builder.BindSkinnedVertexBuffer(skinnedVertices);
					else
						builder.BindRigidVertexBuffer(rigidVertices, rigidNode);


					if (MeshIndexCountNew > ushort.MaxValue) Console.ForegroundColor = ConsoleColor.Red;
					else if (MeshIndexCountNew > MeshIndexCountOG) Console.ForegroundColor = ConsoleColor.DarkYellow;
					Console.WriteLine(indexBufferFormat == IndexBufferFormat.TriangleList ?
						MeshIndexCountOG.ToString() : $"{MeshIndexCountOG} > {MeshIndexCountNew}");
					Console.ResetColor();

					builder.BindIndexBuffer(indices, indexBufferFormat);

					if (usePerMeshNodeMapping && meshNodeIndices.Count > 0)
						builder.MapNodes(meshNodeIndices.ToArray());

					builder.EndMesh();
					builder.EndPermutation();
				}
				builder.EndRegion();
			}

            //check vertex and index buffer counts for each mesh to ensure that they fit within the limits for the resource
            foreach (var mesh in builder.Meshes)
            {
				switch (mesh.VertexFormat)
                {
                    case VertexBufferFormat.Skinned:
                        if (mesh.SkinnedVertices.Length > ushort.MaxValue)
                        {
                            return new TagToolError(CommandError.OperationFailed, $"Number of vertices ({mesh.SkinnedVertices.Length}) exceeded the limit! (65535)");
                        }
                        break;
                    case VertexBufferFormat.Rigid:
                        if (mesh.RigidVertices.Length > ushort.MaxValue)
                        {
                            return new TagToolError(CommandError.OperationFailed, $"Number of vertices ({mesh.RigidVertices.Length}) exceeded the limit! (65535)");
                        }
                        break;
                }
                if (mesh.Indices.Length > ushort.MaxValue)
                {
                    return new TagToolError(CommandError.OperationFailed, $"Number of vertex indices ({mesh.Indices.Length}) exceeded the limit! (65535)");
                }
            }

            Console.Write("\n   Building render_geometry...");

            var newDefinition = builder.Build(Cache.Serializer);
            Definition.Regions = newDefinition.Regions;
            Definition.Geometry = newDefinition.Geometry;
            Definition.Nodes = newDefinition.Nodes;
            Definition.Materials = newDefinition.Materials;

            Console.WriteLine("done.");

            //
            // TODO: Build the new render_model and update the original render_model here...
            //

            Console.Write("   Writing render_model tag data...");

			using (var cacheStream = Cache.OpenCacheReadWrite())
				Cache.Serialize(cacheStream, Tag, Definition);

			Console.WriteLine("done.");

			if (stringIdCount != Cache.StringTable.Count)
			{
				Console.Write("Saving string ids...");

			    Cache.SaveStrings();

				Console.WriteLine("done");
			}

			Console.WriteLine("   Replaced render_geometry successfully.\n");

			if (showTriangleStripWarning)
				return new TagToolWarning($"One or more meshes using TriangleStrips produced more indices than TriangleList would have.");

			return true;
		}

        private static string FixBoneName(string name)
        {
			// HAX BELOW
			//if (bone.Name.StartsWith("_"))
			//bone.Name = bone.Name.Substring(4);
			//if (bone.Name.EndsWith("2"))
			//bone.Name = bone.Name.Replace("2", "_tip");
			//else if (bone.Name != "spine1" && bone.Name.EndsWith("1"))
			//bone.Name = bone.Name.Replace("1", "_low");

			if (Regex.IsMatch(name, @"Armature_\d\d\d_.*"))
            {
                return Regex.Match(name, @"Armature_\d\d\d_(.*)").Groups[1].Value;
            }
            else if (Regex.IsMatch(name, @"Armature_.*"))
            {
                return Regex.Match(name, @"Armature_(.*)").Groups[1].Value;
            }

			return name;
        }
    }
}
