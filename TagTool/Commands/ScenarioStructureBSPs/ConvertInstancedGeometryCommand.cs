using System;
using System.Collections.Generic;
using System.IO;
using TagTool.Cache;
using TagTool.Common;
using TagTool.IO;
using TagTool.Tags.Definitions;
using TagTool.Geometry;
using TagTool.Tags;
using TagTool.Cache.HaloOnline;
using TagTool.Tags.Resources;

namespace TagTool.Commands.Scenarios
{
    class ConvertInstancedGeometryCommand : Command
    {
        private GameCache CacheContext { get; }
        private Scenario Scnr { get; }

        public ConvertInstancedGeometryCommand(GameCache cacheContext, Scenario scnr) :
            base(false,

                "ConvertInstancedGeometry",
                "Convert Instanced Geometry to forge objects",

                "ConvertInstancedGeometry",

                "Convert Instanced Geometry to forge objects")
        {
            CacheContext = cacheContext;
            Scnr = scnr;
        }

        public override object Execute(List<string> args)
        {
            using (var stream = CacheContext.OpenCacheReadWrite())
            {
                for (var sbspindex = 0; sbspindex < Scnr.StructureBsps.Count; sbspindex++)
                {
                    var Sbsp = (ScenarioStructureBsp)CacheContext.Deserialize(stream, Scnr.StructureBsps[sbspindex].StructureBsp);
                    var sLdT = (ScenarioLightmap)CacheContext.Deserialize(stream, Scnr.Lightmap);
                    var Lbsp = (ScenarioLightmapBspData)CacheContext.Deserialize(stream, sLdT.LightmapDataReferences[sbspindex]);

                    //set resource definition
                    var resourceDefinition = CacheContext.ResourceCache.GetRenderGeometryApiResourceDefinition(Lbsp.Geometry.Resource);
                    Lbsp.Geometry.SetResourceBuffers(resourceDefinition);

                    foreach (ScenarioStructureBsp.InstancedGeometryInstance InstancedGeometryBlock in Sbsp.InstancedGeometryInstances)
                    {
                        if (InstancedGeometryBlock.MeshIndex < 0)
                            continue;

                        //strip digits from the end of the instancedgeometry name
                        string tempname = CacheContext.StringTable.GetString(InstancedGeometryBlock.Name);
                        var digits = new[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
                        var instancedgeoname = tempname.TrimEnd(digits);

                        string NewName = $"objects\\reforge\\instanced_geometry\\{instancedgeoname}";

                        //if the tag we are trying to create already exists, continue
                        if (CacheContext.TryGetTag<Crate>(NewName, out var unused))
                            continue;

                        //duplicate traffic cone bloc
                        CachedTag originalbloctag = CacheContext.GetTag<Crate>(@"objects\gear\human\industrial\street_cone\street_cone");
                        var newbloc = CacheContext.TagCache.AllocateTag(originalbloctag.Group, NewName);
                        var originalbloc = CacheContext.Deserialize(stream, originalbloctag);
                        CacheContext.Serialize(stream, newbloc, originalbloc);

                        //duplicate traffic cone hlmt
                        CachedTag originalhlmttag = CacheContext.GetTag<Model>(@"objects\gear\human\industrial\street_cone\street_cone");
                        var newhlmt = CacheContext.TagCache.AllocateTag(originalhlmttag.Group, NewName);
                        var originalhlmt = CacheContext.Deserialize(stream, originalhlmttag);
                        CacheContext.Serialize(stream, newhlmt, originalhlmt);

                        //duplicate traffic cone render model
                        CachedTag originalmodetag = CacheContext.GetTag<RenderModel>(@"objects\gear\human\industrial\street_cone\street_cone");
                        var newmode = CacheContext.TagCache.AllocateTag(originalmodetag.Group, NewName);
                        var originalmode = CacheContext.Deserialize(stream, originalmodetag);
                        CacheContext.Serialize(stream, newmode, originalmode);

                        //copy block elements and resources from sbsp for new mode
                        RenderModel editedmode = (RenderModel)CacheContext.Deserialize(stream, newmode);

                        //
                        // warning: this relies on GetSingleMeshResourceDefinition updating the vertex/index buffer indices in the Lbsp, not safe
                        //

                        var newResourceDefinition = GetSingleMeshResourceDefinition(Lbsp.Geometry, InstancedGeometryBlock.MeshIndex);
                        editedmode.Geometry.Resource = CacheContext.ResourceCache.CreateRenderGeometryApiResource(newResourceDefinition);

                        //copy meshes tagblock
                        editedmode.Geometry.Meshes = new List<Mesh>
                            {
                                Lbsp.Geometry.Meshes[InstancedGeometryBlock.MeshIndex]
                            };

                        //copy over materials block, and reindex mesh part materials
                        var newmaterials = new List<RenderMaterial>();
                        for (var i = 0; i < editedmode.Geometry.Meshes[0].Parts.Count; i++)
                        {
                            newmaterials.Add(Sbsp.Materials[editedmode.Geometry.Meshes[0].Parts[i].MaterialIndex]);
                            editedmode.Geometry.Meshes[0].Parts[i].MaterialIndex = (short)i;
                        }
                        editedmode.Materials = newmaterials;

                        CacheContext.Serialize(stream, newmode, editedmode);

                        //fixup hlmt references
                        var tmphlmt = (Model)CacheContext.Deserialize(stream, newhlmt);
                        tmphlmt.RenderModel = newmode;
                        CacheContext.Serialize(stream, newhlmt, tmphlmt);

                        //fixup bloc references
                        var tmpbloc = (Crate)CacheContext.Deserialize(stream, newbloc);
                        tmpbloc.Model = newhlmt;
                        CacheContext.Serialize(stream, newbloc, tmpbloc);

                        //add new object to forge globals
                        CachedTag forgeglobal = CacheContext.GetTag<ForgeGlobalsDefinition>(@"multiplayer\forge_globals");
                        var tmpforg = (ForgeGlobalsDefinition)CacheContext.Deserialize(stream, forgeglobal);
                        tmpforg.Palette.Add(new ForgeGlobalsDefinition.PaletteItem
                        {
                            Name = instancedgeoname,
                            Type = ForgeGlobalsDefinition.PaletteItemType.Structure,
                            CategoryIndex = 12,
                            DescriptionIndex = -1,
                            MaxAllowed = 0,
                            Object = newbloc
                        });
                        CacheContext.Serialize(stream, forgeglobal, tmpforg);
                    }
                }
            }

            Console.WriteLine("Done!");

            return true;
        }

        private RenderGeometryApiResourceDefinition GetSingleMeshResourceDefinition(RenderGeometry renderGeometry, int meshindex)
        {
            RenderGeometryApiResourceDefinition result = new RenderGeometryApiResourceDefinition
            {
                IndexBuffers = new TagBlock<D3DStructure<IndexBufferDefinition>>(),
                VertexBuffers = new TagBlock<D3DStructure<VertexBufferDefinition>>()
            };

            // valid for gen3, InteropLocations should also point to the definition.
            result.IndexBuffers.AddressType = CacheAddressType.Definition;
            result.VertexBuffers.AddressType = CacheAddressType.Definition;

            var mesh = renderGeometry.Meshes[meshindex];

            for (int i = 0; i < mesh.ResourceVertexBuffers.Length; i++)
            {
                var vertexBuffer = mesh.ResourceVertexBuffers[i];
                if (vertexBuffer != null)
                {
                    var d3dPointer = new D3DStructure<VertexBufferDefinition>();
                    d3dPointer.Definition = vertexBuffer;
                    result.VertexBuffers.Add(d3dPointer);
                    mesh.VertexBufferIndices[i] = (short)(result.VertexBuffers.Elements.Count - 1);
                }
                else
                    mesh.VertexBufferIndices[i] = -1;
            }

            for (int i = 0; i < mesh.ResourceIndexBuffers.Length; i++)
            {
                var indexBuffer = mesh.ResourceIndexBuffers[i];
                if (indexBuffer != null)
                {
                    var d3dPointer = new D3DStructure<IndexBufferDefinition>();
                    d3dPointer.Definition = indexBuffer;
                    result.IndexBuffers.Add(d3dPointer);
                    mesh.IndexBufferIndices[i] = (short)(result.IndexBuffers.Elements.Count - 1);
                }
                else
                    mesh.IndexBufferIndices[i] = -1;
            }

            // if the mesh is unindexed the index in the index buffer should be 0, but the buffer is empty. Copying what h3\ho does.
            if (mesh.Flags.HasFlag(MeshFlags.MeshIsUnindexed))
            {
                mesh.IndexBufferIndices[0] = 0;
                mesh.IndexBufferIndices[1] = 0;
            }

            return result;
        }
    }
}
