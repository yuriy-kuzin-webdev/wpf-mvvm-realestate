using RealEstateApp.Services.EntityFramework.Models;
using RealEstateApp.Utility;
using RealEstateApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Models
{
    class ClientModel : ObservableObject
    {
        private Client _client;
        public ClientModel()
        {
            _client = new Client();
        }
        public ClientModel(Client client)
        {
            _client = client;
        }
        public Client Client
        {
            get => _client;
        }
        public int Id
        {
            get => _client.Id;
        }
        public string FirstName
        {
            get => _client.FirstName;
            set
            {
                _client.FirstName = value;
                OnPropertyChanged();
            }
        }
        public string LastName
        {
            get => _client.LastName;
            set
            {
                _client.LastName = value;
                OnPropertyChanged();
            }
        }
        public string MiddleName
        {
            get => _client.MiddleName;
            set
            {
                _client.MiddleName = value;
                OnPropertyChanged();
            }
        }
        public string Phone
        {
            get => _client.Phone;
            set
            {
                _client.Phone = value;
                OnPropertyChanged();
            }
        }
    }
}
