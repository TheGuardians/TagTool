using System.Collections.Generic;
using TagTool.Common;

namespace TagTool.Cache.Monolithic
{
    public class StringTableMonolithic : StringTable
    {
        private Dictionary<string, StringId> ReverseLookup;
        private object Mutex = new object();

        public StringTableMonolithic()
        {
            Resolver = new StringIdResolverUnnamespaced();
            ReverseLookup = new Dictionary<string, StringId>();
        }

        public override StringId AddString(string newString)
        {
            lock (Mutex)
            {
                if (ReverseLookup.TryGetValue(newString, out StringId value))
                    return value;

                var strIndex = Count;
                Add(newString);
                ReverseLookup.Add(newString, Resolver.IndexToStringID(strIndex));
                return GetStringId(strIndex);
            }
        }

        public override string GetString(StringId id)
        {
            lock (Mutex)
            {
                var index = Resolver.StringIDToIndex(id);
                if (index > 0 && index < Count)
                    return this[index];
                else
                    return "invalid";
            }
        }

        public override StringId GetStringId(string str)
        {
            lock (Mutex)
            {
                if (ReverseLookup.TryGetValue(str, out StringId value))
                    return value;
                else
                    return StringId.Invalid;
            }
        }

        public override StringId GetStringId(int index)
        {
            lock (Mutex)
            {
                if (index < 0 || index >= this.Count)
                    return StringId.Invalid;

                return Resolver.IndexToStringID(index);
            }
        }

        private class NoNamespaceStringIdResolver : StringIdResolver
        {
            public NoNamespaceStringIdResolver()
            {
                IndexBits = 31;
                SetBits = 0;
                LengthBits = 0;
            }

            public override int GetMaxSetStringIndex()
            {
                return int.MaxValue;
            }

            public override int GetMinSetStringIndex()
            {
                return int.MaxValue;
            }

            public override int[] GetSetOffsets()
            {
                return new int[0];
            }
        }
    }
}
