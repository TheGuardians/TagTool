using System;

namespace TagTool.Common
{
    static class ArrayUtil
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
    }
}
