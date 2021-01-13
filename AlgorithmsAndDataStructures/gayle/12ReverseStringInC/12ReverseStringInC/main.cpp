#include <iostream>
using namespace std;

void reverse(char* str)
{
	int length = strlen(str);

	for (int i = 0; i < length / 2; i++)
	{
		char temp = str[length - i - 1];
		str[length - i - 1] = str[i];
		str[i] = temp;
	}
}

int main()
{
	char str[] = "abcd";
	cout << "string to reverse: " << str << endl;
	reverse(str);
	cout << "reversed string: " << str << endl;

}