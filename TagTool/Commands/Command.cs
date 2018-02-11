using System;
using System.Collections.Generic;

namespace TagTool.Commands
{
    /// <summary>
    /// Base class for a tag manipulation command.
    /// </summary>
    public abstract class Command
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Command"/> class.
        /// </summary>
        /// <param name="flags">Command flags.</param>
        /// <param name="name">The command's name.</param>
        /// <param name="description">The command's description.</param>
        /// <param name="usage">The command's usage string.</param>
        /// <param name="helpMessage">The command's help message.</param>
        protected Command(CommandFlags flags, string name, string description, string usage, string helpMessage)
        {
            Name = name;
            Description = description;
            Usage = usage;
            HelpMessage = helpMessage;
            Flags = flags;
        }

        /// <summary>
        /// Gets the command's name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the command's description.
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// Gets the command's usage string.
        /// </summary>
        public string Usage { get; private set; }

        /// <summary>
        /// Gets the command's help message.
        /// </summary>
        public string HelpMessage { get; private set; }

        /// <summary>
        /// Gets the command's flags.
        /// </summary>
        public CommandFlags Flags { get; private set; }

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="args">The command arguments.</param>
        /// <returns><c>true</c> if the command's arguments were valid.</returns>
        public abstract object Execute(List<string> args);
    }

    [Flags]
    public enum CommandFlags
    {
        None = 0,

        /// <summary>
        /// The command can be inherited by child contexts.
        /// </summary>
        Inherit = 1
    }
}