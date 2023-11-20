using System.Windows;
using System.Windows.Controls;
using WpfEfCoreTest.ViewModel;

namespace WpfEfCoreTest.View
{
    /// <summary>
    ///     Логика взаимодействия для F111View.xaml
    /// </summary>
    public partial class F111View : Window
    {
        public static ListView AllDataF111ToUser;


        public F111View()
        {
            InitializeComponent();

            DataContext = new F111VM();

            AllDataF111ToUser = LvF111User;
        }
    }
}