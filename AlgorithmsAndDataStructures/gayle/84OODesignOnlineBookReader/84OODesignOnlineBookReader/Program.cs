using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _84OODesignOnlineBookReader
{
    class Program
    {
        static void Main(string[] args)
        {
        }
    }

    public class Book
    {
        
    }

    public class User
    {
        // currently reading book(s)
        private ICollection<Book> _books;

    }

    public class Reader
    {
        private ICollection<User> _users;

    }


}
