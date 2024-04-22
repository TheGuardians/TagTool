using System;
using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Tags.Definitions.Common;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "gui_model_widget_definition", Tag = "mdl3", Size = 0x38, MaxVersion = CacheVersion.Halo3Retail)]
    [TagStructure(Name = "gui_model_widget_definition", Tag = "mdl3", Size = 0x84)]
    public class GuiModelWidgetDefinition : TagStructure
	{
        public ModelWidgetFlags Flags;
        public GuiDefinition GuiRenderBlock;
        public List<CameraSettingsBlock> CameraSettings;

        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public ModelWidgetGlobalsDefinition ModelWidgetGlobals;

        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public List<ModelWidgetCameraSliceBlock> TextureCameraSlices;

        [Flags]
        public enum ModelWidgetFlags : int
        {
            DoNotApplyOldContentUpscaling = 1 << 0,
            OverrideTemplateFlags = 1 << 1,
            EnableAnimationDebugging = 1 << 2,
            AllowListItemToOverrideAnimationSkin = 1 << 3
        }

        [TagStructure(Size = 0x40, MinVersion = CacheVersion.Halo3ODST)]
        public class ModelWidgetGlobalsDefinition : TagStructure
        {
            public RealArgbColor TronShaderColor;
            public float TronShaderIntensity;
            public float FOV; // degrees
            public float ZoomSpeed; // wu per tick
            public List<KeyframeTransitionFunctionBlock> ZoomTransitionFunction;
            public GamepadButtonDefinition MovementLeft;
            public GamepadButtonDefinition MovementRight;
            public GamepadButtonDefinition MovementUp;
            public GamepadButtonDefinition MovementDown;
            public GamepadButtonDefinition Unknown16;
            public GamepadButtonDefinition Unknown17;
            public GamepadButtonDefinition ZoomIn;
            public GamepadButtonDefinition ZoomOut;
            public GamepadButtonDefinition RotateLeft;
            public GamepadButtonDefinition RotateRight;
            public GamepadButtonDefinition RotateUp;
            public GamepadButtonDefinition RotateDown;
        }

        [TagStructure(Size = 0x3C, MaxVersion = CacheVersion.Halo3Retail)]
        [TagStructure(Size = 0xA0, MinVersion = CacheVersion.Halo3ODST)]
        public class CameraSettingsBlock : TagStructure
		{
            public StringId Name;

            [TagField(MaxVersion = CacheVersion.Halo3Retail)]
            public float Fov;
            [TagField(MaxVersion = CacheVersion.Halo3Retail)]
            public float InitialRadialOffset;
            [TagField(MaxVersion = CacheVersion.Halo3Retail)]
            public float FinalRadialOffset;
            [TagField(MaxVersion = CacheVersion.Halo3Retail)]
            public float CameraRadialStepSize;
            [TagField(MaxVersion = CacheVersion.Halo3Retail)]
            public float InitialVerticalOffset;
            [TagField(MaxVersion = CacheVersion.Halo3Retail)]
            public float FinalVerticalOffset;
            [TagField(MaxVersion = CacheVersion.Halo3Retail)]
            public float CameraVerticalStepSize;
            [TagField(MaxVersion = CacheVersion.Halo3Retail)]
            public float CameraRotationalStep;

            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public RealPoint3d ModelWorldPosition; // arbitrary location in the world to place the model (wu)
            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public RealPoint3d MinimumWorldPosition;
            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public RealPoint3d MaximumWorldPosition;
            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public RealPoint3d MinimumCameraOffset; // wu
            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public RealPoint3d MinimumCameraFocalOffset; // wu
            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public RealPoint3d MaximumCameraOffset; // wu
            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public RealPoint3d MaximumCameraFocalOffset; // wu

            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public float InitialZoom; // [0,1]
            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public float MovementSpeed;
            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public float MagnetismConstant;

            public List<KeyframeTransitionFunctionBlock> RadialTransitionFxn; // MovementScaleFxn?

            [TagField(MaxVersion = CacheVersion.Halo3Retail)]
            public List<KeyframeTransitionFunctionBlock> VerticalTransitionFxn;

            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public RealEulerAngles2d InitialRotation; // degrees
            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public RealEulerAngles2d MinimumRotation; // degrees
            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public RealEulerAngles2d MaximumRotation; // degrees
            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public float RotationSpeed;
            [TagField(MinVersion = CacheVersion.Halo3ODST, ValidTags = new[] { "obje", "scen" })]
            public CachedTag Model;
            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public StringId Variant;
        }

        [TagStructure(Size = 0x14)]
        public class KeyframeTransitionFunctionBlock : TagStructure
        {
            public TagFunction CustomFunction;
        }

        [TagStructure(Size = 0x14)]
        public class UnknownBlock : TagStructure
		{
            public TagFunction Unknown;
        }

        [TagStructure(Size = 0x14)]
        public class TexCam : TagStructure
		{
            public StringId Name;
            public Bounds<float> XBounds;   
            public Bounds<float> YBounds;
        }

        public enum GamepadButtonDefinition : short
        {
            LeftTrigger,
            RightTrigger,
            DpadUp,
            DpadDown,
            DpadLeft,
            DpadRight,
            Start,
            Back,
            LeftThumb,
            RightThumb,
            ButtonA,
            ButtonB,
            ButtonX,
            ButtonY,
            LeftBumper,
            RightBumper,
            LeftStickLeft,
            LeftStickRight,
            LeftStickUp,
            LeftStickDown,
            RightStickLeft,
            RightStickRight,
            RightStickUp,
            RightStickDown,
            Unknown
        }

        [TagStructure(Size = 0x14)]
        public class ModelWidgetCameraSliceBlock : TagStructure
        {
            public StringId Name; // use empty name for default
            public float Left;
            public float Right;
            public float Top;
            public float Bottom;
        }
    }
}
