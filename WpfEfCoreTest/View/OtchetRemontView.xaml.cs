using System.Windows;
using WpfEfCoreTest.ViewModel;

namespace WpfEfCoreTest.View
{
    /// <summary>
    ///     Логика взаимодействия для OtchetRemontView.xaml
    /// </summary>
    public partial class OtchetRemontView : Window
    {
        public OtchetRemontView()
        {
            InitializeComponent();

            DataContext =
                new OtchetRemontVM(new DefaultDialogService(), new JsonFileService());
        }
    }
}