using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrewQuestScraper
{
    public static class DateParsing
    {
        public static bool IsInternationalDateFormat(string[] dateStrings)
        {
            bool result = false;
            foreach (var dateString in dateStrings)
            {
                GetDateTimeFromString(dateString, out result);
                if (result)
                {
                    return true;
                }
                else
                {
                    continue;
                }
            }
            return result;
        }

        public static DateTime? GetDateTimeFromString(string dateString, bool isInternational)
        {
            if (isInternational)
            {
                return ParseInternationalDateString(dateString);
            }
            else
            {
                return GetDateTimeFromString(dateString);
            }
        }
        public static DateTime? GetDateTimeFromString(string dateString)
        {
            bool unused = false;
            return GetDateTimeFromString(dateString, out unused);
        }
        public static DateTime? GetDateTimeFromString(string dateString, out bool isInternationalDateFormat)
        {
            DateTime date = DateTime.MinValue;
            isInternationalDateFormat = false;

            if (DateTime.TryParse(dateString, out date))
            {
                return date;
            }
            else
            {
                var internationalDate = ParseInternationalDateString(dateString);
                isInternationalDateFormat = internationalDate != null;
                return internationalDate;
            }
        }

        public static DateTime? ParseInternationalDateString(string dateString)
        {
            try
            {
                // try diff format
                DateTime date = DateTime.ParseExact(dateString, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                return date;
            }
            catch
            {
                Console.WriteLine("Error parsing date string : " + dateString);
            }
            return null;
        }
    }
}
