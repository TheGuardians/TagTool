// TagToolUtilities.h

#pragma once

#include <string>

namespace TagTool {

	namespace Util
	{
		private ref class Helpers {
		public:

			static std::string ExtractDirectoryName(std::string path);
			static std::string ReadFile(std::string path);
			static std::string MarshalStringA(System::String ^ s);
			static std::wstring MarshalStringW(System::String ^ s);

		};
	}
}
