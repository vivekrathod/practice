using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism;

namespace NavigationMenu
{
    public class NavigationMenuViewModel : IActiveAware
    {
        private bool _isActive;
        public bool IsActive
        {
            get
            {
                return _isActive;
            }
            set
            {
                _isActive = value;
                //IsActiveChanged(this, new EventArgs());
            }
        }
        public event EventHandler IsActiveChanged;
    }
}
