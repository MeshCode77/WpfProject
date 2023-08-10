using System.Windows;
using WpfEfCoreTest.Model;
using WpfEfCoreTest.ViewModel;

namespace WpfEfCoreTest.View
{
    /// <summary>
    ///     Логика взаимодействия для EditNKWnd.xaml
    /// </summary>
    public partial class EditNKWnd : Window
    {
        public EditNKWnd(Komplect nameKomplect)
        {
            InitializeComponent();

            DataContext = new SprKomplVM();

            SprKomplVM.SelectedNK = nameKomplect;

            SprKomplVM.nameKompl = nameKomplect.NameKompl;
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}