using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using TaskFlow.Models;
using TaskFlow.Repositories;

namespace TaskFlow.ViewModels
{
    public partial class HomeViewModel : BaseViewModel
    {
        public ObservableCollection<TodoList> todoLists { get; } = [];

        private readonly ITodoListRepository? _todoListRepository;

        [ObservableProperty]
        private TodoList? _selectedList;

        public HomeViewModel()
        {
            Title = "Weave your time, design your future.";
            _todoListRepository = App.ServiceProvider!.GetRequiredService<ITodoListRepository>();
            LoadData();
        }

        [RelayCommand]
        private void GoToCreateList()
        {
            NavigationService.NavigateTo("CreateListPage");
        }

        private void LoadData()
        {
            todoLists.Clear();

            var lists = Task.Run(async ()
                => await _todoListRepository!.GetToDoLists(CurrentUser!.Id)).Result;

            if (lists != null)
            {
                foreach (var list in lists)
                {
                    todoLists.Add(list);
                }
            }
        }


        [RelayCommand]
        private void GoToListDetails(TodoList todoList)
        {
            NavigationService.NavigateTo("ListDetailsPage", todoList);
        }
    }
}
