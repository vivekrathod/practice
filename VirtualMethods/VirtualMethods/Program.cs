using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualMethods
{
    class Program
    {
        static void Main(string[] args)
        {
            Shape s = new Triangle();
            s.Draw();
            
            s = new Square();
            s.Draw();
        }

        static void Draw(Shape s)
        {
            s.Draw();
        }
    }

    class Shape
    {
        public virtual void Draw()
        {
            Console.WriteLine("Drawing Shape..");
        }
    }

    class Triangle : Shape
    {
        public void Draw()
        {
            Console.WriteLine("Drawing Triangle..");
        }
    }

    class Square : Shape
    {
        public void Draw()
        {
            Console.WriteLine("Drawing Square..");
        }
    }
}
