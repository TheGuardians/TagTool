using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Serialization;

namespace TagTool.Cache
{
    public interface IGameCacheContext
    {
        ISerializationContext CreateSerializationContext(object tag);
    }

    public interface ITagCache
    {
        byte[] GetTagRaw(int tagIndex);
        byte[] GetTagRaw(string tagName);
    }

    public interface IResourceCache
    {

    }

    public interface IStringIDCache
    {

    }

    public interface ILocaleCache
    {

    }
}
