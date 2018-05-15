using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Geometry;
using TagTool.IO;
using TagTool.Tags.Definitions;

namespace TagTool.Commands.Porting
{
    partial class PortTagCommand
    {
        private object ConvertGen2RenderModel(RenderModel mode)
        {
            // TODO: Do not use RenderModelBuilder. Kill it. Kill it with fire.
            var builder = new RenderModelBuilder(CacheContext.Version);

            foreach (var section in mode.Sections)
            {
                using (var stream = new MemoryStream(BlamCache.GetRawFromID(section.BlockOffset, section.BlockSize)))
                using (var reader = new EndianReader(stream, BlamCache.Reader.Format))
                {
                    // TODO: Read and convert resource data here.
                }
            }

            var result = builder.Build(CacheContext.Serializer, null);

            // TODO: Set up modifications to the 'mode' variable before returning it.
            return mode;
        }
    }
}