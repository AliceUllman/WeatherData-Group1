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
            using (StreamWriter streamWriter = new StreamWriter(path + fileName, true))//true är append och lägger på text, utan true skriver den över text filens innehåll
            {
                streamWriter.WriteLine(text);
            }
        }

        public static void TestingRegEx(List<string> dataList)
        {
            Regex regex = new Regex(@"(?<Date>^(?<Year>\d{4})-(?<Month>0[1-9]|1[0-2])-(?<Day>0[1-9]|1[0 - 9]|2[0-9]|3[0-1])) (?<Time>\d{2}:\d{2}:\d{2}),(?<Position>Inne|Ute),(?<Temprature>\-?\d+\.?\d*),(?<Humidity>\d{2})");
            for  (int i = 0; i < 10; i++ )
            {
                Match match = regex.Match(dataList[i]);

                string date = match.Groups["Date"].Value;
                string position = match.Groups["Position"].Value;
                string temprature = match.Groups["Temprature"].Value;
                string humidity = match.Groups["Humidity"].Value;

                if (match.Success)
                {
                    
                    Console.WriteLine($"{dataList[i]} yes");
                    Console.WriteLine($"datum: {date}");
                    Console.WriteLine($"position: {position}");
                    Console.WriteLine($"temprature: {temprature}");
                    Console.WriteLine($"humidity: {humidity}");
                    
                }
                else
                {
                    Console.WriteLine($"{dataList[i]} no");
                    Console.WriteLine($"datum: {date}");
                    Console.WriteLine($"position: {position}");
                    Console.WriteLine($"temprature: {temprature}");
                    Console.WriteLine($"humidity: {humidity}");
                }
            }
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
