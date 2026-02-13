using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherData_Group1.Models;

namespace WeatherData_Group1.Services
{
    public static class DataSearch
    {
        public static Day SearchDayByDate(List<Day> days, string searchDay )
        {
            var day = days.Where(d => d.Date.Contains(searchDay, StringComparison.OrdinalIgnoreCase)).FirstOrDefault(); //StringComparison.OrdinalIgnoreCase = ignores case sensitivity in the search
            return day;
        }

        public static List<Day> SearchDaysByDates(List<Day> days, string searchMonth, string searchDay)
        {

            return null;   
        }


    }
}
