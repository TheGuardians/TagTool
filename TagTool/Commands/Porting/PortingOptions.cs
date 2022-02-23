using System;
using System.ComponentModel;
using TagTool.Audio;

namespace TagTool.Commands.Porting
{
    public class PortingOptions
    {
        [Description("Maximum number of threads to use")]
        public int MaxThreads = Environment.ProcessorCount * 2;

        [Description("Audio codec to use for ported sounds")]
        public Compression AudioCodec = Compression.MP3;

        [Description("Path to reach lightmap cache directory")]
        public string ReachLightmapCache = null;

        [Description("Enable experimental super node conversion")]
        public bool ReachSuperNodeConversion = false;

        [Description("Enable reach decorator porting (WIP)")]
        public bool ReachDecorators = false;

        [Description("Enable experimental collision leaf mapping")]
        public bool CollisionLeafMapping = false;

        [Description("Enable experimental Gen3 collision BSP generator")]
        public bool Gen3Collision = false;

        public static PortingOptions Current = new PortingOptions();
    }
}
