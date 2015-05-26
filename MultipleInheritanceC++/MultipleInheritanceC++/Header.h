class Top
{
public:
	int a;
	void method1() {};
};

class Left :public Top
{
public:
	int b;
};

class Right :public Top
{
public:
	int c;
};

class Bottom :public Left, public Right
{
public:
	int d;
};

class VirtualLeft : public virtual Top
{
public:
	int b;
};

class VirtualRight : public virtual Top
{
public:
	int c;
};

class VirtualBottom :public VirtualLeft, public VirtualRight
{
public:
	int d;
};