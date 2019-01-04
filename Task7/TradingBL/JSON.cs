using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;



namespace DataLoader
{
    //загрузка данных из json
    public class JSONLoader : ILoader
    {
        string _fileName;

        public JSONLoader(string fileName)
        {
            this._fileName = fileName;
        }
        public Queue<Candle> GetCandles()
        {
            Queue<Candle> r = new Queue<Candle>();
            dynamic jsonObject = JsonConvert.DeserializeObject(File.ReadAllText(_fileName));
            List<Candle> candles = new List<Candle>();
            var t = ((JArray)jsonObject.data).Count;
            for (int i = 0; i < t; i++)
            {
                Candle candle = new Candle()
                {
                    High = (decimal)jsonObject.data[i].high,
                    Low = (decimal)jsonObject.data[i].low,
                    Open = (decimal)jsonObject.data[i].open,
                    Close = (decimal)jsonObject.data[i].close,
                    Time = DateTimeOffset.FromUnixTimeMilliseconds((long)jsonObject.data[i].timeStamp).UtcDateTime,
                    TimeStamp = (long)jsonObject.data[i].timeStamp,
                    Volume = (int)jsonObject.data[i].volume
                };
                candles.Add(candle);
            }
            candles.Sort();
            foreach (var item in candles)
            {
                r.Enqueue(item);
            }
            return r;
        }
        public void ForMacd(out List<DateTime> dt, out List<double> cd)
        {
            dt = new List<DateTime>();
            cd = new List<double>();
            dynamic jsonObject = JsonConvert.DeserializeObject(File.ReadAllText(_fileName));
            List<Candle> candles = new List<Candle>();
            var t = ((JArray)jsonObject.data).Count;
            for (int i = 0; i < t; i++)
            {
                cd.Add((double)jsonObject.data[i].close);
                dt.Add(DateTimeOffset.FromUnixTimeMilliseconds((long)jsonObject.data[i].timeStamp).UtcDateTime);
            }

        }
    }
}
