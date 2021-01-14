#include <iostream>;
#include <Windows.h>;

using namespace std;
using namespace System;

class MyClass
{
};

void ThrowAccessViolation()
{
	// access violation
		char temp = 'A';
		char* ptr = 0;
		ptr = &temp + 0x111111;
		*ptr = 'B';
}

void DevideByZero()
{
	int x =5,y=0;
	int z = x/y;
}

void RaiseWin32StructuredCException()
{
	RaiseException( 
                1,                    // exception code 
                0,                    // continuable exception 
                0, NULL);
}

void CatchAccessViolationUsingSEH()
{
	__try
		{
			ThrowAccessViolation();
		}
	__except(1)
		{
			Console::WriteLine("caught SEH?");
		}
}

int main(array<System::String ^> ^args)
{
	Console::WriteLine("Hello World!");
	
	//CatchAccessViolationUsingSEH();

	try
	{
		//throw runtime_error("runtime error...");
		//throw 20;
		//throw MyClass();
		//DevideByZero();

		// /EHa enables catching of SEH exceptions using try/catch
		// With /clr, /EHa is mandatory
		
		// this will be caught
		// RaiseWin32StructuredCException();

		// this will NOT be caught even though it is SEH
		// .NET clr somehow does not seem to allow this using try/catch? and stangely it does seem to allow this when using __try/__except (see CatchAccessViolationUsingSEH())
		ThrowAccessViolation();

		//DebugBreak();
	}
	catch (exception& ex)
	{
		Console::WriteLine("caught std exception");
	}
	catch(Runtime::InteropServices::SEHException^ ex)
	{
		Console::WriteLine("caught SEH exception");
	}
	catch(Exception^ ex)
	{
		Console::WriteLine("caught .net exception");
	}
	catch(...)
	{
		Console::WriteLine("caught unknown exception type");
	}
	return 0;
};