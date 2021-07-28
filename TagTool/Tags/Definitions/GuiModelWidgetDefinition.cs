using System;
using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Tags.GUI;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "gui_model_widget_definition", Tag = "mdl3", Size = 0x38, MaxVersion = CacheVersion.Halo3Retail)]
    [TagStructure(Name = "gui_model_widget_definition", Tag = "mdl3", Size = 0x84)]
    public class GuiModelWidgetDefinition : TagStructure
	{
        public ModelWidgetFlags Flags;
        public GuiDefinition GuiRenderBlock;
        public List<CameraRefinement> CameraControl;

        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public uint Unknown2;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public uint Unknown3;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public uint Unknown4;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public uint Unknown5;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public uint Unknown6;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public float FOV = 25.0f;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public uint Unknown8;

        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public List<UnknownBlock> ZoomFunction;

        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public ushort MovementLeft;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public ushort MovementRight;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public ushort MovementUp;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public ushort MovementDown;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public ushort Unknown14;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public ushort Unknown15;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public ushort ZoomIn;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public ushort ZoomOut;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public ushort RotateLeft;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public ushort RotateRight;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public ushort Unknown20;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public ushort Unknown21;

        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public List<TexCam> TextureCameraSections;

        [TagStructure(Size = 0x3C, MaxVersion = CacheVersion.Halo3Retail)]
        [TagStructure(Size = 0xA0, MinVersion = CacheVersion.Halo3ODST)]
        public class CameraRefinement : TagStructure
		{
            public StringId Biped2;
            public uint Unknown;
            public uint Unknown2;
            public uint Unknown3;
            public float Unknown4;
            public float BipedAngle; //[0 to 1]
            public uint Unknown6;
            [TagField(MaxVersion = CacheVersion.Halo3Retail)]
            public RealPoint2d BaseOffsetOld;
            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public RealPoint3d BaseOffsetNew;
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

            public List<ZoomData> ZoomData1;
            [TagField(MaxVersion = CacheVersion.Halo3Retail)]
            public List<ZoomData> ZoomData2;

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
    }

    [Flags]
    public enum ModelWidgetFlags : int
    {
        None = 0,
        Bit0 = 1 << 0,
        Bit1 = 1 << 1,
        Bit2 = 1 << 2,
        Bit3 = 1 << 3,
        Bit4 = 1 << 4,
        Bit5 = 1 << 5,
        Bit6 = 1 << 6,
        Bit7 = 1 << 7,
        Bit8 = 1 << 8,
        Bit9 = 1 << 9,
        Bit10 = 1 << 10,
        Bit11 = 1 << 11,
        Bit12 = 1 << 12,
        Bit13 = 1 << 13,
        Bit14 = 1 << 14,
        Bit15 = 1 << 15,
        Bit16 = 1 << 16,
        Bit17 = 1 << 17,
        Bit18 = 1 << 18,
        Bit19 = 1 << 19,
        Bit20 = 1 << 20,
        Bit21 = 1 << 21,
        Bit22 = 1 << 22,
        Bit23 = 1 << 23,
        Bit24 = 1 << 24,
        Bit25 = 1 << 25,
        Bit26 = 1 << 26,
        Bit27 = 1 << 27,
        Bit28 = 1 << 28,
        Bit29 = 1 << 29,
        Bit30 = 1 << 30,
        Bit31 = 1 << 31
    }
}
