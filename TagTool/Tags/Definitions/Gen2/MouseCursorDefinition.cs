using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "mouse_cursor_definition", Tag = "mcsr", Size = 0xC)]
    public class MouseCursorDefinition : TagStructure
    {
        /// <summary>
        /// 0 - Normal mouse cursor
        /// 1 - Busy mouse cursor
        /// 2 - Hover mouse cursor
        /// 3 - Text select mouse cursor
        /// 
        /// </summary>
        public List<MouseCursorBitmapReferenceBlock> MouseCursorBitmaps;
        public float AnimationSpeedFps;
        
        [TagStructure(Size = 0x8)]
        public class MouseCursorBitmapReferenceBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag Bitmap;
        }
    }
}

