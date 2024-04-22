using TagTool.Common;
using TagTool.IO;
using TagTool.Tags;

namespace TagTool.Cache.Monolithic
{
    public class WideDataArray<T> where T : IDatum
    {
        public DataArray<DataPartition> Partitions;

        public WideDataArray(PersistChunkReader reader)
        {
            var header = reader.Deserialize<WideDataArrayPersistHeader>();
            Partitions = new DataArray<DataPartition>(reader, DeserializeDataPartition);
        }

        private DataPartition DeserializeDataPartition(PersistChunkReader reader)
        {
            return new DataPartition() { Data = new DataArray<T>(reader) };
        }

        public bool TryGetDatum(WideDatumHandle wideIndex, out T datum)
        {
            datum = default;
            var part = Partitions.TryGetDatum(wideIndex.PartitionIndex);
            if (part == null)
                return false;
            return part.Data.TryGetDatum(wideIndex.DatumIndex, out datum);
        }

        public T TryGetDatum(WideDatumHandle wideIndex)
        {
            return TryGetDatum(wideIndex, out T datum) ? datum : default;
        }

        public class DataPartition : IDatum
        {
            public ushort Identifier;
            public DataArray<T> Data;

            ushort IDatum.Identifier { get => Identifier; set => Identifier = value; }
        }

        [TagStructure(Size = 0x30)]
        public class WideDataArrayPersistHeader : TagStructure
        {
            [TagField(Length = 32)]
            public string Name;
            public int MaximumCount;
            public int DatumSize;
            public int Unknown1;
            public Tag Signature;
        }
    }
}
