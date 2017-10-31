namespace TagTool.Analysis
{
    /// <summary>
    /// Interface for a class which can visit tag element guesses.
    /// </summary>
    public interface ITagElementGuessVisitor
    {
        void Visit(uint offset, TagBlockGuess guess);
        void Visit(uint offset, DataReferenceGuess guess);
        void Visit(uint offset, TagReferenceGuess guess);
        void Visit(uint offset, ResourceReferenceGuess guess);
    }
}
