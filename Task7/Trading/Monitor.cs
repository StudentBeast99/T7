using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using DataLoader;
namespace Trading
{
    public class Monitor
    {
        //событие доюавления данных
        public delegate void NewCandleEvent(Candle candle, decimal indicator);
        public event NewCandleEvent NewCandle;

        //источник данных
        public Reporter Reporter { get; set; }
        //индикатор
        public IIndicator Indicator { get; set; }

        public Monitor(Reporter reporter, IIndicator indicator)
        {
            this.Reporter = reporter;
            this.Indicator = indicator;
        }

        //backgroung поток
        public void Run()
        {
            ThreadPool.QueueUserWorkItem(x => Update());
        }
        //обновление данных
        private void Update()
        {
            while (this.Reporter.Candles.Count != 0)
            {
                var candle = this.Reporter.GetCandle();
                NewCandle?.Invoke(candle, this.Indicator.Calculate(candle));
                Thread.Sleep(1000);
            }
        }
    }
}
