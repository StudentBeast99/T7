using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLoader
{
    
    public class Reporter
    {
        public ILoader Loader { get; set; }
        public Queue<Candle> Candles { get; set; }

        public Reporter(ILoader loader)
        {
            Loader = loader;
            Candles = loader.GetCandles();
        }

        public Candle GetCandle()
        {
            return Candles.Dequeue();
        }

        public void Update()
        {
            Candles = Loader.GetCandles();
        }
    }
}
