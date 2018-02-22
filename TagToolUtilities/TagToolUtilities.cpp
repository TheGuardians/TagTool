#include <d3dx9.h>

#include "stdafx.h"
#include "TagToolUtilities.h"

#include <string>

using namespace System;
using namespace System::Runtime::InteropServices;

std::string MarshalString(String^ s)
{
	std::string outputstring;
	const char* kPtoC = (const char*)(Marshal::StringToHGlobalAnsi(s)).ToPointer();
	outputstring = kPtoC;
	Marshal::FreeHGlobal(IntPtr((void*)kPtoC));
	return outputstring;
}

array<Byte>^ TagTool::Utilities::DirectXUtilities::AssembleShader(String ^ _source)
{
	if (_source == nullptr) throw gcnew Exception("source is null");
	
	std::string source = MarshalString(_source);
	
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
