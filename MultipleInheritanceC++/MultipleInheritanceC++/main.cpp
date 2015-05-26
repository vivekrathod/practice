#include <iostream>
#include "Header.h"
using namespace std;

int main()
{
	Bottom *bottom = new Bottom();
	try
	{
		bottom->b = 12;
		cout << "Bottom::Left:b is " << bottom->b << endl;
		bottom->c = 3;
		cout << "Bottom::Left:c is " << bottom->c << endl;
		bottom->d = 44;
		cout << "Bottom::d is " << bottom->d << endl;
		Left *left = bottom;
		left->b = 8;
		cout << "Left::b is " << left->b << endl;
		left->a = 7;
		cout << "Left::a is " << left->a << endl;
		Right *right = bottom;
		right->c = 4;
		cout << "Right::c is " << right->c << endl;
		// note that right->a will NOT be the same as left->a because there are two copies for Top 
		// object in Bottom (one via Left and other via Right)
		cout << "Right::a is " << right->a << endl;
		// this upcast is ambiguous for the same reason
		// Top *top = bottom;
		Top *top = (Left*)bottom;
		cout << "Top::a via Left pointer is " << top->a << endl;
		top = (Right*)bottom;
		cout << "Top::a via Right pointer is " << top->a << endl;

		// bottom->a is ambiguous because there are two valid paths to Top::a (one via Left and other via Right)
		bottom->Left::a = 9;
		cout << "Bottom::Left::Top:a is " << bottom->Left::a << endl;
		// note that bottom->Left::a and bottom->Right::a are NOT the same as 
		// there are two copies for Top object in Bottom (one via Left and other via Right)
		cout << "Bottom::Right::Top:a is " << bottom->Right::a << endl;
	}
	catch (...)	{ cout << "exception.." << endl; }
	delete bottom;

	// use virtual inheritance
	VirtualBottom *vbottom = new VirtualBottom();
	try
	{
		vbottom->VirtualLeft::a = 9;
		cout << "VirtualBottom::VirtualLeft::Top:a is " << vbottom->VirtualLeft::a << endl;
		// note that vbottom->VirtualLeft::a and vbottom->VirtualRight::a are the same
		// now, there is only 1 copy of Top object in Bottom even of there are two paths to it (one via Left and other via Right)
		cout << "VirtualBottom::VirtualRight::Top:a is " << vbottom->VirtualRight::a << endl;
		// and so there is not need to qualify 'a'
		cout << "VirtualBottom::Top:a is " << vbottom->a << endl;
		// now this works
		Top *top = vbottom;
		cout << "VirtualBottom::Top:a via Top pointer is " << top->a << endl;
		// and so does this - but note the difference in addresses
		VirtualLeft *vleft = vbottom;
		VirtualRight *vright = vbottom;
		cout << "vbottom points to " << hex << vbottom << endl;
		cout << "top points to " << hex << top << endl;
		cout << "vleft points to " << hex << vleft << endl;
		cout << "vright  points to " << hex << vright << endl;
	}
	catch (...){ cout << "exception.." << endl; }
	delete vbottom;
}