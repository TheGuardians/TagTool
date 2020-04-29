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
    class ConvertGeometryCommand : Command
    {
        private GameCache CacheContext { get; }
        private Scenario Scnr { get; }

        public ConvertGeometryCommand(GameCache cacheContext, Scenario scnr) :
            base(false,

                "ConvertGeometry",
                "Convert Geometry to forge objects",

                "ConvertGeometry",

                "Convert Geometry to forge objects")
        {
            CacheContext = cacheContext;
            Scnr = scnr;
        }

        public override object Execute(List<string> args)
        {
            bool iscluster = false;
            if (args.Count > 0 && args[0] == "cluster")
            {
                iscluster = true;
            }

            using (var stream = CacheContext.OpenCacheReadWrite())
            {
                for (var sbspindex = 0; sbspindex < Scnr.StructureBsps.Count; sbspindex++)
                {
                    var sbsp = (ScenarioStructureBsp)CacheContext.Deserialize(stream, Scnr.StructureBsps[sbspindex].StructureBsp);

                    var converter = new GeometryToObjectConverter((GameCacheHaloOnlineBase)CacheContext, stream, CacheContext, stream, Scnr, sbspindex);
                    
                    var converted = new HashSet<short>();

                    var loopcounter = iscluster ? sbsp.Clusters.Count : sbsp.InstancedGeometryInstances.Count;

                    for (int geometryIndex = 0; geometryIndex  < loopcounter; geometryIndex++)
                    {
                        var meshindex = iscluster ? sbsp.Clusters[geometryIndex].MeshIndex : sbsp.InstancedGeometryInstances[geometryIndex].MeshIndex;
                        if (converted.Contains(meshindex))
                            continue;
                        converted.Add(meshindex);
                        //strip digits from the end of the instancedgeometry name
                        //string instancedgeoname = CacheContext.StringTable.GetString(InstancedGeometryBlock.Name);
                        //var digits = new[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
                        //var instancedgeoname = tempname.TrimEnd(digits);

                        //string NewName = $"objects\\reforge\\instanced_geometry\\{currentmeshindex}";

                        var objectTag = converter.ConvertGeometry(geometryIndex, null, iscluster);

                        //if sbsp resource is null this tag will return null, and we skip to the next bsp
                        if (objectTag == null)
                            break;

                        var instanceName = $"geometry_{geometryIndex:000}";
                        Console.WriteLine($"Converting geometry '{instanceName}'...");

                        //add new object to forge globals
                        CachedTag forgeglobal = CacheContext.TagCache.GetTag<ForgeGlobalsDefinition>(@"multiplayer\forge_globals");
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
