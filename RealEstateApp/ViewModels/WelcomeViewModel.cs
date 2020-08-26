using RealEstateApp.Interfaces;
using RealEstateApp.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.ViewModels
{
    class WelcomeViewModel : ObservableObject, ILoadCollection
    {
        public string WelcomeMessage { get; private set; }
        public ObservableCollection<string> RightsDescription { get; private set; }
        
        public WelcomeViewModel(bool isAdmin)
        {
            SetRightsDescription(isAdmin);
        }
        private void SetRightsDescription(bool isAdmin)
        {
            WelcomeMessage = isAdmin
                ? "Вы зашли как админ"
                : "Вы зашли как риэлтор";


            RightsDescription = isAdmin

                ? new ObservableCollection<string>(new[]
                {
                    "-Редактировать записи",
                    "-Добавлять/Удалять записи",
                    "-Управлять пользователями системы"
                })

                : new ObservableCollection<string>( new[]
                {
                    "-Просмотр квартир",
                    "-Снимать/Ставить бронь",
                    "-Продавать квартиры"
                });
        }

        public void LoadCollection(object param) { }
        public void Load() { }
    }
}
