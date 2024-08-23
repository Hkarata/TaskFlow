﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using TaskFlow.Models;
using TaskFlow.ViewModels;

namespace TaskFlow.Views
{
    /// <summary>
    /// Interaction logic for ListPage.xaml
    /// </summary>
    public partial class ListPage : Page
    {
        public TodoList? TodoList { get; set; }
        public ListPage()
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow!.MainFrame.Navigated += MainFrame_Navigated;

            InitializeComponent();
            DataContext = new ListDetailsViewModel();
        }

        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {
            if (e.ExtraData is TodoList todoList)
            {
                TodoList = todoList;
            }
        }
    }
}
