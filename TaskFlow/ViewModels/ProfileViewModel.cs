using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using TaskFlow.Models;
using TaskFlow.Repositories;
using TaskFlow.Services;

namespace TaskFlow.ViewModels
{
    public partial class ProfileViewModel : BaseViewModel
    {
        [ObservableProperty]
        private UserSession? _user;

        [ObservableProperty]
        private string _password;

        private readonly IUserRepository userRepository;

        public ProfileViewModel()
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            User = mainWindow!.CurrentUser;
            Title = "Profile management";
            userRepository = App.ServiceProvider.GetRequiredService<IUserRepository>();
        }


        [RelayCommand]
        private async Task UpdateAccount()
        {
            var user = new User
            {
                Id = User!.Id,
                Name = User!.Name,
                Email = User!.Email,
                Password = PasswordService.HashPassword(Password)
            };

            await userRepository.CreateOrUpdateAccount(user);
        }

        [RelayCommand]
        private async Task DeleteAccount()
        {
            await userRepository.DeleteAccount(User!.Id);

            SessionService.ClearSession();

            var mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow!.CurrentUser = null;
            mainWindow.IsLoggedIn = false;
            NavigationService.NavigateTo("WelcomePage");
        }
    }
}
