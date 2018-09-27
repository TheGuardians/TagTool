using System;
using System.IO;
using TagTool.Tags;

namespace TagTool.Serialization
{
    /// <summary>
    /// Interface for a block of data being serialized.
    /// </summary>
    public interface IDataBlock
    {
        /// <summary>
        /// Gets the stream open on the data block.
        /// </summary>
        MemoryStream Stream { get; }

        /// <summary>
        /// Gets the writer open on the data block's stream.
        /// </summary>
        BinaryWriter Writer { get; }

        /// <summary>
        /// Writes a pointer to an object at the current position in the block.
        /// </summary>
        /// <param name="targetOffset">The target offset.</param>
        /// <param name="type">The type of object that the pointer will point to.</param>
        void WritePointer(uint targetOffset, Type type);

        /// <summary>
        /// Called before an object is serialized into the block.
        /// </summary>
        /// <param name="info">Information about the tag element.</param>
        /// <param name="obj">The object intended to be serialized.</param>
        /// <returns>The object which should actually be serialized.</returns>
        object PreSerialize(TagFieldAttribute info, object obj);

        /// <summary>
        /// Suggests a power of two to align the block on.
        /// </summary>
        /// <param name="align">The power of two to suggest.</param>
        void SuggestAlignment(uint align);

        /// <summary>
        /// Finalizes the block, writing it out to a stream.
        /// </summary>
        /// <param name="outStream">The output stream.</param>
        /// <returns>The offset of the block within the output stream.</returns>
        uint Finalize(Stream outStream);
    }
}
