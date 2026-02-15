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
        List<string> meny = new List<string> { "All", "Search", "Humidity", "Mold", "AutumnDay", "WinterDay", "Varmast till kallast", "Report" };

        int selectedIndex = 0;

        while (true)
        {
            Console.CursorVisible = false;
            Printer(selectedIndex, meny);

            var key = Console.ReadKey(true).Key;

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
                        ListAllToMenu(days);
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
                                    Console.WriteLine("Finns inte");
                                    Thread.Sleep(1500);
                                    Console.Clear();
                                    continue;
                                }
                                else
                                {
                                    ListAllToMenu(foundDay);
                                    Console.WriteLine("\nPress any key to go back");
                                    key = Console.ReadKey(true).Key;
                                    break;
                                }
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
                        Console.WriteLine("Type 1 for inside");
                        Console.WriteLine("Type 2 for outside");
                        var input = Console.ReadLine();
                        if (input == "1")
                        {
                            var sortedHumidity = days.OrderBy(d => d.AvgHumidityinside).ToList();
                            Console.WriteLine("Inside:");
                            foreach (var point in sortedHumidity)
                            {
                                Console.WriteLine($"{point.Date} : {point.AvgHumidityinside}");
                            }
                        }

                        if (input == "2")
                        {
                            var sortedHumidity = days.OrderBy(d => d.AvgHumidityOutside).ToList();
                            Console.WriteLine("Outside:");
                            foreach (var point in sortedHumidity)
                            {
                                Console.WriteLine($"{point.Date} : {point.AvgHumidityOutside}");

                            }
                        }
                        Console.WriteLine("Press any key to return...");
                        Console.ReadKey();
                        break;

                    case 3://Mold
                        Console.Clear();
                        Console.WriteLine("Type 1 for inside");
                        Console.WriteLine("Type 2 for outside");
                        input = Console.ReadLine();
                        if (input == "1")
                        {
                            ListAllToMenu(SortMoldInside(days));
                            break;
                        }
                        if (input == "2")
                        {
                            ListAllToMenu(SortMoldOutside(days));
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
                        Console.WriteLine("Type 1 for inside");
                        Console.WriteLine("Type 2 for outside");
                        input = Console.ReadLine();
                        if (input == "1")
                        {
                            Console.WriteLine("Inside varmast:");
                            ListAllToMenu(SortInsideTemp(days));
                            break;
                        }
                        if (input == "2")
                        {
                            Console.WriteLine("Outside varmast:");
                            ListAllToMenu(SortOutsideTemp(days));
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


    private static void ListAllToMenu(List<Day> days)
    {
        Console.Clear();
        foreach (var day in days)
        {
            PrintDay(day, 8);
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
        Console.WriteLine($"{day.Date.PadRight(padding)} TempIn:{day.AvgTempInside.ToString().PadRight(padding)} TempOut:{day.AvgTempOutside.ToString().PadRight(padding)} AvgHumIn: {day.AvgHumidityinside.ToString().PadRight(padding)} AvgHumOut: {day.AvgHumidityOutside.ToString().PadRight(padding)} MoldRiskIn: {day.MouldRiskInside.ToString().PadRight(padding)} MoldRiskOut: {day.MouldRiskOutside.ToString().PadRight(padding)}");
    }
    public static void PrintDay(Day day)
    {
        Console.WriteLine($"{day.Date} TempIn:{day.AvgTempInside} TempOut:{day.AvgTempOutside} AvgHumIn: {day.AvgHumidityinside} AvgHumOut: {day.AvgHumidityOutside} MoldRiskIn: {day.MouldRiskInside} MoldRiskOut: {day.MouldRiskOutside}");
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
    private static List<Day> SortMoldOutside(List<Day> days)
    {
        List<Day> sortedDays = days.OrderBy(d => d.MouldRiskOutside).ToList();
        return sortedDays;
    }
}
