#include "D3DIncludeExt.h"

#include "Helpers.h"

using namespace TagTool::Util;

D3DIncludeExt::D3DIncludeExt(std::string _root_directory)
{
	SetParentDirectory(nullptr, _root_directory + "\\");
}

std::string D3DIncludeExt::GetParentDirectory(const void* ptr) {
	return Directories[ptr];
}

void D3DIncludeExt::SetParentDirectory(const void* ptr, std::string dir) {
	Directories[ptr] = dir;
}

HRESULT __stdcall D3DIncludeExt::Open(D3D_INCLUDE_TYPE IncludeType, LPCSTR pFileName, LPCVOID pParentData, LPCVOID *ppData, UINT *pBytes) {

	// Filepaths
	auto root_directory = GetParentDirectory(pParentData);
	auto filepath = root_directory + pFileName;

	bool is_uniforms = std::string(filepath).find("parameters.hlsl") != std::string::npos;
	bool is_editor_only = std::string(filepath).find("editor_only.hlsl") != std::string::npos;

	// Read File
	char* data;
	{
		std::string source_code;

		if (is_uniforms && uniforms_override.size() > 1) source_code = uniforms_override;
		else if (is_editor_only && uniforms_override.size() > 1) source_code = "";
		else source_code = Helpers::ReadFile(filepath);

		data = new char[source_code.size()];
		memcpy(data, source_code.data(), source_code.size());
		*ppData = data;
		*pBytes = (UINT)source_code.size();
	}

	auto this_dir = Helpers::ExtractDirectoryName(filepath) + "\\";
	SetParentDirectory(data, this_dir);

	return S_OK;
}

HRESULT __stdcall D3DIncludeExt::Close(LPCVOID pData) {
	delete[] pData;
	return S_OK;
}