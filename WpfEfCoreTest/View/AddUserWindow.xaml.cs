using System.Windows;
using WpfEfCoreTest.ViewModel;

namespace WpfEfCoreTest.View
{
    /// <summary>
    /// Логика взаимодействия для AddUserWindow.xaml
    /// </summary>
    public partial class AddUserWindow : Window
    {
        //public User User { get; private set; }

        public AddUserWindow()
        {
            InitializeComponent();

            //User = u;
            //this.DataContext = User;
            //DataContext = new SprUsersVM();  
        }

        //private void Accept_Click(object sender, RoutedEventArgs e)
        //{
        //    this.DialogResult = true;
        //}
    }
}
