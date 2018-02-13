using TagTool.Common;
using TagTool.Tags.Definitions;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TagTool.Commands.Porting
{
    partial class PortTagCommand
    {
        private MultilingualUnicodeStringList ConvertStrings(MultilingualUnicodeStringList unic)
        {
            ushort[] stringIndex = new ushort[12];
            ushort[] stringCount = new ushort[12];

            for(int i = 0; i < 24; i++)
            {
                if (i % 2 == 0)
                {
                    stringIndex[i / 2] = unic.OffsetCounts[i];
                }
                else
                {
                    stringCount[i / 2] = unic.OffsetCounts[i];
                }
            }

            //
            // Convert LocaleTables to a Dictionary
            //

            Dictionary<int, KeyValuePair<int, List<string>>> table = new Dictionary<int, KeyValuePair<int, List<string>>>();
            
            var localeTable = BlamCache.LocaleTables;

            for(int i = 0; i < localeTable.Count; i++)
            {
                var localizedStringList = localeTable[i];
                for(int j = 0; j < localizedStringList.Count(); j++)
                {
                    var localizedString = localizedStringList[j];
                    var tempPair = table.ContainsKey(localizedString.Index) ? table[localizedString.Index] : new KeyValuePair<int, List<string>>(localizedString.StringIndex, new List<string> { "", "", "", "", "", "", "", "", "", "", "", "" });
                    tempPair.Value[i] = localizedString.String;
                    table[localizedString.Index] = tempPair;
                }
            }

            //
            // Determine the max count while ignoring the languages that have 0
            //

            List<ushort> fixedStartIndex = new List<ushort>();
            int zeroCount=0;
            foreach(var startIndex in stringIndex)
            {
               if(startIndex > 0)
               {
                    fixedStartIndex.Add(startIndex);
               }
               else if(startIndex == 0)
               {
                    zeroCount++;
               }
            }
            int begin;

            if (zeroCount == 12)
                begin = 0;
            else
                begin = fixedStartIndex.Max();
             
            var count = stringCount.Max();

            string data = "";
            unic.Strings = new List<LocalizedString>();

            for(int i = begin; i < begin + count; i++)
            {
                var pair = table[i];
                var stringBlock = new LocalizedString {

                    StringID = ConvertStringId(new StringId((uint)pair.Key)),
                    StringIDStr = null,
                };

                var locales = pair.Value;

                for(int j = 0; j < 12; j++)
                {
                    stringBlock.Offsets[j] = Encoding.UTF8.GetBytes(data).Length;

                    if (i < stringIndex[j] || i >= stringIndex[j] + stringCount[j]) // Index is out of bounds for this particular locale
                    {
                        stringBlock.Offsets[j] = -1;
                    }
                    else
                    {
                        data = data + locales[j] + '\0';
                    }
                }
                unic.Strings.Add(stringBlock);
            }

            unic.Data = Encoding.UTF8.GetBytes(data);

            unic.OffsetCounts = new ushort[24];

            return unic;
        }
    }
}

