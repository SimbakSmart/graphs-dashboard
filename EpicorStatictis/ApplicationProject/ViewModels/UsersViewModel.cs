

using CommunityToolkit.Mvvm.ComponentModel;
using Core.Models;
using Infraestructure.Services;
using LiveCharts;
using LiveCharts.Wpf;

namespace ApplicationProject.ViewModels
{
    public partial  class UsersViewModel : ObservableObject
    {

        private UsersServices us = null;

        [ObservableProperty]
        private SeriesCollection _seriesBarCollection;
        [ObservableProperty]
        public string[] _userLabels;
        [ObservableProperty]
        public Func<double, string> _userFormatter;

        [ObservableProperty]
        private List<Users> _listBar = null;

        public UsersViewModel()
        {
           
            us = new UsersServices();
            GetBarGraph();
        }

        private async void GetBarGraph()
        {


            ListBar?.Clear();
            ListBar = await us.GetTotalsByResponsableAsync();
            string[] _userArray = ListBar.Select(q => q.Name).ToArray();

            SeriesBarCollection = new SeriesCollection
            {
               new RowSeries
               {
                    Values= new ChartValues<int> ( ListBar.Select(q => q.Total).ToArray())
               }
            };

            UserLabels= _userArray;
            UserFormatter = value => value.ToString("N");
        }
    }
}
