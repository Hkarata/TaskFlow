using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using TaskFlow.Models;
using TaskFlow.Repositories;
using TaskFlow.Services;

namespace TaskFlow.ViewModels
{
    public partial class RegisterViewModel : BaseViewModel
    {
        private readonly IUserRepository? _userRepository;
        public RegisterViewModel()
        {
            Title = "Create an account";
            _userRepository = App.ServiceProvider!.GetService<IUserRepository>();
        }

        [ObservableProperty]
        private string _name = string.Empty;

        [ObservableProperty]
        private string _email = string.Empty;

        [ObservableProperty]
        private string _password = string.Empty;



        [RelayCommand]
        private void GoBack()
        {
            NavigationService.GoBack();
        }

        [RelayCommand]
        private async Task CreateAccount()
        {
            var user = new User
            {
                Name = Name,
                Email = Email,
                Password = PasswordService.HashPassword(Password)
            };

            await _userRepository!.CreateOrUpdateAccount(user);

            NavigationService.NavigateTo("LoginPage");
        }
    }
}
