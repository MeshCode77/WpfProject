using System.Windows;
using WpfEfCoreTest.ViewModel;

namespace WpfEfCoreTest.View
{
    /// <summary>
    ///     Логика взаимодействия для AddFormularView.xaml
    /// </summary>
    public partial class AddFormularView : Window
    {
        public AddFormularView()
        {
            InitializeComponent();

            DataContext = new FormularVM();
        }
    }
}