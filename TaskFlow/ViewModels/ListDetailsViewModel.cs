using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Navigation;
using TaskFlow.Models;
using TaskFlow.Repositories;

namespace TaskFlow.ViewModels
{
    public partial class ListDetailsViewModel : BaseViewModel
    {
        [ObservableProperty]
        private TodoList? _todoList;

        private readonly ITodoListRepository? _todoListRepository;

        public ListDetailsViewModel()
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow!.MainFrame.Navigated += MainFrame_Navigated;
            _todoListRepository = App.ServiceProvider!.GetRequiredService<ITodoListRepository>();
        }


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

        [RelayCommand]
        private void GoToEditItemPage(TodoItem item)
        {
            NavigationService.NavigateTo("EditItemPage", item);
        }

        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {
            if (e.ExtraData is TodoList todoList)
            {
                TodoList = todoList;
                LoadToDoItems();
            }
        }

        private void LoadToDoItems()
        {
            TodoList = Task.Run(async () => await _todoListRepository!.GetToDoListWithItems(TodoList!.Id)).Result;
        }
    }
}
