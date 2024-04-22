using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
                    case '>' when !quoted: // Treat like a normal char when quoted
                        redirectStart = (partStart != -1) ? results.Count + 1 : results.Count;
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
                    case ' ' when !quoted: // Treat like a normal char when quoted
                    case '\t' when !quoted:
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