using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace TagTool.Analysis
{
    /// <summary>
    /// A guess at the layout of a tag.
    /// </summary>
    public class TagLayoutGuess
    {
        private readonly Dictionary<uint, ITagElementGuess> _guessesByOffset = new Dictionary<uint, ITagElementGuess>();

        public TagLayoutGuess(uint size)
        {
            Size = size;
        }

        /// <summary>
        /// Gets the size of the layout in bytes.
        /// </summary>
        public uint Size { get; private set; }

        /// <summary>
        /// Adds a guess to the layout.
        /// </summary>
        /// <param name="offset">The offset of the guess from the beginning of the layout.</param>
        /// <param name="guess">The guess to add.</param>
        public void Add(uint offset, ITagElementGuess guess)
        {
            if (offset >= Size)
                throw new ArgumentOutOfRangeException("offset", "Offset cannot be past the end of the layout");
            if (_guessesByOffset.TryGetValue(offset, out ITagElementGuess otherGuess))
            {
                if (!otherGuess.Merge(guess))
                    Debug.WriteLine("WARNING: Merge failure between {0} and {1} at offset 0x{2:X}", otherGuess.GetType(), guess.GetType(), offset);
            }
            else
            {
                _guessesByOffset[offset] = guess;
            }
        }

        /// <summary>
        /// Merges another layout into this one.
        /// </summary>
        /// <param name="otherLayout">The layout to merge with.</param>
        public void Merge(TagLayoutGuess otherLayout)
        {
            if (otherLayout.Size != Size)
                Size = Math.Min(Size, otherLayout.Size); // hackhackhack
            foreach (var guess in otherLayout._guessesByOffset)
                Add(guess.Key, guess.Value);
        }

        /// <summary>
        /// Tries to get the guess at an offset.
        /// </summary>
        /// <param name="offset">The offset to get the guess at.</param>
        /// <returns>The guess if one exists, or <c>null</c> otherwise.</returns>
        public ITagElementGuess TryGet(uint offset)
        {
            _guessesByOffset.TryGetValue(offset, out ITagElementGuess result);
            return result;
        }

        /// <summary>
        /// Dispatches each guess in the layout to a visitor object.
        /// </summary>
        /// <param name="visitor">The visitor object.</param>
        public void Accept(ITagElementGuessVisitor visitor)
        {
            foreach (var guess in _guessesByOffset)
                guess.Value.Accept(guess.Key, visitor);
        }
    }
}
