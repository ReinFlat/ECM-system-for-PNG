using DevExpress.Mvvm;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Navigation;

namespace ECM_system_for_PNG.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private Page _currentPage;
        public Page CurrentPage
        {
            get { return _currentPage; }
            set
            {
                _currentPage = value;
            }
        }

        // Страницы приложения
        public Pages.Home PageHome { get; }
        public Pages.Folder PageFolder { get; }
        public Pages.Document PageDocument { get; }
        public Pages.Email PageEmail { get; }
        public Pages.Settings PageSettings { get; }

        // Конструктор класса
        public MainViewModel()
        {
            // Инициализация страниц
            PageHome = new Pages.Home();
            PageFolder = new Pages.Folder();
            PageDocument = new Pages.Document();
            PageEmail = new Pages.Email();
            PageSettings = new Pages.Settings();

            // Установка начальной страницы
            CurrentPage = PageHome;
        }

        // Команды для навигации по страницам
        public ICommand NavigateToHome => new RelayCommand(() => CurrentPage = PageHome);
        public ICommand NavigateToFolder => new RelayCommand(() => CurrentPage = PageFolder);
        public ICommand NavigateToDocument => new RelayCommand(() => CurrentPage = PageDocument);
        public ICommand NavigateToEmail => new RelayCommand(() => CurrentPage = PageEmail);
        public ICommand NavigateToSettings => new RelayCommand(() => CurrentPage = PageSettings);
    }

}