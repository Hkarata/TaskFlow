using TaskFlow.Models;
using TaskFlow.Services;

namespace TaskFlow
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public INavigationService NavigationService { get; }
        public UserSession? CurrentUser { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            NavigationService = new NavigationService();

            ConfigureRoutes();

            var session = SessionService.LoadSession();

            if (session != null)
            {
                CurrentUser = new UserSession
                {
                    Id = session.Value.userId,
                    Name = session.Value.name,
                    Email = session.Value.email
                };
                NavigationService.NavigateTo("HomePage");
            }
            else
            {
                NavigationService.NavigateTo("WelcomePage");
            }

        
        }

        public MainWindow(INavigationService navigationService)
        {
            NavigationService = navigationService;
        }

        private void Grid_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void ConfigureRoutes()
        {
            NavigationService.Configure("WelcomePage", new Uri("./Views/WelcomePage.xaml", UriKind.Relative));
            NavigationService.Configure("RegisterPage", new Uri("./Views/RegisterPage.xaml", UriKind.Relative));
            NavigationService.Configure("LoginPage", new Uri("./Views/LoginPage.xaml", UriKind.Relative));
            NavigationService.Configure("HomePage", new Uri("./Views/HomePage.xaml", UriKind.Relative));
            NavigationService.Configure("CreateListPage", new Uri("./Views/CreateListPage.xaml", UriKind.Relative));
            NavigationService.Configure("ListDetailsPage", new Uri("./Views/ListDetailsPage.xaml", UriKind.Relative));

        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Close();
        }
    }
}