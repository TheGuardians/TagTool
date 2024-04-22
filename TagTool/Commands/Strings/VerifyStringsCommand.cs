using System;
using System.Collections.Generic;
using TagTool.Cache;

namespace TagTool.Commands.Strings
{
    public class VerifyStringsCommand : Command
    {
        private GameCache Cache { get; }

        public VerifyStringsCommand(GameCache cache) : base(
            true,

            "VerifyStrings",
            "Verifies the string table is resolving properly",

            "VerifyStrings" +

            "Verifies the string table is resolving properly")
        {
            Cache = cache;
        }

        public override object Execute(List<string> args)
        {
            int errorCount = 0;

            Console.WriteLine("Verifying... This may take a while.");

            var stringTable = Cache.StringTable;
            for (int i = 1; i < stringTable.Count; i++)
            {
                var expected = stringTable[i];
                var id = stringTable.GetStringId(expected);
                var actual = stringTable.GetString(id);
                if (!expected.Equals(actual))
                {
                    errorCount++;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"{i}, {expected}, {actual}");
                    Console.ResetColor();
                }
            }

        
            Console.ForegroundColor = errorCount > 0 ? ConsoleColor.Red : ConsoleColor.Green;
            Console.WriteLine($"{errorCount} errors found.");
            Console.ResetColor();
            return true;
        }
    }
}
