using System.Windows;
using System.Windows.Controls;
using WpfEfCoreTest.ViewModel;

namespace WpfEfCoreTest.View
{
    /// <summary>
    /// Логика взаимодействия для SprUsers.xaml
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

            AllUsersView = ViewAllUsers;   // ViewAllUsers - имя ListView в xaml
            AllPodrsView = ViewAllUsers;
            AllInfosView = ViewAllUsers;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //var podr = txbPodr.Text;

        }

        private void dgvSprUsers_Loaded(object sender, RoutedEventArgs e)
        {
            //var userData = from c in tc.Users select c; // загружаем данные в dgvSprUsers

            //dgvSprUsers.ItemsSource = userData.ToList();
        }

        private void dgvSprUsers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //User item = dgvSprUsers.SelectedItem as User;

            //if (item != null)
            //{
            //    if (txbLogin != null)
            //        txbLogin.Text = item.Infos.ToList()[0].Login;

            //    //txbLogin.Text = item.IdPodrNavigation.NamePodr;  // получаем подразделение

            //}

        }

        
    }
}
