using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackUsingArray
{
    class Program
    {
        static void Main(string[] args)
        {
            Stack<int> myStack = new Stack<int>(100);
            myStack.Push(1);
            //Console.WriteLine(myStack.Pop());
            myStack.Push(11);
            Console.WriteLine(myStack.Pop());
            Console.WriteLine(myStack.Pop());
        }
    }

    public class Stack<T>
    {
	    private T[] _array;
	    private int _size;
	    private int _current;
	
	    public Stack(int size)
	    {
		    _current = -1;
		    _size = size;
		    _array = new T[size];
	    }
	
	    public void Push(T obj)
	    {
		    if (_current > _size)
			    throw new ArgumentException("Index out of bounds..");

		    _array[++_current] = obj;
	    }
	
	    public T Pop()
	    {
		    if (_current == -1)
			    throw new ArgumentException("No elements in the stack");
		
		    return _array[_current--];
	    }
    }
}
