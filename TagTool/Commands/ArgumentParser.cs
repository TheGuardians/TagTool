using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using TagTool.Tags;

namespace TagTool.Commands
{
    public static class ArgumentParser
    {
        public static List<string> ParseCommand(string command, out string redirectFile)
        {
            var results = new List<string>();
            var currentArg = new StringBuilder();
            var partStart = -1;
            var quoted = false;
            var redirectStart = -1;
            redirectFile = null;
            for (var i = 0; i < command.Length; i++)
            {
                switch (command[i])
                {
                    case '>':
                        if (quoted)
                            goto default; // Treat like a normal char when quoted
                        redirectStart = (partStart != -1) ? results.Count + 1 : results.Count;
                        goto case ' '; // Treat like a space
                    case ' ':
                        if (quoted)
                            goto default; // Treat like a normal char when quoted
                        if (partStart != -1)
                            currentArg.Append(command.Substring(partStart, i - partStart));
                        if (currentArg.Length > 0)
                        {
                            var arg = currentArg.ToString();
                            results.Add(arg);
                        }
                        currentArg.Clear();
                        partStart = -1;
                        break;
                    case '"':
                        quoted = !quoted;
                        if (partStart != -1)
                            currentArg.Append(command.Substring(partStart, i - partStart));
                        partStart = -1;
                        break;
                    default:
                        if (partStart == -1)
                            partStart = i;
                        break;
                }
            }
            if (partStart != -1)
                currentArg.Append(command.Substring(partStart));
            if (currentArg.Length > 0)
            {
                var arg = currentArg.ToString();
                results.Add(arg);
            }
            if (redirectStart >= 0 && redirectStart < results.Count)
            {
                redirectFile = string.Join(" ", results.Skip(redirectStart));
                results.RemoveRange(redirectStart, results.Count - redirectStart);
            }
            return results;
        }

        public static CachedTagInstance ParseTagName(HaloOnlineCacheContext info, string name)
        {
            if (name.Length == 0 || !char.IsLetter(name[0]) || !name.Contains('.'))
                throw new Exception($"Invalid tag name: {name}");

            var namePieces = name.Split('.');

            var groupTag = ParseGroupTag(info.StringIdCache, namePieces[1]);
            if (groupTag == Tag.Null)
                throw new Exception($"Invalid tag name: {name}");

            var tagName = namePieces[0];
            
            foreach (var nameEntry in info.TagNames)
            {
                if (nameEntry.Value == tagName)
                {
                    var instance = info.TagCache.Index[nameEntry.Key];

                    if (instance.Group.Tag == groupTag)
                        return instance;
                }
            }

            Console.WriteLine($"Invalid tag name: {name}");
            return null;
        }

        public static CachedTagInstance ParseTagSpecifier(HaloOnlineCacheContext info, string arg)
        {
            if (arg.Length == 0 || !(arg == "*" || arg == "null" || char.IsLetter(arg[0]) || arg.StartsWith("0x")))
            {
                Console.WriteLine($"Invalid tag index specifier: {arg}");
                return null;
            }

            if (arg == "*")
                return info.TagCache.Index.Last();
            else if (arg == "null")
                return null;
            else if (char.IsLetter(arg[0]))
                return ParseTagName(info, arg);
            else if (arg.StartsWith("0x"))
                arg = arg.Substring(2);

            if (!int.TryParse(arg, NumberStyles.HexNumber, null, out int tagIndex))
                return null;

            if (!info.TagCache.Index.Contains(tagIndex))
                return null;

            return info.TagCache.Index[tagIndex];
        }

        public static Tag ParseGroupTag(StringIdCache stringIDs, string groupName)
        {
            if (TagDefinition.Exists(groupName))
                return new Tag(groupName);

            foreach (var pair in TagGroup.Instances)
            {
                if (groupName == stringIDs.GetString(pair.Value.Name))
                    return pair.Value.Tag;
            }

            return Tag.Null;
        }

        public static List<Tag> ParseGroupTags(StringIdCache stringIDs, IEnumerable<string> classNames)
        {
            var searchClasses = classNames.Select(a => ParseGroupTag(stringIDs, a)).ToList();

            return (searchClasses.Any(c => c.Value == -1)) ? null : searchClasses;
        }

        public static bool TryParseEnum<TEnum>(string name, out TEnum result)
            where TEnum : struct
        {
            if (Enum.TryParse(name, out result))
                return true;

            var names = Enum.GetNames(typeof(TEnum)).ToList();

            var nameLow = name.ToLower();
            var namesLow = names.Select(i => i.ToLower()).ToList();

            var found = namesLow.Find(n => n == nameLow);

            if (found != null)
                return Enum.TryParse(names[namesLow.IndexOf(nameLow)], out result);

            var nameSnake = name.ToSnakeCase();
            var namesSnake = names.Select(i => i.ToSnakeCase()).ToList();

            found = namesSnake.Find(n => n == nameSnake);

            if (found != null)
                return Enum.TryParse(names[namesSnake.IndexOf(nameSnake)], out result);

            return false;
        }
    }
}