using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Tags;
using TagTool.Tags.Resources;

namespace TagTool.Video
{
    public static class VideoUtils
    {
        public static BinkResource CreateBinkResourceDefinition(byte[] data)
        {
            return new BinkResource
            {
                Data = new TagData(data)
            };
        }

        public static BinkResource CreateEmptyBinkResourceDefinition()
        {
            return new BinkResource
            {
                Data = new TagData()
            };
        }
    }
}
