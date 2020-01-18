using System;
using System.Collections.Generic;
using System.IO;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Tags.Definitions;

namespace TagTool.Commands.Tags
{
    class ExportTagModCommand : Command
    {
        public GameCacheHaloOnline Cache { get; }

        public ExportTagModCommand(GameCacheHaloOnline cache) :
            base(false,

                "ExportTagMod",
                "Generates a TagTool script for the specified tag(s). See 'Help ExportTagMod' instructions.",

                "ExportTagMod <Name> <Directory> {Tag1, ..., TagN}",

                "Generates a TagTool script for the specified tag(s).\n" +
                "Any dependencies/resources are dealt with automatically.\n" +
                "\n" +
                "Instructions:\n" +
                "1. Enter the command. Example: 'ExportTagMod MyMod myModFolder\n" +
                "2. You can now specify the tags you want to be used, seperate each tag with a new line.\n" +
                "3. After you have entered all of your tags, press enter while on an empty line to start the process.\n" +
                "\n" +
                "Warning: Tags with a ton of dependencies will cause the command to take a long time to finish.")
        {
            Cache = cache;
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

            using (var cacheStream = Cache.TagCache.OpenTagCacheRead())
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
                                var instance = Cache.TagCacheGenHO.Tags[entry];

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

                Console.WriteLine("Please specify the tags to be used:");

                string line;

                while ((line = Console.ReadLine()) != "")
                {
                    if (!Cache.TryGetTag(line, out var instance))
                        continue;

                    LoadTagDependencies(instance.Index);
                }

                /*foreach (var index in tagIndices)
                {
                    var instance = CacheContext.GetTag(index);

                    if (instance == null || !instance.IsInGroup("bitm"))
                        continue;

                    var tagName = CacheContext.TagNames.ContainsKey(instance.Index) ?
                        instance.Name :
                        $"0x{instance.Index:X4}";

                    Console.WriteLine($"[Index: 0x{instance.Index:X4}, Offset: 0x{instance.HeaderOffset:X8}, Size: 0x{instance.TotalSize:X4}] {tagName}.{CacheContext.GetString(instance.Group.Name)}");
                }*/

                var importedTags = new HashSet<int>();

                foreach (var index in tagIndices)
                {
                    if (importedTags.Contains(index))
                        continue;

                    var instance = (CachedTagHaloOnline)Cache.TagCache.GetTag(index);

                    if (instance == null)
                        continue;

                    var tagName = instance.Name ?? $"0x{instance.Index:X4}";

                    var groupName = Cache.StringTable.GetString(instance.Group.Name);

                    var file = new FileInfo(Path.Combine(directory.FullName, $"tags\\{tagName}.{groupName}"));
                    var data = Cache.TagCacheGenHO.ExtractTagRaw(cacheStream, instance);

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

                    var instance = (CachedTagHaloOnline)Cache.TagCache.GetTag(index);

                    if (instance == null)
                        continue;

                    var tagName = instance.Name ?? $"0x{instance.Index:X4}";

                    var groupName = Cache.StringTable.GetString(instance.Group.Name);

                    var tagDefinition = Cache.Deserialize(cacheStream, instance);

                    FileInfo ExportResource(PageableResource pageable, string resourceGroup, string suffix = "")
                    {
                        if (!pageable.GetLocation(out var location))
                            return null;

                        var outFile = new FileInfo(Path.Combine(directory.FullName, $"tags\\{tagName}{suffix}.{resourceGroup}"));
                        var cache = Cache.ResourceCaches.GetResourceCache(location);

                        if (!outFile.Directory.Exists)
                            outFile.Directory.Create();

                        using (var stream = cache.File.OpenRead())
                        using (var outStream = outFile.Create())
                            cache.Cache.Decompress(stream, pageable.Page.Index, pageable.Page.CompressedBlockSize, outStream);

                        return outFile;
                    }

                    switch (tagDefinition)
                    {
                        case Bink bink:
                            {
                                scriptWriter.WriteLine($"EditTag 0x{instance.Index:X4}");

                                var resourceFile = ExportResource(bink.ResourceReference.HaloOnlinePageableResource, "bink_resource");

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
                                    var resourceFile = ExportResource(bitm.Resources[i].HaloOnlinePageableResource, "bitmap_texture_interop_resource", bitm.Resources.Count > 1 ? $"_image_{i}" : "_image");

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

                                var resourceFile = ExportResource(mode.Geometry.Resource.HaloOnlinePageableResource, "render_geometry_api_resource_definition", "_geometry");

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
                                    var resourceFile = ExportResource(jmad.ResourceGroups[i].ResourceReference.HaloOnlinePageableResource, "model_animation_tag_resource", jmad.ResourceGroups.Count > 1 ? $"_group_{i}" : "_group");

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

                                var resourceFile = ExportResource(sbsp.DecoratorGeometry.Resource.HaloOnlinePageableResource, "render_geometry_api_resource_definition", "_decorator_geometry");

                                if (resourceFile == null)
                                {
                                    scriptWriter.WriteLine("SetField Geometry.Resource null");
                                }
                                else
                                {
                                    scriptWriter.WriteLine($"SetField Geometry.Resource.Page.Index -1");
                                    scriptWriter.WriteLine($"SetField Geometry.Resource ResourcesB \"tags\\{tagName}_decorator_geometry.render_geometry_api_resource_definition\"");
                                }

                                resourceFile = ExportResource(sbsp.Geometry.Resource.HaloOnlinePageableResource, "render_geometry_api_resource_definition", "_bsp_geometry");

                                if (resourceFile == null)
                                {
                                    scriptWriter.WriteLine("SetField Geometry2.Resource null");
                                }
                                else
                                {
                                    scriptWriter.WriteLine($"SetField Geometry2.Resource.Page.Index -1");
                                    scriptWriter.WriteLine($"SetField Geometry2.Resource ResourcesB \"tags\\{tagName}_bsp_geometry.render_geometry_api_resource_definition\"");
                                }

                                resourceFile = ExportResource(sbsp.CollisionBspResource.HaloOnlinePageableResource, "structure_bsp_tag_resources", "_collision");

                                if (resourceFile == null)
                                {
                                    scriptWriter.WriteLine("SetField CollisionBspResource null");
                                }
                                else
                                {
                                    scriptWriter.WriteLine($"SetField CollisionBspResource.Page.Index -1");
                                    scriptWriter.WriteLine($"SetField CollisionBspResource ResourcesB \"tags\\{tagName}_collision.structure_bsp_tag_resources\"");
                                }

                                resourceFile = ExportResource(sbsp.PathfindingResource.HaloOnlinePageableResource, "structure_bsp_cache_file_tag_resources", "_pathfinding");

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

                                var resourceFile = ExportResource(sLdT.Geometry.Resource.HaloOnlinePageableResource, "render_geometry_api_resource_definition", "_lightmap_geometry");

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

                                var resourceFile = ExportResource(pmdf.Geometry.Resource.HaloOnlinePageableResource, "render_geometry_api_resource_definition", "_particle_geometry");

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

                                var resourceFile = ExportResource(snd_.Resource.HaloOnlinePageableResource, "sound_resource");

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

            Console.WriteLine("Done.");

            return true;
        }
    }
}