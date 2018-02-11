using System.Collections.Generic;

namespace TagTool.Legacy
{
    public abstract class cache_file_resource_gestalt
    {
        public List<RawEntry> DefinitionEntries;

        public cache_file_resource_gestalt()
        {
            DefinitionEntries = new List<RawEntry>();
        }

        public abstract class RawEntry
        {
            public int TagID;
            public int RawID;
            public int Offset;
            public int Size;
            public int LocationType;
            public int SegmentIndex;
			public int DefinitionAddress;

            public List<ResourceFixup> Fixups;
            public List<ResourceDefinitionFixup> DefinitionFixups;

            public RawEntry()
            {
                Fixups = new List<ResourceFixup>();
                DefinitionFixups = new List<ResourceDefinitionFixup>();
            }

            public abstract class ResourceFixup
            {
                public int BlockOffset;
                public int FixupType;
                public int Offset;
                public int RawFixup;
            }

            public abstract class ResourceDefinitionFixup
            {
                public int Offset;
                public int Type;
            }
        }

        public byte[] DefinitionData;
    }
}
