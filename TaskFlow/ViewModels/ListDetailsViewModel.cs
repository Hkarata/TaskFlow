using CommunityToolkit.Mvvm.Input;

namespace TaskFlow.ViewModels
{
    public partial class ListDetailsViewModel : BaseViewModel
    {
        public ListDetailsViewModel()
        {

        }

        [RelayCommand]
        private void GoBack()
        {
            NavigationService.GoBack();
        }
    }
}
