using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Common;
using TagTool.Geometry;
using TagTool.IO;
using TagTool.Serialization;
using TagTool.Tags.Definitions;

namespace TagTool.Commands.Porting
{
    partial class PortTagCommand
    {
        private object ConvertGen2RenderModel(RenderModel mode)
        {
            mode.Geometry = new RenderGeometry
            {
                Meshes = new List<Mesh>()
            };

            foreach (var section in mode.Sections)
            {
                using (var stream = new MemoryStream(BlamCache.GetRawFromID(section.BlockOffset, section.BlockSize)))
                using (var reader = new EndianReader(stream, BlamCache.Reader.Format))
                using (var writer = new EndianWriter(stream, BlamCache.Reader.Format))
                {
                    foreach (var resource in section.Resources)
                    {
                        stream.Position = resource.FieldOffset;

                        switch (resource.Type)
                        {
                            case ResourceTypeGen2.TagBlock:
                                writer.Write(resource.ResoureDataSize / resource.SecondaryLocator);
                                writer.Write(8 + section.SectionDataSize + resource.ResourceDataOffset);
                                break;

                            case ResourceTypeGen2.TagData:
                                writer.Write(resource.ResoureDataSize);
                                writer.Write(8 + section.SectionDataSize + resource.ResourceDataOffset);
                                break;

                            case ResourceTypeGen2.VertexBuffer:
                                // TODO: Load vertex data
                                break;
                        }
                    }
                    
                    stream.Position = 0;
                    var dataContext = new DataSerializationContext(reader);
                    mode.Geometry.Meshes.Add(BlamCache.Deserializer.Deserialize<Mesh>(dataContext));
                }
            }
            
            // TODO: Set up modifications to the 'mode' variable before returning it.
            return mode;
        }
    }
}