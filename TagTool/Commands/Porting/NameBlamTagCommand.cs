using System;
using System.Collections.Generic;
using System.Linq;
using TagTool.Cache;

namespace TagTool.Commands.Porting
{
    class NameBlamTagCommand : Command
    {
        private HaloOnlineCacheContext CacheContext { get; }
        private CacheFile BlamCache { get; }

        public NameBlamTagCommand(HaloOnlineCacheContext cacheContext, CacheFile blamCache) :
            base(true,
                
                "NameBlamTag",
                "Sets the name of the specified tag in the current external cache.",
                
                "NameBlamTag <Tag> <Name>",

                "Sets the name of the specified tag in the current external cache.")
        {
            CacheContext = cacheContext;
            BlamCache = blamCache;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 2)
                return false;

            var tagSpecifier = args[0];

            if (!tagSpecifier.Contains('.'))
                throw new ArgumentException("<Tag>", new FormatException(tagSpecifier));

            var newTagName = args[1];

            if (newTagName.Contains('.'))
                throw new ArgumentException("<Name>", new FormatException(newTagName));

            var tagIdentifiers = tagSpecifier.Split('.');

            if (!CacheContext.TryParseGroupTag(tagIdentifiers[1], out var groupTag))
                throw new FormatException($"Invalid tag name: {tagSpecifier}");

            var tagName = tagIdentifiers[0];

            var result = BlamCache.IndexItems.Find(
                item => item != null && groupTag == item.GroupTag && tagName == item.Name);

            if (result == null)
                throw new FormatException($"Invalid tag name: {tagSpecifier}");

            result.Name = newTagName;

            Console.WriteLine($"\"{tagSpecifier}\" renamed to \"{newTagName}.{tagIdentifiers[1]}\"");
            
            return true;
        }
    }
}
