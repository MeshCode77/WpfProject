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
    /// Логика взаимодействия для SprOborudWnd.xaml
    /// </summary>
    public partial class SprOborudView : Window
    {
        public static ListView UpdateNO;
        public SprOborudView()
        {
            InitializeComponent();

            this.DataContext = new SprOborudVM();

            UpdateNO = LvSprNO;

        }
    }
}
