using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TasksDeadlockDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DeadlockDemo.Test();
        }
    }

    /// <summary>
    /// https://msdn.microsoft.com/en-us/magazine/jj991977.aspx
    /// Demos a deadlock when blocking on Async code
    /// The root cause of this deadlock is due to the way await handles contexts. By default, when an incomplete Task is awaited, the current “context” is captured and used to resume the method when the Task completes. This “context” is the current SynchronizationContext unless it’s null, in which case it’s the current TaskScheduler. GUI and ASP.NET applications have a SynchronizationContext that permits only one chunk of code to run at a time. When the await completes, it attempts to execute the remainder of the async method within the captured context. But that context already has a thread in it, which is (synchronously) waiting for the async method to complete. They’re each waiting for the other, causing a deadlock.
    /// </summary>
    public static class DeadlockDemo
    {
        private static async Task DelayAsync()
        {
            Console.WriteLine("starting async ...");
            await Task.Run(
                () =>
                {
                    Thread.Sleep(100);
                }
            );//.ConfigureAwait(false); // avoid deadlock by configuring the await to not resume on the original context
            Console.WriteLine("ending async ...");
        }

        // This method causes a deadlock when called in a GUI or ASP.NET context.
        public static void Test()
        {
            // Start the delay.
            var delayTask = DelayAsync();
            // Wait for the delay to complete.
            delayTask.Wait(); // cause of deadlock 
        }
    }
}
