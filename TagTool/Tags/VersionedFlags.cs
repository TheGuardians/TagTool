using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Cache;

namespace TagTool.Tags
{
    public class VersionedFlags : TagStructure
    {
        public void ConvertFlags(CacheVersion sourceVersion, CachePlatform sourcePlatform, CacheVersion destVersion, CachePlatform destPlatform)
        {
            TagFieldInfo sourceField = GetTagFieldEnumerable(sourceVersion, sourcePlatform).First();
            TagFieldInfo destField = GetTagFieldEnumerable(destVersion, destPlatform).First();
            object outenum = Activator.CreateInstance(destField.FieldType);
            if (TranslateEnum(sourceField.GetValue(this), out outenum, outenum.GetType()))
            {
                destField.SetValue(this, outenum);
            }
        }

        private bool TranslateEnum<T>(object input, out T output, Type enumType)
        {
            output = default(T);
            if (input == null)
                return false;
            var members = input.ToString()
                .Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .Where(x => enumType.IsEnumDefined(x))
                .ToArray();
            if (members.Length == 0)
                return false;

            output = (T)Enum.Parse(enumType, string.Join(", ", members));
            return true;
        }
    }
}
