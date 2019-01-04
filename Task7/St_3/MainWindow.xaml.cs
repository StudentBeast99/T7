using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DataLoader;
using Trading;
using SciChart.Charting.Model.DataSeries;
using SciChart.Data.Model;
using SciChart.Examples.ExternalDependencies.Data;
using System.Windows.Threading;
using Microsoft.Win32;



namespace St_3
{
   
    public partial class MainWindow : Window
    {
        private OhlcDataSeries<DateTime, double> ohlcSeries;

        private XyDataSeries<DateTime, double> dataSeries;
        private XyyDataSeries<DateTime, double> macdDataSeries;
        List<MacdPoint> macdPoints;
       
        private Monitor Monitor { get; set; }
        private int candleCount = 0;
        public MainWindow()
        {
            InitializeComponent();
            ReloadChartSeries();
            StockChart.XAxis.AutoRange = SciChart.Charting.Visuals.Axes.AutoRange.Once;
            indicatorChar.XAxis.AutoRange = SciChart.Charting.Visuals.Axes.AutoRange.Once;
        }
        //рабоота с файлами
        #region files
        private void XLSX_File_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "XLSX files (*.xlsx)|*.xlsx";
            XLSXLoader xlsxloader;
            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    xlsxloader = new XLSXLoader(openFileDialog.FileName);
                    List<DateTime> dt = new List<DateTime>();
                    List<double> cd = new List<double>();

                    xlsxloader.ForMacd(out dt, out cd);
                    macdPoints = cd.Macd(12, 26, 9).ToList();
                    LoadDataSource(new Reporter(xlsxloader));
                    this.Loaded += OnLoaded;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
       
        private void JSON_Exit_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "JSON files (*.json)|*.json";
            JSONLoader jsonloader;
            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    jsonloader = new JSONLoader(openFileDialog.FileName);
                    List<DateTime> dt = new List<DateTime>();
                    List<double> cd = new List<double>();

                    jsonloader.ForMacd(out dt, out cd);
                    macdPoints = cd.Macd(12, 26, 9).ToList();
                    LoadDataSource(new Reporter(jsonloader));
                    this.Loaded += OnLoaded;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        #endregion
        //загрузка данных
        private void LoadDataSource(Reporter reporter)
        {
          
            this.Monitor = new Monitor(reporter, new Indicator());
            this.Monitor.NewCandle += Monitor_NewCandle;
            ReloadChartSeries();
            this.Monitor.Run();
        }
        //обработка события появления данных
        private void Monitor_NewCandle(Candle candle, decimal indicator)
        {
            StockChart.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { AddNewCandle(candle, indicator); }));
        }
        //добавление свечи
        private void AddNewCandle(Candle candle, decimal indicator)
        {
            using (macdDataSeries.SuspendUpdates())
            using (dataSeries.SuspendUpdates())
            using (ohlcSeries.SuspendUpdates())
            {
                ohlcSeries.Append(candle.Time, (double)candle.Open, (double)candle.High, (double)candle.Low, (double)candle.Close);
                macdDataSeries.Append(candle.Time, macdPoints[candleCount].Macd, macdPoints[candleCount].Signal);
                candleCount++;
                dataSeries.Append(candle.Time, (double)indicator);
                StockChart.XAxis.VisibleRange = new IndexRange(candleCount - 50, candleCount);

                indicatorChar.XAxis.VisibleRange = new IndexRange(candleCount - 50, candleCount);

            }
        }
        //инициализация чарта
        private void ReloadChartSeries()
        {
            ohlcSeries = new OhlcDataSeries<DateTime, double>() { SeriesName = "Candles", FifoCapacity = 10000 };
            macdDataSeries = new XyyDataSeries<DateTime, double>() { SeriesName = "MACD" };
            dataSeries = new XyDataSeries<DateTime, double> { SeriesName = "Histogram", FifoCapacity = 10000 };
            CandleSeries.DataSeries = ohlcSeries;
            MACD2.DataSeries = macdDataSeries;
            Histo.DataSeries = dataSeries;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
        }
    }
}
