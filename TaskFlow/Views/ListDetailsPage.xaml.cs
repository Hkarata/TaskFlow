using System.ComponentModel.Design;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Navigation;
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
        public string MyCodeBehindValue { get; set; }
        public ListPage()
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow!.MainFrame.Navigated += MainFrame_Navigated;

            InitializeComponent();
            DataContext = new ListDetailsViewModel();
            MyCodeBehindValue = "Value from Code-Behind";
        }

        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {
            TodoList = (TodoList)e.ExtraData;
        }
    }
}
