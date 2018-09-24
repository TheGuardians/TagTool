using TagTool.Cache;
using TagTool.Common;
using TagTool.Serialization;

namespace TagTool.Ai
{
    [TagStructure(Size = 0xE4)]
    public class CharacterMorphProperties : TagStructure
	{
        public CachedTagInstance MorphCharacter1;
        public CachedTagInstance MorphCharacter2;
        public CachedTagInstance MorphCharacter3;
        public CachedTagInstance MorphMuffin;
        public CachedTagInstance MorphWeapon1;
        public CachedTagInstance MorphWeapon2;
        public CachedTagInstance MorphWeapon3;
        public uint Unknown;
        public uint Unknown2;
        public uint Unknown3;
        public uint Unknown4;
        public uint Unknown5;
        public uint Unknown6;
        public uint Unknown7;
        public uint Unknown8;
        [TagField(Label = true)]
        public CachedTagInstance Character;
        public uint Unknown9;
        public StringId Unknown10;
        public uint Unknown11;
        public uint Unknown12;
        public uint Unknown13;
        public uint Unknown14;
        public uint Unknown15;
        public uint Unknown16;
        public uint Unknown17;
        public uint Unknown18;
        public uint Unknown19;
        public uint Unknown20;
        public uint Unknown21;
        public uint Unknown22;
        public uint Unknown23;
        public uint Unknown24;
        public uint Unknown25;
    }
}
