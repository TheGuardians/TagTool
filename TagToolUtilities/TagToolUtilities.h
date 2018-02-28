// TagToolUtilities.h

#pragma once

using namespace System;
using namespace System::Runtime::InteropServices;

namespace TagTool {

	namespace Utilities
	{
		public ref class DirectXUtilities {
		public:

			ref struct MacroDefine {
				String^ Name;
				String^ Definition; 
				virtual String^ ToString() override;
			};



			static array<Byte>^ AssemblePCShader(String^ source);
			static bool CompilePCShader(
				String^ SrcData, 
				array<MacroDefine^>^ Defines, 
				String^ FunctionName, 
				String^ Profile, 
				UInt32 flags,
				[Out] array<Byte>^% Shader,
				[Out] String^% ErrorMsgs,
				[Out] String^% ConstantTable
			);
			static bool CompilePCShaderFromFile(
				String^ File,
				array<MacroDefine^>^ Defines,
				String^ FunctionName,
				String^ Profile,
				UInt32 flags,
				[Out] array<Byte>^% Shader,
				[Out] String^% ErrorMsgs,
				[Out] String^% ConstantTable);






			static String^ DisassemblePCShader(array<Byte>^ data);
		};
	}
}
