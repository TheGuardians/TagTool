using System;
using System.Collections.Generic;

namespace TagTool.Commands
{
    /// <summary>
    /// A context which contains commands that can be executed on an object.
    /// </summary>
    public class CommandContext
    {
        /// <summary>
        /// Gets the parent context. Can be <c>null</c> if this is the initial context.
        /// </summary>
        public CommandContext Parent { get; private set; }

        /// <summary>
        /// Gets the name of the context.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the context's command dictionary.
        /// </summary>
        private Dictionary<string, Command> CommandsByName { get; } = new Dictionary<string, Command>();

        /// <summary>
        /// Gets the context's available commands.
        /// </summary>
        public IEnumerable<Command> Commands => CommandsByName.Values;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandContext"/> class.
        /// </summary>
        /// <param name="parent">The parent context. Can be <c>null</c> if none.</param>
        /// <param name="name">The context's name.</param>
        public CommandContext(CommandContext parent, string name)
        {
            Parent = parent;
            Name = name;
        }

        /// <summary>
        /// Adds a command to the context.
        /// </summary>
        /// <param name="command">The command to add.</param>
        public void AddCommand(Command command)
        {
            CommandsByName[command.Name] = command;
        }

        /// <summary>
        /// Looks up a command in the context by name.
        /// Commands inherited from parent contexts will be included.
        /// </summary>
        /// <param name="name">The name of the command to retrieve.</param>
        /// <returns>The command with the given name if found, or <c>null</c> otherwise.</returns>
        public Command GetCommand(string name)
        {
            CommandsByName.TryGetValue(name, out Command result);

            if (result != null)
                return result;

            //
            // Case-insensitive lookup
            //

            var nameLower = name.ToLower();

            foreach (var pair in CommandsByName)
                if (nameLower == pair.Key.ToLower())
                    return pair.Value;

            //
            // Snake-case lookup
            //

            var nameSnake = name.ToSnakeCase();

            foreach (var pair in CommandsByName)
                if (nameSnake == pair.Key.ToSnakeCase())
                    return pair.Value;

            //
            // Check parent contexts for inheritable commands
            //

            for (var p = Parent; p != null; p = p.Parent)
            {
                p.CommandsByName.TryGetValue(name, out result);

                if (result != null && (result.Flags & CommandFlags.Inherit) != 0)
                    return result;

                //
                // Case-insensitive lookup
                //

                foreach (var pair in p.CommandsByName)
                    if (nameLower == pair.Key.ToLower())
                        if ((pair.Value.Flags & CommandFlags.Inherit) != 0)
                            return pair.Value;

                //
                // Snake-case lookup
                //

                foreach (var pair in p.CommandsByName)
                    if (nameSnake == pair.Key.ToSnakeCase())
                        if ((pair.Value.Flags & CommandFlags.Inherit) != 0)
                            return pair.Value;
            }

            return null;
        }
    }
}