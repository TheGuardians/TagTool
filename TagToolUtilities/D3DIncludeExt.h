#pragma once

#include <string>
#include <map>
#include <d3dcompiler.h>

namespace TagTool {
	namespace Util {

		class D3DIncludeExt : public ID3DInclude {
		public:

			std::string root_directory;
			std::map<const void*, std::string> Directories;
			std::string uniforms_override;

			D3DIncludeExt(std::string _root_directory);

			std::string GetParentDirectory(const void* ptr);

			void SetParentDirectory(const void* ptr, std::string dir);

			HRESULT __stdcall Open(
				D3D_INCLUDE_TYPE IncludeType,
				LPCSTR pFileName,
				LPCVOID pParentData,
				LPCVOID *ppData,
				UINT *pBytes);

			HRESULT __stdcall Close(LPCVOID pData);
		};

	}
}