#include <iostream>
using namespace std;

// with /EHa you can catch AV using std C++ try/Catch
void ThrowAccessViolationUnderEHa()
{
	// access violation
		char temp = 'A';
		char* ptr = 0;
		ptr = &temp + 0x111111;
		*ptr = 'B';
}

// change main_ to main to test under /EHa
int main_()
{
	cout << "Hello World!" << endl;
	
	try
	{
		ThrowAccessViolationUnderEHa();
	}
	catch(...)
	{
		cout << "caught unknown exception type" << endl;
	}

	return 1;
}