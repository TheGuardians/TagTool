using TagTool.Cache;
using TagTool.Common;
using System.Collections.Generic;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "podium_settings", Tag = "pdm!", Size = 0x3C)]
    public class PodiumSettings : TagStructure
	{
        public float CameraOffsetX;
        public float CameraOffssetY;
        public float CameraOffsetZ;
        public Angle CameraOrientationY;
        public Angle CameraOrientationP;
        public Angle CameraOrientationR;
        public Angle CameraFov;
        public int TickDelay;
        public CachedTag Unknown;
        public List<PodiumBiped> PodiumBipeds;

        [TagStructure(Size = 0x2C)]
        public class PodiumBiped : TagStructure
		{
            public float OffsetX;
            public float OffsetY;
            public float OffsetZ;
            public Angle RotationI;
            public Angle RotationJ;
            public Angle RotationK;
            public int SpawnDelay;
            public CachedTag SpawnEffect;
        }
    }
}