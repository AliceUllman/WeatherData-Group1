using System.Collections.Generic;
using System.Text.RegularExpressions;
using WeatherData_Group1.Models;
using WeatherData_Group1.Services;

namespace WeatherData_Group1;

internal class Program
{
    static void Main(string[] args)
    {

        List<Day> days = DataExtract.GetDays(); //lista med alla dagar skapas 

        MenyLoop(days);

    }



    public static void MenyLoop(List<Day> days)
    {
        List<string> meny = new List<string> { "All", "Search", "Sort by Humidity", "Sort by MoldRisk", "First AutumnDay", "First WinterDay", "Sort by Temprature", "Report" };

        int selectedIndex = 0;
        int padding = 12;
        while (true)
        {
            Console.CursorVisible = false;
            Printer(selectedIndex, meny);
            ConsoleKey key;
            
            key = Console.ReadKey(true).Key;

            if (key == ConsoleKey.DownArrow)
            {
                selectedIndex = (selectedIndex + 1) % meny.Count;
            }
            else if (key == ConsoleKey.UpArrow)
            {
                selectedIndex = (selectedIndex - 1 + meny.Count) % meny.Count;
            }
            else if (key == ConsoleKey.Enter)
            {
                switch (selectedIndex)
                {
                    case 0: //All
                        SortByDate(days);
                        PrintAllDays(days, padding);
                        break;
                    case 1: //Search
                        Console.Clear();

                        while (true)
                        {
                           
                            Console.WriteLine("Search for a date, day first month after, use this format 00-00 ");
                            Console.Write("Enter a date: ");
                            string inputDate = Console.ReadLine();
                            Console.Clear();
                            if (DateValidation(inputDate))
                            {
                                Day foundDay = DataSearch.SearchDayByDate(days, inputDate);
                                if (foundDay == null)
                                {
                                    Console.WriteLine("Day does not exist");
                                    Thread.Sleep(1500);
                                    Console.Clear();
                                    continue;
                                }
                                else
                                {
                                    PrintOneDay(foundDay, padding);
                                    break;
                                }
                                Console.WriteLine("Press any key to return...");
                                Console.ReadKey();
                            }
                            else
                            {
                                Console.WriteLine("Incorrect try again");
                                Thread.Sleep(3000);
                            }
                        }
                        break;

                    case 2: //Humidity
                        Console.Clear();
                        Console.WriteLine("Write I for inside");
                        Console.WriteLine("Write O for outside");
                        
                        key = Console.ReadKey().Key;
                        Console.Clear();
                        if (key == ConsoleKey.I)
                        {
                            Console.WriteLine("Inside:");
                            PrintAllDays(SortHumInside(days), padding);
                        }
                        else if (key == ConsoleKey.O)
                        {
                            days = SortHumOutside(days);
                            
                            Console.WriteLine("Outside:");
                            PrintAllDays(days, padding);
                        }
                        Console.WriteLine("Press any key to return...");
                        Console.ReadKey();

                        break;

                    case 3://Mold
                        Console.Clear();
                        Console.WriteLine("Press key I for inside");
                        Console.WriteLine("Press key O for outside");

                        key = Console.ReadKey().Key;
                        
                        if (key == ConsoleKey.I)
                        {
                            PrintAllDays(SortMoldInside(days), padding);
                            break;
                        }
                        else if (key == ConsoleKey.O)
                        {
                            SortMoldOutside(days);
                            PrintAllDays(days, padding);
                            break;
                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine("Bruh du hade 1 jobb...");
                            Thread.Sleep(2000);
                        }
                        break;
                    case 4: //AutumnDay
                        {
                            Console.Clear();
                            var sortedAutumnDays = days.OrderBy(d => d.Date).ToList();
                            int autumnCount = 0;
                            for (int i = 0; i < sortedAutumnDays.Count; i++)
                            {
                                if (sortedAutumnDays[i].AvgTempOutside < 10)
                                {
                                    autumnCount++; //fortsätta 
                                }
                                else
                                {
                                    autumnCount = 0; // börja om
                                }

                                if (autumnCount == 5)
                                {
                                    Console.WriteLine("Autumn started on: " + sortedAutumnDays[i - 4].Date);
                                    break;
                                }
                                else if (i == sortedAutumnDays.Count)
                                {
                                    Console.WriteLine("Autumn never happend");
                                }

                            }
                            Console.WriteLine("Press any key to return...");
                            Console.ReadKey();
                            break;
                        }

                    case 5:// WinterDay
                        {
                            Console.Clear();
                            var sortedWinter = days.OrderBy(d => d.Date).ToList();

                            Day officialWinterDay = null; // Startdag för meteorologisk vinter (5 dagar i rad)
                            Day bestStartDay = null;      // Startdag för längsta kalla perioden
                            int currentStreak = 0;
                            int maxStreak = 0;

                            for (int i = 0; i < sortedWinter.Count; i++)
                            {
                                if (sortedWinter[i].AvgTempOutside <= 0)
                                {
                                    currentStreak++; 

                                    if (currentStreak > maxStreak)// Håll koll på den längsta perioden 
                                    {
                                        maxStreak = currentStreak;
                                        bestStartDay = sortedWinter[i - currentStreak + 1]; // Spara startdagen för denna streak
                                    }

                                    // Spara startdatumet när vi når 5 dagar i streak
                                    if (currentStreak == 5 && officialWinterDay == null)
                                    {
                                        officialWinterDay = sortedWinter[i - 4];
                                    }
                                }
                                else
                                {
                                    currentStreak = 0; // Återställ räknaren om temp > 0
                                }
                            }

                            // Output
                            if (officialWinterDay != null)
                            {
                                Console.WriteLine($"Winter started on: {officialWinterDay.Date}");
                            }
                            else
                            {
                                Console.WriteLine("Winter was NOT found.");
                                if (bestStartDay != null)
                                {
                                    Console.WriteLine($"The longest cold period was {maxStreak} consecutive days.");
                                    Console.WriteLine($"It started on: {bestStartDay.Date}");
                                }
                            }

                            Console.WriteLine("Press any key to return...");
                            Console.ReadKey();
                            break;
                        }
                    case 6: //varmast till kallast
                        Console.Clear();
                        Console.WriteLine("Press key I for inside");
                        Console.WriteLine("Press key O for outside");

                        key = Console.ReadKey().Key;

                        if (key == ConsoleKey.I)
                        {
                            Console.WriteLine("Inside Warmest day to coldest:");
                            PrintAllDays(SortInsideTemp(days), padding);
                            break;
                        }
                        if (key == ConsoleKey.O)
                        {
                            Console.WriteLine("Outside Warmest day to coldest:");
                            PrintAllDays(SortOutsideTemp(days), padding);
                            break;
                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine("Bruh du hade 1 jobb...");
                            Thread.Sleep(2000);
                        }
                        break;
                    case 7: // Skriv ut Textfil medeltemp månad, medelfukt månad, medelmögel månad, datum höst och vinter, skriv ut formel mögel.
                        Console.Clear();
                        FileMaker.CreateFile("Report.txt");
                        Console.ReadKey();
                        break;
                }
                key = ConsoleKey.NoName;
            }
            Console.Clear();
        }
    }
    

    public static bool DateValidation(string validateThis)
    {
        Regex regex = new Regex(@"(0[1-9]|1[0-2])-(0[1-9]|[1-2][0-9]|3[0-1])");
        Match match = regex.Match(validateThis);

        if (match.Success)
        {
            return true;
        }
        else return false;
       
    }

    public static void Printer(int index, List<string> text)
    {
        for (int i = 0; i < text.Count(); i++)
        {
            if (index == i)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($" {text[i]} ");
                Console.ResetColor();
            }
            else Console.WriteLine($" {text[i]} ");
        }
        Console.WriteLine(index);
    }


    private static void PrintAllDays(List<Day> days, int padding)
    {
        Console.Clear();
        Console.WriteLine($"{"Date".PadRight(padding)} | {"avg Temp ".PadRight(padding)} | {"avg Temp ".PadRight(padding)} | {"avg Humidity".PadRight(padding)} | {"avg Humidity".PadRight(padding)} | {"avg MoldRisk".PadRight(padding)} | {"avg MoldRisk".PadRight(padding)} | ");
        Console.WriteLine($"{"Date".PadRight(padding)} | {"Inside".PadRight(padding)} | {"Outside".PadRight(padding)} | {"Inside".PadRight(padding)} | {"Outside".PadRight(padding)} | {"Inside".PadRight(padding)} | {"Outside".PadRight(padding)} | ");
        Console.WriteLine($"{new String('-', padding)} | {new String('-', padding)} | {new String('-', padding)} | {new String('-', padding)} | {new String('-', padding)} | {new String('-', padding)} | {new String('-', padding)} | ");
        
        foreach (var day in days)
        {
            PrintDay(day, padding);
        }
        
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();

    }

    public static void ListAllToMenu(Day day)
    {
        Console.Clear();
        if (day != null)
        {
            PrintDay(day, 10);
        }
        else
        {
            Console.WriteLine("error 404: day not found");
            Console.WriteLine("Press any key to go back");
        }
        Console.ReadKey();
    }


    public static void PrintDay(Day day, int padding)
    {     
        Console.WriteLine($"{day.Date.PadRight(padding)} | {day.AvgTempInside.ToString().PadRight(padding)} | {day.AvgTempOutside.ToString().PadRight(padding)} | {day.AvgHumidityinside.ToString().PadRight(padding)} | {day.AvgHumidityOutside.ToString().PadRight(padding)} | {$"{day.MouldRiskInside.ToString()}%".PadRight(padding)} | {$"{day.MouldRiskOutside.ToString()}%".PadRight(padding)} | ");
    }
    public static void PrintOneDay(Day day, int padding)
    {
        Console.WriteLine($"{"Date".PadRight(padding)} | {"avg Temp ".PadRight(padding)} | {"avg Temp ".PadRight(padding)} | {"avg Humidity".PadRight(padding)} | {"avg Humidity".PadRight(padding)} | {"avg MoldRisk".PadRight(padding)} | {"avg MoldRisk".PadRight(padding)} | ");
        Console.WriteLine($"{"Date".PadRight(padding)} | {"Inside".PadRight(padding)} | {"Outside".PadRight(padding)} | {"Inside".PadRight(padding)} | {"Outside".PadRight(padding)} | {"Inside".PadRight(padding)} | {"Outside".PadRight(padding)} | ");
        Console.WriteLine($"{new String('-', padding)} | {new String('-', padding)} | {new String('-', padding)} | {new String('-', padding)} | {new String('-', padding)} | {new String('-', padding)} | {new String('-', padding)} | ");

        Console.WriteLine($"{day.Date.PadRight(padding)} | {day.AvgTempInside.ToString().PadRight(padding)} | {day.AvgTempOutside.ToString().PadRight(padding)} | {day.AvgHumidityinside.ToString().PadRight(padding)} | {day.AvgHumidityOutside.ToString().PadRight(padding)} | {$"{day.MouldRiskInside.ToString()}%".PadRight(padding)} | {$"{day.MouldRiskOutside.ToString()}%".PadRight(padding)} | ");
    }

    public static void SortByDate(List<Day> days)
    {
        days.OrderByDescending(d => d.Date);
        
    }

    public static List<Day> SortInsideTemp(List<Day> days)
    {
        List<Day> sortedDays = days.OrderByDescending(d => d.AvgTempInside).ToList();
        return sortedDays;
    }
    private static List<Day> SortOutsideTemp(List<Day> days)
    {
        List<Day> sortedDays = days.OrderByDescending(d => d.AvgTempOutside).ToList();
        return sortedDays;
    }
    private static List<Day> SortHumInside(List<Day> days)
    {
        List<Day> sortedDays = days.OrderBy(d => d.AvgHumidityinside).ToList();
        return sortedDays;
    }
    private static List<Day> SortHumOutside(List<Day> days)
    {
        List<Day> sortedDays = days.OrderBy(d => d.AvgHumidityOutside).ToList();
        return sortedDays;
    }
    private static List<Day> SortMoldInside(List<Day> days)
    {
        List<Day> sortedDays = days.OrderBy(d => d.MouldRiskInside).ToList();
        return sortedDays;
    }
    private static void SortMoldOutside(List<Day> days)
    {
        days.OrderBy(d => d.MouldRiskOutside).ToList();
    }
}
