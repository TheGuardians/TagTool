using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Common;
using TagTool.Serialization;
using TagTool.Tags;

namespace TagTool.Cache
{
    public interface IGameCacheContext : ITagNames, ITagCache, IStringIDCache, IResourceCache
    {
        CacheVersion GetVersion();
    }

    public interface ITagCache
    {
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

    public interface ITagNames
    {
    }
}
