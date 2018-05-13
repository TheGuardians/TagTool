using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Cache;

namespace TagTool.ShaderGenerator
{
    public interface IShaderGenerator
    {
        HaloOnlineCacheContext CacheContext { get; }
    }
}
