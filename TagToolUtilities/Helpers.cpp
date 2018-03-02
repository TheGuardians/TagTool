#include "stdafx.h"
#include "Helpers.h"

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
using namespace TagTool::Utilities;

std::string Helpers::MarshalStringA(String^ s)
{
	std::string outputstring;
	const char* kPtoC = (const char*)(Marshal::StringToHGlobalAnsi(s)).ToPointer();
	outputstring = kPtoC;
	Marshal::FreeHGlobal(IntPtr((void*)kPtoC));
	return outputstring;
}

std::wstring Helpers::MarshalStringW(String^ s)
{
	return msclr::interop::marshal_as<std::wstring>(s);
}

std::string Helpers::ExtractDirectoryName(std::string path) {

	auto str = gcnew String(path.c_str());
	auto dir = System::IO::Path::GetDirectoryName(str);
	return MarshalStringA(dir);
}

std::string Helpers::ReadFile(std::string _path) {

	auto path = gcnew String(_path.c_str());
	if (System::IO::File::Exists(path)) {

		auto text = System::IO::File::ReadAllText(path);
		return MarshalStringA(text);

	}
	throw gcnew Exception("File does not exist " + path);
}
