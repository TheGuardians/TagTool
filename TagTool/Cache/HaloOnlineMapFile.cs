using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Tags;

namespace TagTool.Cache
{
    [TagStructure(Size = 0x19E20, MinVersion = CacheVersion.HaloOnline106708)]
    public sealed class HaloOnlineMapFile
    {

    }

    [TagStructure(Size = 0x3390, MinVersion = CacheVersion.HaloOnline106708)]
    public sealed class HaloOnlineMapFileHeader
    {

    }


    /*
     * struct
{
    s_cache_file_header cache_file_header;
    s_blf_map_information map_info;
    c_map_variant default_map_variant;
    s_blf_chunk_end_of_file foot
}
     */
}
