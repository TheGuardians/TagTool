


	There is a matching "ShaderDisassembler" folder in the TagTool project. 
Inside of that folder are matching C# versions of "ControlFlowInstruction.cpp", "ALUInstruction.cpp", and 
"FetchInstruction.cpp".

	The reason there are matching C++ versions of those files are to take advantage of C++ bitfield structs
for instruction decoding.

	If ANY editing is done to "ControlFlowInstruction.cpp", "ALUInstruction.cpp", or "FetchInstruction.cpp", 
it's critical that matching changes be made to "ALUInstruction.cs", "ControlFlowInstruction.cs", 
"FetchInstruction.cs", and "InstructionDecoding.cs" in the matching folder in the TagTool project..

Same goes for the opposite.