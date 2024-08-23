using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TaskFlow.Models;

namespace TaskFlow.ViewModels
{
    public partial class ListDetailsViewModel : BaseViewModel
    {

        [RelayCommand]
        private void GoBack()
        {
            NavigationService.GoBack();
        }
    }
}
