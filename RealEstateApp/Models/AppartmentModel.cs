using RealEstateApp.Services.EntityFramework.Models;
using RealEstateApp.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Models
{
    class AppartmentModel : ObservableObject
    {
        private Appartment _appartment;
        public AppartmentModel()
        {
            _appartment = new Appartment();
        }
        public AppartmentModel(Appartment appartment)
        {
            _appartment = appartment;
        }
        public Appartment Appartment
        {
            get => _appartment;
        }
        public int Id
        {
            get => _appartment.Id;
        }
        public int RoomsAmount
        {
            get => _appartment.RoomsAmount;
            set
            {
                _appartment.RoomsAmount = value;
                OnPropertyChanged();
            }
        }
        public string Address
        {
            get => _appartment.Address;
            set
            {
                _appartment.Address = value;
                OnPropertyChanged();
            }
        }
        public double Area
        {
            get => _appartment.Area;
            set
            {
                _appartment.Area = value;
                OnPropertyChanged();
            }
        }
        public double Storey
        {
            get => _appartment.Storey;
            set
            {
                _appartment.Storey = value;
                OnPropertyChanged();
            }
        }
        public double Price
        {
            get => _appartment.Price;
            set
            {
                _appartment.Price = value;
                OnPropertyChanged();
            }
        }
        public string Status
        {
            get
            {
                return _appartment.Deal == null
                    ? "Свободна"
                    : _appartment.Deal.IsConfirmed
                        ? "Выкуплена"
                        : "Забронирована";
            }
        }
    }
}
