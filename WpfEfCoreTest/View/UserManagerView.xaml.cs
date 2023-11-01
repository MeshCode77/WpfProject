using System.Windows;
using System.Windows.Controls;

namespace WpfEfCoreTest.View
{
    public partial class UserManagerView : Window
    {
        public static ListView UpdateSysUserView;

        public UserManagerView()
        {
            InitializeComponent();

            UpdateSysUserView = ViewAllSysUsers;
        }
    }
}