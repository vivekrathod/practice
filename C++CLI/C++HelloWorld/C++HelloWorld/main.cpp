#include <iostream>
#include <string>

int main()
{
	std::cout << "hello world! " << std::endl;
	
	char a;
	std::cin >> a;

	std::cout << "you entered: " << a << std::endl;
}

std::string function(const std::string& temp = "abc")
{
	std::cout << "temp: " << temp << std::endl;
	return temp;
}
