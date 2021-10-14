using System;
using System.Collections.Generic;
using System.Linq;

namespace TagTool.Cache.Resources
{
    public class ResourcePageLruvCache
    {
        // Mutex for concurrent access
        public object Mutex = new object();
        // The max size in bytes before pages get evicted
        public long MaxCapacity { get; }
        // Current used memory size.
        // WARNING: outside of the mutex lock this is just an estimate. Do not base logic around it
        public long CurrentSize => CachedPages.Sum(x => x.Data.Length);
        // Mapping of resource page index to cached pages
        private Dictionary<int, CachedPage> IndexToCachedPage = new Dictionary<int, CachedPage>();
        // List of cached pages ordered by least recently accessed
        private List<CachedPage> CachedPages = new List<CachedPage>(); 

        private class CachedPage
        {
            public int Index;
            public byte[] Data;
        }

        public ResourcePageLruvCache(long maxCapacity)
        {
            MaxCapacity = maxCapacity;
        }

        public bool TryGetPage(int index, out byte[] data)
        {
            lock (Mutex)
            {
                if (IndexToCachedPage.TryGetValue(index, out CachedPage cachedPage))
                {
                    CachedPages.Remove(cachedPage);
                    CachedPages.Add(cachedPage);
                    data = cachedPage.Data;
                    return true;
                }
                else
                {
                    data = null;
                    return false;
                }
            }
        }

        public bool AddPage(int index, byte[] data)
        {
            lock (Mutex)
            {
                if (IndexToCachedPage.ContainsKey(index))
                    return false;

                EvictPages(data.Length);

                var page = new CachedPage() { Index = index, Data = data };
                IndexToCachedPage.Add(index, page);
                // reinsert the page to keep the list ordered by access
                CachedPages.Remove(page);
                CachedPages.Add(page);
                return true;
            }
        }

        public void Clear()
        {
            lock (Mutex)
            {
                CachedPages.Clear();
                IndexToCachedPage.Clear();
            }
        }

        private void EvictPages(long size)
        {
            // keep evicting the least accessed pages until we have enough room
            // or we run out of pages
            while (CurrentSize + size > MaxCapacity)
            {
                if (CachedPages.Count == 0)
                    throw new ArgumentException("Not enough memory", nameof(size));

                var cachedPage = CachedPages[0];
                IndexToCachedPage.Remove(cachedPage.Index);
                CachedPages.Remove(cachedPage);
            }
        }
    }
}
