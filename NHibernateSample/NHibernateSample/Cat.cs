using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHibernateSample
{
    public class Cat 
    {
        public virtual Int64 Id {get;set;}
        public virtual string Name { get; set; }
        public virtual char Sex { get; set; }
        public virtual int Weight { get; set; }

        public override bool Equals(object obj)
        {
            if (this == obj)
                return true;

            if (obj == null || (GetType() != obj.GetType()))
                return false;

            Cat cat = obj as Cat;
            return cat.Name == Name && cat.Sex == Sex && cat.Weight == Weight;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode() ^ Sex.GetHashCode() ^ Weight.GetHashCode();
        }
    }
}
