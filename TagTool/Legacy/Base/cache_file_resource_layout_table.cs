using System.Collections.Generic;

namespace TagTool.Legacy
{
    public abstract class cache_file_resource_layout_table
    {
        public List<SharedCache> SharedCaches;
        public List<Page> Pages;
        public List<SoundRawChunk> SoundRawChunks;
        public List<Segment> Segments;

        public cache_file_resource_layout_table()
        {
            SharedCaches = new List<SharedCache>();
            Pages = new List<Page>();
            SoundRawChunks = new List<SoundRawChunk>();
            Segments = new List<Segment>();
        }

        public abstract class SharedCache
        {
            public string FileName;

            public override string ToString()
            {
                return FileName;
            }
        }

        public abstract class Page
        {
            public int CacheIndex;
            public int RawOffset;
            public int CompressedSize;
            public int DecompressedSize;
            public int RawChunkCount;
        }

        public abstract class SoundRawChunk
        {
            public int RawSize;
            public List<Size> Sizes;

            public SoundRawChunk()
            {
                Sizes = new List<Size>();
            }

            public abstract class Size
            {
                public int PermutationSize;
            }
        }

        public abstract class Segment
        {
            public int RequiredPageIndex;
            public int OptionalPageIndex;
            public int OptionalPageIndex2;
            public int RequiredPageOffset;
            public int OptionalPageOffset;
            public int OptionalPageOffset2;
            public int SoundNumber;
            public int SoundRawIndex;
        }
    }
}
