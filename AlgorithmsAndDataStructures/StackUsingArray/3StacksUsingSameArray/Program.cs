using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3StacksUsingSameArray
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] array = new int[5];
            Stack1<int> stack1 = new Stack1<int>(array);
            Stack2<int> stack2 = new Stack2<int>(array);
            Stack3<int> stack3 = new Stack3<int>(array);

            //stack1.Push(1);
            //stack1.Push(11);
            
            //stack2.Push(2);
            //stack2.Push(22);
            
            stack3.Push(3);

            //Console.WriteLine(stack1.Pop());
            //Console.WriteLine(stack1.Pop());
            
            //Console.WriteLine(stack2.Pop());
            //Console.WriteLine(stack2.Pop());

            Console.WriteLine(stack3.Pop());
            Console.WriteLine(stack1.Pop());
        }
    }

    public abstract class Stack<T>
    {
	    private T[] _array;
	    private int _size;
	    private int _current;
	    protected int _init; 
	    private int _offset = 3; // no of stacks we wish to implement
	
	    public Stack(T[] array)
	    {
		    _size = array.Length;
		    _array = array;
		    _current = _init - _offset;
	    }
	
	    public void Push(T obj)
	    {
		    int index = _current + _offset;
		    if (index > _size)
			    throw new ArgumentException("Index out of bounds..");
		
		    _array[index] = obj;
		    _current = index;
	    }
	
	    public T Pop()
	    {
		    if (_current == _init - _offset)
			    throw new ArgumentException("No elements in the stack");
		
		    T obj = _array[_current];
		    _current = _current - _offset;
		    return obj;
	    }
    }

    public class Stack1<T> : Stack<T>
    {
	    public Stack1(T[] array) : base(array)
	    {
		    _init = 0;
	    }
    }

    public class Stack2<T> : Stack<T>
    {
	    public Stack2(T[] array) : base(array)
	    {
		    _init = 1;
	    }
    }

    public class Stack3<T> : Stack<T>
    {
	    public Stack3(T[] array) : base(array)
	    {
		    _init = 2;
	    }
    }
}
