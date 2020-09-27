using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "mouse_cursor_definition", Tag = "mcsr", Size = 0x10)]
    public class MouseCursorDefinition : TagStructure
    {
        /// <summary>
        /// Mouse Cursor Bitmaps:
        /// </summary>
        /// <remarks>
        /// 0 - Normal mouse cursor
        /// 1 - Busy mouse cursor
        /// 2 - Hover mouse cursor
        /// 3 - Text select mouse cursor
        /// 
        /// </remarks>
        public List<LocalBitmapReference> MouseCursorBitmaps;
        public float AnimationSpeedFps;
        
        [TagStructure(Size = 0x10)]
        public class LocalBitmapReference : TagStructure
        {
            public CachedTag Bitmap;
        }
    }
}

