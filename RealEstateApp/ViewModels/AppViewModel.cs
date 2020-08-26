using RealEstateApp.Interfaces;
using RealEstateApp.Models;
using RealEstateApp.Services.EntityFramework;
using RealEstateApp.Services.EntityFramework.Models;
using RealEstateApp.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RealEstateApp.ViewModels
{
    class AppViewModel : ObservableObject
    {
        private RealEstateContext _context;
        private object _currentView;
        //Navigate mode
        private bool _isReloadOn;

        public AuthModal AuthModal { get; private set; }

        public ILoadCollection AppartmentsVM { get; }
        public ILoadCollection ClientsVM { get; }
        public ILoadCollection DealsVM { get; }
        public ILoadCollection UsersVM { get; }
        public ILoadCollection StatsVM { get; }
        
        
        public ICommand ChangeViewCommand { get; }
        public ICommand GetDealsCommand { get; }
        public ICommand AuthenticateCommand { get; }
        public ICommand LogoutCommand { get; }
        public ICommand SwitchCommand { get; }
        public AppViewModel()
        {
            _context = new RealEstateContext();

            if (!_context.Database.Exists())
                _context.RunSeeds();

            AppartmentsVM = new AppartmentsViewModel(_context);
            ClientsVM = new ClientsViewModel(_context);
            DealsVM = new DealsViewModel(_context);
            UsersVM = new UsersViewModel(_context);
            StatsVM = new StatsViewModel(_context);
            
            AuthModal = new AuthModal();

            ChangeViewCommand = new RelayCommand<object>(ChangeView);
            GetDealsCommand = new RelayCommand<object>(GetDeals);
            AuthenticateCommand = new RelayCommand<object>(Authenticate);
            LogoutCommand = new RelayCommand<object>(Logout);

            IsReloadOn = true;
            SwitchCommand = new RelayCommand<object>(_ => IsReloadOn = !IsReloadOn);
        }

        public object CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }
        public bool IsReloadOn
        {
            get => _isReloadOn;
            set
            {
                _isReloadOn = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// Переключает между вьхами
        /// </summary>
        /// <param name="param"></param>
        private void ChangeView(object param)
        {
            CurrentView = param;
            if (IsReloadOn)
                (CurrentView as ILoadCollection).Load();
        }
        /// <summary>
        /// Просмотр сделок из любой вью
        /// </summary>
        /// <param name="obj"></param>
        private void GetDeals(object obj)
        {
            CurrentView = DealsVM;
            (DealsVM as DealsViewModel).LoadDeals(obj);
        }
        /// <summary>
        /// Аутентификация
        /// </summary>
        /// <param name="pbox"></param>
        private void Authenticate(object pbox)
        {
            string password = (pbox as System.Windows.Controls.PasswordBox).Password;
            
            User record = _context.Users.ToList()
                .Where(user => user.Name == AuthModal.Name && user.Password == password)
                .FirstOrDefault();
            if (record != null)
            {
                AuthModal.Visibility = System.Windows.Visibility.Collapsed;
                AuthModal.IsAdmin = record.IsAdmin.HasValue ? record.IsAdmin.Value : false;
                CurrentView = new WelcomeViewModel(AuthModal.IsAdmin);
            }
            else
            {
                AuthModal.Message = "Не удалось подключиться";
            }
            (pbox as System.Windows.Controls.PasswordBox).Clear();
        }
        /// <summary>
        /// Приостановка сессии
        /// </summary>
        /// <param name="obj"></param>
        private void Logout(object obj)
        {
            AuthModal.IsAdmin = false;
            AuthModal.Message = "Рабочая сессия закончена";
            AuthModal.Name = string.Empty;
            AuthModal.Visibility = System.Windows.Visibility.Visible;
            CurrentView = null;
        }
    }
}
