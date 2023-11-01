using System.Windows;
using WpfEfCoreTest.Model;
using WpfEfCoreTest.ViewModel;

namespace WpfEfCoreTest.View
{
    /// <summary>
    ///     Логика взаимодействия для EditPodrWnd.xaml
    /// </summary>
    public partial class EditPodrWnd : Window
    {
        public EditPodrWnd(Podr NamePodr)
        {
            InitializeComponent();

            DataContext = new SprPodrVM();

            SprPodrVM.SelectedPodr = NamePodr;

            SprPodrVM.namePodr = NamePodr.NamePodr;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}