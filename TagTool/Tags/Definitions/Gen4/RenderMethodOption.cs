using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "render_method_option", Tag = "rmop", Size = 0xC)]
    public class RenderMethodOption : TagStructure
    {
        public List<RenderMethodOptionParameterBlock> Parameters;
        
        [TagStructure(Size = 0x60)]
        public class RenderMethodOptionParameterBlock : TagStructure
        {
            public StringId ParameterName;
            public StringId ParameterUiOverrideName;
            public RenderMethodParameterTypeEnum ParameterType;
            public RenderMethodExternEnum SourceExtern;
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag DefaultBitmap;
            public float DefaultRealValue;
            public int DefaultIntBoolValue;
            public ushort Flags;
            public RenderMethodBitmapFilterModeEnum DefaultFilterMode;
            public RenderMethodBitmapAddressModeEnum DefaultAddressMode;
            public ushort AnisotropyAmount;
            public ArgbColor DefaultColor;
            public float DefaultBitmapScale;
            public UiAndUsageFlags UsageFlags;
            public FunctionTypeEnum ForceFunctionType;
            public ColorGraphTypeEnum ForceFunctionColorCount;
            public float SuggestedRealMin;
            public float SuggestedRealMax;
            public int TicksFromMinToMax;
            public byte[] HelpText;
            
            public enum RenderMethodParameterTypeEnum : int
            {
                Bitmap,
                Color,
                Real,
                Int,
                Bool,
                ArgbColor
            }
            
            public enum RenderMethodExternEnum : int
            {
                None,
                TexaccumTarget,
                NormalTarget,
                ZTarget,
                ShadowMask,
                Shadow1Target,
                Shadow2Target,
                Shadow3Target,
                Shadow4Target,
                TextureCameraTarget,
                ReflectionTarget,
                RefractionTarget,
                DualvmfDirectionPs,
                DualvmfIntensityPs,
                DualvmfDirectionVs,
                DualvmfIntensityVs,
                GelTextureOfAnalyticalLight,
                Unused1,
                Unused2,
                ChangeColorPrimary,
                ChangeColorSecondary,
                ChangeColorTertiary,
                ChangeColorQuaternary,
                EmblemColorBackground,
                EmblemColorPrimary,
                EmblemColorSecondary,
                DynamicEnvironmentMap1,
                DynamicEnvironmentMap2,
                CookTorranceArray,
                VmfDiffuseTable,
                VmfDiffuseTableVs,
                DirectionLut,
                ZonalRotationTable,
                PhongSpecularTable,
                DiffusePowerSpecularTable,
                LightDir0,
                LightColor0,
                LightDir1,
                LightColor1,
                LightDir2,
                LightColor2,
                LightDir3,
                LightColor3,
                Unused3,
                Unused4,
                Unused5,
                DynamicLightGel0,
                FlatEnvmapMatrixX,
                FlatEnvmapMatrixY,
                FlatEnvmapMatrixZ,
                DebugTint,
                ScreenConstants,
                ActiveCamoDistortionTexture,
                SceneLdrTexture,
                WaterMemexportAddr,
                TreeAnimationTimer,
                DepthConstants,
                CameraForward,
                WrinkleWeightsA,
                WrinkleWeightsB,
                StaticLightingPrevis
            }
            
            public enum RenderMethodBitmapFilterModeEnum : short
            {
                Trilinear,
                Point,
                Bilinear,
                Unused0,
                Anisotropic,
                Unused1,
                Anisotropic1,
                LightprobeTextureArray,
                TextureArrayQuadlinear,
                TextureArrayQuadanisotropic
            }
            
            public enum RenderMethodBitmapAddressModeEnum : short
            {
                Wrap,
                Clamp,
                Mirror,
                BlackBorder,
                Mirroronce,
                MirroronceBorder
            }
            
            [Flags]
            public enum UiAndUsageFlags : uint
            {
                UseForceFunctionType = 1 << 0,
                UseForceFunctionColorCount = 1 << 1,
                ParameterInvisibleInUi = 1 << 2,
                LockFunctionValue = 1 << 3
            }
            
            public enum FunctionTypeEnum : short
            {
                Identity,
                Constant,
                Transition,
                Periodic,
                Linear,
                LinearKey,
                MultiLinearKey,
                Spline,
                MultiSpline,
                Exponent,
                Spline2
            }
            
            public enum ColorGraphTypeEnum : short
            {
                Scalar,
                Constant,
                _2Color,
                _3Color,
                _4Color
            }
        }
    }
}
