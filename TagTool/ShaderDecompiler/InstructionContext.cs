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


		public string PreText { get; set; }

		public string ChildrenText
		{
			get
			{
				var childText = "";
				while (Children.Count > 0)
					ChildrenText += Children.Dequeue().GetHLSL;
				return childText;
			}
			set { }
		}

		public string PostText { get; set; }

		public string GetHLSL
		{
			get
			{
				return 
					PreText +
					ChildrenText +
					PostText;
			}
		}

		public int Depth { get; set; }
	}
}
