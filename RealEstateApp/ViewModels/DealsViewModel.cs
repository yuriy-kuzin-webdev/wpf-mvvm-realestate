using RealEstateApp.Interfaces;
using RealEstateApp.Models;
using RealEstateApp.Services.EntityFramework;
using RealEstateApp.Services.EntityFramework.Models;
using RealEstateApp.Services.WinApi;
using RealEstateApp.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace RealEstateApp.ViewModels
{
    class DealsViewModel : ObservableObject, ILoadCollection
    {
        private RealEstateContext _context;
        //Deals props
        private ObservableCollection<DealModel> _deals;
        private DealModel _selectedDeal;
        private Visibility _selectedDealVisibility;
        private Visibility _existingDealVisibility;
        private Visibility _newDealVisibility;
        private Visibility _dropDealVisibility;
        private Visibility _toolBarVisibility;

        //Appartment props
        private AppartmentModel _appartment;
        private Visibility _appartmentVisibility;
        private ObservableCollection<Appartment> _appartments;

        //Client props
        private ClientModel _client;
        private Visibility _clientVisibility;
        private ObservableCollection<Client> _clients;

        //Genereal commands
        public ICommand ShowCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand DropCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand SortCommand { get; }

        //Appartment commands
        public ICommand SelectAppartmentCommand { get; }
        public ICommand SearchAppartmentCommand { get; }


        //Client commands
        public ICommand SelectClientCommand { get; }
        public ICommand SearchClientCommand { get; }

        //Reload
        public ICommand ReloadCommand { get; }

        public DealsViewModel(RealEstateContext context)
        {
            _context = context;

            Load();




            //General commands
            ShowCommand = new RelayCommand<DealModel>(ShowDeal);
            SaveCommand = new RelayCommand<int>(SaveDeal);
            DropCommand = new RelayCommand<object>(DropDeal);
            CancelCommand = new RelayCommand<int>(CancelDealChanges);
            SortCommand = new RelayCommand<bool?>(SortDeals);
            //Appartment commands
            SelectAppartmentCommand = new RelayCommand<Appartment>(SelectAppartment);
            SearchAppartmentCommand = new RelayCommand<object>(SearchAppartment);
            //Client commands
            SelectClientCommand = new RelayCommand<Client>(SelectClient);
            SearchClientCommand = new RelayCommand<object>(SearchClient);
            //Reload 
            ReloadCommand = new RelayCommand<bool>(Reload);
        }
        /// <summary>
        /// public method to view deals from another views
        /// </summary>
        /// <param name="obj">entity of type Appartment or Client</param>
        public void LoadDeals(object obj)
        {
            List<Deal> deals = _context.Deals
                .Include("Appartment")
                .Include("Client")
                .ToList();

            if (obj is Appartment)
            {
                Appartment app = (Appartment)obj;
                deals = deals.Where(d => d.Appartment.Id == app.Id).ToList();
            }
            else 
            if (obj is Client)
            {
                Client cl = (Client)obj;
                deals = deals.Where(d => d.Client.Id == cl.Id).ToList();
            }

            if(deals != null)
                Deals = new ObservableCollection<DealModel>
                    (deals.Select(d => new DealModel(d)));


            SelectedDealVisibility = DropDealVisibility = Visibility.Collapsed;
            SelectedDeal = new DealModel
            {
                Appartment = new Appartment(),
                Client = new Client()
            };

            Appartment = new AppartmentModel();
            AppartmentVisibility = Visibility.Collapsed;

            Client = new ClientModel();
            ClientVisibility = Visibility.Collapsed;

            ToolBarVisibility = Visibility.Visible;
        }
        #region general commands
        private void SortDeals(bool? option)
        {
            List<Deal> deals;

            if (option.HasValue)
            {
                deals = _context.Deals
                    .Include("Appartment")
                    .Include("Client")
                    .Where(d => d.IsConfirmed == option.Value)
                    .ToList();
            }
            else
            {
                deals = _context.Deals
                    .Include("Appartment")
                    .Include("Client")
                    .ToList();
            }

            Deals = new ObservableCollection<DealModel>
                    (deals.Select(d => new DealModel(d)));
        }
        private void ShowDeal(DealModel dealModel)
        {
            if (dealModel != null)
            {
                SelectedDeal = dealModel;
                DropDealVisibility = Visibility.Visible;
            }
            else
            {
                SelectedDeal = new DealModel
                {
                    Appartment = new Appartment(),
                    Client = new Client()
                };
                DropDealVisibility = Visibility.Collapsed;
            }

            ExistingDealVisibility = dealModel != null
                ? Visibility.Visible
                : Visibility.Collapsed;

            NewDealVisibility = dealModel == null
                ? Visibility.Visible
                : Visibility.Collapsed;

            SelectedDealVisibility = Visibility.Visible;
            ToolBarVisibility = Visibility.Collapsed;
        }
        private void SaveDeal(int id)
        {
            if (!WinApiMessageBox.ConfirmAction("Сохранить изменения?"))
                return;
            if (SelectedDeal.Appartment.Id == 0 || SelectedDeal.Client.Id == 0)
                return;

            if (id == 0)
            {
                Deals.Add(SelectedDeal);
                _context.Deals.Add(SelectedDeal.Deal);
            }

            _context.SaveChanges();
            OnPropertyChanged("Deals");

            SelectedDealVisibility = DropDealVisibility = Visibility.Collapsed;
            ToolBarVisibility = Visibility.Visible;
        }
        private void DropDeal(object param)
        {
            if (!WinApiMessageBox.ConfirmAction("Снять бронь и удалить данную запись?"))
                return;

            _context.Deals.Remove(SelectedDeal.Deal);
            _context.SaveChanges();

            Deals.Remove(SelectedDeal);
            OnPropertyChanged("Deals");

            DropDealVisibility = SelectedDealVisibility = Visibility.Collapsed;
            ToolBarVisibility = Visibility.Visible;
        }
        private void CancelDealChanges(int id)
        {
            if (id != 0)
            {
                _context.Entry<Deal>(SelectedDeal.Deal).Reload();

                OnPropertyChanged("SelectedDeal");
                OnPropertyChanged("Deals");
            }
            SelectedDealVisibility = DropDealVisibility = Visibility.Collapsed;
            ToolBarVisibility = Visibility.Visible;
        }
        #endregion general commands
        #region appartment commands
        private void SelectAppartment(Appartment appartment)
        {
            if(appartment == null)
            {
                AppartmentVisibility = Visibility.Visible;
            }
            else
            {
                SelectedDeal.Appartment = appartment;
                SelectedDeal.Appartment.Id = appartment.Id;
                OnPropertyChanged("SelectedDeal");
                AppartmentVisibility = Visibility.Collapsed;
            }
        }
        private void SearchAppartment(object param)
        {
            IEnumerable<Appartment> searchResults = _context.Appartments
                .Include("Deal").Where(a => a.Deal == null).ToList();

            if (!string.IsNullOrWhiteSpace(Appartment.Address) && searchResults != null)
                searchResults = searchResults.Where(a => a.Address.Contains(Appartment.Address));

            if (Appartment.RoomsAmount != 0 && searchResults != null)
                searchResults = searchResults.Where(a => a.RoomsAmount == Appartment.RoomsAmount);

            if(Appartment.Area != 0 && searchResults != null)
                searchResults = searchResults.Where(a => a.Area == Appartment.Area);

            if (Appartment.Storey != 0 && searchResults != null)
                searchResults = searchResults.Where(a => a.Storey == Appartment.Storey);

            if (Appartment.Price != 0 && searchResults != null)
                searchResults = searchResults.Where(a => a.Price == Appartment.Price);

            if(searchResults != null)
                Appartments = new ObservableCollection<Appartment>(searchResults);
        }
        #endregion appartment commands
        #region client commands
        private void SelectClient(Client client)
        {
            if(client == null)
            {
                ClientVisibility = Visibility.Visible;
            }
            else
            {
                SelectedDeal.Client = client;
                SelectedDeal.Client.Id = client.Id;
                OnPropertyChanged("SelectedDeal");
                ClientVisibility = Visibility.Collapsed;
            }
        }
        private void SearchClient(object param)
        {
            IEnumerable<Client> clients = _context.Clients
                .Include("Deals").ToList();

            if (!string.IsNullOrWhiteSpace(Client.FirstName))
                clients = clients.Where(c => c.FirstName.Contains(Client.FirstName));

            if (!string.IsNullOrWhiteSpace(Client.LastName))
                clients = clients.Where(c => c.LastName.Contains(Client.LastName));

            if (!string.IsNullOrWhiteSpace(Client.MiddleName))
                clients = clients.Where(c => c.MiddleName.Contains(Client.MiddleName));

            if (!string.IsNullOrWhiteSpace(Client.Phone))
                clients = clients.Where(c => c.Phone.Contains(Client.Phone));

            Clients = new ObservableCollection<Client>(clients);
        }
        #endregion client commands
        #region reload
        /// <summary>
        /// Reloads apps and clients
        /// </summary>
        /// <param name="option">позволяет выбирать клиентов или апартаменты</param>
        private void Reload(bool option)
        {
            if(option)
            {
                Appartment = new AppartmentModel();

                IEnumerable<Appartment> availableApps = _context.Appartments
                    .Include("Deal").Where(a => a.Deal == null).ToList();

                Appartments = new ObservableCollection<Appartment>(availableApps);
            }
            else
            {
                Client = new ClientModel();
                Clients = new ObservableCollection<Client>(_context.Clients.ToList());
            }
        }
        #endregion reload
        #region loadcollection
        private void LoadCollection()
        {
            List<Deal> deals = _context.Deals
                .Include("Appartment")
                .Include("Client")
                .ToList();

            Deals = new ObservableCollection<DealModel>
                (deals.Select(d => new DealModel(d)));

            Reload(true);
            Reload(false);
        }
        private void LoadCollection(bool status)
        {
            List<Deal> deals = _context.Deals
                .Where(d => d.IsConfirmed == status)
                .Include("Appartment")
                .Include("Client")
                .ToList();

            Deals = new ObservableCollection<DealModel>
                (deals.Select(d => new DealModel(d)));
        }

        public void Load()
        {
            LoadCollection();

            SelectedDealVisibility = DropDealVisibility = Visibility.Collapsed;
            SelectedDeal = new DealModel
            {
                Appartment = new Appartment(),
                Client = new Client()
            };

            Appartment = new AppartmentModel();
            AppartmentVisibility = Visibility.Collapsed;

            Client = new ClientModel();
            ClientVisibility = Visibility.Collapsed;

            ToolBarVisibility = Visibility.Visible;
        }
        #endregion loadcollection
        #region general props
        public ObservableCollection<DealModel> Deals
        {
            get => _deals;
            set
            {
                _deals = value;
                OnPropertyChanged();
            }
        }
        public DealModel SelectedDeal
        {
            get => _selectedDeal;
            set
            {
                _selectedDeal = value;
                OnPropertyChanged();
            }
        }
        public Visibility SelectedDealVisibility
        {
            get => _selectedDealVisibility;
            set
            {
                _selectedDealVisibility = value;
                OnPropertyChanged();
            }
        }
        public Visibility ExistingDealVisibility
        {
            get => _existingDealVisibility;
            set
            {
                _existingDealVisibility = value;
                OnPropertyChanged();
            }
        }
        public Visibility NewDealVisibility
        {
            get => _newDealVisibility;
            set
            {
                _newDealVisibility = value;
                OnPropertyChanged();
            }
        }
        public Visibility DropDealVisibility
        {
            get => _dropDealVisibility;
            set
            {
                _dropDealVisibility = value;
                OnPropertyChanged();
            }
        }
        public Visibility ToolBarVisibility
        {
            get => _toolBarVisibility;
            set
            {
                _toolBarVisibility = value;
                OnPropertyChanged();
            }
        }
        #endregion general props
        #region appartment modal props
        public AppartmentModel Appartment
        {
            get => _appartment;
            set
            {
                _appartment = value;
                OnPropertyChanged();
            }
        }
        public Visibility AppartmentVisibility
        {
            get => _appartmentVisibility;
            set
            {
                _appartmentVisibility = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<Appartment> Appartments
        {
            get => _appartments;
            set
            {
                _appartments = value;
                OnPropertyChanged();
            }
        }
        #endregion appartment modal props
        #region client modal props
        public ClientModel Client
        {
            get => _client;
            set
            {
                _client = value;
                OnPropertyChanged();
            }
        }
        public Visibility ClientVisibility
        {
            get => _clientVisibility;
            set
            {
                _clientVisibility = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<Client> Clients
        {
            get => _clients;
            set
            {
                _clients = value;
                OnPropertyChanged();
            }
        }
        #endregion client modal props
    }
}
