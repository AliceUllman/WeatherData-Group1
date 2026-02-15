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

            MouldRiskInside = GetMouldRiskPercent(GetAvgTempInside(dataPoints), GetAvgHumidityInside(dataPoints));
            MouldRiskOutside = GetMouldRiskPercent(GetAvgTempOutside(dataPoints), GetAvgHumidityOutside(dataPoints));
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


        public static string GetMouldRisk(double temperature, double humidity)
        {
            const double IdealTemp = 25.0;
            const double MaxTemp = 50.0;
            const double MinTemp = 0.0;
            const double IdealHumidity = 90.0;
            const double MinHumidity = 70.0;

            //how favorable the temprature is for mold, 0 = worst conditions(0%), 1 = best conditions(100%)
            double tempFactor;

            if (temperature < MinTemp || temperature > MaxTemp)
                tempFactor = 0.0;
            else if (temperature <= IdealTemp)
                tempFactor = temperature / IdealTemp; //Scale the likelihood up until 25 Celsius (25/25 = 1)
            else
                tempFactor = Math.Max(0.0, (MaxTemp - temperature) / (MaxTemp - IdealTemp)); //Scale the likelihood down, math max makes sure number does not go negative

            // how favorable the Humidity is for mold, 0 = worst conditions(0%), 1 = best conditions(100%)
            double humidityFactor;
            if (humidity >= MinHumidity)
            {
                humidityFactor = Math.Min(humidity / IdealHumidity, 1.0); //math min to make sure it is not more than 100%
            }
            else { humidityFactor = 0.0; }

            double riskPercent = tempFactor * humidityFactor * 100.0;

            if (riskPercent < 5) return "Very unlikely";
            if (riskPercent < 15) return "Unlikely";
            if (riskPercent < 30) return "Some chance";
            if (riskPercent < 45) return "Moderate chance";
            if (riskPercent < 65) return "Likely";
            if (riskPercent < 85) return "Very likely";
            return "Almost certain";
        }

        public static double GetMouldRiskPercent(double temperature, double humidity)
        {
            const double IdealTemp = 25.0;
            const double MaxTemp = 50.0;
            const double MinTemp = 0.0;
            const double IdealHumidity = 90.0;
            const double MinHumidity = 70.0;

            //how favorable the temprature is for mold, 0 = worst conditions(0%), 1 = best conditions(100%)
            double tempFactor;

            if (temperature < MinTemp || temperature > MaxTemp)
                tempFactor = 0.0;
            else if (temperature <= IdealTemp)
                tempFactor = temperature / IdealTemp; //Scale the likelihood up until 25 Celsius (25/25 = 1)
            else
                tempFactor = Math.Max(0.0, (MaxTemp - temperature) / (MaxTemp - IdealTemp)); //Scale the likelihood down, math max makes sure number does not go negative

            // how favorable the Humidity is for mold, 0 = worst conditions(0%), 1 = best conditions(100%)
            double humidityFactor;
            if (humidity >= MinHumidity)
            {
                humidityFactor = Math.Min(humidity / IdealHumidity, 1.0); //math min to make sure it is not more than 100%
            }
            else { humidityFactor = 0.0; }

            double riskPercent = tempFactor * humidityFactor * 100.0;
            return riskPercent;
        }
    }
}
