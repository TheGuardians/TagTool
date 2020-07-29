using System;
using System.Collections.Generic;
using System.Linq;
using TagTool.Commands.Common;

namespace TagTool.Commands.Porting
{
    class DoNotReplaceGroupsCommand : Command
    {
        public DoNotReplaceGroupsCommand()
               : base(true,

                     "DoNotReplaceGroups",
                     "Prevents the specified tag groups from being replaced when porting tags.",

                     "DoNotReplaceGroups [remove] <tag groups>",

                     "Prevents the specified tag groups from being replaced when porting tags.\n" +
                     "Tag group format: \"grp1,grp2,grp3\". Use \"all\" to clear the list.")
        {
            UserDefinedDoNotReplaceGroups = new List<string>();
        }

        public static List<string> UserDefinedDoNotReplaceGroups = new List<string>();

        public override object Execute(List<string> args)
        {
            if (args.Count == 0)
            {
                Console.WriteLine("Current groups: " + string.Join(", ", UserDefinedDoNotReplaceGroups));
                return true;
            }
            else if (args.Count < 1 || args.Count > 2)
                return new TagToolError(CommandError.ArgCount);

            bool adding = true;
            if (args.Count == 2)
            {
                if (args[0].ToLower() == "remove")
                {
                    adding = false;
                    if (args[1] == "all")
                    {
                        UserDefinedDoNotReplaceGroups.Clear();
                        return true;
                    }
                    args.RemoveAt(0);
                }
                else
                {
                    return new TagToolError(CommandError.ArgInvalid);
                }
            }

            List<string> groups = args[0].Split(',').ToList();

            foreach (var group in groups)
            {
                string tempGroup = group;
                while (tempGroup.Length < 4)
                    tempGroup += " ";

                if (adding && !UserDefinedDoNotReplaceGroups.Contains(tempGroup))
                    UserDefinedDoNotReplaceGroups.Add(tempGroup);
                else if (!adding && UserDefinedDoNotReplaceGroups.Contains(tempGroup))
                    UserDefinedDoNotReplaceGroups.Remove(tempGroup);
            }

            return true;
        }
    }
}

