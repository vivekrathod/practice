using ModuleA.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ModuleA
{
    public class ModuleAModule : IModule
    {
        private readonly IRegionManager _regionManager;

        public ModuleAModule(IRegionManager regionManager)
        {
            this._regionManager = regionManager;
        }
        public void OnInitialized(IContainerProvider containerProvider)
        {
            IRegion region = _regionManager.Regions["ContentControlRegion"];
            var view1 = containerProvider.Resolve<ViewA>();
            region.Add(view1);

            var view2 = containerProvider.Resolve<ViewA>();
            view2.Content = new TextBlock { Text = "Another instance of ViewA", HorizontalAlignment=System.Windows.HorizontalAlignment.Center, VerticalAlignment = System.Windows.VerticalAlignment.Center };
            // simply adding it does not display (activate) the view
            region.Add(view2);
            // display (activate) the view
            region.Activate(view2);
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            
        }
    }
}
