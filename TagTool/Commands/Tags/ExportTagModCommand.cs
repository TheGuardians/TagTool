using System;
using System.Collections.Generic;
using System.IO;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Serialization;
using TagTool.Tags;
using TagTool.Tags.Definitions;

namespace TagTool.Commands.Tags
{
    class ExportTagModCommand : Command
    {
        public HaloOnlineCacheContext CacheContext { get; }

        public ExportTagModCommand(HaloOnlineCacheContext cacheContext) :
            base(false,
                
                "ExportTagMod",
                "",
                
                "ExportTagMod <Name> <Directory> {Tag1, ..., TagN}",
                
                "")
        {
            CacheContext = cacheContext;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count < 2)
                return false;

            var name = args[0];
            var directory = new DirectoryInfo(args[1]);

            if (!directory.Exists)
                directory.Create();

            var scriptFile = new FileInfo(Path.Combine(directory.FullName, $"{name}.cmds"));

            using (var cacheStream = CacheContext.OpenTagCacheRead())
            using (var scriptWriter = new StreamWriter(scriptFile.Exists ? scriptFile.Open(FileMode.Open, FileAccess.ReadWrite) : scriptFile.Create()))
            {
                var tagIndices = new HashSet<int>();

                void LoadTagDependencies(int index)
                {
                    var queue = new List<int> { index };

                    while (queue.Count != 0)
                    {
                        var nextQueue = new List<int>();

                        foreach (var entry in queue)
                        {
                            if (!tagIndices.Contains(entry))
                            {
                                var instance = CacheContext.TagCache.Index[entry];

                                if (instance == null || instance.IsInGroup("rmt2") || instance.IsInGroup("rmdf") || instance.IsInGroup("vtsh") || instance.IsInGroup("pixl") || instance.IsInGroup("glvs") || instance.IsInGroup("glps"))
                                    continue;

                                tagIndices.Add(entry);

                                foreach (var dependency in instance.Dependencies)
                                {
                                    if (dependency == entry)
                                        continue;

                                    if (!nextQueue.Contains(dependency))
                                        nextQueue.Add(dependency);
                                }
                            }
                        }

                        queue = nextQueue;
                    }
                }

                string line;

                while ((line = Console.ReadLine()) != "")
                {
                    CachedTagInstance instance = null;

                    try
                    {
                        instance = ArgumentParser.ParseTagSpecifier(CacheContext, line);
                    }
                    catch
                    {
                        continue;
                    }

                    if (instance == null)
                        continue;

                    LoadTagDependencies(instance.Index);
                }

                /*foreach (var index in tagIndices)
                {
                    var instance = CacheContext.GetTag(index);

                    if (instance == null || !instance.IsInGroup("bitm"))
                        continue;

                    var tagName = CacheContext.TagNames.ContainsKey(instance.Index) ?
                        CacheContext.TagNames[instance.Index] :
                        $"0x{instance.Index:X4}";

                    Console.WriteLine($"[Index: 0x{instance.Index:X4}, Offset: 0x{instance.HeaderOffset:X8}, Size: 0x{instance.TotalSize:X4}] {tagName}.{CacheContext.GetString(instance.Group.Name)}");
                }*/

                var importedTags = new HashSet<int>();

                foreach (var index in tagIndices)
                {
                    if (importedTags.Contains(index))
                        continue;

                    var instance = CacheContext.GetTag(index);

                    if (instance == null)
                        continue;

                    var tagName = CacheContext.TagNames.ContainsKey(instance.Index) ?
                        CacheContext.TagNames[instance.Index] :
                        $"0x{instance.Index:X4}";

                    var groupName = CacheContext.GetString(instance.Group.Name);

                    var file = new FileInfo(Path.Combine(directory.FullName, $"tags\\{tagName}.{groupName}"));
                    var data = CacheContext.TagCache.ExtractTagRaw(cacheStream, instance);

                    if (!file.Directory.Exists)
                        file.Directory.Create();

                    using (var outStream = file.Create())
                        outStream.Write(data, 0, data.Length);

                    scriptWriter.WriteLine($"CreateTag \"{instance.Group.Tag}\" 0x{instance.Index:X4}");

                    if (!tagName.StartsWith("0x"))
                        scriptWriter.WriteLine($"NameTag 0x{instance.Index:X4} {tagName}");

                    scriptWriter.WriteLine($"ImportTag 0x{instance.Index:X4} \"tags\\{tagName}.{groupName}\"");
                    scriptWriter.WriteLine();

                    importedTags.Add(index);
                }

                var completedTags = new HashSet<int>();

                foreach (var index in tagIndices)
                {
                    if (completedTags.Contains(index))
                        continue;

                    var instance = CacheContext.GetTag(index);

                    if (instance == null)
                        continue;

                    var tagName = CacheContext.TagNames.ContainsKey(instance.Index) ?
                        CacheContext.TagNames[instance.Index] :
                        $"0x{instance.Index:X4}";

                    var groupName = CacheContext.GetString(instance.Group.Name);

                    var tagContext = new TagSerializationContext(cacheStream, CacheContext, instance);
                    var tagDefinition = CacheContext.Deserialize(tagContext, TagDefinition.Find(instance.Group));

                    FileInfo ExportResource(PageableResource pageable, string resourceGroup, string suffix = "")
                    {
                        if (!pageable.GetLocation(out var location))
                            return null;

                        var outFile = new FileInfo(Path.Combine(directory.FullName, $"tags\\{tagName}{suffix}.{resourceGroup}"));
                        var cache = CacheContext.GetResourceCache(location);

                        if (!outFile.Directory.Exists)
                            outFile.Directory.Create();

                        using (var stream = CacheContext.OpenResourceCacheRead(location))
                        using (var outStream = outFile.Create())
                            cache.Decompress(stream, pageable.Page.Index, pageable.Page.CompressedBlockSize, outStream);

                        return outFile;
                    }
                    
                    switch (tagDefinition)
                    {
                        case Bink bink:
                            {
                                scriptWriter.WriteLine($"EditTag 0x{instance.Index:X4}");

                                var resourceFile = ExportResource(bink.Resource, "bink_resource");

                                if (resourceFile == null)
                                {
                                    scriptWriter.WriteLine("SetField Resource null");
                                }
                                else
                                {
                                    scriptWriter.WriteLine($"SetField Resource.Page.Index -1");
                                    scriptWriter.WriteLine($"SetField Resource ResourcesB \"tags\\{tagName}.bink_resource\"");
                                }

                                scriptWriter.WriteLine("SaveTagChanges");
                                scriptWriter.WriteLine("ExitTo tags");
                                scriptWriter.WriteLine();
                            }
                            break;

                        case Bitmap bitm:
                            {
                                scriptWriter.WriteLine($"EditTag 0x{instance.Index:X4}");

                                for (var i = 0; i < bitm.Resources.Count; i++)
                                {
                                    var resourceFile = ExportResource(bitm.Resources[i].Resource, "bitmap_texture_interop_resource", bitm.Resources.Count > 1 ? $"_image_{i}" : "_image");

                                    if (resourceFile == null)
                                    {
                                        scriptWriter.WriteLine($"SetField Resources[{i}].Resource null");
                                        continue;
                                    }

                                    scriptWriter.WriteLine($"SetField Resources[{i}].Resource.Page.Index -1");
                                    scriptWriter.WriteLine($"SetField Resources[{i}].Resource ResourcesB \"tags\\{tagName}{(bitm.Resources.Count > 1 ? $"_image_{i}" : "_image")}.bitmap_texture_interop_resource\"");
                                }

                                scriptWriter.WriteLine("SaveTagChanges");
                                scriptWriter.WriteLine("ExitTo tags");
                                scriptWriter.WriteLine();
                            }
                            break;

                        case RenderModel mode:
                            {
                                scriptWriter.WriteLine($"EditTag 0x{instance.Index:X4}");

                                var resourceFile = ExportResource(mode.Geometry.Resource, "render_geometry_api_resource_definition", "_geometry");

                                if (resourceFile == null)
                                {
                                    scriptWriter.WriteLine("SetField Geometry.Resource null");
                                }
                                else
                                {
                                    scriptWriter.WriteLine($"SetField Geometry.Resource.Page.Index -1");
                                    scriptWriter.WriteLine($"SetField Geometry.Resource ResourcesB \"tags\\{tagName}_geometry.render_geometry_api_resource_definition\"");
                                }

                                scriptWriter.WriteLine("SaveTagChanges");
                                scriptWriter.WriteLine("ExitTo tags");
                                scriptWriter.WriteLine();
                            }
                            break;

                        case ModelAnimationGraph jmad:
                            {
                                scriptWriter.WriteLine($"EditTag 0x{instance.Index:X4}");

                                for (var i = 0; i < jmad.ResourceGroups.Count; i++)
                                {
                                    var resourceFile = ExportResource(jmad.ResourceGroups[i].Resource, "model_animation_tag_resource", jmad.ResourceGroups.Count > 1 ? $"_group_{i}" : "_group");

                                    if (resourceFile == null)
                                    {
                                        scriptWriter.WriteLine($"SetField ResourceGroups[{i}].Resource null");
                                        continue;
                                    }

                                    scriptWriter.WriteLine($"SetField ResourceGroups[{i}].Resource.Page.Index -1");
                                    scriptWriter.WriteLine($"SetField ResourceGroups[{i}].Resource ResourcesB \"tags\\{tagName}{(jmad.ResourceGroups.Count > 1 ? $"_group_{i}" : "_group")}.model_animation_tag_resource\"");
                                }

                                scriptWriter.WriteLine("SaveTagChanges");
                                scriptWriter.WriteLine("ExitTo tags");
                                scriptWriter.WriteLine();
                            }
                            break;

                        case ScenarioStructureBsp sbsp:
                            {
                                scriptWriter.WriteLine($"EditTag 0x{instance.Index:X4}");

                                var resourceFile = ExportResource(sbsp.Geometry.Resource, "render_geometry_api_resource_definition", "_decorator_geometry");

                                if (resourceFile == null)
                                {
                                    scriptWriter.WriteLine("SetField Geometry.Resource null");
                                }
                                else
                                {
                                    scriptWriter.WriteLine($"SetField Geometry.Resource.Page.Index -1");
                                    scriptWriter.WriteLine($"SetField Geometry.Resource ResourcesB \"tags\\{tagName}_decorator_geometry.render_geometry_api_resource_definition\"");
                                }

                                resourceFile = ExportResource(sbsp.Geometry2.Resource, "render_geometry_api_resource_definition", "_bsp_geometry");

                                if (resourceFile == null)
                                {
                                    scriptWriter.WriteLine("SetField Geometry2.Resource null");
                                }
                                else
                                {
                                    scriptWriter.WriteLine($"SetField Geometry2.Resource.Page.Index -1");
                                    scriptWriter.WriteLine($"SetField Geometry2.Resource ResourcesB \"tags\\{tagName}_bsp_geometry.render_geometry_api_resource_definition\"");
                                }

                                resourceFile = ExportResource(sbsp.CollisionBspResource, "structure_bsp_tag_resources", "_collision");

                                if (resourceFile == null)
                                {
                                    scriptWriter.WriteLine("SetField CollisionBspResource null");
                                }
                                else
                                {
                                    scriptWriter.WriteLine($"SetField CollisionBspResource.Page.Index -1");
                                    scriptWriter.WriteLine($"SetField CollisionBspResource ResourcesB \"tags\\{tagName}_collision.structure_bsp_tag_resources\"");
                                }

                                resourceFile = ExportResource(sbsp.PathfindingResource, "structure_bsp_cache_file_tag_resources", "_pathfinding");

                                if (resourceFile == null)
                                {
                                    scriptWriter.WriteLine("SetField PathfindingResource null");
                                }
                                else
                                {
                                    scriptWriter.WriteLine($"SetField PathfindingResource.Page.Index -1");
                                    scriptWriter.WriteLine($"SetField PathfindingResource ResourcesB \"tags\\{tagName}_pathfinding.structure_bsp_cache_file_tag_resources\"");
                                }

                                scriptWriter.WriteLine("SaveTagChanges");
                                scriptWriter.WriteLine("ExitTo tags");
                                scriptWriter.WriteLine();
                            }
                            break;

                        case ScenarioLightmapBspData sLdT:
                            {
                                scriptWriter.WriteLine($"EditTag 0x{instance.Index:X4}");

                                var resourceFile = ExportResource(sLdT.Geometry.Resource, "render_geometry_api_resource_definition", "_lightmap_geometry");

                                if (resourceFile == null)
                                {
                                    scriptWriter.WriteLine("SetField Geometry.Resource null");
                                }
                                else
                                {
                                    scriptWriter.WriteLine($"SetField Geometry.Resource.Page.Index -1");
                                    scriptWriter.WriteLine($"SetField Geometry.Resource ResourcesB \"tags\\{tagName}_lightmap_geometry.render_geometry_api_resource_definition\"");
                                }

                                scriptWriter.WriteLine("SaveTagChanges");
                                scriptWriter.WriteLine("ExitTo tags");
                                scriptWriter.WriteLine();
                            }
                            break;

                        case ParticleModel pmdf:
                            {
                                scriptWriter.WriteLine($"EditTag 0x{instance.Index:X4}");

                                var resourceFile = ExportResource(pmdf.Geometry.Resource, "render_geometry_api_resource_definition", "_particle_geometry");

                                if (resourceFile == null)
                                {
                                    scriptWriter.WriteLine("SetField Geometry.Resource null");
                                }
                                else
                                {
                                    scriptWriter.WriteLine($"SetField Geometry.Resource.Page.Index -1");
                                    scriptWriter.WriteLine($"SetField Geometry.Resource ResourcesB \"tags\\{tagName}_particle_geometry.render_geometry_api_resource_definition\"");
                                }

                                scriptWriter.WriteLine("SaveTagChanges");
                                scriptWriter.WriteLine("ExitTo tags");
                                scriptWriter.WriteLine();
                            }
                            break;

                        case Sound snd_:
                            {
                                scriptWriter.WriteLine($"EditTag 0x{instance.Index:X4}");

                                var resourceFile = ExportResource(snd_.Resource, "sound_resource");

                                if (resourceFile == null)
                                {
                                    scriptWriter.WriteLine("SetField Resource null");
                                }
                                else
                                {
                                    scriptWriter.WriteLine($"SetField Resource.Page.Index -1");
                                    scriptWriter.WriteLine($"SetField Resource ResourcesB \"tags\\{tagName}.sound_resource\"");
                                }

                                scriptWriter.WriteLine("SaveTagChanges");
                                scriptWriter.WriteLine("ExitTo tags");
                                scriptWriter.WriteLine();
                            }
                            break;
                    }
                    
                    completedTags.Add(instance.Index);
                }

                scriptWriter.WriteLine();
                scriptWriter.WriteLine("SaveTagNames");
                scriptWriter.WriteLine("UpdateMapFiles");
                scriptWriter.WriteLine("DumpLog");
                scriptWriter.WriteLine("Exit");
                scriptWriter.WriteLine();
            }

            return true;
        }
    }
}
