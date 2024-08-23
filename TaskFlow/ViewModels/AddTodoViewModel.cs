using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;
using TaskFlow.Models;
using TaskFlow.Repositories;

namespace TaskFlow.ViewModels
{
    public partial class AddTodoViewModel : BaseViewModel
    {
        private readonly ITodoRepository? _todoRepository;
        public AddTodoViewModel()
        {
            Title = "Add a new task";
            _todoRepository = App.ServiceProvider!.GetRequiredService<ITodoRepository>();
        }

        [ObservableProperty]
        private string _description = string.Empty;

        [ObservableProperty]
        private ComboBoxItem _priority;

        [RelayCommand]
        private void GoBack()
        {
            NavigationService.NavigateTo("HomePage");
        }

        [RelayCommand]
        private async Task AddToDo(string listId)
        {
            var todoItem = new TodoItem
            {
                Id = Guid.NewGuid(),
                Description = Description,
                Priority = (ItemPriority)Enum.Parse(typeof(ItemPriority), Priority.Content.ToString()),
                Flag = ItemFlag.NotStarted,
                TodoListId = Guid.Parse(listId)
            };

            await _todoRepository!.CreateOrUpdateToDo(todoItem);

            NavigationService.NavigateTo("HomePage");
        }
    }
}
