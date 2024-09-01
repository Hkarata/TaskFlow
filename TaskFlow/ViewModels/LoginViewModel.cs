using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using TaskFlow.Models;
using TaskFlow.Repositories;
using TaskFlow.Services;

namespace TaskFlow.ViewModels
{
    public partial class LoginViewModel : BaseViewModel
    {
        private readonly IUserRepository? _userRepository;
        public LoginViewModel()
        {
            Title = "Welcome back!";
            _userRepository = App.ServiceProvider!.GetService<IUserRepository>();
        }


        [ObservableProperty]
        private string _email = string.Empty;

        [ObservableProperty]
        private string _password = string.Empty;

        [ObservableProperty]
        private bool _isAccountInValid;

        [ObservableProperty]
        private string _validationMessage = string.Empty;

        [RelayCommand]
        private void GoBack()
        {
            NavigationService.GoBack();
        }

        [RelayCommand]
        private async Task Login()
        {
            try
            {
                var user = await _userRepository!.GetAccount(Email);
                if (user == null)
                {
                    IsAccountInValid = true;
                    ValidationMessage = "User with the provided email does not exist.";
                    return;
                }

                if (PasswordService.VerifyHashedPassword(user.Password, Password))
                {
                    IsAccountInValid = false;
                    StartUserSession(user.Id, user.Name, user.Email);
                    NavigationService.NavigateTo("HomePage");
                }
                else
                {
                    IsAccountInValid = true;
                    ValidationMessage = "Valid email but invalid password.";
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Please configure database connection", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private static void StartUserSession(Guid userId, string name, string email)
        {
            var expirationTime = DateTime.Now.AddHours(1);
            SessionService.SaveSession(userId, name, email, expirationTime);

            var CurrentUser = new UserSession
            {
                Id = userId,
                Name = name,
                Email = email
            };

            var mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow!.CurrentUser = CurrentUser;
            mainWindow.IsLoggedIn = true;
        }
    }
}
