

using CommunityToolkit.Mvvm.ComponentModel;
using Core.Models;
using Infraestructure.Helpers;
using Infraestructure.Services;
using LiveCharts;
using LiveCharts.Wpf;

namespace ApplicationProject.ViewModels
{
    public partial class UsersViewModel : ObservableObject
    {
        [ObservableProperty]
        private bool _isLoading;

        [ObservableProperty]
        private DateTime? _startDate;
        [ObservableProperty]
        private DateTime? _endDate;
        private UsersServices us = null;

        #region BAR GRAPH BY USERS   
        [ObservableProperty]
        private SeriesCollection _seriesBarCollection;
        [ObservableProperty]
        public string[] _userLabels;
        [ObservableProperty]
        public Func<double, string> _userFormatter;

        [ObservableProperty]
        private List<Users> _listBar = null;
        #endregion


        #region BAR GRAPH BY STATUS
        [ObservableProperty]
        private SeriesCollection _statusCollection;
        [ObservableProperty]
        public string[] _statusLabels;
        [ObservableProperty]
        public Func<double, string> _statusFormatter;

        [ObservableProperty]
        private List<Users> _listSatus = null;
        #endregion


        public UsersViewModel()
        {
            IsLoading = false;
            us = new UsersServices();
            // Task.Run(async () => await LoadDataAsync());
            //Task task =  LoadDataAsync().WaitAsync();  
            //LoadDataAsync().WaitAsync();

            Task.Run(async () => await LoadDataAsync());
        }

        public async Task LoadDataAsync()
        {
            IsLoading = true;
            await BarGraphByUsersAsync();
            await BarGraphByStatusAsync();
           // await us.DisposeAsync();
            IsLoading = false;

        }

        private async Task BarGraphByUsersAsync(FiltersParams filters = null)
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

            UserLabels = _userArray;
            UserFormatter = value => value.ToString("N");

        }

        private async Task BarGraphByStatusAsync(FiltersParams filters = null)
        {


            ListSatus?.Clear();
            ListSatus = await us.GetTotalsByStatuseAsync();

            string[] _statusArray = ListSatus.Select(q => q.Status).ToArray();

            StatusCollection = new SeriesCollection
            {
               new ColumnSeries
               {
                    Values= new ChartValues<int> ( ListSatus.Select(q => q.Total).ToArray())
               }
            };

            StatusLabels = _statusArray;
            StatusFormatter = value => value.ToString("N");

        }


    }
}
