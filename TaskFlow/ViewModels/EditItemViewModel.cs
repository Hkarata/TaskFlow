using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using TaskFlow.Models;
using TaskFlow.Repositories;

namespace TaskFlow.ViewModels
{
    public partial class EditItemViewModel : BaseViewModel
    {
        [ObservableProperty]
        private TodoItem? _todoItem;

        [ObservableProperty]
        private ComboBoxItem? _selectedPriority;

        [ObservableProperty]
        private ComboBoxItem? _selectedFlag;

        private readonly ITodoRepository? _todoRepository;

        public EditItemViewModel()
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow!.MainFrame.Navigated += MainFrame_Navigated;
            _todoRepository = App.ServiceProvider!.GetRequiredService<ITodoRepository>();
        }

        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {
            if (e.ExtraData is TodoItem item)
            {
                TodoItem = item;
                SelectedPriority = new ComboBoxItem();
                SelectedFlag = new ComboBoxItem();
                SelectedPriority!.Content = item.Priority.ToString();
                SelectedFlag!.Content = item.Flag.ToString();
            }
        }

        [RelayCommand]
        private void GoBack()
        {
            NavigationService.GoBack();
        }

        [RelayCommand]
        private async Task Save()
        {
            var todoItem = new TodoItem
            {
                Id = TodoItem!.Id,
                Description = TodoItem!.Description,
                Priority = (ItemPriority)Enum.Parse(typeof(ItemPriority), SelectedPriority!.Content.ToString()!),
                Flag = (ItemFlag)Enum.Parse(typeof(ItemFlag), SelectedFlag!.Content.ToString()!),
                TodoList = TodoItem!.TodoList
            };

            await _todoRepository!.CreateOrUpdateToDo(todoItem);

            NavigationService.NavigateTo("ListDetailsPage", todoItem.TodoList);
        }
    }
}
