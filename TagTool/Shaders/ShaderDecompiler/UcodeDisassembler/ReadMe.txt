


	There is a matching "ShaderDisassembler" folder in the TagToolUtilities project. 
Inside of that folder are matching C++ versions of "ControlFlowInstruction.cs", "ALUInstruction.cs", and 
"FetchInstruction.cs".

Those are written in C++ to take advantage of bitfield structs, which makes decoding instructions much easier.

	If ANY editing is done to "ControlFlowInstruction.cs", "ALUInstruction.cs", "FetchInstruction.cs", or 
"InstructionDecoding.cs", it's critical that matching changes be made to "ALUInstruction.cpp", 
"ControlFlowInstruction.cpp", and "FetchInstruction.cpp" in the matching folder in the TagToolUtilities project.

Same goes for the opposite.