using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using OfficeOpenXml;

namespace DataLoader
{
    //загрузка данных из Excel
    public class XLSXLoader : ILoader
    {
        string _fileName;

        public XLSXLoader(string fileName)
        {
            this._fileName = fileName;
        }

        public Queue<Candle> GetCandles()
        {
            Queue<Candle> r = new Queue<Candle>();

            using (ExcelPackage xlPackage = new ExcelPackage(new FileInfo(_fileName)))
            {
                var myWorksheet = xlPackage.Workbook.Worksheets.First();
                var totalRows = myWorksheet.Dimension.End.Row;
                var totalColumns = myWorksheet.Dimension.End.Column;

                for (int rowNum = 2; rowNum <= totalRows; rowNum++)
                {
                    var row = myWorksheet.Cells[rowNum, 1, rowNum, totalColumns]
                        .Select(c => c.Value == null ? string.Empty : c.Value.ToString()).ToArray();

                    string rawDateTime = row[2] + " " + row[3];

                    DateTime dateTime = DateTime.ParseExact(rawDateTime, "yyyyMMdd HHmmss", null);

                    Candle candle = new Candle()
                    {
                        High = int.Parse(row[5]),
                        Low = int.Parse(row[6]),
                        Open = int.Parse(row[4]),
                        Close = int.Parse(row[7]),
                        Time = dateTime,
                        TimeStamp = ((DateTimeOffset)dateTime).ToUnixTimeMilliseconds(),
                        Volume = int.Parse(row[8]),
                    };

                    r.Enqueue(candle);
                }
            }
            return r;
        }
        public void ForMacd(out List<DateTime> dt, out List<double> cd)
        {
            List<DateTime> TimeData = new List<DateTime>();
            List<double> OpenData = new List<double>();
            List<double> HighData = new List<double>();
            List<double> LowData = new List<double>();
            List<double> CloseData = new List<double>();
            List<long> VolumeData = new List<long>();
            dt = new List<DateTime>();
            cd = new List<double>();
            using (ExcelPackage xlPackage = new ExcelPackage(new FileInfo(_fileName)))
            {
                var myWorksheet = xlPackage.Workbook.Worksheets.First();
                var totalRows = myWorksheet.Dimension.End.Row;
                var totalColumns = myWorksheet.Dimension.End.Column;

                for (int rowNum = 2; rowNum <= totalRows; rowNum++)
                {
                    var row = myWorksheet.Cells[rowNum, 1, rowNum, totalColumns]
                        .Select(c => c.Value == null ? string.Empty : c.Value.ToString()).ToArray();

                    string rawDateTime = row[2] + " " + row[3];

                    DateTime dateTime = DateTime.ParseExact(rawDateTime, "yyyyMMdd HHmmss", null);

                    cd.Add(int.Parse(row[7]));
                    dt.Add(dateTime);
                    CloseData.Add(int.Parse(row[7]));
                    TimeData.Add(dateTime);
                }
            }

        }
    }
}
