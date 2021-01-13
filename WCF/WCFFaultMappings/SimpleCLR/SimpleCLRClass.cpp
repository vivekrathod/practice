#using "MSCorLib.dll"
#include <iostream>

using namespace System;

namespace SimpleCLR
{
	public ref class SimpleCLRClass
	{
		public:
			void ThrowSEHException()
			{
				throw gcnew System::Runtime::InteropServices::SEHException();
			}

			void ThrowDivideByZero()
			{
				throw gcnew DivideByZeroException();
			}

			void ThrowBufferOverflow()
			{

			}

			void ThrowAccessViolation()
			{
				char* p = 0;
				int a = strlen(p);
			}
	};
}