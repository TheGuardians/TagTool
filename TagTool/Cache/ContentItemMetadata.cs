using TagTool.Tags;
using static System.Runtime.InteropServices.CharSet;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Cache
{
    [TagStructure(Size = 0xF8)]
    public class ContentItemMetadata
    {
        public ulong Id;

        [TagField(CharSet = Unicode, Length = 16)]
        public string Name = string.Empty;

        [TagField(CharSet = Ansi, Length = 128)]
        public string Description = string.Empty;

        [TagField(CharSet = Ansi, Length = 16)]
        public string Author = string.Empty;

        public int ContentType;
        public bool UserIsOnline;

        [TagField(Flags = Padding, Length = 3)]
        public byte[] Unused1 = new byte[3];

        public ulong UserId;
        public ulong ContentSize;
        public ulong Timestamp;

        [TagField(Flags = Padding, Length = 4)]
        public byte[] Unused2 = new byte[4];

        public int CampaignId;
        public int MapId;
        public int GameEngineType;
        public int CampaignDifficulty;
        public sbyte CampaignInsertionPoint;
        public bool IsSurvival;

        [TagField(Flags = Padding, Length = 2)]
        public byte[] Unused3 = new byte[2];

        public ulong MapChecksum;
    }
}