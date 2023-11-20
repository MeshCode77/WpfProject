using System.Windows;
using WpfEfCoreTest.ViewModel;

namespace WpfEfCoreTest.View
{
    /// <summary>
    ///     Логика взаимодействия для AddF111View.xaml
    /// </summary>
    public partial class AddF111View : Window
    {
        public AddF111View()
        {
            InitializeComponent();

            DataContext = new F111VM();
        }
    }
}