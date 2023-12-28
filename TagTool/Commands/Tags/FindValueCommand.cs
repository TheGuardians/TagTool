using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TagTool.Cache;
using TagTool.Cache.Monolithic;
using TagTool.Commands.Common;
using TagTool.Common;
using TagTool.Tags;

namespace TagTool.Commands.Tags
{
    public class FindValueCommand : Command
    {
        public GameCache Cache;
        public CachedTag Tag;

        public FindValueCommand(GameCache cache, CachedTag tag) : base(false,

            "FindValue",
            "Performs a deep search for a field value.",

            "FindValue <phrase> <group>",

            "Performs a deep search for field values that contain the provided phrase."
            + "\nOptionally specify the short name of a tag group to search within (e.g. hlmt)."
            + "\nWhen used within an edited tag, search will be performed within that tag only.")
        {
            Cache = cache;
            Tag = tag;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count < 1 || args.Count > 2)
                return new TagToolError(CommandError.ArgCount);

            var phrase = args[0].ToLower().Trim();
            var tagSet = Cache.TagCache.NonNull();

            if (args.Count == 2)
            {
                if (!Cache.TagCache.TryParseGroupTag(args[1], out var tagGroup))
                    return new TagToolError(CommandError.CustomError, $"\"{args[1]}\" is not a valid tag group.");
                else
                    tagSet = Cache.TagCache.NonNull().Where(t => t.IsInGroup(tagGroup));
            }
            else if (Tag != null)
                tagSet = new List<CachedTag>() { Tag };
                

            Console.WriteLine("Searching...\n");

            //monolithic cache doesn't play well with async due to taglayouts
            if (Cache is GameCacheMonolithic)
                foreach (var tag in tagSet)
                    PerformSearch(tag, phrase);
            else
                Parallel.ForEach(tagSet, tag => PerformSearch(tag, phrase));

            Console.WriteLine("\nFinished.");
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

                string outputPrefix = $"{ShortTagName(tag)} :: ";

                if (Tag != null)
                    outputPrefix = "";

                switch (data)
                {
                    case StringId stringId:
                        {
                            try
                            {
                                var stringValue = Cache.StringTable.GetString(stringId);
                                if (stringValue.ToLower().Contains(phrase))
                                    Console.WriteLine($"{outputPrefix}{path} = {stringValue}");
                            }
                            catch
                            {
                                new TagToolWarning($"{outputPrefix}{path} invalid string id found! {path}");
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
                                Console.WriteLine($"{outputPrefix}{path} = {stringValue}");
                        }
                        break;
                }
            }
        }
    }
}
