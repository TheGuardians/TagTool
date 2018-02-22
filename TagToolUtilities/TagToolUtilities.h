// TagToolUtilities.h

#pragma once

using namespace System;



namespace TagTool {

	namespace Utilities
	{
		public ref class DirectXUtilities {
		public:
			static array<Byte>^ AssembleShader(String^ source);
		};
	}
}
