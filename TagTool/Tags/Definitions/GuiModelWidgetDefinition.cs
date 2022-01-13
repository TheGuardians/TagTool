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
        public ModelWidgetData ModelData;

        [Flags]
        public enum ModelWidgetFlags : int
        {
            DoNotApplyOldContentUpscaling = 1 << 0,
            OverrideTemplateFlags = 1 << 1,
            EnableAnimationDebugging = 1 << 2,
            UNUSED = 1 << 3
        }

        [TagStructure(Size = 0x4C, MinVersion = CacheVersion.Halo3ODST)]
        public class ModelWidgetData : TagStructure
        {
            public uint Unknown4;
            public uint Unknown5;
            public uint Unknown6;
            public uint Unknown7;
            public uint Unknown8;
            public float FOV;
            public uint Unknown10;
            public List<ZoomFunctionBlock> ZoomFunction;
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
            public GamepadButtonDefinition Unknown22;
            public GamepadButtonDefinition Unknown23;
            public List<TexCamBlock> TextureCameraSections;

            [TagStructure(Size = 0x14)]
            public class TexCamBlock : TagStructure
            {
                public StringId Name;
                public Bounds<float> BoundsX;
                public Bounds<float> BoundsY;
            }

            [TagStructure(Size = 0x14)]
            public class ZoomFunctionBlock : TagStructure
            {
                public TagFunction Function;
            }
        }

        [TagStructure(Size = 0x3C, MaxVersion = CacheVersion.Halo3Retail)]
        [TagStructure(Size = 0xA0, MinVersion = CacheVersion.Halo3ODST)]
        public class CameraSettingsBlock : TagStructure
		{
            public StringId Name;
            public float Fov;
            public float InitialRadialOffset;
            public float FinalRadialOffset;
            public float CameraRadialStepSize;
            public float InitialVerticalOffset;
            public float FinalVerticalOffset;
            public float CameraVerticalStepSize;
            public float CameraRotationalStep;

            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public float BaseOffsetNew;
            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public RealVector3d Unknown10;
            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public RealVector3d Unknown13;
            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public RealVector3d Unknown16;
            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public RealVector3d Unknown19;
            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public RealVector3d Unknown22;

            public List<ZoomData> RadialTransitionFxn;
            [TagField(MaxVersion = CacheVersion.Halo3Retail)]
            public List<ZoomData> VerticalTransitionFxn;

            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public uint Unknown26;
            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public uint Unknown27;
            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public Angle Unknown28;
            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public uint Unknown29;
            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public Angle Unknown30;
            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public uint Unknown31;
            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public uint Unknown32;
            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public CachedTag Unknown33;
            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public uint Unknown34;

            [TagStructure(Size = 0x14)]
            public class ZoomData : TagStructure
			{
                public TagFunction Unknown;
            }
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
    }
}
