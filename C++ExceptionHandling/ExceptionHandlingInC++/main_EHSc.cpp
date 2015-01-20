#include <iostream>
using namespace std;

void ThrowAccessViolationUnderEHSc()
{
	// access violation
		char temp = 'A';
		char* ptr = 0;
		ptr = &temp + 0x111111;
		*ptr = 'B';
}

// with /EHSc you require __try to catch this AV
void CatchAccessViolationUsingSEH()
{
	__try
		{
			ThrowAccessViolationUnderEHSc();
		}
	__except(1)
		{
			cout << "caught SEH?" << endl;
		}
}

// chnage main_ to main to test under /EHSc
int main_()
{
	cout << "Hello World!" << endl;
	//CatchAccessViolationUsingSEH();

	try
	{
		ThrowAccessViolationUnderEHSc();
	}
	catch(...)
	{
		cout << "caught unknown exception type" << endl;
	}
	return 1;
}
