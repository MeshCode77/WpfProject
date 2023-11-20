using System.Windows;
using WpfEfCoreTest.Model;
using WpfEfCoreTest.ViewModel;

namespace WpfEfCoreTest.View
{
    /// <summary>
    ///     Логика взаимодействия для EditUserWindow.xaml
    /// </summary>
    public partial class EditUserWindow : Window
    {
        public EditUserWindow(User user)
        {
            InitializeComponent();

            DataContext = new SprUsersVM();

            //SprUsersVM.SelectedUser = user;

            SprUsersVM.Lname = user.Lname;
            SprUsersVM.Fname = user.Fname;
            SprUsersVM.Mname = user.Mname;

            SprUsersVM.NamePodr = user.UserPodr.NamePodr;

            if (user.UserInfo != null)
            {
                SprUsersVM.Doljnost = user.UserInfo.Doljnost;
                SprUsersVM.NameComp = user.UserInfo.NameComp;
                SprUsersVM.Login = user.UserInfo.Login;
                SprUsersVM.Pass = user.UserInfo.Pass;
                SprUsersVM.Mac = user.UserInfo.Mac;
                SprUsersVM.Vtel = user.UserInfo.Vtel;
            }
        }
    }
}