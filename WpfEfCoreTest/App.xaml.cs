using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WpfEfCoreTest.View;
using WpfEfCoreTest.ViewModel;

namespace WpfEfCoreTest
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly TestContext context;

        public App()
        {
            context = new TestContext();
        }


        //protected override void OnStartup(StartupEventArgs e)
        //{
        //    MainWindow = new MainWindow()
        //    {
        //        DataContext = new MainWindowVM(context)
        //    };

        //    MainWindow.Show();
        //}
    }
}
