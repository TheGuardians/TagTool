namespace TagTool.Analysis
{
    /// <summary>
    /// Interface for a tag element guess.
    /// </summary>
    public interface ITagElementGuess
    {
        /// <summary>
        /// Gets the size of the tag element in bytes.
        /// </summary>
        uint Size { get; }

        /// <summary>
        /// Merges this guess with another one.
        /// </summary>
        /// <param name="other">The guess to merge with.</param>
        /// <returns><c>true</c> if the merge was successful.</returns>
        bool Merge(ITagElementGuess other);

        /// <summary>
        /// Dispatches this guess to a visitor object.
        /// </summary>
        /// <param name="offset">The element offset to pass to the visitor.</param>
        /// <param name="visitor">The visitor object.</param>
        void Accept(uint offset, ITagElementGuessVisitor visitor);
    }
}
