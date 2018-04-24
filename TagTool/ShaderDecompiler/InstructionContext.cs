using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.ShaderDecompiler.UcodeDisassembler;

namespace TagTool.ShaderDecompiler
{
	public class InstructionContext
	{
		/// <summary>
		/// The instruction which created this context.
		/// </summary>
		public Instruction Instruction { get; set; }

		/// <summary>
		/// The type of instruction which owns this context.
		/// </summary>
		public InstructionType InstructionType { get; set; }

		/// <summary>
		/// Contexts directly in this context.
		/// </summary>
		public Queue<InstructionContext> Children { get; set; }


		public string ContextPrefix { get; set; }
		public string ContextOpen { get; set; }

		private string childrenContexts;
		public string ChildrenContexts
		{
			get
			{
				while (Children.Count > 0)
					ChildrenContexts += Children.Dequeue().GetCode;
				return childrenContexts;
			}
			set
			{
				childrenContexts = value;
			}
		}

		public string ContextClose { get; set; }
		public string ContextPostfix { get; set; }

		public string GetCode
		{
			get
			{
				return 
					ContextPrefix +
					ContextOpen +
					ChildrenContexts +
					ContextClose +
					ContextPostfix;
			}
		}

		public int Depth { get; set; }
	}
}
