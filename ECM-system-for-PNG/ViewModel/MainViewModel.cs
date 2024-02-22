using DevExpress.Mvvm;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using System;
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

        // Команды для навигации по страницам
        public ICommand PageHome { get; }
        public ICommand PageFolder { get; }
        public ICommand PageDocument { get; }
        public ICommand PageEmail { get; }
        public ICommand PageSettings { get; }

        // Конструктор класса
        public MainViewModel()
        {
            // Инициализация команд
            // Каждая команда принимает в качестве параметра имя страницы
            // и устанавливает свойство CurrentPage в соответствующий объект
            PageHome = new RelayCommand<string>(pageName =>
            {
                CurrentPage = new Pages.Home();
                
            });
            PageFolder = new RelayCommand<string>(pageName =>
            {
                CurrentPage = new Pages.Folder();
            });
            PageDocument = new RelayCommand<string>(pageName =>
            {
                CurrentPage = new Pages.Document();
            });
            PageEmail = new RelayCommand<string>(pageName =>
            {
                CurrentPage = new Pages.Email();
            });
            PageSettings = new RelayCommand<string>(pageName =>
            {
                CurrentPage = new Pages.Settings();
            });

            // Установка начальной страницы
            CurrentPage = new Pages.Home();
        }
    }
}