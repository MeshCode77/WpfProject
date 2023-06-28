using System.Windows;
using WpfEfCoreTest.Model;
using WpfEfCoreTest.ViewModel;

namespace WpfEfCoreTest.View
{
    /// <summary>
    ///     Логика взаимодействия для EditFormularView.xaml
    /// </summary>
    public partial class EditFormularView : Window
    {
        public EditFormularView(Formular rowFormular)
        {
            InitializeComponent();

            //FormularVM formularVm = new FormularVM();

            DataContext = new FormularVM();

            FormularVM.selectedRowFormular = rowFormular;

            FormularVM.Idf111 = rowFormular.Idf111;
            //FormularVM.IdKomplect = selKomplect.Id; // id наименования комплектующего 
            FormularVM.NumForm = rowFormular.NumForm;
            FormularVM.InvNum = rowFormular.InvNum;
            FormularVM.Model = rowFormular.Model;
            FormularVM.Count = rowFormular.Count;
            FormularVM.Serial = rowFormular.Serial;
            FormularVM.DataTo = rowFormular.DataTo;
            FormularVM.DateIn = rowFormular.DateIn;
            FormularVM.DateOut = rowFormular.DateOut;
            FormularVM.NumAkt = rowFormular.NumAkt;
            FormularVM.YearProd = rowFormular.YearProd;
            FormularVM.GarantyTo = rowFormular.GarantyTo;
            //FormularVM.NameKomplect = selKomplect.NameKompl;
        }
    }
}