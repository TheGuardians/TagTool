﻿using System;
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

        [Description("Enable reach decorator porting (WIP)")]
        public bool ReachDecorators = false;

        [Description("Enable legacy Gen1 collision BSP generator")]
        public bool Gen1Collision = false;

        public static PortingOptions Current = new PortingOptions();
    }
}
