using RealEstateApp.Services.EntityFramework.Models;
using RealEstateApp.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Models
{
    class DealModel : ObservableObject
    {
        private Deal _deal;
        public DealModel()
        {
            _deal = new Deal();
        }
        public DealModel(Deal deal)
        {
            _deal = deal;
        }
        public int Id
        {
            get => _deal.AppartmentId;
        }
        public Deal Deal
        {
            get => _deal;
        }
        public Client Client
        {
            get => _deal.Client;
            set
            {
                _deal.Client = value;
                OnPropertyChanged();
            }
        }
        public Appartment Appartment
        {
            get => _deal.Appartment;
            set
            {
                _deal.Appartment = value;
                OnPropertyChanged();
            }
        }
        public bool IsConfirmed
        {
            get => _deal.IsConfirmed;
            set
            {
                _deal.IsConfirmed = value;
                OnPropertyChanged();
            }
        }       
    }
}
