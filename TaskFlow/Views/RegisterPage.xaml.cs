using System.Windows.Controls;
using TaskFlow.ViewModels;

namespace TaskFlow.Views;

public partial class RegisterPage : Page
{
    public RegisterPage()
    {
        InitializeComponent();
        DataContext = new RegisterViewModel();
    }
}