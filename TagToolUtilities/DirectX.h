// TagToolUtilities.h

#pragma once

using namespace System;

#include <string>
#include <vcclr.h>  

namespace TagTool {
	namespace Util
	{

		public ref class DirectX {
		public:

			System::Collections::Generic::Dictionary<String^, String^>^ FileOverrides;
			void SetCompilerFileOverrides(System::Collections::Generic::Dictionary<String^, String^>^ file_overrides);

			ref struct MacroDefine {
				String^ Name;
				String^ Definition;
				virtual String^ ToString() override;
			};

			static array<Byte>^ AssemblePCShader(String^ source);

			bool CompilePCShader(
				String^ SrcData,
				String ^ SrcName,
				array<MacroDefine^>^ Defines,
				String^ FunctionName,
				String^ Profile,
				UInt32 Flags1,
				UInt32 Flags2,
				[System::Runtime::InteropServices::Out] array<Byte>^% Shader,
				[System::Runtime::InteropServices::Out] String^% ErrorMsgs);

			bool CompilePCShaderFromFile(
				String^ File,
				array<MacroDefine^>^ Defines,
				String^ FunctionName,
				String^ Profile,
				UInt32 Flags1,
				UInt32 Flags2,
				[System::Runtime::InteropServices::Out] array<Byte>^% Shader,
				[System::Runtime::InteropServices::Out] String^% ErrorMsgs);

			static String^ DisassemblePCShader(array<Byte>^ Data, UInt32 Flags);

		};
	}
}