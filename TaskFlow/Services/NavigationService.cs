using System.Windows;
using System.Windows.Controls;

namespace TaskFlow.Services
{

    public interface INavigationService
    {
        void Configure(string key, Uri pageUri);
        void NavigateTo(string route);
        void NavigateTo(string route, object? parameter);
        void GoBack();
    }

    public class NavigationService : INavigationService
    {
        private readonly Dictionary<string, Uri> _pagesByKey = [];
        private readonly Frame _mainFrame;

        public NavigationService()
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            _mainFrame = mainWindow!.MainFrame;
        }
        public void Configure(string key, Uri pageUri)
        {
            _pagesByKey.TryAdd(key, pageUri);
        }

        public void NavigateTo(string route)
        {
            if (_pagesByKey.TryGetValue(route, out var pageUri))
            {
                _mainFrame.Navigate(pageUri);
            }
        }

        public void NavigateTo(string route, object? parameter)
        {
            if (_pagesByKey.TryGetValue(route, out var pageUri))
            {
                _mainFrame.Navigate(pageUri, parameter);
            }
        }

        public void GoBack()
        {
            if (_mainFrame.CanGoBack)
            {
                _mainFrame.GoBack();
            }
        }
    }
}
