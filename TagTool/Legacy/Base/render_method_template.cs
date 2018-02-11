using System.Collections.Generic;

namespace TagTool.Legacy
{
    public abstract class render_method_template
    {
        public List<ArgumentBlock> ArgumentBlocks;
        public List<UsageBlock> UsageBlocks;

        public render_method_template()
        {
            ArgumentBlocks = new List<ArgumentBlock>();
            UsageBlocks = new List<UsageBlock>();
        }

        public abstract class ArgumentBlock
        {
            public string Argument;

            public override string ToString()
            {
                return Argument;
            }
        }

        public abstract class UsageBlock
        {
            public string Usage;

            public override string ToString()
            {
                return Usage;
            }
        }
    }
}
