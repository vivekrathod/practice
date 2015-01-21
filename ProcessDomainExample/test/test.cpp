#include <iostream>

using namespace System;
using namespace System::Runtime;
using namespace System::Runtime::Serialization;

namespace test 
{
	//[Serializable]
	public ref class refclass : MarshalByRefObject
	{
	public:
		
		refclass()
		{
			//System::Diagnostics::Debugger::Break();
		}

		void overflow(char* src)
		{
			char dest[] = "abc";
			strcpy(dest, src);
		}

		void invokeBufferOverflow()
		{
			Console::WriteLine(AppDomain::CurrentDomain);
			// buffer overflow
			const int length = 100000;
			char src[length];
			for (int i = 0; i < length; i++)
			{
				src[i] = 'a';
			}
			src[length-1] = '\0';
			overflow(src);
		}

		void invokeStdException()
		{
			throw new std::runtime_error("sample error");
		}

		void invokeAccessViolation()
		{
			// randmom memory write
			char tempChar = 'a';
			char *charPtr = &tempChar;
			charPtr = charPtr + 0xFFFFFF;
			*charPtr = 'b';
		}

		void invokeStackOverflow(int i)
		{
			int j = i*2;
			return invokeStackOverflow(j);
		}

		void SimpleMethod()
		{
			//System::Diagnostics::Debugger::Break();
		}
	};
	
}