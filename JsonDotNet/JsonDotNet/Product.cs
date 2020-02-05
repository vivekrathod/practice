using System;

namespace JsonDotNet
{
    public class Product
    {
        public DateTime ExpiryDate { get; set; }
        public string Name { get; set; }
    }

    public class ColorfulProduct
    {
        public Product Product { get; set; }
        public string Color { get; set; }
    }
}