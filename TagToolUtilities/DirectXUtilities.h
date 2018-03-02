// TagToolUtilities.h

#pragma once

using namespace System;

#include <string>

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
				String ^ SrcName,
				array<MacroDefine^>^ Defines,
				String^ FunctionName,
				String^ Profile,
				UInt32 Flags1,
				UInt32 Flags2,
				[System::Runtime::InteropServices::Out] array<Byte>^% Shader,
				[System::Runtime::InteropServices::Out] String^% ErrorMsgs
			);
			static bool CompilePCShaderFromFile(
				String^ File,
				array<MacroDefine^>^ Defines,
				String^ FunctionName,
				String^ Profile,
				UInt32 Flags1,
				UInt32 Flags2,
				[System::Runtime::InteropServices::Out] array<Byte>^% Shader,
				[System::Runtime::InteropServices::Out] String^% ErrorMsgs, 
				System::Collections::Generic::Dictionary<String^, String^>^ file_overrides);

			static String^ DisassemblePCShader(array<Byte>^ Data, UInt32 Flags);
		};
	}
}