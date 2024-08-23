using System.Windows.Controls;
using TaskFlow.ViewModels;

namespace TaskFlow.Views
{
    /// <summary>
    /// Interaction logic for CreateList.xaml
    /// </summary>
    public partial class CreateList : Page
    {
        public CreateList()
        {
            InitializeComponent();
            DataContext = new CreateListViewModel();
        }
    }
}
