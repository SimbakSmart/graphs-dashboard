
using CommunityToolkit.Mvvm.ComponentModel;
using Core.Models;
using Infraestructure.Helpers;
using Infraestructure.Services;

namespace ApplicationProject.ViewModels
{
    public partial class QueuesViewModel : ObservableObject
    {
        private QueueServices qs = null;

        #region TOTALES   
        [ObservableProperty]
        private List<Queues> _total;

        [ObservableProperty]
        private int _granTotal;

        [ObservableProperty]
        private int _totalOpen;

        [ObservableProperty]
        private int _totalClosed;
        #endregion


        #region [SINGLENTON]
        static QueuesViewModel instance;
        public static QueuesViewModel GetInstance()
        {
            if (instance == null)
            {
                return new QueuesViewModel();
            }

            return instance;
        }
        #endregion


        public QueuesViewModel()
        {
            instance = this;
            qs = new QueueServices();
           // Task.Run(async () => await LoadDataAsync());
        }

        public async Task LoadDataAsync()
        {
            qs = new QueueServices();
            await GetTotalsAsync();
           
        }

        public  async Task GetTotalsAsync(FiltersParams filters = null)
        {
            if (filters != null)
            {
                Total = await qs.GetTotalsAsync(filters);
            }
            else
            {
                Total = await qs.GetTotalsAsync();
            }

            GranTotal = Total.Select(t => t.Total).FirstOrDefault();
            TotalOpen = Total.Select(t => t.TotalOpen).FirstOrDefault();
            TotalClosed = Total.Select(t => t.TotalClosed).FirstOrDefault();
        }

    }
}
