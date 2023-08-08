using System.Windows;
using System.Windows.Controls;

namespace WpfEfCoreTest.View
{
    /// <summary>
    ///     Логика взаимодействия для SprUsers.xaml
    /// </summary>
    public partial class SprUsers : Window
    {
        public static ListView AllUsersView;
        public static ListView AllPodrsView;
        public static ListView AllInfosView;

        public SprUsers()
        {
            InitializeComponent();

            //DataContext = new SprUsersVM();

            AllUsersView = ViewAllUsers; // ViewAllUsers - имя ListView в xaml
            AllPodrsView = ViewAllUsers;
            AllInfosView = ViewAllUsers;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}