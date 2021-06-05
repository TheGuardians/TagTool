using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.IO;
using TagTool.Tags;

namespace TagTool.Serialization
{
    public interface IBlamSerializable
    {
        int GetRequiredAlignment();

        void Serialize(TagSerializer serializer, ISerializationContext context, MemoryStream tagStream, IDataBlock block, object instance, Type valueType, TagFieldAttribute valueInfo);

        object Deserialize(TagDeserializer deserializer, EndianReader reader, ISerializationContext context, TagFieldAttribute valueInfo, Type valueType);
    }
}
