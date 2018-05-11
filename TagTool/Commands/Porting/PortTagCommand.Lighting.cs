using TagTool.Cache;
using TagTool.Common;
using TagTool.IO;
using TagTool.Serialization;
using TagTool.Tags.Definitions;
using System;
using System.Collections.Generic;
using System.IO;
using TagTool.Tags;

namespace TagTool.Commands.Porting
{
    partial class PortTagCommand
    {
        private LensFlare ConvertLensFlare(LensFlare lensFlare)
        {
            lensFlare.OcclusionReflectionIndex = 0;

            foreach (var reflection in lensFlare.Reflections)
            {
                reflection.RotationOffset_HO = reflection.RotationOffset_H3;
                reflection.TintModulationFactor_HO = reflection.TintModulationFactor_H3;
                reflection.Unknown2 = 0;
                reflection.BitmapOverride = null;

                var radius = new byte[52];
                var scale = new byte[52];
                var brightness = new byte[52];

                //
                // Radius
                //

                using (var writer = new EndianWriter(new MemoryStream(radius), EndianFormat.LittleEndian))
                {
                    writer.Format = EndianFormat.BigEndian;
                    writer.Write(0x08340000);
                    writer.Format = EndianFormat.LittleEndian;
                    byte[] value1 = BitConverter.GetBytes(reflection.RadiusBounds.Lower);
                    byte[] value2 = BitConverter.GetBytes(reflection.RadiusBounds.Upper);
                    writer.Write(value1);
                    writer.Write(value2);
                    writer.Write(0x0);
                    writer.Write(0x0);

                    writer.Format = EndianFormat.BigEndian;
                    writer.Write(0x0);
                    writer.Write(0x0);
                    writer.Write(0x14000000);
                    writer.Write(0x01000000);

                    writer.Write(0x040000CD);
                    writer.Write(0xFFFF7F7F);
                    writer.Write(0x0000803F);
                    writer.Write(0x0);
                }
                reflection.RadiusCurveFunction = new TagFunction
                {
                    Data = radius,
                };

                //
                // Brightness
                //

                using (var writer = new EndianWriter(new MemoryStream(brightness), EndianFormat.LittleEndian))
                {
                    writer.Format = EndianFormat.BigEndian;
                    writer.Write(0x08340000);
                    writer.Format = EndianFormat.LittleEndian;
                    byte[] value1 = BitConverter.GetBytes(reflection.BrightnessBounds.Lower);
                    byte[] value2 = BitConverter.GetBytes(reflection.BrightnessBounds.Upper);
                    writer.Write(value1);
                    writer.Write(value2);
                    writer.Write(0x0);
                    writer.Write(0x0);

                    writer.Format = EndianFormat.BigEndian;
                    writer.Write(0x0);
                    writer.Write(0x0);
                    writer.Write(0x14000000);
                    writer.Write(0x01000000);

                    writer.Write(0x040000CD);
                    writer.Write(0xFFFF7F7F);
                    writer.Write(0x0000803F);
                    writer.Write(0x0);
                }

                reflection.BrightnessCurveFunction = new TagFunction
                {
                    Data = brightness,
                };

                //
                // Scale
                //

                using (EndianWriter function = new EndianWriter(new MemoryStream(scale), EndianFormat.BigEndian))
                {
                    function.Write(0x08340000);
                    function.Write(0x0);
                    function.Write(0x0000803F);
                    function.Write(0x0);
                    function.Write(0x0);

                    function.Write(0x0);
                    function.Write(0x0);
                    function.Write(0x14000000);
                    function.Write(0x01000000);

                    function.Write(0x040000CD);
                    function.Write(0xFFFF7F7F);
                    function.Write(0x0000803F);
                    function.Write(0x0);
                }

                reflection.ScaleCurveXFunction = new TagFunction
                {
                    Data = scale,
                };

                reflection.ScaleCurveYFunction = new TagFunction
                {
                    Data = scale,
                };
            }

            return lensFlare;
        }

        private ScenarioLightmap ConvertScenarioLightmap(Stream cacheStream, string blamTagName, ScenarioLightmap scenarioLightmap)
        {
            if (BlamCache.Version > CacheVersion.Halo3Retail)
                return scenarioLightmap;

            scenarioLightmap.LightmapDataReferences = new List<ScenarioLightmap.LightmapDataReference>();

            foreach (var entry in scenarioLightmap.Lightmaps)
            {
                var wasReplacing = IsReplacing;

                IsReplacing = false;
                var Lbsp = (ScenarioLightmapBspData)ConvertData(cacheStream, entry, scenarioLightmap, blamTagName);
                IsReplacing = wasReplacing;

                CachedTagInstance edTag = null;
                TagGroup edGroup = null;

                var groupTag = new Tag("Lbsp");

                if (TagGroup.Instances.ContainsKey(groupTag))
                {
                    edGroup = TagGroup.Instances[groupTag];
                }
                else
                {
                    edGroup = new TagGroup(groupTag, Tag.Null, Tag.Null, CacheContext.GetStringId("scenario_lightmap_bsp_data"));
                }

                edTag = CacheContext.TagCache.AllocateTag(edGroup);
                CacheContext.TagNames[edTag.Index] = blamTagName + "_data";

                var edContext = new TagSerializationContext(cacheStream, CacheContext, edTag);
                CacheContext.Serializer.Serialize(edContext, Lbsp);

                scenarioLightmap.LightmapDataReferences.Add(new ScenarioLightmap.LightmapDataReference
                {
                    LightmapData = edTag
                });
            }

            return scenarioLightmap;
        }

        private ScenarioLightmapBspData ConvertScenarionLightmapBspData(ScenarioLightmapBspData Lbsp)
        {
            //Test
            return Lbsp;
        }

        private CortanaEffectDefinition ConvertCortanaEffect(CortanaEffectDefinition crte)
        {
            foreach(var block in crte.Unknown2)
            {
                block.Unknown3 = new CortanaEffectDefinition.UnknownBlock1.UnknownObject();

                if (block.Unknown1.Count > 0)
                    block.Unknown3.Unknown1 = block.Unknown1[0];
                else
                    block.Unknown3.Unknown1 = null;

                if (block.Unknown2.Count > 0)
                    block.Unknown3.Unknown2 = block.Unknown2[0];
                else
                    block.Unknown3.Unknown2 = null;
            }

            return crte;
        }
    }
}

