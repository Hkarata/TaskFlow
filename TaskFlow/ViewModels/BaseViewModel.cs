using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows;
using TaskFlow.Models;
using TaskFlow.Services;

namespace TaskFlow.ViewModels;

public partial class BaseViewModel : ObservableObject
{

    protected readonly INavigationService NavigationService;
    protected readonly UserSession? CurrentUser;

    protected BaseViewModel()
    {
        var mainWindow = Application.Current.MainWindow as MainWindow;
        NavigationService = mainWindow!.NavigationService;
        CurrentUser = mainWindow!.CurrentUser;
    }


    [ObservableProperty]
    private string _title = string.Empty;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotBusy))]
    private bool _isBusy = false;

    public bool IsNotBusy => !IsBusy;
}