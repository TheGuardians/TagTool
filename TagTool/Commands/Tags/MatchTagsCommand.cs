using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Serialization;
using TagTool.Tags.Definitions;
using TagTool.Commands.Common;
using TagTool.Tags;

namespace TagTool.Commands.Tags
{
    // TODO: This code is shit, clean it up
    class MatchTagsCommand : Command
    {
        private static readonly int[] MapIdsToCompare =
        {
            0,   // mainmenu
            320, // guardian
            340, // riverworld
            705, // s3d_avalanche
            703, // s3d_edge
            700, // s3d_reactor
            31,  // s3d_turf
            390, // cyberdyne
            380, // chill
            410, // bunkerworld
            30,  // zanzibar
            310, // deadlock
            400, // shrine
        };

        private HaloOnlineCacheContext CacheContext { get; }

        public MatchTagsCommand(HaloOnlineCacheContext cacheContext) : base(
            true,

            "MatchTags",
            "Find equivalent tags in different engine versions",

            "MatchTags <output csv> <tags.dat...>",

            "The tags in the current tag cache will be compared with the tags in each of the\n" +
            "listed tags.dat files to find tags that are the same in all of them. Results\n" +
            "will be written to a CSV which can be used to convert tags between the\n" +
            "different versions.")
        {
            CacheContext = cacheContext;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count < 2)
                return false;
            var outputPath = args[0];

            // Load each file and do version detection
            var infos = new List<HaloOnlineCacheContext>();
            foreach (var path in args.Skip(1))
            {
                Console.WriteLine("Loading {0}...", path);

                // Load the cache file
                var cacheContext = new HaloOnlineCacheContext(new FileInfo(path).Directory);
                infos.Add(cacheContext);
            }

            var result = new TagVersionMap();
            using (var baseStream = CacheContext.OpenTagCacheRead())
            {
                // Get the scenario tags for this cache
                Console.WriteLine("Finding base scenario tags...");
                var baseScenarios = FindScenarios(CacheContext, baseStream);
                var baseVersion = CacheContext.Version;
                var baseTagData = new Dictionary<int, object>();
                foreach (var scenario in baseScenarios)
                    baseTagData[scenario.Tag.Index] = scenario.Data;

                // Now compare with each of the other caches
                foreach (var info in infos)
                {
                    using (var stream = info.OpenTagCacheRead())
                    {
                        Console.WriteLine("Finding scenario tags in {0}...", info.TagCacheFile.FullName);

                        // Get the scenario tags and connect them to the base tags
                        var scenarios = FindScenarios(info, stream);
                        var tagsToCompare = new Queue<QueuedTag>();
                        for (var i = 0; i < scenarios.Count; i++)
                        {
                            tagsToCompare.Enqueue(scenarios[i]);
                            if (i < baseScenarios.Count)
                                result.Add(baseVersion, baseScenarios[i].Tag.Index, info.Version, scenarios[i].Tag.Index);
                        }

                        // Process each tag in the queue, enqueuing all of its dependencies as well
                        while (tagsToCompare.Count > 0)
                        {
                            // Get the tag and its data
                            var tag = tagsToCompare.Dequeue();
                            TagPrinter.PrintTagShort(tag.Tag);
                            var data = tag.Data;
                            if (data == null)
                            {
                                // No data yet - deserialize it
                                var context = new TagSerializationContext(stream, info, tag.Tag);
                                var type = TagDefinition.Find(tag.Tag.Group.Tag);
                                data = info.Deserializer.Deserialize(context, type);
                            }

                            // Now get the data for the base tag
                            var baseTag = result.Translate(info.Version, tag.Tag.Index, baseVersion);
                            if (baseTag == -1 || CacheContext.TagCache.Index[baseTag].Group.Tag != tag.Tag.Group.Tag)
                                continue;
                            if (!baseTagData.TryGetValue(baseTag, out object baseData))
                            {
                                // No data yet - deserialize it
                                var context = new TagSerializationContext(baseStream, CacheContext, CacheContext.TagCache.Index[baseTag]);
                                var type = TagDefinition.Find(tag.Tag.Group.Tag);
                                baseData = CacheContext.Deserializer.Deserialize(context, type);
                                baseTagData[baseTag] = baseData;
                            }

                            // Compare the two blocks
                            CompareBlocks(baseData, baseVersion, data, info.Version, result, tagsToCompare);
                        }
                    }
                }
            }

            // Write out the CSV
            Console.WriteLine("Writing results...");
            using (var writer = new StreamWriter(File.Open(outputPath, FileMode.Create, FileAccess.Write)))
                result.WriteCsv(writer);

            Console.WriteLine("Done!");
            return true;
        }

        private static void CompareBlocks(object dataA, CacheVersion versionA, object dataB, CacheVersion versionB, TagVersionMap result, Queue<QueuedTag> tagQueue)
        {
            if (dataA == null || dataB == null)
                return;
            var type = dataA.GetType();
            if (type == typeof(CachedTagInstance))
            {
                // If the objects are tags, then we've found a match
                var tagA = (CachedTagInstance)dataA;
                var tagB = (CachedTagInstance)dataB;
                if (tagA.Group.Tag != tagB.Group.Tag)
                    return;
                if (tagA.IsInGroup("rmt2") || tagA.IsInGroup("rmdf") || tagA.IsInGroup("vtsh") || tagA.IsInGroup("pixl") || tagA.IsInGroup("rm  ") || tagA.IsInGroup("bitm"))
                    return;
                var translated = result.Translate(versionA, tagA.Index, versionB);
                if (translated >= 0)
                    return;
                result.Add(versionA, tagA.Index, versionB, tagB.Index);
                tagQueue.Enqueue(new QueuedTag { Tag = tagB });
            }
            else if (type.IsArray)
            {
                if (type.GetElementType().IsPrimitive)
                    return;

                // If the objects are arrays, then loop through each element
                var arrayA = (Array)dataA;
                var arrayB = (Array)dataB;
                if (arrayA.Length != arrayB.Length)
                    return; // If the sizes are different, we probably can't compare them
                for (var i = 0; i < arrayA.Length; i++)
                    CompareBlocks(arrayA.GetValue(i), versionA, arrayB.GetValue(i), versionB, result, tagQueue);
            }
            else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
            {
                if (type.GenericTypeArguments[0].IsPrimitive)
                    return;

                // If the objects are lists, then loop through each element
                var countProperty = type.GetProperty("Count");
                var countA = (int)countProperty.GetValue(dataA);
                var countB = (int)countProperty.GetValue(dataB);
                if (countA != countB)
                    return; // If the sizes are different, we probably can't compare them
                var getItem = type.GetMethod("get_Item");
                for (var i = 0; i < countA; i++)
                {
                    var itemA = getItem.Invoke(dataA, new object[] { i });
                    var itemB = getItem.Invoke(dataB, new object[] { i });
                    CompareBlocks(itemA, versionA, itemB, versionB, result, tagQueue);
                }
            }
            else if (type.GetCustomAttributes(typeof(TagStructureAttribute), false).Length > 0)
            {
				var tagFieldInfosA = ReflectionCache.GetTagFieldEnumerable(dataA.GetType(), versionA);
				var tagFieldInfosB = ReflectionCache.GetTagFieldEnumerable(dataB.GetType(), versionB);

				// The objects are structures
				for (int a = 0, b = 0; a < tagFieldInfosA.Count && b < tagFieldInfosB.Count; a++, b++)
				{
					var tagFieldInfoA = tagFieldInfosA[a];
					var tagFieldInfoB = tagFieldInfosB[b];

					// Keep going on the left until the field is on the right
					while (!CacheVersionDetection.IsBetween(versionB, tagFieldInfoA.Attribute.MinVersion, tagFieldInfoA.Attribute.MaxVersion))
						if (++a < tagFieldInfosA.Count)
							tagFieldInfoA = tagFieldInfosA[a];
						else
							return;

					// Keep going on the right until the field is on the left
					while (!CacheVersionDetection.IsBetween(versionA, tagFieldInfoB.Attribute.MinVersion, tagFieldInfoB.Attribute.MaxVersion))
						if (++b < tagFieldInfosB.Count)
							tagFieldInfoB = tagFieldInfosB[b];
						else
							return;

					if (tagFieldInfoA.MetadataToken != tagFieldInfoB.MetadataToken)
						throw new InvalidOperationException("WTF, left and right fields don't match!");

					// Process the fields
					var fieldDataA = tagFieldInfoA.GetValue(dataA);
					var fieldDataB = tagFieldInfoB.GetValue(dataB);
					CompareBlocks(fieldDataA, versionA, fieldDataB, versionB, result, tagQueue);
				}
			}
        }

        private static List<QueuedTag> FindScenarios(HaloOnlineCacheContext info, Stream stream)
        {
            // Get a dictionary of scenarios by map ID
            var scenarios = new Dictionary<int, QueuedTag>();
            foreach (var scenarioTag in info.TagCache.Index.FindAllInGroup("scnr"))
            {
                var context = new TagSerializationContext(stream, info, scenarioTag);
                var scenario = info.Deserializer.Deserialize<Scenario>(context);
                scenarios[scenario.MapId] = new QueuedTag { Tag = scenarioTag, Data = scenario };
            }
            
            var tags = from id in MapIdsToCompare
                       where scenarios.ContainsKey(id)
                       select scenarios[id];

            return tags.ToList();
        }

        private class QueuedTag
        {
            public CachedTagInstance Tag { get; set; }

            public object Data { get; set; }
        }
    }
}
