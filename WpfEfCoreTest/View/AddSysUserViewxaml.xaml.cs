using System.Windows;
using WpfEfCoreTest.Model;

namespace WpfEfCoreTest.View
{
    /// <summary>
    ///     Логика взаимодействия для AddSysUserViewxaml.xaml
    /// </summary>
    public partial class AddSysUserViewxaml : Window
    {
        public AddSysUserViewxaml(UserSy user)
        {
            InitializeComponent();

            user = null;
            //UserSysVM.Fname = null;
            //UserSysVM.Login = null;
            //UserSysVM.Pass = null;
        }
    }
}