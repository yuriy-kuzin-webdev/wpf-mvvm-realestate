using RealEstateApp.Services.EntityFramework.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Services.EntityFramework
{
    class RealEstateContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Appartment> Appartments { get; set; }
        public DbSet<Deal> Deals { get; set; }
        public RealEstateContext() : base("RealEstateDataBase") 
        {
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //One to one or zero
            modelBuilder
                .Entity<Deal>()
                .HasRequired<Appartment>(d => d.Appartment).WithOptional(a => a.Deal);
            modelBuilder
                .Entity<Deal>()
                .HasKey(d => d.AppartmentId);
        }
    }
}
