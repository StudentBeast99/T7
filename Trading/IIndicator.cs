using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLoader;

namespace Trading
{
    public interface IIndicator
    {
        decimal Calculate(Candle candle);
    }
}
