using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLoader;
namespace Trading
{
    //индикатор
    public class Indicator : IIndicator
    {
        private Candle LastCandle { get; set; }

        public Indicator()
        {
            LastCandle = null;
        }

        public decimal Calculate(Candle candle)
        {
            if (this.LastCandle == null)
            {
                this.LastCandle = candle;
                return 0;
            }
            if(candle.High - candle.Low == 0)
            {
                return 0;
            }
            var candleMed = (candle.High + candle.Low) / 2;
            var candleDif = candle.High - candle.Low;
            var lastCandleMed = (this.LastCandle.High + this.LastCandle.Low) / 2;
            return (candleMed - lastCandleMed) * candleDif / candle.Volume;
        }
    }
}
