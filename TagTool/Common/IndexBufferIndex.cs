namespace TagTool.Common
{
    public struct IndexBufferIndex
    {
        public int Value;

        public IndexBufferIndex(int index = 0) => Value = index;
        public static implicit operator IndexBufferIndex(int index) => new IndexBufferIndex(index);
        public static implicit operator int(IndexBufferIndex index) => index.Value;
        public static implicit operator uint(IndexBufferIndex index) => (uint)index.Value;
    }
}
