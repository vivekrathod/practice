// CppCliSample.cpp : main project file.

#include "stdafx.h"

using namespace System;

int main(array<System::String ^> ^args)
{
	DateTime utcTime = DateTime::UtcNow;
	//utcTime.
	Console::WriteLine(L"Hello World {0}{1}{2}{3}{4}{5}", utcTime.Year, utcTime.Month, utcTime.Day, utcTime.Hour,utcTime.Second,utcTime.Millisecond);

    return 0;
}
