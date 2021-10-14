using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using TagTool.Cache;

namespace TagTool.Commands.Common
{
    public class ExecuteCSharpCommand : Command
    {
        private static ScriptState<object> State;
        private CommandContextStack ContextStack;

        public static readonly object GlobalCacheKey = new object();
        public static readonly object GlobalPortingCacheKey = new object();
        public static readonly object GlobalTagKey = new object();
        public static readonly object GlobalDefinitionKey = new object();
        public static readonly object GlobalElementKey = new object();

        public ExecuteCSharpCommand(CommandContextStack contextStack) :
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
                "Element - The current block element. (EditBlock)\n" +
                "UserVars - Access vars set using SetVariable e.g. UserVars[\"my_var_name\"]\n"
                )
        {
            ContextStack = contextStack;
        }

        public override object Execute(List<string> args)
        {
            var evalContext = new EvaluationContext(ContextStack);

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
                    if (quit = line.TrimEnd() == ":q")
                        break;
                    if (line.TrimEnd() == ":x")
                        break;

                    lines += $"{line}\r\n";
                }

                if (!quit)
                    EvaluateScript(evalContext, lines);
            }
            else if (args.Count > 1 && args[0].Trim() == "<")
            {
                // pipe a script file e.g   cs << "path to script"

                var scriptFile = new FileInfo(args[1]);
                if (!scriptFile.Exists)
                    return new TagToolError(CommandError.FileNotFound, scriptFile.FullName);

                args.RemoveRange(0, 2);
                evalContext.Args = args;
                evalContext.ScriptFile = scriptFile;

                var input = File.ReadAllText(scriptFile.FullName);
                EvaluateScript(evalContext, input, inline: false, isolate: true, sourceDirectory: scriptFile.Directory);
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
                EvaluateScript(evalContext, input);
            }

            return true;
        }

        public static bool OutputIsRedirectable(List<string> args)
        {
            // allow piping output to a file iif the input is a piped file for now
            // as we can't easily disambiguate usage of > in expressions
            return args.Count > 0 && args[0] == "<";
        }

        private static ScriptPreprocessResult PreprocessScript(string script)
        {
            var result = new ScriptPreprocessResult();

            var lines = script.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None)
                .Select(line => line.Trim())
                .ToArray();

            for (int i = 0; i < lines.Length; i++)
            {
                var line = lines[i];

                if (line.StartsWith("#"))
                {
                    var directive = line.Substring(1, line.IndexOf(' ') - 1).Trim();
                    switch (directive)
                    {
                        case "import":
                            var importName = line.Substring(line.IndexOf(' ') + 1).Trim();
                            result.Imports.Add(importName);
                            // comment the line to keep the line numbers intact  
                            line = $"// {line}";
                            break;
                    }

                }

                result.Source += $"{line}\n";
            }

            return result;
        }

        public static object EvaluateScript(EvaluationContext evalContext, string input, bool inline = false, DirectoryInfo sourceDirectory = null, bool isolate = false)
        {
            if (sourceDirectory == null)
                sourceDirectory = new DirectoryInfo(Program.TagToolDirectory);

            var preprocessResult = PreprocessScript(input);

            var references = new List<Assembly>();
            references.Add(typeof(ExecuteCSharpCommand).Assembly);

            foreach (var importName in preprocessResult.Imports)
            {
                try
                {
                    var assemblyName = FindAssembly(sourceDirectory, importName);
                    if (assemblyName == null)
                        throw new FileNotFoundException("Failed to find assembly");

                    var assembly = Assembly.Load(assemblyName);
                    references.Add(assembly);
                }
                catch (Exception ex)
                {
                    new TagToolError(CommandError.CustomError, $"Failed to load assembly `{importName}`. {ex.Message}");
                }
            }

            var scriptOptions = ScriptOptions.Default
                .WithAllowUnsafe(true)
                .WithOptimizationLevel(Microsoft.CodeAnalysis.OptimizationLevel.Debug)
                .WithReferences(references)
                .WithEmitDebugInformation(true)
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
                    "TagTool.Commands.Common"
                 );


            try
            {
                ScriptState<object> newState = null;
                if (State == null || isolate)
                    newState = CSharpScript.RunAsync(preprocessResult.Source, scriptOptions, evalContext).GetAwaiter().GetResult();
                else
                    newState = State.ContinueWithAsync(preprocessResult.Source, scriptOptions).GetAwaiter().GetResult();

                if (!isolate)
                    State = newState;

                if (newState.ReturnValue != null)
                {
                    if (!inline)
                        PrintReplResult(newState.ReturnValue);

                    return newState.ReturnValue;
                }
            }
            catch (Exception ex)
            {
                // We generally just want to crash here as we don't have anything meaningful to return.
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

        public static string EvaluateInlineExpressions(CommandContextStack contextStack, string input, int offset = 0)
        {
            var evalContext = new EvaluationContext(contextStack);

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
                        string after = EvaluateInlineExpressions(contextStack, input.Substring(i + 1), offset + i + 1);
                        if (after == null)
                            return null;

                        var expression = input.Substring(startIndex + 2, i - startIndex - 2);
                        return before + EvaluateScript(evalContext, expression, true) + after;
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

        static AssemblyName FindAssembly(DirectoryInfo sourceDirectory, string assemblyNameOrPath)
        {
            /*
             * Examples:
             * ./example/Assembly.dll - Relative to the source directory
             * example/Assembly.dll - Relative to tagtool root directory
             * AssmelyName - Partial name, latest version
             * AssemblyName, Version=1.0.0.0 - Partial name, culture invariant 
             * AssemblyName, Version=1.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089 - full name
             */

            if (assemblyNameOrPath.Contains(".dll"))
            {
                string fullPath;
                if (assemblyNameOrPath.StartsWith("./"))
                    fullPath = Path.Combine(sourceDirectory.FullName, assemblyNameOrPath);
                else
                    fullPath = Path.Combine(Program.TagToolDirectory, assemblyNameOrPath);

                if (!File.Exists(fullPath))
                    return null;

                return AssemblyName.GetAssemblyName(fullPath);
            }

            // first try to find an already loaded assembly in the current app domain
            var an = new AssemblyName(assemblyNameOrPath);
            an = AppDomain.CurrentDomain.GetAssemblies()
                .Select(a => a.GetName())
                .Where(an2 => AssemblyNameCompatible(an, an2))
                .OrderByDescending(a => a.Version)
                .FirstOrDefault();
            if (an != null)
                return an;

            // if that fails search the global assembly cache
            return FindAssemblyInGAC(new AssemblyName(assemblyNameOrPath));
        }

        static AssemblyName FindAssemblyInGAC(AssemblyName assemblyName)
        {
            var searchPaths = new string[]
            {
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "Microsoft.NET", "assembly", Environment.Is64BitProcess ? "GAC_64" : "GAC_32"),
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "Microsoft.NET", "assembly", "GAC_MSIL"),
            };

            foreach (var path in searchPaths)
            {
                var found = SearchAssemblyCache(new DirectoryInfo(path), assemblyName)
                   .OrderByDescending(x => x.Version)
                   .FirstOrDefault();

                if (found != null)
                    return found;
            }

            return null;
        }

        static IEnumerable<AssemblyName> SearchAssemblyCache(DirectoryInfo cacheDirectory, AssemblyName assemblyName)
        {
            var assemblyDirectory = new DirectoryInfo(Path.Combine(cacheDirectory.FullName, assemblyName.Name));
            if (!assemblyDirectory.Exists)
                yield break;

            foreach (var directory in assemblyDirectory.GetDirectories())
            {
                foreach (var file in directory.GetFiles("*.dll", SearchOption.AllDirectories))
                {
                    var an = AssemblyName.GetAssemblyName(file.FullName);
                    if (AssemblyNameCompatible(assemblyName, an))
                        yield return an;
                }
            }
        }

        private static bool AssemblyNameCompatible(AssemblyName target, AssemblyName source)
        {
            if (target.Name != source.Name)
                return false;

            if (target.Version != null && source.Version != target.Version)
                return false;

            if (target.CultureInfo != null && !Equals(target.CultureInfo, CultureInfo.InvariantCulture) && target.CultureName != source.CultureName)
                return false;
            else if (!Equals(source.CultureInfo, CultureInfo.InvariantCulture))
                return false;

            if (!target.GetPublicKeyToken()?.SequenceEqual(target.GetPublicKeyToken()) ?? false)
                return false;

            return true;
        }

        class ScriptPreprocessResult
        {
            public string Source { get; set; }
            public List<string> Imports { get; set; } = new List<string>();
        }

        public class EvaluationContext
        {
            private CommandContextStack ContextStack;

            public EvaluationContext(CommandContextStack stack) => ContextStack = stack;

            //
            // Globals
            //

            public FileInfo ScriptFile { get; set; }

            public List<string> Args { get; set; } = new List<string>();

            public IReadOnlyDictionary<string, string> UserVars => ContextStack.ArgumentVariables;

            public GameCache Cache => GetGlobal<GameCache>(GlobalCacheKey);

            public GameCache PortingCache => GetGlobal<GameCache>(GlobalPortingCacheKey);

            public CachedTag Tag => GetGlobal<CachedTag>(GlobalTagKey);

            public dynamic Definition => GetGlobal<dynamic>(GlobalDefinitionKey);

            public dynamic Element => GetGlobal<dynamic>(GlobalElementKey);
            
            //
            // Methods
            //

            public void DumpGlobals()
            {
                foreach (var property in typeof(EvaluationContext)
                    .GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                    .OrderBy(x => x.Name))
                {
                    var value = property.GetValue(this);
                    if (value == null)
                        continue;

                    Console.WriteLine($"{property.Name} : {property.PropertyType} = {value}");
                }
            }

            public void DumpVariables()
            {
                foreach (var variable in State.Variables.OrderBy(x => x.Name))
                {
                    var value = variable.Value;
                    if (value == null)
                        continue;

                    Console.WriteLine($"{variable.Name} : {variable.Type} = {value}");
                }
            }

            public void RunCommand(string command)
            {
                CommandRunner.Current.RunCommand(command);
            }

            public void Break(params object[] breakInspect)
            {
                System.Diagnostics.Debugger.Break();
            }

            //
            // Internal
            //

            private T GetGlobal<T>(object key, T defaultVal = default(T))
            {
                // walk up the context stack trying to find the global for given key
                var context = ContextStack.Context;
                while (context != null)
                {
                    if (context.ScriptGlobals.TryGetValue(key, out var val))
                        return (T)val;
                    context = context.Parent;
                }
                return defaultVal;
            }
        }
    }
}
