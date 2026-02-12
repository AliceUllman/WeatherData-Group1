using WeatherData_Group1.Models;
using WeatherData_Group1.Services;

namespace WeatherData_Group1;

internal class Program
{
    static void Main(string[] args)
    {

        //testing
        //Console.WriteLine("Hello, Testing!");
        //Console.WriteLine("greg");
        //Console.WriteLine("hello");

        //string txt = "grag";
        //string path = @"TextFileGreg.txt";
        // (?<Date>^(?<Year>\d{4})-(?<Month>0[1-9]|1[0-2])-(?<Day>0[1-9]|1[0-9]|2[0-9]|3[0-1])) (?<Time>\d{2}:\d{2}:\d{2}),(?<Position>Inne|Ute),(?<Temprature>\-?\d+\.?\d*),(?<Humidity>\d{2})
        //DataExtract.WriteRow(path, txt);
        //DataExtract.ReadAll(path);

        //DataExtract.PrintDayAndAvgTemp();

        //var sortedDays = SortInsideTemp(DataExtract.GetDays());

        //foreach (var day in sortedDays) 
        //{
        //    Console.WriteLine($"{day.Date} Inside:{day.AvgTempInside:F2} Outside:{day.AvgTempOutside:F2}");
        //}

        List<string> meny = new List<string>{ "All", "Search", "Humidity", "Mold", "AutumnDay", "WinterDay", "Report" };
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
                        break;
                    case 1: //Search
                        break;

                }
            }

            //Console.Clear();
            Console.SetCursorPosition(0, 0);
        }
    }

    public static List<Day> SortInsideTemp(List<Day> days)
    {
        List<Day> sortedDays = days.OrderBy(d => d.AvgTempInside).ToList();
        return sortedDays;
    }

    public static void Printer(int index, List<string> text) 
    { 
        for (int i = 0; i< text.Count(); i++ )
        {
            if(index == i)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($" {text[i]} ");
                Console.ResetColor();
            }
            else Console.WriteLine($" {text[i]} ");
        }
        Console.WriteLine(index);
    }
    public void MenyGo() 
    {
        while (true)
        {
            Console.WriteLine("Weather data app");

            Console.WriteLine("[A]ll | [S]earch | sort by [H]umidity | sort by [M]old risk | [F] Meteorological autumn | [R]eport");
            var key = Console.ReadKey(true).Key;
            switch (key)
            {
                case ConsoleKey.A:
                    Console.Clear();
                    //ListAllToMenu(SortByTemp(Lista));
                    break;
                case ConsoleKey.H:
                    Console.Clear();
                    Console.WriteLine("Sorted by humidity");
                    //ListAllToMenu(SortByHum(Lista));
                    break;
                case ConsoleKey.M:
                    Console.Clear();
                    Console.WriteLine("Sorted by Mold risk");
                    break;
                //ListAllToMenu(SortByMoldRisk(Lista));
                case ConsoleKey.S:
                    Console.Clear();
                    //Search(Lista);
                    break;
                case ConsoleKey.F:
                    Console.Clear();
                    Console.WriteLine("Meteorological autumn date");
                    //var fallday =
                    break;
                case ConsoleKey.W:
                    Console.Clear();
                    Console.WriteLine("Meteorological Winter date");
                    //var winterday =
                    Console.WriteLine($"(Winterday) {"Average temperature: " + "Winterday.avgtemp"}");
                    break;
            }
        }
    }

    //private static void ListAllToMenu(Dictionary<string, Day> dictionary) { };
}
        

    



//Skapa klass för att läsa och skriva filer
//

//Uppgift - Väderdata
//◦ Projektet är en applikation som, utifrån befintlig och autentisk
//temperatur - och luftfuktighetsdata kan söka, sortera
//och dra slutsatser.
//◦ I det här projektet jobbar vi i grupperna.
//◦ Datafil i textformat: tempdata5 - med fel.txt
//  ◦ Datafilen är autentisk, och har datafel, och luckor.
//  ◦ Innehåller 304 480 mätningar
//  ◦ Datum mellan 2016 - 05 - 31 till 2017 - 01 - 10
//      ◦ Ignorera data för maj 2016 och jan 2017(med kod) 2016 05 och 2017 01
//  ◦ Yttersensorns placering är inte optimal(solen)

//Följande information ska kunna visas

//Utomhus:
//◦ Medeltemperatur och luftfuktighet per dag, för valt datum(sökmöjlighet med
//validering)
//◦ Sortering av varmast till kallaste dagen enligt medeltemperatur per dag
//◦ Sortering av torrast till fuktigaste dagen enligt medelluftfuktighet per dag
//◦ Sortering av minst till störst risk av mögel
//◦ Datum för meteorologisk Höst
//◦ Datum för meteologisk vinter(OBS Mild vinter!)

//Inomhus
//◦ Medeltemperatur för valt datum(sökmöjlighet med validering)
//◦ Sortering av varmast till kallaste dagen enligt medeltemperatur per dag
//◦ Sortering av torrast till fuktigaste dagen enligt medelluftfuktighet per dag
//◦ Sortering av minst till störst risk av mögel

//Textfil som ska skapas
//◦ En textfil ska skapas som innehåller följande information.
//◦ Medeltemperatur ute och inne, per månad
//◦ Medelluftfuktighet inne och ute, per månad
//◦ Medelmögelrisk inne och ute, per månad.
//◦ Datum för höst och vinter 2016(om något av detta inte inträffar, ange när det var som
//närmast)
//◦ Skriv också ut algoritmen för mögel

//Dataregler
//◦ Meterologisk höst:
//◦ Hösten anländer om det råder hösttemperatur fem dygn i följd. Hösttemperatur är det då
//dygnsmedeltemperaturen är under 10,0°C, men ännu inte fem dygn i följd.Höstens
//ankomstdatum avser det första dygnet av fem med hösttemperaturer. Den 1 augusti har satts
//som det tidigaste tillåtna datumet för höstens ankomst.Senast möjliga datum för höstens
//ankomst är den 14 februari.
//◦ Meterologisk vinter
//◦ Vintern anländer om det råder vintertemperatur fem dygn i följd. Vintertemperatur är det då
//dygnsmedeltemperaturen är 0,0°C eller lägre, men ännu inte fem dygn i följd.Vinterns
//ankomstdatum avser det första dygnet av fem med vintertemperaturer.
//https://www.smhi.se/kunskapsbanken/meteorologi/arstider/vinter
//https://www.smhi.se/kunskapsbanken/meteorologi/arstider/host

//Algoritm -
//Mögel
//◦ I uppgiften ingår att skapa en
//algoritm / formel för mögelrisk.
//◦ Diskutera I Gruppen
//◦ Försök förenkla detta till en hanterbar
//formel, som fungerar för alla data I filen.
//◦ Skapa en skala för risk mellan 0 - 100
//https://www.penthon.com/vanliga-fragor/faq/vad-innebar-mogelindex/

//Teknik
//◦ I den här uppgiften ska ni använda bl a följande Teknik:
//◦ LINQ
//◦ RegEx
//◦ Läsa och skriva filer
//◦ +allt annat vi lärt oss
