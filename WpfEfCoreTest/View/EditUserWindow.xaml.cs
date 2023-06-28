using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfEfCoreTest.Model;
using WpfEfCoreTest.ViewModel;

namespace WpfEfCoreTest.View
{
    /// <summary>
    /// Логика взаимодействия для EditUserWindow.xaml
    /// </summary>
    public partial class EditUserWindow : Window
    {
        public EditUserWindow(User user)
        {
            InitializeComponent();

            DataContext = new SprUsersVM();

            SprUsersVM.SelectedUser = user;

            SprUsersVM.Lname = user.Lname;
            SprUsersVM.Fname = user.Fname;
            SprUsersVM.Mname = user.Mname;

            SprUsersVM.NamePodr = user.UserPodr.NamePodr;

            if (user.UserInfo != null)
            {
                SprUsersVM.Doljnost = user.UserInfo.Doljnost;
                SprUsersVM.NameComp = user.UserInfo.NameComp;
            }
            return;
            

        }
    }
}
