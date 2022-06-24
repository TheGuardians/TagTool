using TagTool.Tags.Definitions;
using LightGen2 = TagTool.Tags.Definitions.Gen2.Light;

namespace TagTool.Commands.Porting.Gen2
{
    partial class PortTagGen2Command : Command
    {
        public Light ConvertLight(LightGen2 gen2Light)
        {
            byte[] color = { 1, 52, 1, 0, 255, 204, 192, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 255, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            byte[] intensity = { 1, 52, 0, 0, 0, 0, 128, 64, 0, 0, 128, 64, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            var light = new Light
            {
                MaximumDistance = gen2Light.CutoffDistance,
                FrustumNearWidth = gen2Light.NearWidth,
                FrustumHeightScale = gen2Light.HeightStretch,
                FrustumFieldOfView = gen2Light.FieldOfView,
                GelBitmap = gen2Light.GelMap,
                Color = new Light.LightColorFunctionStruct(),
                Intensity = new Light.LightScalarFunctionStruct(),
                DistanceDiffusion = gen2Light.FalloffDistance,
                AngularSmoothness = (float)0.5,
                PercentSpherical = (float)0.5,
                DestroyLightAfter = gen2Light.Duration,
                NearPriority = Light.LightPriorityEnumeration.Default,
                FarPriority = Light.LightPriorityEnumeration.Default,
                TransitionDistance = Light.LightPriorityBiasEnumeration.Default,
                LensFlare = gen2Light.LensFlare
            };

            if (gen2Light.Type == LightGen2.TypeValue.Sphere)
            {
                light.Type = Light.TypeValue.Sphere;
                light.MaximumDistance = gen2Light.Radius;
            }
            else
            {
                light.Type = Light.TypeValue.Frustum;
            }

            // If the light is dynamic setup dynamic flags in light tag, if it's lightmap only disable the 
            if (gen2Light.DefaultLightmapSetting != LightGen2.DefaultLightmapSettingValue.LightmapsOnly)
            {
                light.Flags |= Light.LightFlags.AllowShadowsAndGels;
                light.Flags |= Light.LightFlags.RenderWhileActiveCamo;
            }
            else
            {
                light.MaximumDistance = 0;
            }

            // names in definitions are different so if statement for each flag that matters
            if (!gen2Light.Flags.HasFlag(LightGen2.FlagsValue.NoShadow))
            {
                light.Flags |= Light.LightFlags.ShadowCasting;
            }

            if (gen2Light.Flags.HasFlag(LightGen2.FlagsValue.OnlyRenderInFirstPerson))
            {
                light.Flags |= Light.LightFlags.RenderFirstPersonOnly;
            }

            if (gen2Light.Flags.HasFlag(LightGen2.FlagsValue.OnlyRenderInThirdPerson))
            {
                light.Flags |= Light.LightFlags.RenderThirdPersonOnly;
            }

            if (gen2Light.Flags.HasFlag(LightGen2.FlagsValue.MultiplayerOverride))
            {
                light.Flags |= Light.LightFlags.RenderInMultiplayerOverride;
            }

            if (gen2Light.Flags.HasFlag(LightGen2.FlagsValue.FirstPersonFromCamera))
            {
                light.Flags |= Light.LightFlags.MoveToCameraInFirstPerson;
            }

            if (gen2Light.Flags.HasFlag(LightGen2.FlagsValue.LightFramerateKiller))
            {
                light.Flags |= Light.LightFlags.NeverPriorityCull;
            }

            //TODO Light Color and Intensity Functions
            light.Color.Function.Data = color;
            light.Intensity.Function.Data = intensity;

            return light;
        }
    }
}
