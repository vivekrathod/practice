using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Constants;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace NavigationMenu
{
    /// <summary>
    /// Register components of module with Unity/Prism
    /// </summary>
    [Module(ModuleName = ModuleNames.NAVIGATION_MENU)]
    public class NavigationMenuModule : IModule
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //this._regionManager.RegisterViewWithRegion(RegionNames.MENU_REGION, typeof(NavigationMenu));
            //throw new NotImplementedException();
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            //throw new NotImplementedException();
        }

        private readonly IRegionManager _regionManager;

        public NavigationMenuModule(IRegionManager regionManager)
        {
            this._regionManager = regionManager;
        }
    }
}
