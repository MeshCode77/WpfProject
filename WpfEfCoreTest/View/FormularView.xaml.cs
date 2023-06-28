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
using WpfEfCoreTest.ViewModel;

namespace WpfEfCoreTest.View
{
    /// <summary>
    /// Логика взаимодействия для FormularView.xaml
    /// </summary>
    public partial class FormularView : Window
    {
        public static ListView AllDataFormularToIdF111;
        public FormularView()
        {
            InitializeComponent();

            DataContext = new FormularVM();

            AllDataFormularToIdF111 = LvFormular;
        }
    }
}
