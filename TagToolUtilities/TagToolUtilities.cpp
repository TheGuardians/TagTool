#include <d3dx9.h>

#include "stdafx.h"
#include "TagToolUtilities.h"

#include <string>
#include <vector>

#include <msclr\marshal.h>
#include <msclr\marshal_cppstd.h>

using namespace System;
using namespace System::Runtime::InteropServices;
using namespace System::Collections::Generic;

std::string MarshalStringA(String^ s)
{
	std::string outputstring;
	const char* kPtoC = (const char*)(Marshal::StringToHGlobalAnsi(s)).ToPointer();
	outputstring = kPtoC;
	Marshal::FreeHGlobal(IntPtr((void*)kPtoC));
	return outputstring;
}

std::wstring MarshalStringW(String^ s)
{
	return msclr::interop::marshal_as<std::wstring>(s);
}

array<Byte>^ TagTool::Utilities::DirectXUtilities::AssemblePCShader(String ^ _source)
{
	if (_source == nullptr) throw gcnew Exception("source is null");

	std::string source = MarshalStringA(_source);

	LPD3DXBUFFER buffer = nullptr;
	LPD3DXBUFFER errors = nullptr;

	HRESULT result = D3DXAssembleShader(source.c_str(), (UINT)source.length(), NULL, NULL, 0, &buffer, &errors);

	if (result != S_OK) {

		String^ errors_str = gcnew String(reinterpret_cast<char*>(errors->GetBufferPointer()));
		throw gcnew Exception(errors_str);

	}

	auto arr = gcnew array<Byte>(buffer->GetBufferSize());
	auto ptr = reinterpret_cast<char*>(buffer->GetBufferPointer());
	for (DWORD i = 0; i < buffer->GetBufferSize(); i++) {
		arr[i] = ptr[i];
	}

	return arr;
}

bool TagTool::Utilities::DirectXUtilities::CompilePCShader(String ^ _SrcData, array<MacroDefine^>^ _Defines, String ^ _Include, String ^ _FunctionName, String ^ _Profile, UInt32 flags, array<Byte>^% Shader, String ^% ErrorMsgs, String ^% ConstantTable)
{
	std::string SrcData = MarshalStringA(_SrcData);
	std::string Include = MarshalStringA(_Include);
	std::string FunctionName = MarshalStringA(_FunctionName);
	std::string Profile = MarshalStringA(_Profile);

	// Create the macros array
	auto num_macros = _Defines->Length + 1;
	D3DXMACRO* macros = new D3DXMACRO[num_macros];
	memset(macros, 0, sizeof(D3DXMACRO) * num_macros);
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

	LPD3DXBUFFER shader = nullptr;
	LPD3DXBUFFER errors = nullptr;
	LPD3DXCONSTANTTABLE constanttable = nullptr;

	HRESULT result = D3DXCompileShader(
		SrcData.data(),
		(UINT)SrcData.size(),
		macros,
		nullptr,
		FunctionName.c_str(),
		Profile.c_str(),
		(DWORD)flags,
		&shader,
		&errors,
		&constanttable);

	List<Byte>^ data = gcnew List<Byte>();
	for (DWORD i = 0; i < shader->GetBufferSize(); i++) {
		auto val = reinterpret_cast<unsigned char*>(shader->GetBufferPointer())[i];
		data->Add(val);
	}
	Shader = data->ToArray();

	if (result != S_OK) ErrorMsgs = gcnew String(reinterpret_cast<char*>(errors->GetBufferPointer()));

	//TODO: Implement large address aware constant table
	// https://msdn.microsoft.com/en-us/library/windows/desktop/bb172731(v=vs.85).aspx


	//ConstantTable = gcnew String(reinterpret_cast<char*>(errors->GetBufferPointer()));


	return result == S_OK;
}

//array<Byte>^ TagTool::Utilities::DirectXUtilities::CompilePCShader(String ^ _source, String ^ _defines, bool from_file)
//{
//	if (_source == nullptr) throw gcnew Exception("source is null");
//
//	std::string source = MarshalStringA(_source);
//	std::string defines = MarshalStringA(_defines);
//
//	LPD3DXBUFFER buffer = nullptr;
//	LPD3DXBUFFER errors = nullptr;
//
//	HRESULT result = D3DXCompileShader(source.data(), source.size(), )
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

String ^ TagTool::Utilities::DirectXUtilities::DisassemblePCShader(array<Byte>^ _data)
{
	if (_data == nullptr) throw gcnew Exception("data is null");

	LPD3DXBUFFER buffer = nullptr;

	std::vector<unsigned char> data;
	data.reserve(_data->Length);
	for (auto i = 0; i < _data->Length; i++) {
		auto val = _data[i];
		data.push_back(val);
	}

	HRESULT result = D3DXDisassembleShader(reinterpret_cast<DWORD*>(data.data()), false, nullptr, &buffer);

	if (result != S_OK) {
		if (result == D3DXERR_INVALIDDATA) {
			throw gcnew Exception("Invalid data");
		}
		else {
			throw gcnew Exception();
		}
	}

	return gcnew String(reinterpret_cast<char*>(buffer->GetBufferPointer()));
}

String ^ TagTool::Utilities::DirectXUtilities::MacroDefine::ToString()
{
	return "#define " + Name + " " + Definition;
}
