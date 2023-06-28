using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfEfCoreTest.Model;
using WpfEfCoreTest.ViewModel;

namespace WpfEfCoreTest.View
{
    /// <summary>
    /// Логика взаимодействия для F111View.xaml
    /// </summary>
    public partial class F111View : Window
    {
        public static ListView AllDataF111ToUser;
        

        public F111View()
        {
            InitializeComponent();

            DataContext = new F111VM();

            //DataContext = new MainWindowVM();

            AllDataF111ToUser = LvF111User;

            

            //if (F111VM.SelectedRowF111 != null)
            //{
            //    btnAdd.IsEnabled = true;
            //}
            //else
            //{
            //    btnAdd.IsEnabled = false;
            //}

            //if (F111VM.SelectedRowF111 != null)
            //{
            //    btnAdd.IsEnabled = true;
            //}


            //DataContext = new MainWindowVM();
        }
    }
}
