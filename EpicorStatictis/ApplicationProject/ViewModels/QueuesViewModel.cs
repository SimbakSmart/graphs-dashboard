
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

        #region GRAPH PIE PRIORITY  
        [ObservableProperty]
        private List<Queues> _listPriority = null;

        [ObservableProperty]
        private IEnumerable<ISeries> _seriesPriority;
        #endregion


        #region GRAPH PIE IMPACT   
        [ObservableProperty]
        private List<Queues> _listImpact = null;

        [ObservableProperty]
        private IEnumerable<ISeries> _seriesImpact;
        #endregion

        #region GRAPH PIE SERVICE 
        [ObservableProperty]
        private List<Queues> _listService = null;

        [ObservableProperty]
        private IEnumerable<ISeries> _seriesService;
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
            await UrgencyPieChartAsync();
            await PriorityPieChartAsync();
            await ImpactPieChartAsync();
            await ServicePieChartAsync();

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

            SKColor yellowColor = new SKColor(240, 180, 45, 255);
            SKColor greenColor = new SKColor(70, 202, 182, 255);
            SKColor redColor = new SKColor(228, 103, 87, 255);
            SKColor brownColor = new SKColor(146, 104, 18, 255);
            // Define custom colors for pie slices
            var sliceColors = new[] { redColor, greenColor, yellowColor, brownColor};

            SeriesUrgency = _totalUrgencyArray.AsPieSeries((value, series) =>
            {
                series.Name = _urgencyArray[_index++ % _urgencyArray.Length] + " " + value.ToString();
                series.DataLabelsPosition = LiveChartsCore.Measure.PolarLabelsPosition.Middle;
                series.DataLabelsSize = 20;
                series.DataLabelsPaint = new SolidColorPaint(new SKColor(255, 255, 255));
                series.ToolTipLabelFormatter = point => $"{point.StackedValue!.Share:P2}";
                // Assign custom color to the slice
                series.Fill = new SolidColorPaint(sliceColors[_index % sliceColors.Length]);

            });
        }

        private async Task PriorityPieChartAsync(FiltersParams filters = null)
        {
            int _index = 0;
            ListPriority?.Clear();

            if (filters != null)
            {
                ListPriority = await qs.GetTotalsByPriorityAsync(filters);
            }
            else
            {
                ListPriority = await qs.GetTotalsByPriorityAsync();
            }

            string[] _priorityArray = ListPriority.Select(q => q.Priority).ToArray();
            double[] _totalPriorityArray = ListPriority.Select(q => (double)q.Total).ToArray();

            SKColor blueColor = new SKColor(41, 121, 255);
            SKColor orangeColor = new SKColor(255, 109, 0);
            SKColor pinkColor = new SKColor(255, 23, 68);
            SKColor greenColor = new SKColor(0, 151, 167);
            var sliceColors = new[] { pinkColor, orangeColor, blueColor, greenColor };

            SeriesPriority = _totalPriorityArray.AsPieSeries((value, series) =>
            {
                series.Name = _priorityArray[_index++ % _priorityArray.Length] + " " + value.ToString();
                series.DataLabelsPosition = LiveChartsCore.Measure.PolarLabelsPosition.Middle;
                series.DataLabelsSize = 20;
                series.DataLabelsPaint = new SolidColorPaint(new SKColor(255, 255, 255));
                series.ToolTipLabelFormatter = point => $"{point.StackedValue!.Share:P2}";
                series.Fill = new SolidColorPaint(sliceColors[_index % sliceColors.Length]);

            });
        }


        private async Task ImpactPieChartAsync(FiltersParams filters = null)
        {
            int _index = 0;
            ListImpact?.Clear();

            if (filters != null)
            {
                ListImpact = await qs.GetTotalsByImpactAsync(filters);
            }
            else
            {
                ListImpact = await qs.GetTotalsByImpactAsync();
            }

            string[] _impactArray = ListImpact.Select(q => q.Impact).ToArray();
            double[] _totalImpactArray = ListImpact.Select(q => (double)q.Total).ToArray();

            SKColor blueColor2 = new SKColor(41, 121, 255);
            SKColor orangeColor2 = new SKColor(255, 109, 0);
            SKColor pinkColor2 = new SKColor(255, 23, 68);
            SKColor greenColor2 = new SKColor(0, 151, 167);
            var sliceColors = new[] { pinkColor2, orangeColor2, blueColor2, greenColor2 };

            SeriesImpact= _totalImpactArray.AsPieSeries((value, series) =>
            {
                series.Name = _impactArray[_index++ % _impactArray.Length] + " " + value.ToString();
                series.DataLabelsPosition = LiveChartsCore.Measure.PolarLabelsPosition.Middle;
                series.DataLabelsSize = 20;
                series.DataLabelsPaint = new SolidColorPaint(new SKColor(255, 255, 255));
                series.ToolTipLabelFormatter = point => $"{point.StackedValue!.Share:P2}";
                series.Fill = new SolidColorPaint(sliceColors[_index % sliceColors.Length]);

            });
        }


        private async Task ServicePieChartAsync(FiltersParams filters = null)
        {
            int _index = 0;
            ListService?.Clear();

            if (filters != null)
            {
                ListService = await qs.GetTotalsByServiceAsync(filters);
            }
            else
            {
                ListService = await qs.GetTotalsByServiceAsync();
            }

            string[] _serviceArray = ListService.Select(q => q.Service).ToArray();
            double[] _totalServiceArray = ListService.Select(q => (double)q.Total).ToArray();

            SKColor yellowColor = new SKColor(240, 180, 45, 255);
            SKColor greenColor = new SKColor(70, 202, 182, 255);
            SKColor redColor = new SKColor(228, 103, 87, 255);
            SKColor brownColor = new SKColor(146, 104, 18, 255);
        
            var sliceColors = new[] { redColor, greenColor, yellowColor, brownColor };

            SeriesService = _totalServiceArray.AsPieSeries((value, series) =>
            {
                series.Name = _serviceArray[_index++ % _serviceArray.Length] + " " + value.ToString();
                series.DataLabelsPosition = LiveChartsCore.Measure.PolarLabelsPosition.Middle;
                series.DataLabelsSize = 20;
                series.DataLabelsPaint = new SolidColorPaint(new SKColor(255, 255, 255));
                series.ToolTipLabelFormatter = point => $"{point.StackedValue!.Share:P2}";
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
                    await PriorityPieChartAsync(filters);
                    await ImpactPieChartAsync(filters);
                    await ServicePieChartAsync(filters);

                    await qs.DisposeAsync();

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
