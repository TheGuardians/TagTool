using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TagTool.Commands
{
    /// <summary>
    /// A stack of command contexts.
    /// </summary>
    public class CommandContextStack
    {
        private readonly Stack<CommandContext> _contextStack = new Stack<CommandContext>();

        /// <summary>
        /// Gets the currently-active context, or <c>null</c> if there is none.
        /// </summary>
        public CommandContext Context
        {
            get { return (_contextStack.Count > 0) ? _contextStack.Peek() : null; }
        }

        /// <summary>
        /// Gets the current path.
        /// </summary>
        /// <returns>The current path.</returns>
        public string GetPath()
        {
            var result = new StringBuilder();
            foreach (var context in _contextStack.Where(c => c.Name != null))
            {
                if (result.Length > 0)
                    result.Insert(0, "\\");
                result.Insert(0, context.Name);
            }
            return result.ToString();
        }



        /// <summary>
        /// Pushes a context onto the stack, making it active.
        /// </summary>
        /// <param name="context">The context to push.</param>
        public void Push(CommandContext context)
        {
            _contextStack.Push(context);
        }

        /// <summary>
        /// Pops the current context off the stack, making the previous one active.
        /// </summary>
        /// <returns><c>true</c> if more contexts still remain on the stack, <c>false</c> if the stack is now empty.</returns>
        public bool Pop()
        {
            _contextStack.Pop();
            return (_contextStack.Count != 0);
        }
    }
}
