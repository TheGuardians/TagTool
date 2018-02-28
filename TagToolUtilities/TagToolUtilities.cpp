
#include <d3dcompiler.h>

#include "stdafx.h"
#include "TagToolUtilities.h"

#include <string>
#include <vector>
#include <iterator>
#include <fstream>
#include <map>

#include <msclr\marshal.h>
#include <msclr\marshal_cppstd.h>

using namespace System;
using namespace System::Runtime::InteropServices;
using namespace System::Collections::Generic;

std::string TagTool::Utilities::DirectXUtilities::MarshalStringA(String^ s)
{
	std::string outputstring;
	const char* kPtoC = (const char*)(Marshal::StringToHGlobalAnsi(s)).ToPointer();
	outputstring = kPtoC;
	Marshal::FreeHGlobal(IntPtr((void*)kPtoC));
	return outputstring;
}

std::wstring TagTool::Utilities::DirectXUtilities::MarshalStringW(String^ s)
{
	return msclr::interop::marshal_as<std::wstring>(s);
}

array<Byte>^ TagTool::Utilities::DirectXUtilities::AssemblePCShader(String ^ source)
{
	throw gcnew System::NotImplementedException();
	// TODO: insert return statement here
}

bool TagTool::Utilities::DirectXUtilities::CompilePCShader(
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
	std::string SrcData = MarshalStringA(_SrcData);
	std::string SrcName = MarshalStringA(_SrcName);
	std::string FunctionName = MarshalStringA(_FunctionName);
	std::string Profile = MarshalStringA(_Profile);

	// Create the macros array
	auto num_macros = _Defines->Length + 1;
	D3D_SHADER_MACRO* macros = new D3D_SHADER_MACRO[num_macros];
	memset(macros, 0, sizeof(D3D_SHADER_MACRO) * num_macros);
	for (auto i = 0; i < _Defines->Length; i++) {

		auto cs_macro = _Defines[i];

		std::string _name = MarshalStringA(cs_macro->Name);
		std::string definition = MarshalStringA(cs_macro->Definition);

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

	if (result != S_OK) ErrorMsgs = gcnew String(reinterpret_cast<char*>(errors->GetBufferPointer()));
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

std::string TagTool::Utilities::DirectXUtilities::ExtractDirectoryName(std::string path) {

	auto str = gcnew String(path.c_str());
	auto dir = System::IO::Path::GetDirectoryName(str);
	return MarshalStringA(dir);
}

class ExtInclude : public ID3DInclude {
public:

	std::string root_directory;
	std::map<const void*, std::string> Directories;

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

	HRESULT Open(
		D3D_INCLUDE_TYPE IncludeType,
		LPCSTR           pFileName,
		LPCVOID          pParentData,
		LPCVOID          *ppData,
		UINT             *pBytes
	) {
		// Filepaths
		auto root_directory = GetParentDirectory(pParentData);
		auto filepath = root_directory + pFileName;

		// Read File
		char* data;
		{
			std::ifstream testFile(filepath, std::ios::binary);
			std::vector<char> fileContents((std::istreambuf_iterator<char>(testFile)), std::istreambuf_iterator<char>());

			data = new char[fileContents.size()];
			memcpy(data, fileContents.data(), fileContents.size());
			*ppData = data;
			*pBytes = (UINT)fileContents.size();
		}

		auto this_dir = TagTool::Utilities::DirectXUtilities::ExtractDirectoryName(filepath) + "\\";
		SetParentDirectory(data, this_dir);

		return S_OK;
	}

	HRESULT Close(
		LPCVOID pData
	) {
		delete[] pData;
		return S_OK;
	}
};

bool TagTool::Utilities::DirectXUtilities::CompilePCShaderFromFile(
	String ^ _File,
	array<MacroDefine^>^ _Defines,
	String ^ _FunctionName,
	String ^ _Profile,
	UInt32 Flags1,
	UInt32 Flags2,
	array<Byte>^% Shader,
	String ^% ErrorMsgs
)
{
	std::wstring File = MarshalStringW(_File);
	std::string FunctionName = MarshalStringA(_FunctionName);
	std::string Profile = MarshalStringA(_Profile);

	// Create the macros array
	auto num_macros = _Defines->Length + 1;
	D3D_SHADER_MACRO* macros = new D3D_SHADER_MACRO[num_macros];
	memset(macros, 0, sizeof(D3D_SHADER_MACRO) * num_macros);
	for (auto i = 0; i < _Defines->Length; i++) {

		auto cs_macro = _Defines[i];

		std::string _name = MarshalStringA(cs_macro->Name);
		std::string definition = MarshalStringA(cs_macro->Definition);

		auto cstr_name = new char[_name.size()];
		auto cstr_definition = new char[definition.size()];
		memcpy(cstr_name, _name.data(), _name.size());
		memcpy(cstr_definition, definition.data(), definition.size());

		macros[i].Name = cstr_name;
		macros[i].Definition = cstr_definition;
	}

	LPD3DBLOB shader = nullptr;
	LPD3DBLOB errors = nullptr;

	auto root_directory = System::IO::Path::GetDirectoryName(_File);
	auto std_root_directory = MarshalStringA(root_directory);
	ExtInclude include = ExtInclude(std_root_directory);
	

	HRESULT result = D3DCompileFromFile(
		File.c_str(),
		macros,
		&include,
		FunctionName.c_str(),
		Profile.c_str(),
		(DWORD)Flags1,
		(DWORD)Flags2,
		&shader,
		&errors);

	if (result != S_OK) ErrorMsgs = gcnew String(reinterpret_cast<char*>(errors->GetBufferPointer()));
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

String ^ TagTool::Utilities::DirectXUtilities::DisassemblePCShader(array<Byte>^ _Data, UInt32 Flags)
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

String ^ TagTool::Utilities::DirectXUtilities::MacroDefine::ToString()
{
	return "#define " + Name + " " + Definition;
}
//
//array<Byte>^ TagTool::Utilities::DirectXUtilities::AssemblePCShader(String ^ _source)
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
