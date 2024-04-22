using TagTool.Tags;

namespace TagTool.Ai
{
    [TagStructure(Size = 0xC)]
    public class CharacterVocalizationProperties : TagStructure
	{
        public float CharacterSkipFraction; // [0,1]
        public float LookCommentTime; // How long does the player look at an AI before the AI responds? (seconds)
        public float LookLongCommentTime; // How long does the player look at the AI before he responds with his 'long look' comment? (seconds)
    }
}
