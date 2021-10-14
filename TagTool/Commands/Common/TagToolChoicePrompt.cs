using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TagTool.Commands.Common
{
    public class TagToolChoicePrompt
    {
        public class Single
        {
            private readonly List<string> ChoiceList;
            private readonly string Indent;
            private readonly string SpaceKeyOption;

            /// <summary>
            /// Prompts user to pick from list of up to 36 choices
            /// </summary>
            /// <param name="choiceList">String List/Array of choices</param>
            /// <param name="spaceKeyOption">e.g. "Select all" or "More options"</param>
            /// <param name="indent">The amount of spaces to indent output text by</param>
            /// <returns>Prompt method returns index of chosen item or -1 for Enter Key option</returns>
            public Single(List<string> choiceList, string spaceKeyOption = null, int indent = 0)
            {
                ChoiceList = choiceList.GetRange(0, choiceList.Count < 36 ? choiceList.Count : 36);
                Indent = new string(' ', indent);
                SpaceKeyOption = spaceKeyOption;
            }

            /// <summary>
            /// Prompts user to pick from list of up to 36 choices
            /// </summary>
            /// <param name="choiceList">String List/Array of choices</param>
            /// <param name="spaceKeyOption">e.g. "Select all" or "More options"</param>
            /// <param name="indent">The amount of spaces to indent output text by</param>
            /// <returns>Prompt method returns index of chosen item or -1 for Enter Key option</returns>
            public Single(string[] choiceList, string spaceKeyOption = null, int indent = 0) :
                this(choiceList.ToList(), spaceKeyOption, indent) { }

            /// <summary>
            /// Prompts user for selection.
            /// </summary>
            /// <returns>Index of chosen item or -1 for Enter Key option</returns>
            public int Prompt()
            {
                Console.WriteLine();

                int choice = -1;

                ChoiceList.Select((v, i) =>
                    $"{Indent}  [{Convert.ToChar((i < 26) ? i + 65 : i + 22)}] {v}")
                    .ToList().ForEach(Console.WriteLine);

                if (SpaceKeyOption != null)
                    Console.WriteLine($"\n{Indent}  [SPACE] {SpaceKeyOption}");

                Console.Write($"\n{Indent}Please make a selection: ");

                var outBackup = Console.Out;
                Console.SetOut(StreamWriter.Null);

                AwaitInput:
                var input = Console.ReadKey();
                int key = (int)input.Key;

                if (key >= 48 && key <= 57 && key - 22 < ChoiceList.Count())
                    choice = key - 22;
                else if (key >= 65 && key <= 90 && key - 65 < ChoiceList.Count())
                    choice = key - 65;
                else if (!(input.Key == ConsoleKey.Spacebar && SpaceKeyOption != null))
                    goto AwaitInput;

                Console.SetOut(outBackup);
                Console.WriteLine($"{(choice < 0 ? "SPACE" : input.KeyChar.ToString().ToUpper())}");

                return choice;
            }
        }


        public class Multiple
        {
            private Dictionary<string, bool> ChoiceDict;
            private readonly string Indent;
            private readonly string SpaceKeyOption;
            private Enum ChoiceListOG = null;

            /// <summary>
            /// Prompts user to pick from list of up to 36 choices
            /// </summary>
            /// <param name="choiceList">String List/Array/Dictionary/Enum of choices</param>
            /// <param name="spaceKeyOption">e.g. "Select all" or "More options"</param>
            /// <param name="indent">The amount of spaces to indent output text by</param>
            /// <returns>Prompt method returns dictionary of items and their selected staus (true/false) or null for space key option</returns>
            public Multiple(Dictionary<string, bool> choiceList, string spaceKeyOption = null, int indent = 0)
            {
                ChoiceDict = choiceList.Take(36).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
                Indent = new string(' ', indent);
                SpaceKeyOption = spaceKeyOption;
            }

            /// <summary>
            /// Prompts user to pick from list of up to 36 choices
            /// </summary>
            /// <param name="choiceList">String List/Array/Dictionary/Enum of choices</param>
            /// <param name="spaceKeyOption">e.g. "Select all" or "More options"</param>
            /// <param name="indent">The amount of spaces to indent output text by</param>
            /// <returns>Prompt method returns either a dictionary of items and their selected staus (true/false) or null for space key option or fills an enum</returns>
            public Multiple(string[] choiceList, string spaceKeyOption = null, int indent = 0) : this(
                choiceList.Select(x => new KeyValuePair<string, bool>(x, false)).ToDictionary(kvp => kvp.Key, kvp => kvp.Value), spaceKeyOption, indent ) { }

            /// <summary>
            /// Prompts user to pick from list of up to 36 choices
            /// </summary>
            /// <param name="choiceList">String List/Array/Dictionary/Enum of choices</param>
            /// <param name="spaceKeyOption">e.g. "Select all" or "More options"</param>
            /// <param name="indent">The amount of spaces to indent output text by</param>
            /// <returns>Prompt method returns either a dictionary of items and their selected staus (true/false) or null for space key option or fills an enum</returns>
            public Multiple(List<string> choiceList, string spaceKeyOption = null, int indent = 0) : this(
                choiceList.Select(x => new KeyValuePair<string, bool>(x, false)).ToDictionary(kvp => kvp.Key, kvp => kvp.Value), spaceKeyOption, indent ) { }

            /// <summary>
            /// Prompts user to pick from list of up to 36 choices
            /// </summary>
            /// <param name="choiceList">String List/Array/Dictionary/Enum of choices</param>
            /// <param name="spaceKeyOption">e.g. "Select all" or "More options"</param>
            /// <param name="indent">The amount of spaces to indent output text by</param>
            /// <returns>Prompt method returns either a dictionary of items and their selected staus (true/false) or null for space key option or fills an enum</returns>
            public Multiple(Enum choiceList, string spaceKeyOption = null, int indent = 0)
            {
                var type = choiceList.GetType();
                var names = Enum.GetNames(type);
                ChoiceDict = names.Select((x, i) => new KeyValuePair<string, bool>(
                    x.ToString(), choiceList.HasFlag((Enum)Enum.Parse(type, x.ToString()))
                    )).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
                Indent = new string(' ', indent);
                SpaceKeyOption = spaceKeyOption;
                ChoiceListOG = choiceList;
            }

            /// <summary>
            /// Prompts user for selection
            /// </summary>
            /// <returns>Dictionary&lt;string, bool&gt; representing the selection of the user</returns>
            public Dictionary<string, bool> Prompt()
            {
                Console.WriteLine();
                
            AwaitInput:
                
                ChoiceDict.Select((p, i) =>
                    $"{Indent}  [{Convert.ToChar((i < 26) ? i + 65 : i + 22)}][{(p.Value ? "X" : " ")}] {p.Key}")
                    .ToList().ForEach(Console.WriteLine);

                if (SpaceKeyOption != null)
                    Console.WriteLine($"\n{Indent}  [SPACE] {SpaceKeyOption}");

                Console.WriteLine($"\n{Indent}Press Enter to finalise...");

                var outBackup = Console.Out;
                Console.SetOut(StreamWriter.Null);

                var input = Console.ReadKey();
                int key = (int)input.Key;

                if (key >= 48 && key <= 57 && key - 22 < ChoiceDict.Count())
                    ChoiceDict[ChoiceDict.ElementAt(key - 22).Key] = !ChoiceDict[ChoiceDict.ElementAt(key - 22).Key];
                else if (key >= 65 && key <= 90 && key - 65 < ChoiceDict.Count())
                    ChoiceDict[ChoiceDict.ElementAt(key - 65).Key] = !ChoiceDict[ChoiceDict.ElementAt(key - 65).Key];

                Console.SetOut(outBackup);

                if (input.Key == ConsoleKey.Enter)
                    return ChoiceDict;//.Where(x => x.Value == true).Select(x => x.Key).ToArray();
                else if (input.Key == ConsoleKey.Spacebar && SpaceKeyOption != null)
                    return null;

                Console.SetCursorPosition(0, Console.CursorTop - (ChoiceDict.Count + 2));
                goto AwaitInput;
            }

            /// <summary>
            /// Prompts user for selection
            /// </summary>
            /// <returns>Returns a flag enum set as the user selected</returns>
            public void Prompt(out Enum returnEnum)
            {
                if (!ChoiceListOG.GetType().IsDefined(typeof(FlagsAttribute), false))
                {
                    new TagToolWarning("Enum is not of flags type, so multiple values cannot be set.");
                    returnEnum = null;
                }
                var list = String.Join(",", new Multiple(ChoiceListOG).Prompt().Where(x => x.Value == true).Select(x => x.Key).ToArray());
                returnEnum = (Enum)Enum.Parse(ChoiceListOG.GetType(), list);
            }
        }

        public class YN
        {
            private readonly string Question;
            private readonly string Indent;

            /// <summary>
            /// Prompts user for yes or no response
            /// </summary>
            /// <param name="question">Question to ask</param>
            /// <param name="indent">The amount of spaces to indent output text by</param>
            public YN(string question = null, int indent = 0)
            {
                Indent = new string(' ', indent);
                Question = question;
            }
            public bool Prompt()
            {
                Console.WriteLine($"\n{Indent}{Question}");
                AwaitInput:
                var input = Console.ReadKey();
                if (!(input.Key == ConsoleKey.Y || input.Key == ConsoleKey.N))
                    goto AwaitInput;

                Console.WriteLine();
                return input.Key == ConsoleKey.Y;
            }
        }
    }
}