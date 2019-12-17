using TagTool.Cache;
using TagTool.Common;
using System.Collections.Generic;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "gui_model_widget_definition", Tag = "mdl3", Size = 0x38, MaxVersion = CacheVersion.Halo3Retail)]
    [TagStructure(Name = "gui_model_widget_definition", Tag = "mdl3", Size = 0x84)]
    public class GuiModelWidgetDefinition : TagStructure
	{
        public uint Flags;
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
            public CachedTagInstance Unknown33;
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
}
