using RealEstateApp.Services.EntityFramework.Models;
using RealEstateApp.Utility;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RealEstateApp.Models
{
    class UserModel : ObservableObject
    {
        private User _user;

        public UserModel()
        {
            _user = new User();
        }
        public UserModel(User user)
        {
            _user = user;
        }
        public User User
        {
            get => _user;
        }
        public int Id
        {
            get => _user.Id;
        }
        public string Name
        {
            get => _user.Name;
            set
            {
                 _user.Name = value;
                OnPropertyChanged();
            }
        }
        public bool IsAdmin
        {
            get => _user.IsAdmin.HasValue ? _user.IsAdmin.Value : false;
            set
            {
                _user.IsAdmin = value;
                OnPropertyChanged();
            }
                
        }
        public string Password
        {
            set => _user.Password = value;
        }
    }
}
