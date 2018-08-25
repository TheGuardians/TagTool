using TagTool.Cache;
using TagTool.Common;
using TagTool.Geometry;
using TagTool.IO;
using TagTool.Serialization;
using TagTool.Tags.Definitions;
using TagTool.Tags.Resources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TagTool.Tags;

namespace TagTool.Commands.Porting
{
    public class PortArmorVariantCommand : Command
    {
        private HaloOnlineCacheContext CacheContext { get; }
        private CacheFile BlamCache;
        private RenderGeometryConverter GeometryConverter { get; }

        public PortArmorVariantCommand(HaloOnlineCacheContext cacheContext, CacheFile blamCache) :
            base(true,

                "PortArmorVariant",
                "Ports an mp_masterchief armor variant.",

                "PortArmorVariant <Spartan | Elite> <Variant Name> [Scenery] [Replace: <Tag>] [Regions: <Region 1> <Region 2> ... <Region N>]",

                "Ports an mp_masterchief armor variant.")
        {
            CacheContext = cacheContext;
            BlamCache = blamCache;
            GeometryConverter = new RenderGeometryConverter(cacheContext, blamCache);
        }

        public override object Execute(List<string> args)
        {
            if (args.Count < 2)
                return false;

            //
            // Verify Blam tag instance
            //

            var unitName = args[0].ToLower();

            if (unitName != "spartan" && unitName != "elite")
            {
                Console.WriteLine("ERROR: Only 'spartan' and 'elite' armor variants are allowed.");
                return false;
            }

            args.RemoveAt(0);

            var blamTagName = unitName == "spartan" ?
                @"objects\characters\masterchief\mp_masterchief\mp_masterchief" :
                @"objects\characters\elite\mp_elite\mp_elite";

            Console.Write($"Verifying {blamTagName}.render_model...");

            CacheFile.IndexItem blamTag = null;

            foreach (var tag in BlamCache.IndexItems)
            {
                if ((tag.GroupTag == "mode") && (tag.Name == blamTagName))
                {
                    blamTag = tag;
                    break;
                }
            }

            if (blamTag == null)
            {
                Console.WriteLine($"ERROR: Blam tag does not exist: {blamTagName}.render_model");
                return true;
            }

            Console.WriteLine("done.");

            //
            // Load the Blam tag definition
            //

            var variantName = args[0];
            args.RemoveAt(0);

            CachedTagInstance edModeTag = null;
            var isScenery = false;
            var regionNames = new List<string>();

            while (args.Count != 0)
            {
                switch (args[0].ToLower())
                {
                    case "scenery":
                        isScenery = true;
                        args.RemoveAt(0);
                        break;

                    case "replace:":
                        edModeTag = CacheContext.GetTag(args[1]);
                        args.RemoveAt(1);
                        args.RemoveAt(0);
                        break;

                    case "regions:":
                        regionNames.AddRange(args.Skip(1));
                        args.Clear();
                        break;

                    default:
                        throw new InvalidDataException($"{args}");
                }
            }
            
            var blamContext = new CacheSerializationContext(ref BlamCache, blamTag);
            var edModeDefinition = BlamCache.Deserializer.Deserialize<RenderModel>(blamContext);

            var materials = edModeDefinition.Materials.Select(i => new RenderMaterial
            {
                BreakableSurfaceIndex = i.BreakableSurfaceIndex,
                Properties = i.Properties,
                RenderMethod = i.RenderMethod,
                Skins = i.Skins,
                Unknown = i.Unknown,
                Unknown2 = i.Unknown2,
                Unknown3 = i.Unknown3,
                Unknown4 = i.Unknown4
            }).ToList();

            edModeDefinition = (RenderModel)ConvertData(null, edModeDefinition, false);

            var variantRegions = new List<RenderModel.Region>();

            foreach (var region in edModeDefinition.Regions)
            {
                if (regionNames.Count != 0 && !regionNames.Contains(CacheContext.GetString(region.Name)))
                    continue;

                var variantRegion = new RenderModel.Region
                {
                    Name = region.Name,
                    Permutations = new List<RenderModel.Region.Permutation>()
                };

                foreach (var permutation in region.Permutations)
                    if (variantName == CacheContext.GetString(permutation.Name))
                        variantRegion.Permutations.Add(permutation);

                variantRegions.Add(variantRegion);
            }

            var variantMeshes = new List<int>();
            var variantMaterials = new List<int>();
            var variantVertexBuffers = new List<int>();
            var variantIndexBuffers = new List<int>();

            foreach (var region in variantRegions)
            {
                foreach (var permutation in region.Permutations)
                {
                    for (var i = permutation.MeshIndex; i < (short)(permutation.MeshIndex + permutation.MeshCount); i++)
                    {
                        var mesh = edModeDefinition.Geometry.Meshes[i];

                        foreach (var part in mesh.Parts)
                            if (part.MaterialIndex != -1 && !variantMaterials.Contains(part.MaterialIndex))
                                variantMaterials.Add(part.MaterialIndex);

                        foreach (var vertexBuffer in mesh.VertexBufferIndices)
                            if (vertexBuffer != ushort.MaxValue && !variantVertexBuffers.Contains(vertexBuffer))
                                variantVertexBuffers.Add(vertexBuffer);

                        foreach (var indexBuffer in mesh.IndexBufferIndices)
                            if (indexBuffer != ushort.MaxValue && !variantIndexBuffers.Contains(indexBuffer))
                                variantIndexBuffers.Add(indexBuffer);

                        if (!variantMeshes.Contains(i))
                            variantMeshes.Add(i);
                    }
                }
            }

            variantMeshes.Sort();
            variantMaterials.Sort();
            variantVertexBuffers.Sort();
            variantIndexBuffers.Sort();

            foreach (var meshIndex in variantMeshes)
            {
                var mesh = edModeDefinition.Geometry.Meshes[meshIndex];

                foreach (var part in mesh.Parts)
                    if (part.MaterialIndex != -1)
                        part.MaterialIndex = (short)variantMaterials.IndexOf(part.MaterialIndex);
            }

            foreach (var region in variantRegions)
                foreach (var permutation in region.Permutations)
                    if (permutation.MeshIndex != -1)
                        permutation.MeshIndex = (short)variantMeshes.IndexOf(permutation.MeshIndex);

            foreach (var meshIndex in variantMeshes)
            {
                var mesh = edModeDefinition.Geometry.Meshes[meshIndex];

                for (var i = 0; i < mesh.VertexBufferIndices.Length; i++)
                {
                    if (!variantVertexBuffers.Contains(mesh.VertexBufferIndices[i]))
                        mesh.VertexBufferIndices[i] = ushort.MaxValue;
                    else
                        mesh.VertexBufferIndices[i] = (ushort)variantVertexBuffers.IndexOf(mesh.VertexBufferIndices[i]);
                }

                for (var i = 0; i < mesh.IndexBufferIndices.Length; i++)
                {
                    if (!variantIndexBuffers.Contains(mesh.IndexBufferIndices[i]))
                        mesh.IndexBufferIndices[i] = ushort.MaxValue;
                    else
                        mesh.IndexBufferIndices[i] = (ushort)variantIndexBuffers.IndexOf(mesh.IndexBufferIndices[i]);
                }
            }

            edModeDefinition.Regions = variantRegions;
            edModeDefinition.Geometry.Meshes = edModeDefinition.Geometry.Meshes.Where(i => variantMeshes.Contains(edModeDefinition.Geometry.Meshes.IndexOf(i))).ToList();

            //
            // Port Blam render_model materials
            //

            materials = materials.Where(i => variantMaterials.Contains(materials.IndexOf(i))).ToList();

            using (var stream = CacheContext.OpenTagCacheReadWrite())
            {
                for (var i = 0; i < materials.Count; i++)
                {
                    var material = materials[i];

                    if (material.RenderMethod.Index == -1)
                        continue;

                    var blamRenderMethod = materials[i].RenderMethod;
                    var blamRenderMethodTag = BlamCache.IndexItems.GetItemByID(blamRenderMethod.Index);

                    var renderMethodExists = false;

                    foreach (var instance in CacheContext.TagCache.Index.FindAllInGroup("rm  "))
                    {
                        if (CacheContext.TagNames.ContainsKey(instance.Index) && CacheContext.TagNames[instance.Index] == blamRenderMethodTag.Name)
                        {
                            renderMethodExists = true;
                            material.RenderMethod = instance;
                            break;
                        }
                    }

                    if (!renderMethodExists)
                        material.RenderMethod = CacheContext.GetTag<Shader>(@"shaders\invalid");
                }
            }

            edModeDefinition.Materials = materials;

            //
            // Load Blam resource data
            //

            var resourceData = BlamCache.GetRawFromID(edModeDefinition.Geometry.ZoneAssetHandle);

            if (resourceData == null)
            {
                Console.WriteLine("Blam render_geometry resource contains no data.");
                return true;
            }

            //
            // Load Blam resource definition
            //

            Console.Write("Loading Blam render_geometry resource definition...");

            var definitionEntry = BlamCache.ResourceGestalt.TagResources[edModeDefinition.Geometry.ZoneAssetHandle & ushort.MaxValue];

            var resourceDefinition = new RenderGeometryApiResourceDefinition
            {
                VertexBuffers = new List<TagStructureReference<VertexBufferDefinition>>(),
                IndexBuffers = new List<TagStructureReference<IndexBufferDefinition>>()
            };

            using (var definitionStream = new MemoryStream(BlamCache.ResourceGestalt.FixupInformation))
            using (var definitionReader = new EndianReader(definitionStream, EndianFormat.BigEndian))
            {
                var dataContext = new DataSerializationContext(definitionReader, null, CacheAddressType.Definition);

                definitionReader.SeekTo(definitionEntry.FixupInformationOffset + (definitionEntry.FixupInformationLength - 24));

                var vertexBufferCount = definitionReader.ReadInt32();
                definitionReader.Skip(8);
                var indexBufferCount = definitionReader.ReadInt32();

                definitionReader.SeekTo(definitionEntry.FixupInformationOffset);

                for (var i = 0; i < vertexBufferCount; i++)
                {
                    resourceDefinition.VertexBuffers.Add(new TagStructureReference<VertexBufferDefinition>
                    {
                        Definition = new VertexBufferDefinition
                        {
                            Count = definitionReader.ReadInt32(),
                            Format = (VertexBufferFormat)definitionReader.ReadInt16(),
                            VertexSize = definitionReader.ReadInt16(),
                            Data = new TagData
                            {
                                Size = definitionReader.ReadInt32(),
                                Unused4 = definitionReader.ReadInt32(),
                                Unused8 = definitionReader.ReadInt32(),
                                Address = new CacheAddress(CacheAddressType.Memory, definitionReader.ReadInt32()),
                                Unused10 = definitionReader.ReadInt32()
                            }
                        }
                    });
                }

                definitionReader.Skip(vertexBufferCount * 12);

                for (var i = 0; i < indexBufferCount; i++)
                {
                    resourceDefinition.IndexBuffers.Add(new TagStructureReference<IndexBufferDefinition>
                    {
                        Definition = new IndexBufferDefinition
                        {
                            Format = (IndexBufferFormat)definitionReader.ReadInt32(),
                            Data = new TagData
                            {
                                Size = definitionReader.ReadInt32(),
                                Unused4 = definitionReader.ReadInt32(),
                                Unused8 = definitionReader.ReadInt32(),
                                Address = new CacheAddress(CacheAddressType.Memory, definitionReader.ReadInt32()),
                                Unused10 = definitionReader.ReadInt32()
                            }
                        }
                    });
                }
            }

            Console.WriteLine("done.");

            //
            // Convert Blam resource data
            //

            using (var edResourceStream = new MemoryStream())
            {
                //
                // Convert Blam render_geometry_api_resource_definition
                //

                using (var blamResourceStream = new MemoryStream(resourceData))
                {
                    //
                    // Convert Blam vertex buffers
                    //

                    Console.Write("Converting vertex buffers...");

                    var previousVertexBufferCount = -1;

                    for (var i = 0; i < resourceDefinition.VertexBuffers.Count; i++)
                    {
                        if (!variantVertexBuffers.Contains(i))
                            continue;

                        blamResourceStream.Position = definitionEntry.ResourceFixups[i].Offset;
                        if (i > 0)
                            previousVertexBufferCount = resourceDefinition.VertexBuffers[i - 1].Definition.Count;
                        GeometryConverter.ConvertVertexBuffer(resourceDefinition, blamResourceStream, edResourceStream, i, previousVertexBufferCount);
                    }

                    Console.WriteLine("done.");

                    //
                    // Convert Blam index buffers
                    //

                    Console.Write("Converting index buffers...");

                    for (var i = 0; i < resourceDefinition.IndexBuffers.Count; i++)
                    {
                        if (!variantIndexBuffers.Contains(i))
                            continue;

                        blamResourceStream.Position = definitionEntry.ResourceFixups[resourceDefinition.VertexBuffers.Count * 2 + i].Offset;
                        GeometryConverter.ConvertIndexBuffer(resourceDefinition, blamResourceStream, edResourceStream, i);
                    }

                    Console.WriteLine("done.");
                }

                resourceDefinition.VertexBuffers = resourceDefinition.VertexBuffers.Where(i => variantVertexBuffers.Contains(resourceDefinition.VertexBuffers.IndexOf(i))).ToList();
                resourceDefinition.IndexBuffers = resourceDefinition.IndexBuffers.Where(i => variantIndexBuffers.Contains(resourceDefinition.IndexBuffers.IndexOf(i))).ToList();

                //
                // Finalize the new ElDorado geometry resource
                //

                Console.Write("Writing resource data...");

                edModeDefinition.Geometry.Resource = new PageableResource
                {
                    Page = new RawPage(),
                    Resource = new TagResource
                    {
                        Type = TagResourceType.RenderGeometry,
                        ResourceFixups = new List<TagResource.ResourceFixup>(),
                        ResourceDefinitionFixups = new List<TagResource.ResourceDefinitionFixup>(),
                        Unknown2 = 1
                    }
                };

                edResourceStream.Position = 0;

                var resourceContext = new ResourceSerializationContext(edModeDefinition.Geometry.Resource);
                CacheContext.Serializer.Serialize(resourceContext, resourceDefinition);
                edModeDefinition.Geometry.Resource.ChangeLocation(ResourceLocation.ResourcesB);
                CacheContext.AddResource(edModeDefinition.Geometry.Resource, edResourceStream);

                Console.WriteLine("done.");
            }

            edModeDefinition.Name = CacheContext.GetStringId(variantName);

            if (edModeTag == null)
            {
                for (var i = 0; i < CacheContext.TagCache.Index.Count; i++)
                {
                    if (CacheContext.TagCache.Index[i] == null)
                    {
                        CacheContext.TagCache.Index[i] = edModeTag = new CachedTagInstance(i, TagGroup.Instances[new Tag("mode")]);
                        break;
                    }
                }

                if (edModeTag == null)
                    edModeTag = CacheContext.TagCache.AllocateTag(TagGroup.Instances[new Tag("mode")]);
            }

            //
            // Create a new armor model tag
            //

            Model edHlmtDefinition = null;
            CachedTagInstance edHlmtTag = null;

            if (isScenery)
            {
                Console.Write($"Verifying {blamTagName}.model...");

                CacheFile.IndexItem blamHlmtTag = null;

                foreach (var tag in BlamCache.IndexItems)
                {
                    if ((tag.GroupTag == "hlmt") && (tag.Name == blamTagName))
                    {
                        blamHlmtTag = tag;
                        break;
                    }
                }

                if (blamHlmtTag == null)
                {
                    Console.WriteLine($"ERROR: Blam tag does not exist: {blamTagName}.model");
                    return true;
                }

                Console.WriteLine("done.");

                blamContext = new CacheSerializationContext(ref BlamCache, blamHlmtTag);
                edHlmtDefinition = (Model)ConvertData(null, BlamCache.Deserializer.Deserialize<Model>(blamContext), false);

                edHlmtDefinition.RenderModel = edModeTag;
                edHlmtDefinition.ReduceToL1SuperLow = 36.38004f;
                edHlmtDefinition.ReduceToL2Low = 27.28503f;
                edHlmtDefinition.Variants = new List<Model.Variant>();
                edHlmtDefinition.Materials = new List<Model.Material>();
                edHlmtDefinition.NewDamageInfo = new List<Model.GlobalDamageInfoBlock>();
                edHlmtDefinition.Targets = new List<Model.Target>();

                var collisionRegions = new List<Model.CollisionRegion>();

                foreach (var collisionRegion in edHlmtDefinition.CollisionRegions)
                {
                    var found = false;

                    foreach (var variantRegion in variantRegions)
                    {
                        if (collisionRegion.Name == variantRegion.Name)
                        {
                            found = true;
                            break;
                        }
                    }

                    if (!found)
                        continue;

                    found = false;

                    foreach (var permutation in collisionRegion.Permutations)
                    {
                        if (permutation.Name == CacheContext.GetStringId(variantName))
                        {
                            found = true;
                            break;
                        }
                    }

                    if (found)
                        collisionRegions.Add(collisionRegion);
                }

                foreach (var collisionRegion in collisionRegions)
                {
                    Model.CollisionRegion.Permutation permutation = null;

                    foreach (var collisionPermutation in collisionRegion.Permutations)
                    {
                        if (collisionPermutation.Name == CacheContext.GetStringId(variantName))
                        {
                            permutation = collisionPermutation;
                            break;
                        }
                    }

                    if (permutation == null)
                        throw new KeyNotFoundException();

                    collisionRegion.Permutations = new List<Model.CollisionRegion.Permutation> { permutation };
                }

                edHlmtDefinition.CollisionRegions = collisionRegions;

                for (var i = 0; i < CacheContext.TagCache.Index.Count; i++)
                {
                    if (CacheContext.TagCache.Index[i] == null)
                    {
                        CacheContext.TagCache.Index[i] = edHlmtTag = new CachedTagInstance(i, TagGroup.Instances[new Tag("hlmt")]);
                        break;
                    }
                }

                if (edHlmtTag == null)
                    edHlmtTag = CacheContext.TagCache.AllocateTag(TagGroup.Instances[new Tag("hlmt")]);
            }

            //
            // Create a new armor scenery tag
            //

            Scenery edScenDefinition = null;
            CachedTagInstance edScenTag = null;

            if (isScenery)
            {
                edScenDefinition = new Scenery
                {
                    ObjectType = new GameObjectType
                    {
                        Halo3Retail = GameObjectTypeHalo3Retail.Scenery,
                        Halo3ODST = GameObjectTypeHalo3ODST.Scenery,
                        HaloOnline = GameObjectTypeHaloOnline.Scenery
                    },
                    BoundingRadius = 0.44f,
                    BoundingOffset = new RealPoint3d(-0.02f, 0.0f, 0.0f),
                    AccelerationScale = 1.2f,
                    SweetenerSize = GameObject.SweetenerSizeValue.Medium,
                    Model = edHlmtTag,
                    ChangeColors = new List<GameObject.ChangeColor>
                    {
                        new GameObject.ChangeColor(),
                        new GameObject.ChangeColor(),
                        new GameObject.ChangeColor(),
                        new GameObject.ChangeColor(),
                        new GameObject.ChangeColor()
                    },
                    NodeMaps = new List<GameObject.NodeMap>()
                };

                for (sbyte i = 0; i < 51; i++)
                    edScenDefinition.NodeMaps.Add(new GameObject.NodeMap { TargetNode = i });
                
                for (var i = 0; i < CacheContext.TagCache.Index.Count; i++)
                {
                    if (CacheContext.TagCache.Index[i] == null)
                    {
                        CacheContext.TagCache.Index[i] = edScenTag = new CachedTagInstance(i, TagGroup.Instances[new Tag("scen")]);
                        break;
                    }
                }

                if (edScenTag == null)
                    edScenTag = CacheContext.TagCache.AllocateTag(TagGroup.Instances[new Tag("scen")]);
            }

            //
            // Serialize new ElDorado tag definitions
            //

            using (var cacheStream = CacheContext.OpenTagCacheReadWrite())
            {
                CacheContext.Serializer.Serialize(new TagSerializationContext(cacheStream, CacheContext, edModeTag), edModeDefinition);
                CacheContext.TagNames[edModeTag.Index] = isScenery ?
                    (unitName == "spartan" ?
                        $@"objects\characters\masterchief\mp_masterchief\armor\{variantName}" :
                        $@"objects\characters\elite\mp_elite\armor\{variantName}") :
                    (unitName == "spartan" ?
                        @"objects\characters\masterchief\mp_masterchief\mp_masterchief" :
                        @"objects\characters\elite\mp_elite\mp_elite");

                if (isScenery)
                {
                    CacheContext.Serializer.Serialize(new TagSerializationContext(cacheStream, CacheContext, edHlmtTag), edHlmtDefinition);
                    CacheContext.Serializer.Serialize(new TagSerializationContext(cacheStream, CacheContext, edScenTag), edScenDefinition);
                    CacheContext.TagNames[edHlmtTag.Index] = CacheContext.TagNames[edScenTag.Index] = unitName == "spartan" ?
                        $@"objects\characters\masterchief\mp_masterchief\armor\{variantName}" :
                        $@"objects\characters\elite\mp_elite\armor\{variantName}";
                }
            }

            return true;
        }

        private object ConvertData(Stream cacheStream, object data, bool replace)
        {
            if (data == null)
                return null;

            var type = data.GetType();

            if (type.IsPrimitive)
                return data;

            if (type == typeof(CachedTagInstance))
                return null;

            if (type == typeof(StringId))
                return ConvertStringId((StringId)data);

            if (type.IsArray)
                return ConvertArray(cacheStream, (Array)data, replace);

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
                return ConvertList(cacheStream, data, type, replace);

            if (type.GetCustomAttributes(typeof(TagStructureAttribute), false).Length > 0)
                return ConvertStructure(cacheStream, data, type, replace);

            return data;
        }

        private StringId ConvertStringId(StringId stringId)
        {
            var value = BlamCache.Strings.GetString(stringId);

            if (!CacheContext.StringIdCache.Contains(value))
            {
                CacheContext.StringIdCache.AddString(value);

                Console.Write($"Saving new string id \"{value}\"...");

                using (var stringIdStream = CacheContext.OpenStringIdCacheReadWrite())
                    CacheContext.StringIdCache.Save(stringIdStream);

                Console.WriteLine("done.");
            }
            return CacheContext.GetStringId(value);
        }

        private Array ConvertArray(Stream cacheStream, Array array, bool replace)
        {
            if (array.GetType().GetElementType().IsPrimitive)
                return array;

            for (var i = 0; i < array.Length; i++)
            {
                var oldValue = array.GetValue(i);
                var newValue = ConvertData(cacheStream, oldValue, replace);
                array.SetValue(newValue, i);
            }

            return array;
        }

        private object ConvertList(Stream cacheStream, object list, Type type, bool replace)
        {
            if (type.GenericTypeArguments[0].IsPrimitive)
                return list;

            var count = (int)type.GetProperty("Count").GetValue(list);

            var getItem = type.GetMethod("get_Item");
            var setItem = type.GetMethod("set_Item");

            for (var i = 0; i < count; i++)
            {
                var oldValue = getItem.Invoke(list, new object[] { i });
                var newValue = ConvertData(cacheStream, oldValue, replace);
                setItem.Invoke(list, new object[] { i, newValue });
            }

            return list;
        }

        private object ConvertStructure(Stream cacheStream, object data, Type type, bool replace)
        {
			using (var enumerator = ReflectionCache.GetTagFieldEnumerator(type, CacheContext.Version))
			{
				while (enumerator.Next())
				{
					var oldValue = enumerator.Field.GetValue(data);
					var newValue = ConvertData(cacheStream, oldValue, replace);
					enumerator.Field.SetValue(data, newValue);
				}
			}
            return data;
        }
    }
}

