using System;
using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Commands.Common;
using System.Linq;

namespace TagTool.Commands.Tags
{
    class ListTagsCommand : Command
    {
        private GameCache Cache { get; }

        public ListTagsCommand(GameCache cache)
            : base(true,

                  "ListTags",
                  "Lists tag instances that are of the specified tag groups.",

                  "ListTags [TagGroup]... {Children:True|FilterType:String}",

                  "e.g. ListTags scnr scenario_structure_bsp contains:unk\n" +
                  "e.g. ListTags ends:world sbsp scnr starts:levels\\dlc\n" +
                  "e.g. ListTags has: eli obje children: true has: equi\n\n" +

                  "- If no tag group is specified, all tags will be listed.\n" +
                  "- Accepts multiple has/contains filters to narrow results.\n" +
                  "- Multiple tag groups can be specified to search from.\n" +
                  "- Children:True matches tags inheriting from specified groups.")
        {
            Cache = cache;
        }

        public override object Execute(List<string> args)
        {
            var invalidArgs = new List<string>();
            var filter = (SearchLineage: false, TagGroups: new List<string>(), Start: "", Middle: new List<string>(), End: "");
            
            while (args.Count >= 1)
            {
                var option = args[0].ToLower().Split(':');
                args.RemoveAt(0);

                // Is tag group
                if (option.Length == 1)
                {
                    var tagGroup = Tag.Parse(Cache, option[0]);
                    if (tagGroup.IsNull())
                        invalidArgs.Add(option[0]);
                    else
                        filter.TagGroups.Add(tagGroup.ToString());
                }
                // Is option
                else
                {
                    // Grab option value from next arg if needed
                    if (option[1] == "")
                    {
                        if (args.Count == 0 || args[0].Contains(":"))
                        {
                            invalidArgs.Add(option[0]+":");
                            continue;
                        }
                        else
                        {
                            option[1] = args[0].ToLower();
                            args.RemoveAt(0);
                        }
                    }

                    // Fill filter lists
                    if (option[0].StartsWith("start") || option[0].StartsWith("begin"))
                        filter.Start = option[1];
                    else if (Array.IndexOf(new string[] { "filter", "name", "named", "contain", "contains", "containing", "has" }, option[0]) >= 0)
                        filter.Middle.Add(option[1]);
                    else if (option[0].StartsWith("end"))
                        filter.End = option[1];
                    else if (option[0] == "children" && option[1] == "true")
                        filter.SearchLineage = true;
                    else
                        invalidArgs.Add(option[0] + ":" + option[1]);
                }
            }

            
            // Warn of invalid tag groups/options
            if (invalidArgs.Count > 0)
                return new TagToolError(CommandError.ArgInvalid, $"\"{String.Join(", ", invalidArgs.ToArray())}\"");


            // Print matches
            foreach (var tag in Cache.TagCache.TagTable)
            {
                if (
                    tag is null ||
                    tag.Name is null ||
                    tag.Group is null ||
                    !tag.Name.StartsWith(filter.Start) ||
                    (filter.Middle.Count() > 0 && !filter.Middle.All(str => tag.Name.ToString().Contains(str))) ||
                    !tag.Name.EndsWith(filter.End)
                ) continue;

                if (filter.TagGroups.Count > 0)
                {
                    if (filter.SearchLineage)
                    {
                        var groupLineage = new List<String> {tag.Group.Tag.ToString(), tag.Group.ParentTag.ToString(), tag.Group.GrandParentTag.ToString()};
                        if (filter.TagGroups.Intersect(groupLineage).Count() == 0)
                            continue;
                    }
                    else
                    {
                        if (!filter.TagGroups.Contains(tag.Group.Tag.ToString()))
                            continue;
                    }
                }

                if (tag.Name == "") tag.Name = "[UNNAMED]";

                Console.WriteLine($"[Index: 0x{tag.Index:X4}, Offset: 0x{tag.DefinitionOffset:X8}] {tag.Name}.{tag.Group}");
            }
			
			return true;
        }
    }
}