using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WeatherData_Group1.Models;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WeatherData_Group1.Services
{
    public static class DataExtract
    {
        private static Regex Regex = new Regex(@"(?<Date>^(?<Year>\d{4})-(?<Month>0[1-9]|1[0-2])-(?<Day>0[1-9]|1[0-9]|2[0-9]|3[0-1])) (?<Time>\d{2}:\d{2}:\d{2}),(?<Position>Inne|Ute),(?<Temprature>\-?[1-9]?[0-9]\.[1-9]?[0-9]),(?<Humidity>\d{2})");

        private static string path = @"..\..\..\TxtFiles\";

        private static string fileName = "tempdata5-med fel.txt";
        
        public static void ReadAll(string fileName)
        {
            try
            {
                using (StreamReader reader = new StreamReader(path + fileName)) //kopplas till filen via 
                {
                    string line = reader.ReadLine();
                    int rowCount = 0;
                    while (line != null)
                    {
                        Console.WriteLine(rowCount + " " + line);
                        rowCount++;
                        line = reader.ReadLine();
                    }
                }
            }
            catch
            {
                Console.WriteLine("Filen finns inte");
            }
        }
       
        public static List<string> ReadAllWeatherData()
        {
            try
            {
                using (StreamReader reader = new StreamReader(path + fileName))
                {
                    var daList = new List<string>();
                    string line = reader.ReadLine();
                    int rowCount = 0;
                    while (line != null)
                    {
                        daList.Add(line);
                        line = reader.ReadLine();
                    }
                    return daList;
                }
            }
            catch
            {
                var noList = new List<string> { "Filen finns inte" };
                return noList;
            }
        }

        public static async Task WriteRowAsync(string fileName, string text)
        {
            using (StreamWriter streamWriter = new StreamWriter(path + fileName, true))
            {
                await streamWriter.WriteLineAsync(text);
            }
        }

        public static void WriteRow(string fileName, string text) 
        {
            using (StreamWriter streamWriter = new StreamWriter(path + fileName, true))//true för append, utan true skriver den över text filens innehåll
            {
                streamWriter.WriteLine(text);
            }
        }

        public static List<DataPoint> GetAllWeatherDataPoints(List<string> dataList)
        {
            List<DataPoint> AllDataPoints = new();

            foreach (var dataPoint in dataList) 
            {
                Match match = Regex.Match(dataPoint);
                
                string year = match.Groups["Year"].Value;
                string month = match.Groups["Month"].Value;
                
                if (match.Success)
                {
                    if (year == "2016" && month == "05") { /*do nothing*/ }
                    else if (year == "2017" && month == "01") { /*do nothing*/ }
                    else
                    {
                        var data = new DataPoint()
                        {
                            FullData = dataPoint,
                            Date = match.Groups["Date"].Value,
                            Year = match.Groups["Year"].Value,
                            Month = match.Groups["Month"].Value,
                            Day = match.Groups["Day"].Value,
                            Time = match.Groups["Time"].Value,
                            Temprature = double.Parse(match.Groups["Temprature"].Value, CultureInfo.InvariantCulture),
                            Humidity = double.Parse(match.Groups["Humidity"].Value, CultureInfo.InvariantCulture),
                            Inside = match.Groups["Position"].Value == "Inne" //this is a condition, if it is fulfilled then it becomes true thus giving the bool the correct value
                        };
                        AllDataPoints.Add(data);
                    }
                }
            }

            return AllDataPoints;
        }

        public static List<Day> GetDays() 
        {
           
            List<DataPoint> AllDataPoints = GetAllWeatherDataPoints(ReadAllWeatherData());
            var dayDataList = AllDataPoints.GroupBy(dp => dp.Date);

            List<Day> days = new();
            foreach (var dayData in dayDataList) 
            {
                List<DataPoint> dataPoints = dayData.ToList();
                string date = dataPoints.Select(dp => dp.Date).First();
                string month = dataPoints.Select(dp => dp.Month).First();
                days.Add(new Day ( date, month, dataPoints));
            }
            return days;
        }
    }
}
