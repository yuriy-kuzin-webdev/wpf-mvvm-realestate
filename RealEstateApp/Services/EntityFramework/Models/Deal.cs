using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Services.EntityFramework.Models
{
    class Deal
    {
        //Подтверждение
        public bool IsConfirmed { get; set; }
        //Relations
        public int AppartmentId { get; set; }
        public int ClientId { get; set; }
        public virtual Appartment Appartment { get; set; }
        public virtual Client Client { get; set; }
    }
}
