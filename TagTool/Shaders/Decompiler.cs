using System;
using System.Collections.Generic;

namespace TagTool.Shaders
{
    // IN-DEPTH DOCUMENTATION FOR XBOX360 SHADER MICROCODE CAN BE FOUND IN THE XBOX360-SDK
    // HIGHLY RECOMMENDED TO READ THE MICROCODE SECTION FORWARDS AND BACKWARDS, AS WELL AS STUDY HLSL A GOOD BIT!!!

    // Basic structure of this decompiler is: (1) Disassemble an xbox360 shader (and acquire the parameters, inputs, outputs,
    // constants in whatever way works.) (2) Parse this into a C# object. (3) Create fragments of HLSL code by processing
    // the object. (4) Embed the fragments into appropriate areas of some boilerplate HLSL.

    public class ASM
	{
		public List<Instruction> Instructions = new List<Instruction> { }; // Instruction listing.

		public class Instruction
		{
			public string OpCode; // The instruction's OpCode. (eg. add,mul,mov,jmp,etc.)
			public bool Saturate; // _sat instruction modifier is available on xbox360. https://msdn.microsoft.com/en-us/library/windows/desktop/hh447231(v=vs.85).aspx
			public List<Operand> Operands = new List<Operand> { }; // Operands should be in the order they appear in the instruction listing.
		}
		public class Operand
		{
			public bool AbsoluteValue;      // _abs source register modifier : absolute value of the register is used by the instruction.
			public bool ArithmeticNegation; // - 	source register modifier : positive & negative values are swapped for use by the instruction.
			public bool LogicalNegation;    // !	source register modifier : boolean values are flipped for use by the instruction.
			public bool DirectAddressing;   // Index is a number: c9, as opposed to a format like: c[9 + aL] (possibly means c9 is an array start with aL indexing eg. c9[aL])
			public string Name;             // Name of the register: c,i,b,vf,tf,o,oPos,oPts,oC,oDepth,eA,eM,aO,aL,L.
			public string Index;            // Number tacked onto the end of the register name (if no relative addressing) eg. c12
			public string Components;       // Components x,y,z,w (swizzling and masking rules shouldn't matter, since HLSL should support both, barring use of 0 and 1 for masking)
		}

		public ASM(string assembly)
		{
			// TODO: Write a new parser to build an ASM object from a shader's disassembly.
			// parameters, inputs, outputs, and constants need to be resolved. The info may be in the UPDB.
			// Otherwise, parameters can be resolved through the parameters tagblock in a pixl/vtsh/glps/glvs.
			// inputs, outputs, and constants would need to be resolved through analyzing how registers are used...
		}
	}

	public class Decompiler
	{
		public string Decompile(string assembly)
		{
			var asm = new ASM(assembly);

			string Parameters = ""; // Parameters
			string Inputs = ""; // Input semantics received by pixel from vertex
			string Outputs = ""; // Output semantics sent from vertex->pixel or pixel->"screen"
			string Constants = ""; // Variables that have a compile-time constant value (eg. "float4 x = float4(1,2,3,4)")
			string Logic = ""; // Main code to work with the different parameters, semantics, and constants above.
			
			// TODO: Everything inside this foreach is currently pseudocode in place to guide the design of the decompiler!
			foreach(var instr in asm.Instructions)
			{
				var operands = new List<string> { };
				
				// Decompilers per opcode set up as separate cases here.
				switch (instr.OpCode)
				{
					case "add":
						Logic += $"{operands[0]} = {operands[1]} + {operands[2]};";
						break;
					default:
					    Console.WriteLine($"OPCODE NOT IMPLEMENTED: {instr.OpCode}");
						break;
				}

				if (instr.Saturate) // Clamps result of instruction to range of 0-1 https://msdn.microsoft.com/en-us/library/windows/desktop/hh447231(v=vs.85).aspx
					Logic += $"{operands[0]} = min(1.0f, max(0.0f, {operands[0]}));";
			}
			
			// Return some boilerplate HLSL with the decompiled pieces above embedded
			return
			$"{Parameters}          "+
			"struct INPUT           "+
			"{                      "+
			$"{Inputs}              "+
			"};                     "+
			"struct OUTPUT          "+
			"{                      "+
			$"{Outputs}             "+ 
			"};                     "+
			"OUTPUT main(INPUT In ) "+
			"{                      "+
			$"{Constants}           "+
			"OUTPUT Out;            "+
			$"{Logic}               "+
			"return Out;            "+
			"}                      ";
		}
	}
}






