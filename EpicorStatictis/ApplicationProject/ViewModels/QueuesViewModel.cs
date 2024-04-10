
using CommunityToolkit.Mvvm.ComponentModel;
using Core.Models;
using Infraestructure.Helpers;
using Infraestructure.Services;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using ApplicationProject.Views;
using CommunityToolkit.Mvvm.Input;

namespace ApplicationProject.ViewModels
{
    public partial class QueuesViewModel : ObservableObject
    {

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SendFiltersCommand))]
        private bool _isLoading;

        [ObservableProperty]
        private DateTime? _startDate;
        [ObservableProperty]
        private DateTime? _endDate;

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

        #region BAR GRAPH RESPONSALES 
        [ObservableProperty]
        private ColumnSeries<double> _userSeriesBar;

        [ObservableProperty]
        private List<Queues> _listBar = null;

        [ObservableProperty]
        public ISeries[] _seriesBar;

        [ObservableProperty]
        public Axis[] _xAxesBar;
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
            IsLoading = false;
            qs = new QueueServices();
             Task.Run(async () => await LoadDataAsync());
        }

        public async Task LoadDataAsync()
        {
            IsLoading = true;
            await GetTotalsAsync();
            await BarGraphByResponsableAsync();
            await qs.DisposeAsync();
            IsLoading = false;

        }

        private async Task GetTotalsAsync(FiltersParams filters = null)
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

        private async Task BarGraphByResponsableAsync(FiltersParams filters = null)
        {
            ListBar?.Clear();

            if (filters != null)
            {
                ListBar = await qs.GetTotalsByResponsableAsync(filters);
            }
            else
            {
                ListBar = await qs.GetTotalsByResponsableAsync();
            }


            UserSeriesBar = new ColumnSeries<double>()
            {
                Name = "Reportes Activos",
                Values = ListBar.Select(q => (double)q.Total).ToList(),
                Padding = 1,
                MaxBarWidth = double.PositiveInfinity,
                Fill = new SolidColorPaint(new SKColor(25, 118, 210, 255)),


            };
            Axis _axis = new Axis()
            {
                Labels = ListBar.Select(q => q.Name).ToList(),
                TextSize = 12,
                LabelsAlignment = LiveChartsCore.Drawing.Align.Start,
                IsVisible = true,
                LabelsRotation = -90,
                Position = AxisPosition.Start,
                Padding = new LiveChartsCore.Drawing.Padding(0)
            };

            SeriesBar = new ISeries[] { UserSeriesBar };
            XAxesBar = new Axis[] { _axis };
        }

    

        [RelayCommand]
        private async Task SendFiltersAsync()
        {
            IsLoading = true;
            try
            {
                if (StartDate.HasValue && EndDate.HasValue)
                {
                    var filters = new FiltersParams.FiltersParamsBuilder()
                                     .WithStartDate(StartDate.Value)
                                     .WithEndDate(EndDate.Value)
                                     .Build();


                    await GetTotalsAsync(filters);
                    await BarGraphByResponsableAsync(filters);
                }
            }
            catch
            {

            }
            finally
            {
                IsLoading = false;
            }

        }

        [RelayCommand]
        private async Task RefreshAsync()
        {
           
            await LoadDataAsync();
            StartDate = null;
            EndDate = null;
        }

    }
}
