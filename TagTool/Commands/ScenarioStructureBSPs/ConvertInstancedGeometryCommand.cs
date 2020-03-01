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
using TagTool.Geometry.Utils;

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
                    var sbsp = (ScenarioStructureBsp)CacheContext.Deserialize(stream, Scnr.StructureBsps[sbspindex].StructureBsp);

                    var converter = new InstancedGeometryToObjectConverter((GameCacheHaloOnlineBase)CacheContext, stream, CacheContext, stream, Scnr, sbspindex);
                    
                    var converted = new HashSet<short>();
                    for (int instanceIndex = 0; instanceIndex  < sbsp.InstancedGeometryInstances.Count; instanceIndex++)
                    {
                        var instance = sbsp.InstancedGeometryInstances[instanceIndex];
                        if (converted.Contains(instance.MeshIndex))
                            continue;
                        converted.Add(instance.MeshIndex);
                        //strip digits from the end of the instancedgeometry name
                        //string instancedgeoname = CacheContext.StringTable.GetString(InstancedGeometryBlock.Name);
                        //var digits = new[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
                        //var instancedgeoname = tempname.TrimEnd(digits);

                        //string NewName = $"objects\\reforge\\instanced_geometry\\{currentmeshindex}";

                        var objectTag = converter.ConvertInstance(instanceIndex);

                        //if sbsp resource is null this tag will return null, and we skip to the next bsp
                        if (objectTag == null)
                            break;

                        var instanceName = "";
                        if (instance.Name != StringId.Invalid)
                            instanceName = CacheContext.StringTable.GetString(instance.Name);
                        else
                            instanceName = $"instance_{instanceIndex:000}";

                        Console.WriteLine($"Converting instance '{instanceName}'...");

                        //add new object to forge globals
                        CachedTag forgeglobal = CacheContext.GetTag<ForgeGlobalsDefinition>(@"multiplayer\forge_globals");
                        var tmpforg = (ForgeGlobalsDefinition)CacheContext.Deserialize(stream, forgeglobal);
                        tmpforg.Palette.Add(new ForgeGlobalsDefinition.PaletteItem
                        {
                            Name = Path.GetFileName(objectTag.Name),
                            Type = ForgeGlobalsDefinition.PaletteItemType.Structure,
                            CategoryIndex = 64,
                            DescriptionIndex = -1,
                            MaxAllowed = 0,
                            Object = objectTag
                        });
                        CacheContext.Serialize(stream, forgeglobal, tmpforg);
                        CacheContext.SaveStrings();
                    }
                }

                if (CacheContext is GameCacheHaloOnlineBase hoCache)
                    hoCache.SaveTagNames();
            }

            Console.WriteLine("Done!");

            return true;
        }
    }
}
