using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLoader;
namespace Trading
{
    public class Indicator : IIndicator
    {
        private Candle LastCandle { get; set; }

        public Indicator()
        {
            LastCandle = null;
        }

        public decimal Calculate(Candle candle)
        {
            if (LastCandle == null)
            {
                LastCandle = candle;
                return 0;
            }
            var candleMed = (candle.High + candle.Low) / 2;
            var candleDif = candle.High - candle.Low;
            var lastCandleMed = (LastCandle.High + LastCandle.Low) / 2;
            return (candleMed - lastCandleMed) * candleDif / candle.Volume;
        }
    }
}
