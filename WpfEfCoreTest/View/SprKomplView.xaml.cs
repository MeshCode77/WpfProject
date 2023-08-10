using System.Windows;
using System.Windows.Controls;

namespace WpfEfCoreTest.View
{
    /// <summary>
    ///     Логика взаимодействия для SprKomplView.xaml
    /// </summary>
    public partial class SprKomplView : Window
    {
        public static ListView UpdateNK;

        public SprKomplView()
        {
            InitializeComponent();

            //DataContext = new SprKomplVM();

            UpdateNK = LvSprNK;
        }
    }
}