using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherData_Group1.Models
{
    public class Day
    {
        public string Date { get; set; }
        public string Month { get; set; }
        public string[] Times { get; set; }

        public double[] Tempratures { get; set; }
        public double[] Humidities { get; set; }

        public bool Inside { get; set; }

        public double AvgTemp { get; set; } 
        public double AvgHumidity { get; set; }

        public Day(string date, string month, string[] times, double[] tempratures, double[] humidities, bool inside) 
        {
            Date = date;
            Month = month;
            Times = times;
            Tempratures = tempratures;
            Humidities = humidities;
            Inside = inside;
            AvgTemp = GetAvgTemp(tempratures);
            AvgHumidity = GetAvgHumidity(humidities);
        }

        private double GetAvgTemp(double[] tempratures) 
        {
            double avg = tempratures.Sum() / tempratures.Count();
            return avg;
        }

        private double GetAvgHumidity(double[] humidities)
        {
            double avg = humidities.Sum() / humidities.Count();
            return avg;
        }
    }
}
