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

        private ScenarioLightmap ConvertScenarioLightmap(Stream cacheStream, Dictionary<ResourceLocation, Stream> resourceStreams, string blamTagName, ScenarioLightmap scenarioLightmap)
        {
            if (BlamCache.Version > CacheVersion.Halo3Retail)
                return scenarioLightmap;

            scenarioLightmap.LightmapDataReferences = new List<ScenarioLightmap.LightmapDataReference>();

            foreach (var entry in scenarioLightmap.Lightmaps)
            {
                var wasReplacing = Flags.HasFlag(PortingFlags.Replace);

                Flags &= ~PortingFlags.Replace;
                var Lbsp = (ScenarioLightmapBspData)ConvertData(cacheStream, resourceStreams, entry, scenarioLightmap, blamTagName);
                if (wasReplacing) Flags |= PortingFlags.Replace;

                Lbsp.Airprobes = new List<ScenarioLightmap.Airprobe>();
                Lbsp.Airprobes.AddRange(scenarioLightmap.Airprobes);

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

                CacheContext.Serialize(cacheStream, edTag, Lbsp);

                scenarioLightmap.LightmapDataReferences.Add(new ScenarioLightmap.LightmapDataReference
                {
                    LightmapData = edTag
                });
            }

            scenarioLightmap.Airprobes.Clear();

            return scenarioLightmap;
        }

        private ScenarioLightmapBspData ConvertScenarioLightmapBspData(ScenarioLightmapBspData Lbsp)
        {
            //Test
            return Lbsp;
        }

        private CameraFxSettings ConvertCameraFxSettings(CameraFxSettings cfxs)
        {
            cfxs.Flags15 = CameraFxSettings.FlagsValue.Disable;
            cfxs.Flags16 = CameraFxSettings.FlagsValue.Disable;
            cfxs.Flags17 = CameraFxSettings.FlagsValue.Disable;

            //
            // Not sure if Flags17 at disable will ignore those values. Using default HO settings
            //

            cfxs.ColorR = 1.0f;
            cfxs.ColorG = 1.0f;
            cfxs.ColorB = 1.0f;

            cfxs.Unknown42 = 5000.0f;
            cfxs.Unknown43 = 0.0f;
            cfxs.Unknown44 = 0.5f;
            cfxs.Unknown45 = 2.0f;
            cfxs.Unknown46 = 1.0f;

            return cfxs;
        }
    }
}

