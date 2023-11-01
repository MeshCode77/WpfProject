using System.Windows;
using WpfEfCoreTest.Model;
using WpfEfCoreTest.ViewModel;

namespace WpfEfCoreTest.View
{
    /// <summary>
    ///     Логика взаимодействия для EditF111View.xaml
    /// </summary>
    public partial class EditF111View : Window
    {
        public EditF111View(F111 rowF111)
        {
            InitializeComponent();

            DataContext = new F111VM();

            F111VM.selectedRowF111 = rowF111;

            F111VM.KartNum = rowF111.KartNum;
            F111VM.NumForm = rowF111.NumForm;
            F111VM.InvNum = rowF111.InvNum;
            F111VM.ZavodNum = rowF111.ZavodNum;
            F111VM.GtDate = rowF111.GtDate;
            F111VM.OutData = rowF111.OutDate;
        }
    }
}