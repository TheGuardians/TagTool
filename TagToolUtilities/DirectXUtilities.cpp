
#include <d3dcompiler.h>

#include "Helpers.h"
#include "DirectXUtilities.h"


#include <string>
#include <vector>
#include <iterator>
#include <fstream>
#include <map>
#include <codecvt>
#include <locale>

#include <msclr\marshal.h>
#include <msclr\marshal_cppstd.h>

using namespace System;
using namespace System::Runtime::InteropServices;
using namespace System::Collections::Generic;
using namespace TagTool::Utilities;

array<Byte>^ DirectXUtilities::AssemblePCShader(String ^ source)
{
	throw gcnew System::NotImplementedException();
	// TODO: insert return statement here
}

class ExtInclude : public ID3DInclude {
public:

	std::string root_directory;
	std::map<const void*, std::string> Directories;
	std::string uniforms_override;

	ExtInclude(std::string _root_directory)
	{
		SetParentDirectory(nullptr, _root_directory + "\\");
	}

	std::string GetParentDirectory(const void* ptr) {
		return Directories[ptr];
	}

	void SetParentDirectory(const void* ptr, std::string dir) {
		Directories[ptr] = dir;
	}

	HRESULT __stdcall Open(D3D_INCLUDE_TYPE IncludeType, LPCSTR pFileName, LPCVOID pParentData, LPCVOID *ppData, UINT *pBytes) {

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

	HRESULT __stdcall Close(LPCVOID pData) {
		delete[] pData;
		return S_OK;
	}
};

bool DirectXUtilities::CompilePCShader(
	String ^ _SrcData,
	String ^ _SrcName,
	array<MacroDefine^>^ _Defines,
	String ^ _FunctionName,
	String ^ _Profile,
	UInt32 Flags1,
	UInt32 Flags2,
	array<Byte>^% Shader,
	String ^% ErrorMsgs
)
{
	std::string SrcData = Helpers::MarshalStringA(_SrcData);
	std::string SrcName = Helpers::MarshalStringA(_SrcName);
	std::string FunctionName = Helpers::MarshalStringA(_FunctionName);
	std::string Profile = Helpers::MarshalStringA(_Profile);

	// Create the macros array
	auto num_macros = _Defines->Length + 1;
	D3D_SHADER_MACRO* macros = new D3D_SHADER_MACRO[num_macros];
	memset(macros, 0, sizeof(D3D_SHADER_MACRO) * num_macros);
	for (auto i = 0; i < _Defines->Length; i++) {

		auto cs_macro = _Defines[i];

		std::string _name = Helpers::MarshalStringA(cs_macro->Name);
		std::string definition = Helpers::MarshalStringA(cs_macro->Definition);

		auto cstr_name = new char[_name.size()];
		auto cstr_definition = new char[definition.size()];
		memcpy(cstr_name, _name.data(), _name.size());
		memcpy(cstr_definition, definition.data(), definition.size());

		macros[i].Name = cstr_name;
		macros[i].Definition = cstr_definition;
	}

	LPD3DBLOB shader = nullptr;
	LPD3DBLOB errors = nullptr;

	HRESULT result = D3DCompile(
		SrcData.data(),
		(UINT)SrcData.size(),
		SrcName.c_str(),
		macros,
		nullptr,
		FunctionName.c_str(),
		Profile.c_str(),
		(DWORD)Flags1,
		(DWORD)Flags2,
		&shader,
		&errors);

	if (result != S_OK) {
		if (result == 0x80070002) {
			throw gcnew Exception("can't find file");
		}
		else {
			ErrorMsgs = gcnew String(reinterpret_cast<char*>(errors->GetBufferPointer()));
		}
	}
	else {
		List<Byte>^ data = gcnew List<Byte>();
		for (DWORD i = 0; i < shader->GetBufferSize(); i++) {
			auto val = reinterpret_cast<unsigned char*>(shader->GetBufferPointer())[i];
			data->Add(val);
		}
		Shader = data->ToArray();
	}

	//TODO: Implement large address aware constant table
	// https://msdn.microsoft.com/en-us/library/windows/desktop/bb172731(v=vs.85).aspx


	//ConstantTable = gcnew String(reinterpret_cast<char*>(errors->GetBufferPointer()));


	return result == S_OK;
}

bool DirectXUtilities::CompilePCShaderFromFile(
	String ^ _File,
	array<MacroDefine^>^ _Defines,
	String ^ _FunctionName,
	String ^ _Profile,
	UInt32 Flags1,
	UInt32 Flags2,
	array<Byte>^% Shader,
	String ^% ErrorMsgs,
	Dictionary<String^, String^>^ file_overrides
)
{
	std::wstring File = Helpers::MarshalStringW(_File);
	std::string FunctionName = Helpers::MarshalStringA(_FunctionName);
	std::string Profile = Helpers::MarshalStringA(_Profile);

	// Create the macros array
	auto num_macros = _Defines->Length + 1;
	D3D_SHADER_MACRO* macros = new D3D_SHADER_MACRO[num_macros];
	memset(macros, 0, sizeof(D3D_SHADER_MACRO) * num_macros);
	for (auto i = 0; i < _Defines->Length; i++) {

		auto cs_macro = _Defines[i];

		std::string _name = Helpers::MarshalStringA(cs_macro->Name);
		std::string definition = Helpers::MarshalStringA(cs_macro->Definition);

		auto cstr_name = new char[_name.size()];
		auto cstr_definition = new char[definition.size()];
		memset(cstr_name, 0, _name.size() + 1);
		memset(cstr_definition, 0, definition.size() + 1);
		memcpy(cstr_name, _name.data(), _name.size() + 1);
		memcpy(cstr_definition, definition.data(), definition.size() + 1);

		macros[i].Name = cstr_name;
		macros[i].Definition = cstr_definition;
	}

	LPD3DBLOB shader = nullptr;
	LPD3DBLOB errors = nullptr;

	auto root_directory = System::IO::Path::GetDirectoryName(_File);
	auto std_root_directory = Helpers::MarshalStringA(root_directory);
	ExtInclude include = ExtInclude(std_root_directory);




	//TODO: Improve this
	if (file_overrides) {

		include.uniforms_override = Helpers::MarshalStringA(file_overrides["parameters.hlsl"]);

	}


	std::string file_path = Helpers::MarshalStringA(_File);
	std::string source_code = Helpers::ReadFile(file_path);

	HRESULT result = D3DCompile(
		source_code.data(),
		(UINT)source_code.size(),
		file_path.c_str(),
		macros,
		&include,
		FunctionName.c_str(),
		Profile.c_str(),
		(DWORD)Flags1,
		(DWORD)Flags2,
		&shader,
		&errors);

	//HRESULT result = D3DCompileFromFile(
	//	File.c_str(),
	//	macros,
	//	&include,
	//	FunctionName.c_str(),
	//	Profile.c_str(),
	//	(DWORD)Flags1,
	//	(DWORD)Flags2,
	//	&shader,
	//	&errors);

	if (result != S_OK) {
		if (result == 0x80070002) {
			throw gcnew Exception("can't find " + _File);
		}
		else {
			ErrorMsgs = gcnew String(reinterpret_cast<char*>(errors->GetBufferPointer()));
		}
	}
	else {
		List<Byte>^ data = gcnew List<Byte>();
		for (DWORD i = 0; i < shader->GetBufferSize(); i++) {
			auto val = reinterpret_cast<unsigned char*>(shader->GetBufferPointer())[i];
			data->Add(val);
		}
		Shader = data->ToArray();
	}

	//TODO: Implement large address aware constant table
	// https://msdn.microsoft.com/en-us/library/windows/desktop/bb172731(v=vs.85).aspx


	//ConstantTable = gcnew String(reinterpret_cast<char*>(errors->GetBufferPointer()));


	return result == S_OK;
}

String ^ DirectXUtilities::DisassemblePCShader(array<Byte>^ _Data, UInt32 Flags)
{
	if (_Data == nullptr) throw gcnew Exception("data is null");

	LPD3DBLOB buffer = nullptr;

	std::vector<unsigned char> data;
	data.reserve(_Data->Length);
	for (auto i = 0; i < _Data->Length; i++) {
		auto val = _Data[i];
		data.push_back(val);
	}

	HRESULT result = D3DDisassemble(reinterpret_cast<DWORD*>(data.data()), data.size(), 0, nullptr, &buffer);

	if (result != S_OK) {
		throw gcnew Exception();
	}

	return gcnew String(reinterpret_cast<char*>(buffer->GetBufferPointer()));
}

String ^ DirectXUtilities::MacroDefine::ToString()
{
	return "#define " + Name + " " + Definition;
}
//
//array<Byte>^ DirectXUtilities::AssemblePCShader(String ^ _source)
//{
//	if (_source == nullptr) throw gcnew Exception("source is null");
//
//	std::string source = MarshalStringA(_source);
//
//	LPD3DXBUFFER buffer = nullptr;
//	LPD3DXBUFFER errors = nullptr;
//
//	HRESULT result = D3DXAssembleShader(source.c_str(), (UINT)source.length(), NULL, NULL, 0, &buffer, &errors);
//
//	if (result != S_OK) {
//
//		String^ errors_str = gcnew String(reinterpret_cast<char*>(errors->GetBufferPointer()));
//		throw gcnew Exception(errors_str);
//
//	}
//
//	auto arr = gcnew array<Byte>(buffer->GetBufferSize());
//	auto ptr = reinterpret_cast<char*>(buffer->GetBufferPointer());
//	for (DWORD i = 0; i < buffer->GetBufferSize(); i++) {
//		arr[i] = ptr[i];
//	}
//
//	return arr;
//}
