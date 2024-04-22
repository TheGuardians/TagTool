using System;
using System.Collections.Generic;

namespace TagTool.Commands.Common
{
    class SetVariableCommand : Command
    {
        private CommandContextStack ContextStack { get; }

        public SetVariableCommand(CommandContextStack contextStack)
            : base(true,

                  "SetVariable",
                  "Any instance of <VariableName> in a command argument will be replaced with it's [Value].",

                  "SetVariable <VariableName> [Value]",
                  "Any instance of <VariableName> in a command argument will be replaced with it's [Value].\n" +
                  "Leaving [Value] blank will remove the variable. Using * as the variable will clear all.\n" +
                  "Variables are not active with this command.",

                  true) // ignore argument variables
        {
            ContextStack = contextStack;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count < 1)
                return new TagToolError(CommandError.ArgCount);

            string variableName = args[0];

            if (variableName == "*")
            {
                ContextStack.ArgumentVariables.Clear();
                Console.WriteLine("All variables cleared.");
            }
            else if (args.Count > 1)
            {
                args.RemoveAt(0);
                ContextStack.ArgumentVariables[variableName] = string.Join(" ", args);
                Console.WriteLine($"Variable \"{variableName}\" set to \"{ContextStack.ArgumentVariables[variableName]}\".");
            }
            else
            {
                if (!ContextStack.ArgumentVariables.Remove(variableName))
                {
                    Console.WriteLine($"Variable \"{variableName}\" not found.");
                }
                else
                {
                    Console.WriteLine($"Variable \"{variableName}\" cleared.");
                }
            }

            return true;
        }
    }
}
