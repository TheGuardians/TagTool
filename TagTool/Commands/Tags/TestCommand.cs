using TagTool.Cache;
using TagTool.IO;
using System.Collections.Generic;
using System.IO;
using System;
using TagTool.Common;
using TagTool.Tags.Definitions;
using TagTool.Tags;
using TagTool.Serialization;
using TagTool.Bitmaps;
using TagTool.Tags.Resources;
using TagTool.Bitmaps.Utils;
using TagTool.Bitmaps.DDS;
using TagTool.Geometry;
using TagTool.BlamFile;
using TagTool.Tags.Definitions.Gen1;
using TagTool.Cache.HaloOnline;
using TagTool.Havok;

namespace TagTool.Commands
{
    
    public class TestCommand : Command
    {
        GameCache Cache;

        public TestCommand(GameCache cache) : base(false, "Test", "Test", "Test", "Test")
        {
            Cache = cache;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count > 0)
                return false;

            
            var mapFilesFolder = new DirectoryInfo(@"D:\Halo\Maps\Halo3");
            var outDir = new DirectoryInfo("CacheTest");
            if (!outDir.Exists)
                outDir.Create();

            var mapFiles = new List<FileInfo>() { new FileInfo(@"D:\halo online test\maps\tags.dat")};
            //mapFilesFolder.GetFiles("*.map"); // 
            //
            // Insert what test command you want below
            //

            //var cache = GameCache.Open(new FileInfo(@"D:\halo\maps\halo3\100_citadel.map"));

            foreach (var mapFile in mapFiles)
            {
                var cache = GameCache.Open(mapFile);
                Console.WriteLine(cache.DisplayName);
                using (var stream = cache.OpenCacheRead())
                {
                    foreach (var tag in cache.TagCache.TagTable)
                    {
                        
                        if (tag.IsInGroup("phmo"))
                        {
                            //Console.WriteLine($"{tag.Name}.phmo");
                            var phmo = cache.Deserialize<PhysicsModel>(stream, tag);

                            foreach(var mopp in phmo.Mopps)
                            {
                                var test = 0;
                            }

                            byte[] result = new byte[phmo.MoppCodes.Length];

                            using (var inputReader = new EndianReader(new MemoryStream(phmo.MoppCodes), CacheVersionDetection.IsLittleEndian(cache.Version) ? EndianFormat.LittleEndian : EndianFormat.BigEndian))
                            {
                                var dataContext = new DataSerializationContext(inputReader);

                                for (int i = 0; i < phmo.Mopps.Count; i++)
                                {
                                    var header = cache.Deserializer.Deserialize<HkpMoppCode>(dataContext); 

                                }

                                phmo.MoppCodes = result;
                            }
                        }

                        if (tag.IsInGroup("sLdT"))
                        {
                            var sLdT = cache.Deserialize<ScenarioLightmap>(stream, tag);
                            //Console.WriteLine($"{tag.Name}.sLdT");
                            if(sLdT.Lightmaps != null)
                            {
                                foreach (var lightmap in sLdT.Lightmaps)
                                {
                                    var geometry = lightmap.Geometry;

                                    foreach (var unknown in geometry.Unknown2)
                                    {
                                        //Console.WriteLine($"{unknown.UnknownByte1}, {unknown.UnknownByte2}, {unknown.Unknown2}, {unknown.Unknown3.Length}");
                                    }
                                }
                            }
                            
                        }

                        if (tag.IsInGroup("Lbsp"))
                        {
                            var lbsp = cache.Deserialize<ScenarioLightmapBspData>(stream, tag);
                            //Console.WriteLine($"{tag.Name}.Lbsp");
                            var geometry = lbsp.Geometry;

                            foreach (var unknown in geometry.Unknown2)
                            {
                                //Console.WriteLine($"{unknown.UnknownByte1}, {unknown.UnknownByte2}, {unknown.Unknown2}, {unknown.Unknown3.Length}");
                            }

                        }

                    }
                }
            }

            
            





            return true;
        }

        public void PrintStringID(int set, int index, int tableIndex, string str, StringIDType type)
        {
            Console.WriteLine($"{tableIndex:X8}, {set:X2}, {index:X4}, {type.ToString()}, {str}");
        }

        public void PrintEnum(int index, string str)
        {
            Console.WriteLine($"string_id_{str} = 0x{index:X4},");
        }
    }



}

