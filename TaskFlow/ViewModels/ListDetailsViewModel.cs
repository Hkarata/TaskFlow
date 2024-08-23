using CommunityToolkit.Mvvm.Input;

namespace TaskFlow.ViewModels
{
    public partial class ListDetailsViewModel : BaseViewModel
    {
        [RelayCommand]
        private void GoBack()
        {
            NavigationService.GoBack();
        }

        [RelayCommand]
        private void GoToAddItemPage(Guid listId)
        {
            NavigationService.NavigateTo("AddTodoPage", listId);
        }
    }
}
