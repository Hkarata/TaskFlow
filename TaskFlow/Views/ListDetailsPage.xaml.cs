using System.Windows.Controls;
using TaskFlow.Models;
using TaskFlow.ViewModels;

namespace TaskFlow.Views
{
    /// <summary>
    /// Interaction logic for ListPage.xaml
    /// </summary>
    public partial class ListPage : Page
    {
        public TodoList? TodoList { get; set; }
        public ListPage()
        {
            InitializeComponent();
            DataContext = new ListDetailsViewModel();
        }
    }
}
