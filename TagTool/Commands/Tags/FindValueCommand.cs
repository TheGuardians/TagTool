using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TagTool.Cache;
using TagTool.Commands.Common;
using TagTool.Common;
using TagTool.Tags;

namespace TagTool.Commands.Tags
{
    public class FindValueCommand : Command
    {
        public GameCache Cache;

        public FindValueCommand(GameCache cache) : base(false,

            "FindValue",
            "Performs a deep search for a field value",

            "Search <phrase>",

            "Performs a deep search for a field value")
        {
            Cache = cache;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count < 1)
                return new TagToolError(CommandError.ArgCount);

            var phrase = args[0].ToLower().Trim();

            Console.WriteLine("Searching...");
            Parallel.ForEach(Cache.TagCache.NonNull(), tag => PerformSearch(tag, phrase));
            Console.WriteLine("Finished.");
            return true;
        }

        public void PerformSearch(CachedTag tag, string phrase)
        {
            using (var stream = Cache.OpenCacheRead())
            {
                var definition = Cache.Deserialize(stream, tag);
                DeepSearch(tag, definition, phrase);
            }
        }

        private static string ShortTagName(CachedTag tag)
        {
            return $"{tag.Name}.{tag.Group.Tag}";
        }

        private void DeepSearch(CachedTag tag, object definition, string phrase)
        {
            DeepSearch(definition);

            void DeepSearch(object data, string path = "")
            {
                if (data == null)
                    return;

                switch (data)
                {
                    case StringId stringId:
                        {
                            try
                            {
                                var stringValue = Cache.StringTable.GetString(stringId);
                                if (stringValue.ToLower().Contains(phrase))
                                    Console.WriteLine($"{ShortTagName(tag)} :: {path} = {stringValue}");
                            }
                            catch
                            {
                                new TagToolError(CommandError.CustomError, $"{ShortTagName(tag)} :: invalid string id found! {path}");
                            }
                        }
                        break;
                    case IList collection:
                        {
                            var elementType = collection is Array ? collection.GetType().GetElementType() : collection.GetType().GenericTypeArguments[0];
                            if (elementType == typeof(byte))
                                return;

                            for (int i = 0; i < collection.Count; i++)
                                DeepSearch(collection[i], $"{path}[{i}]");
                        }
                        break;
                    case TagStructure tagStruct:
                        {
                            foreach (var fieldInfo in tagStruct.GetTagFieldEnumerable(Cache.Version, Cache.Platform))
                                DeepSearch(fieldInfo.GetValue(tagStruct), path.Length > 0 ? $"{path}.{fieldInfo.Name}" : fieldInfo.Name);
                        }
                        break;
                    default:
                        {
                            var stringValue = data.ToString();
                            if (stringValue.ToLower().Contains(phrase))
                                Console.WriteLine($"{ShortTagName(tag)} :: {path} = {stringValue}");
                        }
                        break;
                }
            }
        }
    }
}
