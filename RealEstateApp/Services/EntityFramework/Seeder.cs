using RealEstateApp.Services.EntityFramework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Services.EntityFramework
{
    static class Seeder
    {
        public static void AddClients(this RealEstateContext context)
        {
            context.Clients.AddRange(new[]
            {
                new Client
                {
                    FirstName = "Андрей",
                    MiddleName = "Игоревич",
                    LastName = "Титько",
                    Phone = "0930088172"
                },
                new Client
                {
                    FirstName = "Марьяна",
                    MiddleName = "Степановна",
                    LastName = "Глушко",
                    Phone = "0933337100"
                },
                new Client
                {
                    FirstName = "Инокентий",
                    MiddleName = "Абрамович",
                    LastName = "Рубдинский",
                    Phone = "0665157129"
                },
            });
            context.SaveChanges();
        }
        public static void AddAppartments(this RealEstateContext context)
        {
            context.Appartments.AddRange(new[]
            {
                new Appartment
                {
                    Address = "Харьков, Космическая 10, 12кв",
                    Area = 55.0,
                    RoomsAmount = 2,
                    Storey = 4,
                    Price = 35000.0,

                },
                new Appartment
                {
                    Address = "Харьков, Гарибальди 8, 24кв",
                    Area = 42.0,
                    RoomsAmount = 1,
                    Storey = 5,
                    Price = 27000.0,

                },
                new Appartment
                {
                    Address = "Харьков, ак.Вальтера 12, 36кв",
                    Area = 47.0,
                    RoomsAmount = 1,
                    Storey = 2,
                    Price = 30000.0,
                },
            });
            context.SaveChanges();
        }
        public static void AddUsers(this RealEstateContext context)
        {
            context.Users.AddRange(new[]
            {
                new User{Name = "user", Password = "user", IsAdmin = false},
                new User{Name = "admin", Password = "admin", IsAdmin = true},
            });
            context.SaveChanges();
        }
        public static void AddDeals(this RealEstateContext context)
        {
            context.Deals.AddRange(new[]
            {
                new Deal{ Appartment = context.Appartments.First(app => app.RoomsAmount == 1), Client = context.Clients.First()},
                new Deal{ Appartment = context.Appartments.First(app => app.RoomsAmount == 2), Client = context.Clients.First(), IsConfirmed = true},
            });
            context.SaveChanges();
        }
        public static void RunSeeds(this RealEstateContext context)
        {
            context.AddUsers();
            context.AddClients();
            context.AddAppartments();
            context.AddDeals();
        }
    }
}
