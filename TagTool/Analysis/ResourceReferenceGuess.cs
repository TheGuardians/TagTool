namespace TagTool.Analysis
{
    public class ResourceReferenceGuess : ITagElementGuess
    {
        public uint Size
        {
            get { return 0x4; }
        }

        public bool Merge(ITagElementGuess other)
        {
            return (other is ResourceReferenceGuess);
        }

        public void Accept(uint offset, ITagElementGuessVisitor visitor)
        {
            visitor.Visit(offset, this);
        }
    }
}
