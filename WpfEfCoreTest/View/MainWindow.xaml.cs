using System.Windows;
using System.Windows.Controls;

namespace WpfEfCoreTest.View
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //private TestContext db;

        public static ListView AllDataF111ToUser;
        public static ListView AllDataFormular;

        //public static ObservableCollection<F111> f111s { get; set; }
        public MainWindow()
        {
            InitializeComponent();

            AllDataF111ToUser = lvF111;
            AllDataFormular = lvFormular;
        }


        private void Users_Click(object sender, RoutedEventArgs e)
        {
            var allUsers = new SprUsers();
            allUsers.ShowDialog();
        }

        private void Podr_Click(object sender, RoutedEventArgs e)
        {
            var sprPodr = new SprPodrWindow();

            sprPodr.ShowDialog();
        }

        private void SprOborud_OnClick(object sender, RoutedEventArgs e)
        {
            var sprOborudView = new SprOborudView();
            sprOborudView.ShowDialog();
        }

        private void SprKompl_OnClick(object sender, RoutedEventArgs e)
        {
            var sprKomplView = new SprKomplView();
            sprKomplView.ShowDialog();
        }
    }
}