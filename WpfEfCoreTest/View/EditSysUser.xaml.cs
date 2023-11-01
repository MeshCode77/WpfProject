using System.Windows;
using WpfEfCoreTest.Model;
using WpfEfCoreTest.ViewModel;

namespace WpfEfCoreTest.View
{
    /// <summary>
    ///     Логика взаимодействия для EditSysUser.xaml
    /// </summary>
    public partial class EditSysUser : Window
    {
        public EditSysUser(UserSy user)
        {
            InitializeComponent();

            //DataContext = new UserSysVM();
            //UserSysVM.SelectedUserSys = user;

            //UserSysVM.Id = user.Id;
            UserSysVM.Fname = user.Fname;
            UserSysVM.Login = user.Login;
            UserSysVM.Pass = user.Pass;
        }
    }
}