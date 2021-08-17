using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TagTool.Cache;

namespace TagTool.Commands.Common
{
    public class ExecuteCSharpCommand : Command
    {
        private static ScriptState<object> State;
        private static EvaluationContext Context = new EvaluationContext();

        public ExecuteCSharpCommand(GameCache cache, CachedTag tag = null, object definition = null, object element = null) :
            base(false, 

                "CS",
                "Compile and evaluate csharp code", 

                "CS [code]",

                "CS - Start an interactive shell.\n" +
                "CS <statement> - Executes the given statement.\n" +
                "CS < \"path to .cs file\" [Arguments] - Executes the given file.\n" +
                "CS ! - Clear the current state\n\n" +

                "Globals:\n" +
                "Args - The list of arguments that were passed to the script file.\n" +
                "Cache - The current cache.\n" + 
                "Definition - The current tag definition. (EditTag)\n" +
                "Tag - The current tag. (EditTag)\n" +
                "Element - The current block element. (EditBlock)\n"
                )
        {
            Context.Cache = cache;
            Context.Definition = definition;
            Context.Element = element;
            Context.Tag = tag;
        }

        public override object Execute(List<string> args)
        {
            Context.Args.Clear();

            if (args.Count == 0)
            {
                // when no arguments are given, create an interactive shell

                Console.WriteLine("C# Shell Started.");
                Console.WriteLine("Type :x to Execute, :q to Quit.");
                string lines = "";

                bool quit;
                while (true)
                {
                    Console.Write($"> ");
                    var line = Console.ReadLine();
                    if (quit = line == ":q")
                        break;
                    if (line == ":x")
                        break;

                    lines += $"{line}\r\n";
                }

                if (!quit)
                    EvaluateScript(lines);
            }
            else if (args.Count > 1 && args[0].Trim() == "<")
            {
                // pipe a script file e.g   cs << "path to script"

                var scriptFile = new FileInfo(args[1]);
                if (!scriptFile.Exists)
                    return new TagToolError(CommandError.FileNotFound, scriptFile.FullName);

                args.RemoveRange(0, 2);
                Context.Args = args;

                var input = File.ReadAllText(scriptFile.FullName);
                EvaluateScript(input);
            }
            else if (args.Count == 1 && args[0].Trim() == "!")
            {
                // clear the state
                State = null;
                Console.Write("State cleared.");
            }
            else
            {
                // quick REPL

                var input = CommandRunner.CommandLine;
                // remove command name from the input
                input = input.Substring(input.IndexOf(' ') + 1);
                EvaluateScript(input);
            }

            return true;
        }

        public static object EvaluateScript(string input, bool inline = false)
        {
            var scriptOptions = ScriptOptions.Default
                .WithAllowUnsafe(true)
                .WithOptimizationLevel(Microsoft.CodeAnalysis.OptimizationLevel.Debug)
                .WithReferences(typeof(ExecuteCSharpCommand).Assembly)
                .WithImports(
                    "System",
                    "System.IO",
                    "System.Text",
                    "System.Linq",
                    "System.Collections.Generic",
                    "System.Collections",
                    "TagTool",
                    "TagTool.Common",
                    "TagTool.IO",
                    "TagTool.Serialization",
                    "TagTool.Extensions",
                    "TagTool.Cache",
                    "TagTool.Tags",
                    "TagTool.Tags.Definitions",
                    "TagTool.Tags.Resources",
                    "TagTool.Commands",
                    "TagTool.Commands.Common");
               

            try
            {
                State = State == null ?
                    CSharpScript.RunAsync(input, scriptOptions, Context).Result :
                    State.ContinueWithAsync(input, scriptOptions).Result;

                if (State.ReturnValue != null)
                {
                    if (!inline)
                        PrintReplResult(State.ReturnValue);

                    return State.ReturnValue;
                }
            }
            catch (Exception ex)
            {
                // We generally just want to crash here as we don't have anything meanful to return.
                // null could continue on for a while without issue depending on the commands being executed.
                if (inline)
                    throw;

                new TagToolError(CommandError.CustomError, ex.ToString());
            }

            return null;
        }

        private static void PrintReplResult(object value)
        {
            // serialize before setting the color in order to prevent the console color not getting reset
            // in the event of a crash
            var valueStr = value.ToString();
            var previousColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(valueStr);
            Console.ForegroundColor = previousColor;
        }

        public static string EvaluateInlineExpressions(string input, int offset = 0)
        {
            int startIndex = -1;
            var stack = new Stack<int>();

            for (int i = 1; i < input.Length; i++)
            {
                if (input[i] == '{')
                {
                    stack.Push(i - 1);
                    if (startIndex == -1 && input[i - 1] == '^')
                    {
                        startIndex = i - 1;
                    }
                }
                else if (input[i] == '}' && stack.Count > 0)
                {
                    if (startIndex == stack.Peek())
                    {
                        var before = input.Substring(0, startIndex);
                        string after = EvaluateInlineExpressions(input.Substring(i + 1), offset + i + 1);
                        if (after == null)
                            return null;

                        var expression = input.Substring(startIndex + 2, i - startIndex -2);
                        return before + EvaluateScript(expression, true) + after;
                    }

                    stack.Pop();
                }
            }

            if (stack.Count != 0 && startIndex != -1)
            {
                new TagToolError(CommandError.CustomError, $"(0:{offset + startIndex + 1}): Unmatched brace.");
                return null;
            }

            return input;
        }

        public class EvaluationContext
        {
            // We want to use weak references as we don't own these objects and should not
            // participate in their lifetime. If we didn't, we'd have to have code that
            // sets them to null whenever a context is popped, or it would leak.
            private WeakReference<GameCache> _cache;
            private WeakReference<dynamic> _definition;
            private WeakReference<dynamic> _element;
            private WeakReference<CachedTag> _tag;

            public List<string> Args = new List<string>();

            public GameCache Cache
            {
                get => GetValue(_cache);
                set => SetValue(ref _cache, value);
            }

            public dynamic Definition
            {
                get => GetValue(_definition);
                set => SetValue(ref _definition, value);
            }

            public dynamic Element
            {
                get => GetValue(_element);
                set => SetValue(ref _element, value);
            }

            public dynamic Tag
            {
                get => GetValue(_tag);
                set => SetValue(ref _tag, value);
            }

            private void SetValue<T>(ref WeakReference<T> reference, T value) where T : class
            {
                reference = new WeakReference<T>(value);
            }

            private T GetValue<T>(WeakReference<T> reference) where T : class
            {
                if (reference.TryGetTarget(out var value))
                    return value;
                else
                    return default(T);
            }
        }
    }
}
