using System;

namespace TagTool.Common
{
    public static class ArrayUtil
    {
        /// <summary>
        /// Reallocates an array, replacing a block of data in it.
        /// </summary>
        /// <param name="oldData">The array of data to replace a block in.</param>
        /// <param name="index">The starting index of the block to replace.</param>
        /// <param name="length">The length of the block to replace.</param>
        /// <param name="newData">The data to replace the block with.</param>
        /// <returns></returns>
        public static byte[] Replace(byte[] oldData, int index, int length, byte[] newData)
        {
            if (index < 0 || index + length > oldData.Length)
                throw new ArgumentException("Invalid region to replace");

            // Allocate a new buffer
            var sizeDelta = newData.Length - length;
            var newBuffer = new byte[oldData.Length + sizeDelta];

            // Copy over bytes before the block
            Buffer.BlockCopy(oldData, 0, newBuffer, 0, index);

            // Copy the new data in
            Buffer.BlockCopy(newData, 0, newBuffer, index, newData.Length);

            // Copy everything after the block
            var oldEndOffset = index + length;
            var newEndOffset = index + newData.Length;
            Buffer.BlockCopy(oldData, oldEndOffset, newBuffer, newEndOffset, oldData.Length - oldEndOffset);
            return newBuffer;
        }

        /// <summary>
        /// Copy <paramref name="length"/> elements from <paramref name="sourceSpan"/> beginning at <paramref name="sourceIndex"/> 
        /// into <paramref name="destinationArray"/> beginning at <paramref name="destinationIndex"/>
        /// </summary>
        /// <param name="sourceSpan">The source span to copy from</param>
        /// <param name="sourceIndex">The offset in the source span to begin copying from</param>
        /// <param name="destinationArray">The destination array</param>
        /// <param name="destinationIndex">The offset in the destination array</param>
        /// <param name="length">The number of elements to copy</param>
        public static void CopyTo<T>(this Span<T> sourceSpan, int sourceIndex, T[] destinationArray, int destinationIndex, int length)
        {
            sourceSpan.Slice(sourceIndex, length)
                .CopyTo(destinationArray.AsSpan().Slice(destinationIndex, length));
        }

        /// <summary>
        /// Copy <paramref name="length"/> elements from <paramref name="sourceMemory"/> beginning at <paramref name="sourceIndex"/> 
        /// into <paramref name="destinationArray"/> beginning at <paramref name="destinationIndex"/>
        /// </summary>
        /// <param name="sourceMemory">The source memory to copy from</param>
        /// <param name="sourceIndex">The index in the source memory to begin copying from</param>
        /// <param name="destinationArray">The destination array</param>
        /// <param name="destinationIndex">The offset in the destination array</param>
        /// <param name="length">The number of elements to copy</param>
        public static void CopyTo<T>(this Memory<T> sourceMemory, int sourceIndex, T[] destinationArray, int destinationIndex, int length)
        {
            sourceMemory.Slice(sourceIndex, length)
                .CopyTo(destinationArray.AsMemory(destinationIndex, length));
        }
    }
}
