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
    class AppartmentsViewModel : ObservableObject, ILoadCollection
    {
        private RealEstateContext _context;
        private AppartmentModel _selectedAppartment;
        private Visibility _selectedAppartmentVisibility;
        private Visibility _dropAppartmentOption;
        //search
        private AppartmentModel _appartment;
        private Visibility _searchBarVisibility;

        public ObservableCollection<AppartmentModel> _appartments;

        //General commands
        public ICommand ShowCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand DropCommand { get; }

        //Search Commands
        public ICommand SearchCommand { get; }
        public ICommand FilterCommand { get; }
        public ICommand ReloadCommand { get; }
        public AppartmentsViewModel(RealEstateContext context)
        {
            _context = context;
            Load();

            //General Commands
            ShowCommand = new RelayCommand<AppartmentModel>(ShowAppartment);
            SaveCommand = new RelayCommand<int>(SaveAppartment);
            CancelCommand = new RelayCommand<int>(CancelAppartmentChanges);
            DropCommand = new RelayCommand<object>(DropAppartment);
            //Search Commands
            SearchCommand = new RelayCommand<object>(SearchAppartment);
            FilterCommand = new RelayCommand<bool?>(FilterAppartment);
            ReloadCommand = new RelayCommand<object>(ReloadAppartment);
        }
        #region general commands
        private void ShowAppartment(AppartmentModel appartmentModel)
        {

            if (appartmentModel != null)
            {
                SelectedAppartment = appartmentModel;
                DropAppartmentOption = Visibility.Visible;
            }
            else
            {
                SelectedAppartment = new AppartmentModel();
                DropAppartmentOption = Visibility.Collapsed;
            }

            SelectedAppartmentVisibility = Visibility.Visible;
            SearchBarVisibility = Visibility.Collapsed;
        }
        private void SaveAppartment(int id)
        {
            if (!WinApiMessageBox.ConfirmAction("Сохранить изменения?"))
                return;

            if (id == 0)
            {
                Appartments.Add(SelectedAppartment);
                _context.Appartments.Add(SelectedAppartment.Appartment);
            }

            _context.SaveChanges();
            OnPropertyChanged("Appartments");
            SelectedAppartmentVisibility = Visibility.Collapsed;
            SearchBarVisibility = Visibility.Visible;
        }
        private void CancelAppartmentChanges(int id)
        {
            if (id != 0)
            {
                _context.Entry<Appartment>(SelectedAppartment.Appartment).Reload();

                OnPropertyChanged("SelectedAppartment");
                OnPropertyChanged("Appartments");
            }
            SelectedAppartmentVisibility = Visibility.Collapsed;
            SearchBarVisibility = Visibility.Visible;
        }
        private void DropAppartment(object param)
        {
            if (!WinApiMessageBox.ConfirmAction("Удалить данную квартиру?"))
                return;

            _context.Appartments.Remove(SelectedAppartment.Appartment);
            _context.SaveChanges();

            Appartments.Remove(SelectedAppartment);
            OnPropertyChanged("Appartments");

            DropAppartmentOption = SelectedAppartmentVisibility = Visibility.Collapsed;
            SearchBarVisibility = Visibility.Visible;
        }
        #endregion general commands
        #region search
        private void SearchAppartment(object _ )
        {
            IEnumerable<Appartment> searchResults = _context.Appartments
                .Include("Deal").ToList();

            if (!string.IsNullOrWhiteSpace(Appartment.Address) && searchResults != null)
                searchResults = searchResults.Where(a => a.Address.Contains(Appartment.Address));

            if (Appartment.RoomsAmount != 0 && searchResults != null)
                searchResults = searchResults.Where(a => a.RoomsAmount == Appartment.RoomsAmount);

            if (Appartment.Area != 0 && searchResults != null)
                searchResults = searchResults.Where(a => a.Area == Appartment.Area);

            if (Appartment.Storey != 0 && searchResults != null)
                searchResults = searchResults.Where(a => a.Storey == Appartment.Storey);

            if (Appartment.Price != 0 && searchResults != null)
                searchResults = searchResults.Where(a => a.Price == Appartment.Price);

            if (searchResults != null)
            {
                IEnumerable<AppartmentModel> appartments = searchResults.Select(app => new AppartmentModel(app));
                Appartments = new ObservableCollection<AppartmentModel>(appartments);
            }
        }
        private void FilterAppartment(bool? param)
        {
            IEnumerable<Appartment> searchResults;

            if (param.HasValue)
            {
                searchResults = _context.Appartments
                    .Include("Deal").Where(a => a.Deal.IsConfirmed == param.Value).ToList();
            }
            else
            {
                searchResults = _context.Appartments
                    .Include("Deal").Where(a => a.Deal == null).ToList();
            }

            IEnumerable<AppartmentModel> appartments = searchResults.Select(app => new AppartmentModel(app));
            Appartments = new ObservableCollection<AppartmentModel>(appartments);
        }
        private void ReloadAppartment(object _ )
        {
            Appartment = new AppartmentModel();

            IEnumerable<Appartment> availableApps = _context.Appartments
                .Include("Deal").ToList();

            IEnumerable<AppartmentModel> appartments = availableApps.Select(app => new AppartmentModel(app));
            Appartments = new ObservableCollection<AppartmentModel>(appartments);
        }
        #endregion search
        public void Load()
        {
            Appartments = new ObservableCollection<AppartmentModel>
                (_context.Appartments.Include("Deal").ToList().Select(app => new AppartmentModel(app)));

            SelectedAppartmentVisibility = DropAppartmentOption = Visibility.Collapsed;
            SearchBarVisibility = Visibility.Visible;
            SelectedAppartment = new AppartmentModel();
            Appartment = new AppartmentModel();
        }

        public AppartmentModel SelectedAppartment
        {
            get => _selectedAppartment;
            set
            {
                _selectedAppartment = value;
                OnPropertyChanged();
            }
        }
        public Visibility SelectedAppartmentVisibility
        {
            get => _selectedAppartmentVisibility;
            set
            {
                _selectedAppartmentVisibility = value;
                OnPropertyChanged();
            }
        }
        public Visibility DropAppartmentOption
        {
            get => _dropAppartmentOption;
            set
            {
                _dropAppartmentOption = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<AppartmentModel> Appartments
        {
            get => _appartments;
            set
            {
                _appartments = value;
                OnPropertyChanged();
            }
        }
        public AppartmentModel Appartment
        {
            get => _appartment;
            set
            {
                _appartment = value;
                OnPropertyChanged();
            }
        }
        public Visibility SearchBarVisibility
        {
            get => _searchBarVisibility;
            set
            {
                _searchBarVisibility = value;
                OnPropertyChanged();
            }
        }
    }
}
