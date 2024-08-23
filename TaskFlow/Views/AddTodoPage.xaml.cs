using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using TaskFlow.ViewModels;

namespace TaskFlow.Views
{
    /// <summary>
    /// Interaction logic for AddTodoPage.xaml
    /// </summary>
    public partial class AddTodoPage : Page
    {
        public string? ListId { get; set; }
        public AddTodoPage()
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow!.MainFrame.Navigated += MainFrame_Navigated;

            InitializeComponent();
            DataContext = new AddTodoViewModel();
        }

        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {
            if (e.ExtraData is Guid listId)
            {
                ListId = listId.ToString();
            }
        }
    }
}
