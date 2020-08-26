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
    class ClientsViewModel : ObservableObject, ILoadCollection
    {
        private RealEstateContext _context;
        private ClientModel _selectedClient;
        private Visibility _selectedClientVisibility;
        private Visibility _dropClientOption;

        public ObservableCollection<ClientModel> _clients;

        public ICommand ShowCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand DropCommand { get; }
        public ClientsViewModel(RealEstateContext context)
        {
            _context = context;

            Load();

            ShowCommand = new RelayCommand<ClientModel>(ShowClient);
            SaveCommand = new RelayCommand<int>(SaveClient);
            CancelCommand = new RelayCommand<int>(CancelClientChanges);
            DropCommand = new RelayCommand<object>(DropClient);
        }

        private void ShowClient(ClientModel clientModel)
        {
            if (clientModel != null)
            {
                SelectedClient = clientModel;
                DropClientOption = Visibility.Visible;
            }
            else
            {
                SelectedClient = new ClientModel();
                DropClientOption = Visibility.Collapsed;
            }

            SelectedClientVisibility = Visibility.Visible;
        }
        private void SaveClient(int id)
        {
            if (!WinApiMessageBox.ConfirmAction("Сохранить изменения?"))
                return;

            if (id == 0)
            {
                Clients.Add(SelectedClient);
                _context.Clients.Add(SelectedClient.Client);
            }

            _context.SaveChanges();
            OnPropertyChanged("Clients");
            SelectedClientVisibility = Visibility.Collapsed;
        }
        private void CancelClientChanges(int id)
        {
            if (id != 0)
            {
                _context.Entry<Client>(SelectedClient.Client).Reload();

                OnPropertyChanged("SelectedClient");
                OnPropertyChanged("Clients");
            }
            SelectedClientVisibility = Visibility.Collapsed;
        }
        private void DropClient(object param)
        {
            if (!WinApiMessageBox.ConfirmAction("Удалить данного клиента ?"))
                return;

            _context.Clients.Remove(SelectedClient.Client);
            _context.SaveChanges();

            Clients.Remove(SelectedClient);
            OnPropertyChanged("Clients");

            DropClientOption = SelectedClientVisibility = Visibility.Collapsed;
        }
        public void Load()
        {
            Clients = new ObservableCollection<ClientModel>
                (_context.Clients.ToList().Select(client => new ClientModel(client)));

            SelectedClientVisibility = DropClientOption = Visibility.Collapsed;
            SelectedClient = new ClientModel();
        }
        public ClientModel SelectedClient
        {
            get => _selectedClient;
            set
            {
                _selectedClient = value;
                OnPropertyChanged();
            }
        }
        public Visibility SelectedClientVisibility
        {
            get => _selectedClientVisibility;
            set
            {
                _selectedClientVisibility = value;
                OnPropertyChanged();
            }
        }
        public Visibility DropClientOption
        {
            get => _dropClientOption;
            set
            {
                _dropClientOption = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<ClientModel> Clients
        {
            get => _clients;
            set
            {
                _clients = value;
                OnPropertyChanged();
            }
        }
    }
}
