using RealEstateApp.Interfaces;
using RealEstateApp.Services.EntityFramework;
using RealEstateApp.Services.EntityFramework.Models;
using RealEstateApp.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.ViewModels
{
    class StatsViewModel : ObservableObject, ILoadCollection
    {
        private RealEstateContext _context;
        //appartments count stats
        private int _totalApps;
        private int _reservedApps;
        private int _confirmedApps;
        private int _availableApps;
        //clients count stats
        private int _totalClients;
        private int _activeClients;
        private int _inactiveClients;
        //deals stats
        private int _reservedDeals;
        private int _confirmedDeals;
        private int _totalDeals;
        private double _reservedSum;
        private double _confirmedSum;
        private double _totalSum;

        //Commands

        public StatsViewModel(RealEstateContext context)
        {
            _context = context;
            Load();
        }

        public void LoadCollection(object param) { }
        public void Load()
        {
            LoadApps();
            LoadClients();
            LoadDeals();
        }

        private void LoadApps()
        {
            List<Appartment> list = _context.Appartments.Include("Deal").ToList();
            TotalApps = list.Count;

            list = list.Where(a => a.Deal != null).ToList();
            AvailableApps = TotalApps - list.Count;

            ReservedApps = list.Where(a => a.Deal.IsConfirmed == false).Count();
            ConfirmedApps = list.Where(a => a.Deal.IsConfirmed == true).Count();
        }

        private void LoadClients()
        {
            List<Client> list = _context.Clients.Include("Deals").ToList();
            TotalClients = list.Count;
            ActiveClients = list.Where(c => c.Deals.Count != 0).Count();
            InactiveClients = TotalClients - ActiveClients;
        }

        private void LoadDeals()
        {
            List<Deal> list = _context.Deals.Include("Appartment").ToList();
            TotalDeals = list.Count;
            TotalSum = list.Sum(d => d.Appartment.Price);

            list = list.Where(d => d.IsConfirmed == true).ToList();
            ConfirmedDeals = list.Count();
            ConfirmedSum = list.Sum(d => d.Appartment.Price);

            ReservedDeals = TotalDeals - ConfirmedDeals;
            ReservedSum = TotalSum - ConfirmedSum;
        }
        #region props
        public int TotalApps
        {
            get => _totalApps;
            set
            {
                _totalApps = value;
                OnPropertyChanged();
            }
        }
        public int ReservedApps
        {
            get => _reservedApps;
            set
            {
                _reservedApps = value;
                OnPropertyChanged();
            }
        }
        public int ConfirmedApps
        {
            get => _confirmedApps;
            set
            {
                _confirmedApps = value;
                OnPropertyChanged();
            }
        }
        public int AvailableApps
        {
            get => _availableApps;
            set
            {
                _availableApps = value;
                OnPropertyChanged();
            }
        }
        //clients count stats
        public int TotalClients
        {
            get => _totalClients;
            set
            {
                _totalClients = value;
                OnPropertyChanged();
            }
        }
        public int ActiveClients
        {
            get => _activeClients;
            set
            {
                _activeClients = value;
                OnPropertyChanged();
            }
        }
        public int InactiveClients
        {
            get => _inactiveClients;
            set
            {
                _inactiveClients = value;
                OnPropertyChanged();
            }
        }
        //deals stats
        public int ReservedDeals
        {
            get => _reservedDeals;
            set
            {
                _reservedDeals = value;
                OnPropertyChanged();
            }
        }
        public int ConfirmedDeals
        {
            get => _confirmedDeals;
            set
            {
                _confirmedDeals = value;
                OnPropertyChanged();
            }
        }
        public int TotalDeals
        {
            get => _totalDeals;
            set
            {
                _totalDeals = value;
                OnPropertyChanged();
            }
        }
        public double ReservedSum
        {
            get => _reservedSum;
            set
            {
                _reservedSum = value;
                OnPropertyChanged();
            }
        }
        public double ConfirmedSum
        {
            get => _confirmedSum;
            set
            {
                _confirmedSum = value;
                OnPropertyChanged();
            }
        }
        public double TotalSum
        {
            get => _totalSum;
            set
            {
                _totalSum = value;
                OnPropertyChanged();
            }
        }
        #endregion props
    }
}
