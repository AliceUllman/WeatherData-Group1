using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherData_Group1.Models
{
    public class DataPoint
    {
        public string FullData { get; set; }
        public string Date { get; set; }
        public string Year { get; set; }
        public string Month { get; set; }
        public string Time { get; set; }

        public double Temprature { get; set; }
        public double Humidity { get; set; }

        public bool Inside { get; set; }
        public DataPoint(string fullData, string date, string month, string time, double temprature, double humidity, bool inside) 
        {
            FullData = fullData;
            Date = date;
            Month = month;
            Time = time;
            Temprature = temprature;
            Humidity = humidity;
            Inside = inside;
        }
    }
}
