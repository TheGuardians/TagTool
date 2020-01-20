using System;
using System.Collections.Generic;

namespace TagTool.Cache
{
    public class CacheLocalizedString
    {
        public int StringIndex;
        public string String;
        public int Index;

        public CacheLocalizedString(int index, string locale, int localeIndex)
        {
            StringIndex = index;
            String = locale;
            Index = localeIndex;
        }
    }

    public class LocaleTable : List<CacheLocalizedString> { }

}
