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
    /// Логика взаимодействия для EditNOWnd.xaml
    /// </summary>
    public partial class EditNOWnd : Window
    {
        public EditNOWnd(NameOborud nameOborud)
        {
            InitializeComponent();

            DataContext = new SprOborudVM();

            SprOborudVM.SelectedNO = nameOborud;

            SprOborudVM.nameOborud1 = nameOborud.NameOborud1;
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
