// TagToolUtilities.h

#pragma once

using namespace System;
using namespace System::Runtime::InteropServices;

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
				[Out] array<Byte>^% Shader,
				[Out] String^% ErrorMsgs
			);
			static std::string ExtractDirectoryName(std::string path);
			static bool CompilePCShaderFromFile(
				String^ File,
				array<MacroDefine^>^ Defines,
				String^ FunctionName,
				String^ Profile,
				UInt32 Flags1,
				UInt32 Flags2,
				[Out] array<Byte>^% Shader,
				[Out] String^% ErrorMsgs);

			static String^ DisassemblePCShader(array<Byte>^ Data, UInt32 Flags);

		private:
			static std::string MarshalStringA(String ^ s);
			static std::wstring MarshalStringW(String ^ s);
		};
	}
}
