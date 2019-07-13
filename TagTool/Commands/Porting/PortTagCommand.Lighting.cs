using TagTool.Cache;
using TagTool.Common;
using TagTool.IO;
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

            for(int i = 0; i< scenarioLightmap.Lightmaps.Count; i++)
            {
                var entry = scenarioLightmap.Lightmaps[i];

                var wasReplacing = FlagIsSet(PortingFlags.Replace);

                RemoveFlags(PortingFlags.Replace);
                var Lbsp = ConvertStructure(cacheStream, resourceStreams, entry, scenarioLightmap, blamTagName);
                if (wasReplacing)
                    SetFlags(PortingFlags.Replace);

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
                edTag.Name = $"blamTagName_{i}_data";

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

        private CameraFxSettings ConvertCameraFxSettings(CameraFxSettings cfxs, string blamTagName)
        {
            cfxs.Flags15 = CameraFxSettings.FlagsValue.UseDefault;
            cfxs.Flags16 = CameraFxSettings.FlagsValue.UseDefault;

            switch (blamTagName)
            {
                // citadel godrays
                case @"levels\dlc\fortress\fortress_fx":
                    cfxs.GodraysProperties = new CameraFxSettings.GodraysPropertiesBlock
                    {
                        Flags = CameraFxSettings.FlagsValue.MaximumChangeIsRelative | CameraFxSettings.FlagsValue.Fixed2,
                        Radius = 57.0f,
                        AngleBias = 125.0f,
                        Color = new RealRgbColor(0.9882353f, 0.9372549f, 0.6039216f),
                        Strength = 5000.0f,
                        PowerExponent = 0.9f,
                        BlurSharpness = 1.0f,
                        DecoratorDarkening = 0.7f,
                        HemiRejectionFalloff = 4.0f
                    };
                    break;

                // rat's nest godrays
                case @"levels\dlc\armory\sky\armory_camera":
                    cfxs.GodraysProperties = new CameraFxSettings.GodraysPropertiesBlock
                    {
                        Flags = CameraFxSettings.FlagsValue.MaximumChangeIsRelative | CameraFxSettings.FlagsValue.Fixed2,
                        Radius = 67.5f,
                        AngleBias = -156.0f,
                        Color = new RealRgbColor(1f, 0.7333333f, 0.4745098f),
                        Strength = 5000.0f,
                        PowerExponent = 0.6f,
                        BlurSharpness = 0.08f,
                        DecoratorDarkening = 1.0f,
                        HemiRejectionFalloff = 8.0f
                    };
                    break;

                default:
                    cfxs.GodraysProperties = new CameraFxSettings.GodraysPropertiesBlock
                    {
                        Flags = CameraFxSettings.FlagsValue.UseDefault,
                        Radius = 0.0f,
                        AngleBias = 0.0f,
                        Color = new RealRgbColor(1.0f, 1.0f, 1.0f),
                        Strength = 5000.0f,
                        PowerExponent = 0.0f,
                        BlurSharpness = 0.5f,
                        DecoratorDarkening = 2.0f,
                        HemiRejectionFalloff = 1.0f
                    };
                    break;
            }

            return cfxs;
        }
    }
}

