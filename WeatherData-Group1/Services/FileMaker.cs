using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherData_Group1.Services
{
    internal class FileMaker
    {
        public static void CreateFile(string fileName)
        {
            var days = DataExtract.GetDays();
            var months = days.GroupBy(d => d.Month);

            string path = @"..\..\..\TxtFiles\" + fileName;

            try
            {
                using (StreamWriter writer = new StreamWriter(path))
                {
                    foreach (var m in months)
                    {
                        int daycounter = 0;
                        double insidetemps = 0;
                        double outsidetemps = 0;
                        double humidityinside = 0;
                        double humidityoutside = 0;
                        double moldriskinside = 0;
                        double moldriskoutside = 0;

                        foreach (var d in days)
                        {
                            if (d.Month == m.Key)
                            {
                                daycounter++;

                                insidetemps += d.AvgTempInside;
                                outsidetemps += d.AvgTempOutside;

                                humidityinside += d.AvgHumidityinside;
                                humidityoutside += d.AvgHumidityOutside;

                                moldriskinside += d.MouldRiskInside;
                                moldriskoutside += d.MouldRiskOutside;
                            }
                        }

                        insidetemps = Math.Round(insidetemps / daycounter, 2);
                        outsidetemps = Math.Round(outsidetemps / daycounter, 2);
                        humidityoutside = Math.Round(humidityoutside / daycounter, 2);
                        humidityinside = Math.Round(humidityinside / daycounter, 2);
                        moldriskinside = Math.Round(moldriskinside / daycounter, 2); //
                        moldriskoutside = Math.Round(moldriskoutside / daycounter, 2); // Får se om den här delen håller lol

                        writer.WriteLine($"2016/{m.Key} Averages:    InTemp: {insidetemps}°C OutTemp: {outsidetemps} InHum: {humidityinside} OutHum: {humidityoutside} MoldIn: {moldriskinside} MoldOut: {moldriskoutside}");
                        
                    }
                    writer.WriteLine();
                    writer.WriteLine("Meteoroloogisk vinter: ");
                    writer.WriteLine("Meteoroilgiisk höst: 2016-10-04 ");
                    writer.WriteLine();
                    writer.WriteLine();
                    writer.WriteLine("Mögelformula pff");
                    writer.WriteLine();
                    writer.WriteLine("MögelAlgoritm: ");
                    writer.WriteLine("Input temperature T and humidity H\r\n\r\nIf T < 0 or T > 50 → tempFactor = 0\r\nElse if T ≤ 25 → tempFactor = T / 25\r\nElse → tempFactor = (50 − T) / (50 − 25)\r\nIf tempFactor < 0 → tempFactor = 0\r\n\r\nIf H < 70 → humidityFactor = 0\r\nElse → humidityFactor = H / 90\r\nIf humidityFactor > 1 → humidityFactor = 1\r\n\r\nrisk = tempFactor × humidityFactor × 100");

                }
                Console.WriteLine("Filen har skapats.");
                Console.WriteLine();


                DataExtract.ReadAll(fileName);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ett fel inträffade vid skapandet av filen: " + ex.Message);
            }
        }
    }
}
