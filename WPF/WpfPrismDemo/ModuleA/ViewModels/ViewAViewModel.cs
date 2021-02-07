using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModuleA.ViewModels
{
    public class ViewAViewModel : BindableBase, INavigationAware
    {
        public DelegateCommand ClickCommand { get; private set; }

        private string _text = "Hello from ViewAViewModel";
        public string Text
        {
            get { return _text; }
            set { SetProperty(ref _text, value); }
        }

        private int _pageViews;
        public int PageViews
        {
            get { return _pageViews; }
            set { SetProperty(ref _pageViews, value); }
        }

        public ViewAViewModel()
        {
            ClickCommand = new DelegateCommand(Click, CanClick);
        }

        private void Click()
        {
            Text = "You clicked me!";
        }

        private bool CanClick()
        {
            return true;
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            PageViews++;
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }
    }
}
