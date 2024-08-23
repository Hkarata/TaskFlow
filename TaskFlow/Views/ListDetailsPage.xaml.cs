using System.ComponentModel.Design;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Navigation;
using TaskFlow.ViewModels;

namespace TaskFlow.Views
{
    /// <summary>
    /// Interaction logic for ListPage.xaml
    /// </summary>
    public partial class ListPage : Page
    {
        public ListPage()
        {
            InitializeComponent();
            DataContext = new ListDetailsViewModel();
        }
    }
}
