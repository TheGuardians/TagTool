using TagTool.Common;

namespace TagTool.Cache
{
    public class CacheIndexHeader
    {
        public int TagGroupsOffset;
        public int TagGroupCount;
        public int TagsOffset;
        public DatumIndex ScenarioHandle;
        public DatumIndex GlobalsHandle;
        public int CRC;
        public int TagCount;
        public int TagInfoHeaderCount;
        public int TagInfoHeaderOffset;
        public int TagInfoHeaderCount2;
        public int TagInfoHeaderOffset2;
    }
}
