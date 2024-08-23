using System.Windows.Controls;
using TaskFlow.ViewModels;

namespace TaskFlow.Views;

public partial class LoginPage : Page
{
    public LoginPage()
    {
        InitializeComponent();
        DataContext = new LoginViewModel();
    }
}