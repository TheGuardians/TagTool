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
        /// <param name="inherit">Command flags.</param>
        /// <param name="name">The command's name.</param>
        /// <param name="description">The command's description.</param>
        /// <param name="usage">The command's usage string.</param>
        /// <param name="helpMessage">The command's help message.</param>
        /// <param name="ignoreVars">The command's argument variable policy.</param>
		/// <param name="examples">Examples of the command's usage.</param>
        protected Command(bool inherit, string name, string description, string usage, string helpMessage = "", bool ignoreVars = false, string examples = "")
        {
            Name = name;
            Description = description;
            Usage = usage;
            HelpMessage = helpMessage;
			Examples = examples;	
            Inherit = inherit;
            IgnoreArgumentVariables = ignoreVars;
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
        /// Gets the command's examples.
        /// </summary>
        public string Examples { get; private set; }

        /// <summary>
        /// Gets the command's flags.
        /// </summary>
        public bool Inherit { get; private set; }

        /// <summary>
        /// Gets the command's argument variable policy.
        /// </summary>
        public bool IgnoreArgumentVariables { get; private set; }

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="args">The command arguments.</param>
        /// <returns><c>true</c> if the command's arguments were valid.</returns>
        public abstract object Execute(List<string> args);
    }
}