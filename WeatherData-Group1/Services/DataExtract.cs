using System;
using System.Collections.Generic;
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

        public static void TestingRegEx(List<string> dataList)
        {
            Regex regex = new Regex(@"(?<Date>^(?<Year>\d{4})-(?<Month>0[1-9]|1[0-2])-(?<Day>0[1-9]|1[0-9]|2[0-9]|3[0-1])) (?<Time>\d{2}:\d{2}:\d{2}),(?<Position>Inne|Ute),(?<Temprature>\-?[1-9]?[0-9]\.[1-9]?[0-9]),(?<Humidity>\d{2})");
            
            Regex regex2 = new Regex(@"^(?<Year>\d{4})-(?<Month>06)-(?<Day>01)");


            List<string> AllDataPoints = new();
            foreach (var dataPoint in dataList) 
            {
                Match match = regex.Match(dataPoint);

                string date = match.Groups["Date"].Value;
                string year = match.Groups["Year"].Value;
                string month = match.Groups["Month"].Value;
                string time = match.Groups["Time"].Value;
                string position = match.Groups["Position"].Value;
                string temprature = match.Groups["Temprature"].Value;
                string humidity = match.Groups["Humidity"].Value;
                regex.GroupBy(d = d.Match(dataPoint).Groups["Date"].Value).ToList;
                if (match.Success)
                {
                        
                    if (year == "2016" && month == "05") { }
                    else if (year == "2017" && month == "01"){ }
                    else
                    {
                        //var datapoint = new Day
                        //{
                        //    Date = date,
                        //    Month = month,
                        //    Time = time,
                        //    Position = position
                        //    Temprature = temprature
                        //    Humidity - humidity
                        //};

                        regex.GroupBy
                        AllDataPoints.Add(dataPoint);
                        
                    }
                }
            }

            AllDataPoints.Where(d => d).Include(d => d).ToList;

            for  (int i = 0; i < dataList.Count; i++)//2016-06-01
            {

                Match match = regex.Match(dataList[i]);//2016 05 och 2017 01

                string date = match.Groups["Date"].Value;
                string year = match.Groups["Year"].Value;
                string month = match.Groups["Month"].Value;
                string time = match.Groups["Time"].Value;
                string position = match.Groups["Position"].Value;
                string temprature = match.Groups["Temprature"].Value;
                string humidity = match.Groups["Humidity"].Value;

                if (match.Success)
                {
                    //(year != "2016" && month != "05") || (year != "2017" && month != "01")
                    if (year == "2016" && month == "05" )
                    {
                        //Console.WriteLine($"{dataList[i]} Wrong date 1");
                    }
                    else if (year == "2017" && month == "01")
                    {
                        //Console.WriteLine($"{dataList[i]} Wrong date 2");
                    }
                    else
                    {

                        
                        Console.WriteLine($"{dataList[i]} yes");
                        //Console.WriteLine($"Time: {time}");
                        //Console.WriteLine($"datum: {date}");
                        //Console.WriteLine($"position: {position}");
                        //Console.WriteLine($"temprature: {temprature}");
                        //Console.WriteLine($"humidity: {humidity}");
                    }


                    
                }
                else
                {
                    Console.WriteLine($"{dataList[i]} no");      
                }
            }
            //test.Where(d => d).Include(d => d).ToList;
        }
        public static void mikeRegEx() 
        {
            string[] times = { "11:33:24", "33:12:11", "29:10:20", "blabla 23:52:34", "00:13:23 PM", "09:75:13" };

            Regex regex = new Regex("^(?<hour>[0-2][0-9]):([0-5][0-9]):([0-5][0-9])$");

            foreach (var time in times)
            {
                Match match = regex.Match(time);

                if (match.Success)
                {
                    int hour = int.Parse(match.Groups["hour"].Value);

                    if (hour < 24)
                    {
                        Console.WriteLine(time + " - Helt korrekt format");
                    }
                    else
                    {
                        Console.WriteLine(time + " - Matchar mönstret, men har fel värde");
                    }
                }
                else
                {
                    Console.WriteLine(time + " - Matchar inte alls!");
                }
            }
        }
    }
}
