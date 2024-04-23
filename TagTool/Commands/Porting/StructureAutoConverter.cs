using System;
using System.Collections.Generic;
using System.Linq;
using TagTool.Tags;
using TagTool.Commands.Common;
using TagTool.Cache;

namespace TagTool.Commands.Porting
{
    public class StructureAutoConverter
    {
        private GameCacheInfo sourceCacheInfo;
        private GameCacheInfo destCacheInfo;

        public StructureAutoConverter(GameCacheInfo sourceCacheInfo, GameCacheInfo destCacheInfo)
        {
            this.sourceCacheInfo = sourceCacheInfo;
            this.destCacheInfo = destCacheInfo;
        }

        public StructureAutoConverter(CacheVersion sourceVersion, CachePlatform sourcePlatform, CacheVersion destVersion, CachePlatform destPlatform)
        {
            sourceCacheInfo = new GameCacheInfo(sourceVersion, sourcePlatform);
            destCacheInfo = new GameCacheInfo(destVersion, destPlatform);
        }

        public StructureAutoConverter(GameCache sourceCache, GameCache destCache)
        {
            this.sourceCacheInfo = new GameCacheInfo(sourceCache);
            this.destCacheInfo = new GameCacheInfo(destCache);            
        }

        public class GameCacheInfo
        {
            public CacheVersion Version { get; set; }
            public CachePlatform Platform { get; set; }

            public GameCacheInfo(GameCache cache)
            {
                this.Version = cache.Version;
                this.Platform = cache.Platform;
            }

            public GameCacheInfo(CacheVersion version, CachePlatform platform)
            {
                Version = version;
                Platform = platform;
            }
        }

        public void TranslateTagStructure(TagStructure input, TagStructure output)
        {
            if (input == null || output == null)
            {
                return;
            }

            Type inputtype = input.GetType();
            Type outputtype = output.GetType();

            var inputinfo = TagStructure.GetTagStructureInfo(inputtype, sourceCacheInfo.Version, sourceCacheInfo.Platform);
            var outputinfo = TagStructure.GetTagStructureInfo(outputtype, destCacheInfo.Version, destCacheInfo.Platform);

            //use an ordered list so we can use binary searches to decrease iterations
            List<TagFieldInfo> outputfieldlist = TagStructure.GetTagFieldEnumerable(outputinfo.Types[0], outputinfo.Version, outputinfo.CachePlatform)
                .OrderBy(n => n.Name).ToList();
            List<string> outputnamelist = outputfieldlist.Select(n => n.Name.ToUpper()).ToList();

            foreach (var tagFieldInfo in TagStructure.GetTagFieldEnumerable(inputinfo.Types[0], inputinfo.Version, inputinfo.CachePlatform))
            {
                //don't bother converting the value if it is null, padding, or runtime
                if (tagFieldInfo.Attribute.Flags.HasFlag(TagFieldFlags.Padding) ||
                    tagFieldInfo.Attribute.Flags.HasFlag(TagFieldFlags.Runtime) ||
                    tagFieldInfo.GetValue(input) == null)
                    continue;

                //binary search to find a field in the output with a matching name
                string query = tagFieldInfo.Name.ToUpper();
                int matchindex = outputnamelist.BinarySearch(query);
                if (matchindex >= 0)
                {
                    var outputFieldInfo = outputfieldlist[matchindex];

                    //if field types match, just assign value
                    if (tagFieldInfo.FieldType == outputFieldInfo.FieldType)
                    {
                        outputFieldInfo.SetValue(output, tagFieldInfo.GetValue(input));
                    }
                    //if its a sub-tagstructure, iterate into it
                    else if (tagFieldInfo.FieldType.BaseType == typeof(TagStructure) &&
                        outputFieldInfo.FieldType.BaseType == typeof(TagStructure))
                    {
                        var outstruct = Activator.CreateInstance(outputFieldInfo.FieldType);
                        TranslateTagStructure((TagStructure)tagFieldInfo.GetValue(input), (TagStructure)outstruct);
                        outputFieldInfo.SetValue(output, outstruct);
                    }
                    //if its a tagblock, call convertlist to iterate through and convert each one and return a converted list
                    else if (tagFieldInfo.FieldType.IsGenericType && tagFieldInfo.FieldType.GetGenericTypeDefinition() == typeof(List<>) &&
                        outputFieldInfo.FieldType.IsGenericType && tagFieldInfo.FieldType.GetGenericTypeDefinition() == typeof(List<>))
                    {
                        object inputlist = tagFieldInfo.GetValue(input);
                        object outputlist = Activator.CreateInstance(outputFieldInfo.FieldType);
                        TranslateList(inputlist, outputlist);
                        outputFieldInfo.SetValue(output, outputlist);
                    }
                    //if its an enum, try to parse the value
                    else if (tagFieldInfo.FieldType.BaseType == typeof(Enum) &&
                        outputFieldInfo.FieldType.BaseType == typeof(Enum))
                    {
                        object outenum = Activator.CreateInstance(outputFieldInfo.FieldType);
                        if (TranslateEnum(tagFieldInfo.GetValue(input), out outenum, outenum.GetType()))
                        {
                            outputFieldInfo.SetValue(output, outenum);
                        }
                    }

                    //remove the matched values from the output lists to decrease the size of future searches
                    outputfieldlist.RemoveAt(matchindex);
                    outputnamelist.RemoveAt(matchindex);
                }
            }
        }

        public void TranslateList(object input, object output)
        {
            if (input == null || output == null)
            {
                return;
            }

            Type outputtype = output.GetType();

            IEnumerable<object> enumerable = input as IEnumerable<object>;
            if (enumerable == null)
                throw new InvalidOperationException("listData must be enumerable");

            Type outputelementType = outputtype.GenericTypeArguments[0];
            var addMethod = outputtype.GetMethod("Add");
            foreach (object item in enumerable.OfType<object>())
            {
                var outputelement = Activator.CreateInstance(outputelementType);
                TranslateTagStructure((TagStructure)item, (TagStructure)outputelement);
                addMethod.Invoke(output, new[] { outputelement });
            }
        }

        public bool TranslateEnum<T>(object input, out T output, Type enumType)
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

        public void InitTagBlocks(object input)
        {
            var inputtype = input.GetType();
            var inputinfo = TagStructure.GetTagStructureInfo(inputtype, destCacheInfo.Version, destCacheInfo.Platform);
            foreach (var tagFieldInfo in TagStructure.GetTagFieldEnumerable(inputinfo.Types[0], inputinfo.Version, inputinfo.CachePlatform))
            {
                if (tagFieldInfo.FieldType.IsGenericType && tagFieldInfo.FieldType.GetGenericTypeDefinition() == typeof(List<>))
                {
                    object newlist = Activator.CreateInstance(tagFieldInfo.FieldType);
                    tagFieldInfo.SetValue(input, newlist);
                }
            }
        }
    }
}
