
namespace TagTool.Audio.Converter
{
    class CacheSoundResult
    {
        public int PermutationIndex { get; internal set; }
        public byte[] PermutationBuffer { get; internal set; }
        public int PermutationChunkSize { get; internal set; }
    }
}
