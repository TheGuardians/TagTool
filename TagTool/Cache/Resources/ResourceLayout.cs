using System;
using TagTool.Tags;

namespace TagTool.Cache.Resources
{
    [TagStructure(Size = 0x24)]
    public class ResourceLayout : TagStructure
    {
        public int ImmediatelyRequiredResourceSize;
        public int DeferredRequiredResourceSize;
        public int OptionalResourceSize;
        public int UnusedResourceSize;
        public int PageableCompressedSize;
        public int OptionalCompressedSize;
        public GlobalZoneAttachmentFlags GlobalZoneAttachment;
        public ushort BspZoneAttachment;
        public int DesignerZoneAttachment;
        public int CinematicZoneAttachment;

        [Flags]
        public enum GlobalZoneAttachmentFlags : short
        {
            None = 0,
            Global = 1 << 0,
            Script = 1 << 1,
            HDDOnly = 1 << 2,
            AlwaysStreaming = 1 << 3,
            Unattached = 1 << 4,
        }
    }
}
