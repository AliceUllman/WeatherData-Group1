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

        public double MouldRiskInside { get; set; }
        public double MouldRiskOutside { get; set; }

        public Day(string date, string month, List<DataPoint> dataPoints) 
        {
            Date = date;
            Month = month;
            DataPoints = dataPoints;

            AvgTempInside = Math.Round(GetAvgTempInside(dataPoints),2);
            AvgTempOutside = Math.Round(GetAvgTempOutside(dataPoints),2);

            AvgHumidityinside = Math.Round(GetAvgHumidityInside(dataPoints), 2); 
            AvgHumidityOutside = Math.Round(GetAvgHumidityOutside(dataPoints), 2);

            MouldRiskInside = Math.Round(CalculateMoldRisk(GetAvgTempInside(dataPoints), GetAvgHumidityInside(dataPoints)),2);
            MouldRiskOutside = Math.Round(CalculateMoldRisk(GetAvgTempOutside(dataPoints), GetAvgHumidityOutside(dataPoints)),2);
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

        
        public static double CalculateMoldRisk(double temperature, double humidity)
        {
            // Step 1: Compute critical relative humidity (RHcrit)
            double rhCrit;
            if (temperature <= 20)
            {
                rhCrit = 0.00267 * Math.Pow(temperature, 3) // 0.00267 * temperature^3 - 0.160 * temperature^2 + 3.13 * temperature + 100
                       - 0.160 * Math.Pow(temperature, 2) 
                       + 3.13 * temperature
                       + 100;
            }
            else
            {
                rhCrit = 80.0; // default above 20°C
            }

            // Step 2: Growth suitability factor (GSF)
            double gsf = 0.0;
            if (humidity >= rhCrit)
            {
                gsf = (humidity - rhCrit) / (100.0 - rhCrit);
                gsf = Math.Max(0.0, Math.Min(1.0, gsf)); // Clamp between 0 and 1
            }

            // Step 3: Temperature factor
            double tempFactor = 0.0;
            if (temperature >= 0 && temperature <= 50)
            {
                tempFactor = temperature / 50.0; // normalized 0–1
            }

            // Step 4: Daily mold risk
            double MoldRisk = gsf * tempFactor;

            return MoldRisk;
        }
    }
}
