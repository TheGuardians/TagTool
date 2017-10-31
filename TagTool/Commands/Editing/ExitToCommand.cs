using System;
using System.Collections.Generic;

namespace TagTool.Commands.Editing
{
    class ExitToCommand : Command
    {
        private CommandContextStack ContextStack { get; }

        public ExitToCommand(CommandContextStack contextStack)
            : base( CommandFlags.Inherit,

                  "ExitTo",
                  "Exits each context on the stack until the specified one is found.",

                  "ExitTo <context name>",

                  "Exits each context on the stack until the specified one is found.")
        {
            ContextStack = contextStack;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 1)
                return false;

            var found = false;

            var request = args[0];
            var requestLow = request.ToLower();
            var context = ContextStack.Context;

            // Assert that there is a context with the requested name.
            for (var parent = ContextStack.Context; parent != null; parent = parent.Parent)
            {
                if (parent.Name == request || parent.Name.ToLower() == requestLow)
                {
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                Console.WriteLine($"ERROR: No context named '{request}' was found.");
                return false;
            }

            // Pop each child context off of the stack until the requested is active.
            while (context != null)
            {
                if (request == context.Name || requestLow == context.Name.ToLower())
                    break;
                ContextStack.Pop();
                context = ContextStack.Context;
            }

            return true;
        }
    }
}
