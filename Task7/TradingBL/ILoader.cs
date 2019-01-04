using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLoader
{
    //интерфейс загрузичка данных
    public interface ILoader
    {
        Queue<Candle> GetCandles();
        void ForMacd(out List<DateTime> dt, out List<double> cd);
    }
}
