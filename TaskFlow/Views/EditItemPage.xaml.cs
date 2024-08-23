using System.Windows.Controls;
using TaskFlow.ViewModels;

namespace TaskFlow.Views
{
    /// <summary>
    /// Interaction logic for EditItemPage.xaml
    /// </summary>
    public partial class EditItemPage : Page
    {
        public EditItemPage()
        {
            InitializeComponent();
            DataContext = new EditItemViewModel();
        }
    }
}
