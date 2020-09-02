#pragma once
#include <string>

using namespace System;

namespace Win32UtilitiesLib
{
	public ref class Win32Utilities
	{
	public:
		static bool VerifyEmbeddedDigitalSignature(String^ filePath);
		static std::wstring MarshalString(String^ str)
		{
			if (str == nullptr)
				return std::wstring();
			IntPtr ptr = Runtime::InteropServices::Marshal::StringToHGlobalUni(str);
			std::wstring retVal(reinterpret_cast<wchar_t*>(ptr.ToPointer()));
			Runtime::InteropServices::Marshal::FreeHGlobal(ptr);
			return retVal;
		}
	};
}
