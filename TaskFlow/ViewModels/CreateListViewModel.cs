using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using TaskFlow.Repositories;

namespace TaskFlow.ViewModels
{
    public partial class CreateListViewModel : BaseViewModel
    {
        private readonly ITodoListRepository? _todoListRepository;
        public CreateListViewModel()
        {
            this.Title = "Create a to do List";
            _todoListRepository = App.ServiceProvider!.GetRequiredService<ITodoListRepository>();
        }

        [ObservableProperty]
        private string _listName = string.Empty;

        [RelayCommand]
        private void GoBack()
        {
            NavigationService.GoBack();
        }

        [RelayCommand]
        private async Task CreateList()
        {
            if (string.IsNullOrWhiteSpace(ListName))
            {
                return;
            }

            await _todoListRepository!.CreateOrUpdateToDoList(ListName, CurrentUser!.Id);

            NavigationService.GoBack();
        }
    }
}
