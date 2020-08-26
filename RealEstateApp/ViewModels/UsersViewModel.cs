using RealEstateApp.Interfaces;
using RealEstateApp.Models;
using RealEstateApp.Services.EntityFramework;
using RealEstateApp.Services.EntityFramework.Models;
using RealEstateApp.Services.WinApi;
using RealEstateApp.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace RealEstateApp.ViewModels
{
    class UsersViewModel : ObservableObject, ILoadCollection
    {
        private RealEstateContext _context;
        private UserModel _selectedUser;
        private Visibility _selectedUserVisibility;
        private Visibility _dropUserOption;
        private string _password;

        public ObservableCollection<UserModel> Users { get; private set; }

        public ICommand ShowCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand DropCommand { get; }
        public UsersViewModel(RealEstateContext context)
        {
            _context = context;

            Load();

            ShowCommand = new RelayCommand<UserModel>(ShowUser);
            SaveCommand = new RelayCommand<int>(SaveUser);
            CancelCommand = new RelayCommand<int>(CancelUserChanges);
            DropCommand = new RelayCommand<object>(DropUser);

        }
        private void ShowUser(UserModel userModel)
        {
            if (userModel != null)
            {
                SelectedUser = userModel;
                DropUserOption = Visibility.Visible;
            }
            else
            {
                SelectedUser = new UserModel();
                DropUserOption = Visibility.Collapsed;
            }

            _password = string.Empty;
            SelectedUserVisibility = Visibility.Visible;
        }
        private void SaveUser(int id)
        {
            if (!WinApiMessageBox.ConfirmAction("Сохранить изменения?"))
                return;

            if (!String.IsNullOrWhiteSpace(Password))
            {
                SelectedUser.Password = Password;
            }

            if(id == 0)
            {
                Users.Add(SelectedUser);
                _context.Users.Add(SelectedUser.User);
            }

            _context.SaveChanges();
            OnPropertyChanged("Users");
            SelectedUserVisibility = Visibility.Collapsed;
        }
        private void CancelUserChanges(int id)
        {
            if(id != 0)
            {
                _context.Entry<User>(SelectedUser.User).Reload();

                OnPropertyChanged("SelectedUser");
                OnPropertyChanged("Users");
            }
            SelectedUserVisibility = Visibility.Collapsed;
        }
        private void DropUser(object param)
        {
            if (!WinApiMessageBox.ConfirmAction("Удалить пользователя ?"))
                return;

            _context.Users.Remove(SelectedUser.User);
            _context.SaveChanges();

            Users.Remove(SelectedUser);
            OnPropertyChanged("Users");

            DropUserOption = SelectedUserVisibility = Visibility.Collapsed;
        }

        public void Load()
        {
            Users = new ObservableCollection<UserModel>
                (_context.Users.ToList().Select(user => new UserModel(user)));

            SelectedUserVisibility = DropUserOption = Visibility.Collapsed;
            SelectedUser = new UserModel();
        }

        public UserModel SelectedUser
        {
            get => _selectedUser;
            set
            {
                _selectedUser = value;
                OnPropertyChanged();
            }
        }
        public Visibility SelectedUserVisibility
        {
            get => _selectedUserVisibility;
            set
            {
                _selectedUserVisibility = value;
                OnPropertyChanged();
            }
        }
        public Visibility DropUserOption
        {
            get => _dropUserOption;
            set
            {
                _dropUserOption = value;
                OnPropertyChanged();
            }
        }
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }
    }
}
