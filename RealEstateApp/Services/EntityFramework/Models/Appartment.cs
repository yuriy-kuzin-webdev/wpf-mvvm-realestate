using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Services.EntityFramework.Models
{
    class Appartment
    {
        public int Id { get; set; }
        //Кол-во комнат
        public int RoomsAmount { get; set; }
        //Адресс
        public string Address { get; set; }
        //Площадь
        public double Area { get; set; }
        //Этаж
        public double Storey { get; set; }
        //Цена
        public double Price { get; set; }
        
        //Relations
        public virtual Deal Deal { get; set; }
    }
}
