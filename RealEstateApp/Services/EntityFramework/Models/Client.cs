using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Services.EntityFramework.Models
{
    class Client
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Phone { get; set; }
        //
        public Client()
        {
            Deals = new List<Deal>();
        }
        //Relations
        public virtual ICollection<Deal> Deals { get; set; }
    }
}
