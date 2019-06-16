using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Common;

namespace TagTool.Cache
{
    public class CacheStringTable : List<string>
    {
        public string StringMods = null;

        public string GetItemByID(int ID)
        {
            //go through the modifiers, if the ID matches a modifer return the correct string
            string[] mods = StringMods?.Split(';') ?? new string[] { "0,0" };
            try
            {
                foreach (string mod in mods)
                {
                    string[] Params = mod.Split(','); //[0] - check, [1] - change
                    int check = int.Parse(Params[0]);
                    int change = int.Parse(Params[1]);

                    if (check < 0)
                    {
                        if (ID < check)
                        {
                            ID += change;
                            return this[ID];
                        }
                    }
                    else
                    {
                        if (ID > check)
                        {
                            ID += change;
                            return this[ID];
                        }
                    }
                }
            }
            catch
            {
                return "invalid";
            }

            //if no matching modifier, return the string at index of ID, or null if out of bounds
            try { return this[ID]; }
            catch { return ""; }
        }

        /*
        public string GetString(StringId stringId)
        {
            if (Cache.Version < CacheVersion.Halo3Retail)
                return GetItemByID((int)stringId.Value);

            var index = Cache.Resolver.StringIDToIndex(stringId);

            return this[index];
        }
        */
    }
}
