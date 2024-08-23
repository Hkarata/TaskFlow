using CommunityToolkit.Mvvm.Input;

namespace TaskFlow.ViewModels;

public partial class WelcomeViewModel : BaseViewModel
{
    public WelcomeViewModel()
    {
        Title = "Welcome to TaskFlow";
    }

    [RelayCommand]
    private void GoToRegisterPage()
    {
        NavigationService.NavigateTo("RegisterPage");
    }

    [RelayCommand]
    private void GoToLoginPage()
    {
        NavigationService.NavigateTo("LoginPage");
    }
}