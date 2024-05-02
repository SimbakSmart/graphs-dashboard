
using CommunityToolkit.Mvvm.ComponentModel;
using Core.Models;
using Infraestructure.Helpers;
using Infraestructure.Services;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using CommunityToolkit.Mvvm.Input;
using LiveChartsCore.SkiaSharpView.Extensions;
using System.Drawing;
using System.Windows;
using System.Xml.Linq;

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

        #region  TABLE RANGE DAYS 
        [ObservableProperty]
        private List<Queues> _listByRange;
        #endregion

        #region  GRAPH STATUS         
        [ObservableProperty]
        private ColumnSeries<double> _userSeriesStatus;

        [ObservableProperty]
        private List<Queues> _listStatus = null;

        [ObservableProperty]
        public ISeries[] _seriesStatus;

        [ObservableProperty]
        public Axis[] _xAxesStatus;
        #endregion

        #region GRAPH PIE URGENCY 
        [ObservableProperty]
        private List<Queues> _listUrgency = null;

        [ObservableProperty]
        private IEnumerable<ISeries> _seriesUrgency; 
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
            await GetTotalsByRangeAsync();
            await BarGraphBySatusAsync();    
            await qs.DisposeAsync();
            await UrgencyPieChartAsync();
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

        private async Task GetTotalsByRangeAsync(FiltersParams filters = null)
        {
            if (filters != null)
            {
                ListByRange = await qs.GetTotalsByRangeDayseAsync(filters);
            }
            else
            {
                ListByRange = await qs.GetTotalsByRangeDayseAsync();
            }

        }

        private async Task BarGraphBySatusAsync(FiltersParams filters = null)
        {
            ListStatus?.Clear();
            if (filters != null)
            {
                ListStatus = await qs.GetTotalsByStatuseAsync(filters);
            }
            else
            {
                ListStatus = await qs.GetTotalsByStatuseAsync();
            }


            UserSeriesStatus = new ColumnSeries<double>()
            {
                Name = "Reportes Activos",
                Values = ListStatus.Select(q => (double)q.Total).ToList(),
                Padding = 1,
                MaxBarWidth = double.PositiveInfinity,
                Fill = new SolidColorPaint(new SKColor(25, 118, 210, 255)),
            };


            Axis _axis = new Axis()
            {
                Labels = ListStatus.Select(q => q.Status).ToList(),
                TextSize = 12,
                LabelsAlignment = LiveChartsCore.Drawing.Align.Start,
                IsVisible = true,
                LabelsRotation = -90,
                Position = AxisPosition.Start,
                Padding = new LiveChartsCore.Drawing.Padding(0)
            };

            SeriesStatus = new ISeries[] { UserSeriesStatus };
            XAxesStatus = new Axis[] { _axis };

        }

        private async Task UrgencyPieChartAsync(FiltersParams filters = null)
        {
            int _index = 0;
            ListUrgency?.Clear();

            if (filters != null)
            {
                ListUrgency = await qs.GetTotalsByUrgencyAsync(filters);
            }
            else
            {
                ListUrgency = await qs.GetTotalsByUrgencyAsync();
            }

            string[] _urgencyArray = ListUrgency.Select(q => q.Urgency).ToArray();
            double[] _totalUrgencyArray = ListUrgency.Select(q => (double)q.Total).ToArray();


            // Define custom colors for pie slices
            //var sliceColors = new[] { SKColors.Purple, SKColors.Blue, SKColors.DarkSeaGreen, SKColors.Green, SKColors.Red };
            var sliceColors = new[] { SKColors.Red, SKColors.DeepSkyBlue, SKColors.DarkSeaGreen,SKColors.Yellow};

            SeriesUrgency = _totalUrgencyArray.AsPieSeries((value, series) =>
            {
                series.Name = _urgencyArray[_index++ % _urgencyArray.Length] + " " + value.ToString();
                series.DataLabelsPosition = LiveChartsCore.Measure.PolarLabelsPosition.Middle;
                series.DataLabelsSize = 20;
                series.DataLabelsPaint = new SolidColorPaint(new SKColor(255, 255, 255));
                // series.DataLabelsPaint = new SolidColorPaint(new SKColor(0, 0, 0));
                series.ToolTipLabelFormatter = point => $"{point.StackedValue!.Share:P2}";
                // Assign custom color to the slice
                series.Fill = new SolidColorPaint(sliceColors[_index % sliceColors.Length]);

            });
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
                    await GetTotalsByRangeAsync(filters);
                    await BarGraphBySatusAsync(filters);
                    await UrgencyPieChartAsync(filters);
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
