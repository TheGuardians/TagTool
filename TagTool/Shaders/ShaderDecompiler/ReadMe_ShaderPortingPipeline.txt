

 _____________________________
*** Shader Porting pipeline ***
 *****************************

Stage ONE:
	1. Extract shader data from pixl/vtsh/glps/glvs tag.
	2. Disassemble shader
		I. Initially assume all instructions to be control-flow and decode them in pairs.
			Uno. If a CF instruction is an exec-type instruction, decode the ALU/Fetch instructions it executes.
			Dos. If a CF instruction is an exece-type instruction, end disassembly.
		II. Return an orderly list of instructions, ready for HLSL generation.
------------------------------------------------------------------------------------------------------------------------

Stage TWO
	1. Decode Microcode Directives from 'PixelShaderReference.ConstantData byte[]' and 
	   'VertexShaderReference.ConstantData byte[]' in \TagTool\Shaders\ 
	   (probably best to create a new instruction type to hold their data, even though they aren't actual instructions.)
	2. ???
------------------------------------------------------------------------------------------------------------------------

Stage THREE:
	1. Apply pre-fixups to the orderly list of instructions, such as register changes.
	2. Enumerate the list of instructions.
		I. If the instruction is a control-flow instruction, create a new HLSL context.
		II. If the instruction is an ALU/Fetch instruction, output HLSL into the new context.
	3. Apply post-fixups to the generated HLSL if necessary.
------------------------------------------------------------------------------------------------------------------------

Stage FOUR:
	1. Compile HLSL into DirectX9 SM3 shader binaries using imported d3dCompile function.
	2. Create/Generate/Modify necessary shader-related tags to work with the new shader binaries.
------------------------------------------------------------------------------------------------------------------------