using System;
using System.Collections.Generic;

namespace TagTool.Commands.Common
{
    class ListVariablesCommand : Command
    {
        private CommandContextStack ContextStack { get; }

        public ListVariablesCommand(CommandContextStack contextStack)
            : base(true,

                  "ListVariables",
                  "Lists all currently set argument variables.",

                  "ListVariables",
                  "Lists all currently set argument variables")
        {
            ContextStack = contextStack;
        }

        public override object Execute(List<string> args)
        {
            foreach (var variable in ContextStack.ArgumentVariables)
                Console.WriteLine($"{variable.Key} : {variable.Value}");

            return true;
        }
    }
}
