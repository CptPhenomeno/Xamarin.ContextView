using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;

namespace ContextView.Sample.ViewModels
{
    internal class RestourantModel
    {
        public string Name { get; set; }
        public string WebSite { get; set; }
        public Location Position { get; set; }
    }

    internal class ListViewAllItemContextViewModel
    {
        public ObservableCollection<RestourantModel> Restourants { get; } = new ObservableCollection<RestourantModel>();

        public IAsyncCommand<RestourantModel> OpenWebSiteCommand { get; private set; }
        public IAsyncCommand<RestourantModel> OpenPositionCommand { get; private set; }

        public ListViewAllItemContextViewModel()
        {
            PopulateList();
            OpenWebSiteCommand = CommandFactory.Create<RestourantModel>(OpenWebSiteAsync);
            OpenPositionCommand = CommandFactory.Create<RestourantModel>(OpenPositionAsync);
        }

        private Task OpenWebSiteAsync(RestourantModel restourant)
        {
            if (string.IsNullOrWhiteSpace(restourant?.WebSite) is false)
                return Launcher.OpenAsync(restourant.WebSite);

            return Task.CompletedTask;
        }

        private Task OpenPositionAsync(RestourantModel restourant)
        {
            if (restourant.Position != null)
                return Map.OpenAsync(restourant.Position, new MapLaunchOptions { Name = restourant?.Name });
            
            return Task.CompletedTask;
        }

        private void PopulateList()
        {
            Restourants.Add(new RestourantModel
            {
                Name = "Noma",
                WebSite = "https://noma.dk/",
                Position = new Location(55.68284634790044, 12.610445445207853)
            });

            Restourants.Add(new RestourantModel
            {
                Name = "Celler de Can Roca",
                WebSite = "https://noma.dk/",
                Position = new Location(41.993519758190615, 2.808002263728136)
            });

            Restourants.Add(new RestourantModel
            {
                Name = "Osteria Francescana",
                WebSite = "https://osteriafrancescana.it/it/",
                Position = new Location(44.64491669037648, 10.921624448652121)
            });

            Restourants.Add(new RestourantModel
            {
                Name = "Eleven Madison Park",
                WebSite = "https://www.elevenmadisonpark.com/",
                Position = new Location(40.74181437903045, -73.98717996477956)
            });

            Restourants.Add(new RestourantModel
            {
                Name = "Dinner",
                WebSite = "https://www.dinnerbyheston.co.uk/",
                Position = new Location(51.50213718111088, -0.1599455142128856)
            });
        }
    }
}
