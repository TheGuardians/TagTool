using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TagTool.Commands.Common
{
    public class TagToolChoicePrompt
    {
        private readonly List<string> ChoiceList;
        private readonly string Indent;
        private readonly string EnterKeyOption;

        /// <summary>
        /// Prompts user to pick from list of up to 36 choices
        /// </summary>
        /// <param name="choiceList">String List/Array of choices</param>
        /// <param name="enterKeyOption">e.g. "Enter to select all." or "Enter for more options."</param>
        /// <param name="indent">The amount of spaces to indent output text by.</param>
        /// <returns>Returns index of chosen item or -1 for Enter Key option.</returns>
        public TagToolChoicePrompt(List<string> choiceList, string enterKeyOption = null, int indent = 0)
        {
            ChoiceList = choiceList.GetRange(0, choiceList.Count < 36 ? choiceList.Count : 36);
            Indent = new string(' ', indent);
            EnterKeyOption = enterKeyOption;
        }

        public TagToolChoicePrompt(string[] choiceList, string enterKeyOption = null, int indent = 0) :
            this(choiceList.ToList(), enterKeyOption, indent) { }

        public int Prompt()
        {
            int choice = -1;

            Console.WriteLine($"\n{Indent}Please make a selection:");

            ChoiceList.Select((v, i) =>
                $"{Indent}  [{Convert.ToChar((i < 10) ? i + 48 : i + 55)}] {v}")
                .ToList().ForEach(Console.WriteLine);
            
            if (EnterKeyOption != null)
                Console.WriteLine($"\n{Indent}  {EnterKeyOption}");

            var outBackup = Console.Out;
            Console.SetOut(StreamWriter.Null);

            AwaitInput:
                var input = Console.ReadKey();
                int key = (int)input.Key;

                if (key >= 48 && key <= 57 && key - 48 < ChoiceList.Count())
                    choice = key - 48;
                else if (key >= 65 && key <= 90 && key - 55 < ChoiceList.Count())
                    choice = key - 55;
                else if (!(input.Key == ConsoleKey.Enter && EnterKeyOption != null))
                    goto AwaitInput;

            Console.SetOut(outBackup);
            //Console.WriteLine($"\n{Indent}[{input.KeyChar.ToString().ToUpper()}] selected...");

            return choice;
        }
    }
}