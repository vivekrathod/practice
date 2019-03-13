using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CommonServiceLocator;
using Prism.Modularity;
using Prism.Unity;

namespace PrismSampleApp
{
    /// <summary>
    /// Initializes Prism/Unity components and services
    /// </summary>
    public class Bootstrapper : UnityBootstrapper
    {
        /// <summary>
        /// Make main window class as Prism shell
        /// </summary>
        protected override DependencyObject CreateShell()
        {
            return ServiceLocator.Current.GetInstance<Shell>();
        }

        /// <summary>
        /// Set the application's window to the current shell and display it
        /// </summary>
        protected override void InitializeShell()
        {
            Application.Current.MainWindow = (Shell)this.Shell;
            Application.Current.MainWindow.Show();
        }

        protected override IModuleCatalog CreateModuleCatalog()
        {
            // Use local directory as module catalog
            return new DirectoryModuleCatalog { ModulePath = "Modules" };
        }
    }
}
