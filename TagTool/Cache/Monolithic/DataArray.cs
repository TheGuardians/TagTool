using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TagTool.Common;
using TagTool.IO;
using TagTool.Tags;

namespace TagTool.Cache.Monolithic
{
    public interface IDatum
    {
        ushort Identifier { get; set; }
    }

    public class DataArray<T> : IReadOnlyList<T> where T : IDatum
    {
        private T[] _data;

        public DataArray(PersistChunkReader reader, Func<PersistChunkReader, T> deserializeDatum = null)
        {
            if (deserializeDatum == null)
                deserializeDatum = r => r.Deserialize<T>();

            var header = reader.Deserialize<DataArrayPersistHeader>();

            _data = new T[header.MaximumCount];
            for (int i = 0; i < header.ActualCount; i++)
            {
                var index = reader.ReadDatumIndex();
                var datum = deserializeDatum(reader);
                _data[index.Index] = datum;

                if (datum.Identifier != 0 && datum.Identifier != index.Salt)
                    throw new Exception("Invalid datum identifier");

                datum.Identifier = index.Salt;

                if (reader.ReadTag().Value != 0x21402324) // !@#$ avoid excessive strcmp
                    throw new Exception("Invalid datum footer signature");
            }

            if (reader.ReadTag() != "d@ft")
                throw new Exception("Invalid data array footer signature");
        }

        public bool TryGetDatum(DatumHandle index, out T datum)
        {
            datum = default;
            if (index.Salt == 0 || index.Index >= _data.Length)
                return false;
            datum = _data[index.Index];
            if (datum.Identifier != index.Salt)
                return false;
            return true;
        }

        public T TryGetDatum(DatumHandle index)
        {
            return TryGetDatum(index, out T datum) ? datum : default;
        }

        // IReadOnlyList<T> Interface
        public T this[int index] => _data[index];
        public int Count => _data.Length;
        public IEnumerator<T> GetEnumerator() => _data.Where(x => x.Identifier != 0).GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();


        [TagStructure(Size = 0x34)]
        public class DataArrayPersistHeader : TagStructure
        {
            [TagField(Length = 32)]
            public string Name;
            public int Size;
            public int MaximumCount;
            public int ActualCount;
            public int NextIdentifier;
            public Tag Signature;
        }
    }
}
