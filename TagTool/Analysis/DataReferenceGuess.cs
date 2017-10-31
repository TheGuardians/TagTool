using System;

namespace TagTool.Analysis
{
    public class DataReferenceGuess : ITagElementGuess
    {
        public DataReferenceGuess(uint align)
        {
            Align = align;
        }

        public uint Size
        {
            get { return 0x14; }
        }

        public uint Align { get; private set; }

        public bool Merge(ITagElementGuess other)
        {
            var otherData = other as DataReferenceGuess;
            if (otherData == null)
                return false;
            if (Align == 0)
                Align = otherData.Align;
            else if (otherData.Align != 0)
                Align = Math.Min(Align, otherData.Align);
            return true;
        }

        public void Accept(uint offset, ITagElementGuessVisitor visitor)
        {
            visitor.Visit(offset, this);
        }
    }
}
