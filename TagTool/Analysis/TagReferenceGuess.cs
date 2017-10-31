namespace TagTool.Analysis
{
    public class TagReferenceGuess : ITagElementGuess
    {
        public uint Size
        {
            get { return 0x10; }
        }

        public bool Merge(ITagElementGuess other)
        {
            return (other is TagReferenceGuess);
        }

        public void Accept(uint offset, ITagElementGuessVisitor visitor)
        {
            visitor.Visit(offset, this);
        }
    }
}
