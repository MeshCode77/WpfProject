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
    /// Логика взаимодействия для EditNKWnd.xaml
    /// </summary>
    public partial class EditNKWnd : Window
    {
        public EditNKWnd(Komplect nameKomplect)
        {
            InitializeComponent();

            DataContext = new SprKomplVM();

            SprKomplVM.SelectedNK = nameKomplect;

            SprKomplVM.nameKompl = nameKomplect.NameKompl;
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
