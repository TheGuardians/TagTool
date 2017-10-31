using System;

namespace TagTool.Analysis
{
    public class TagBlockGuess : ITagElementGuess
    {
        public TagBlockGuess(TagLayoutGuess layout, uint align)
        {
            if (layout == null)
                throw new ArgumentNullException("layout");
            ElementLayout = layout;
            Align = align;
        }

        public uint Size
        {
            get { return 0xC; }
        }

        public uint Align { get; private set; }

        /// <summary>
        /// Gets the layout of each element in the tag block.
        /// </summary>
        public TagLayoutGuess ElementLayout { get; private set; }

        public bool Merge(ITagElementGuess other)
        {
            var otherBlock = other as TagBlockGuess;
            if (otherBlock == null)
                return false;
            ElementLayout.Merge(otherBlock.ElementLayout);
            if (Align == 0)
                Align = otherBlock.Align;
            else if (otherBlock.Align != 0)
                Align = Math.Min(Align, otherBlock.Align);
            return true;
        }

        public void Accept(uint offset, ITagElementGuessVisitor visitor)
        {
            visitor.Visit(offset, this);
        }
    }
}
