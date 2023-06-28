using System.Windows;
using System.Windows.Controls;
using WpfEfCoreTest.ViewModel;

namespace WpfEfCoreTest.View
{
    /// <summary>
    /// Логика взаимодействия для SprPodrWindow.xaml
    /// </summary>
    public partial class SprPodrWindow : Window
    {
        public static ListView UpdatePodrsView;

        public SprPodrWindow()
        {
            InitializeComponent();

            this.DataContext = new SprPodrVM(); // new SprUsersVM();

            UpdatePodrsView = LvSprPodr;
        }
    }
}
