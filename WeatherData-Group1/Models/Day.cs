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

        public List<DataPoint> DataPoints { get; set; }

        public double AvgTempInside { get; set; } 
        public double AvgTempOutside { get; set; }

        public double AvgHumidityinside { get; set; }
        public double AvgHumidityOutside { get; set; }

        public Day(string date, string month, List<DataPoint> dataPoints) 
        {
            Date = date;
            Month = month;
            DataPoints = dataPoints;
            
            AvgTempOutside = GetAvgTempOutside(dataPoints);
            AvgTempInside = GetAvgTempInside(dataPoints);

            AvgHumidityinside = GetAvgHumidityInside(dataPoints);
            AvgHumidityOutside = GetAvgHumidityOutside(dataPoints);
        }

        private double GetAvgTempInside(List<DataPoint> dataPoints) 
        {
            List<DataPoint> inside = dataPoints.Where(dp => dp.Inside ).ToList();
            double sum = inside.Select(dp => dp.Temprature).Sum();
            double avg = sum / inside.Count;
           
            return avg;
        }
        private double GetAvgTempOutside(List<DataPoint> dataPoints)
        {
            List<DataPoint> outside = dataPoints.Where(dp => !dp.Inside).ToList();
            double sum = outside.Select(dp => dp.Temprature).Sum();
            double avg = sum / outside.Count;

            return avg;
        }

        private double GetAvgHumidityInside(List<DataPoint> dataPoints)
        {
            List<DataPoint> inside = dataPoints.Where(dp => dp.Inside).ToList();
            double sum = inside.Select(dp => dp.Humidity).Sum();
            double avg = sum / inside.Count;
            
            return avg;
        }
        private double GetAvgHumidityOutside(List<DataPoint> dataPoints)
        {
            List<DataPoint> outside = dataPoints.Where(dp => !dp.Inside).ToList();
            double sum = outside.Select(dp => dp.Humidity).Sum();
            double avg = sum / outside.Count;

            return avg;
        }
    }
}
