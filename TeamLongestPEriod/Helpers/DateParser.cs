using System;
using System.Globalization;

namespace TeamLongestPEriod.Helpers
{
    public class DateParser
    {
        public static DateTime? ParseDateString(string dateString)
        {
            if (string.IsNullOrWhiteSpace(dateString))
                return null;

            if (DateTime.TryParse(dateString, CultureInfo.InvariantCulture,
                                  DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal,
                                  out DateTime result))
            {
                return result;
            }
            else
            {
                return ParseExact(dateString);
            }
        }

        public static DateTime? ParseExact(string dateString)
        {
            if (string.IsNullOrWhiteSpace(dateString))
                return null;

            // Most popular formats
            string[] formats = new[]
            {
                "yyyy-MM-dd",
                "dd/MM/yyyy",
                "MM/dd/yyyy",
                "dd-MM-yyyy",
                "MM-dd-yyyy",
                "yyyy/MM/dd",
                "yyyyMMdd",
                "dd MMM yyyy",
                "dd MMMM yyyy",
                "MMM dd, yyyy",
                "MMMM dd, yyyy",
                "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'",
                "yyyy-MM-ddTHH:mm:ss",
                "yyyy-MM-ddTHH:mm:ssZ"
            };

            if (DateTime.TryParseExact(dateString, formats, CultureInfo.InvariantCulture,
                                       DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal, out DateTime result))
            {
                return result;
            }

            if (DateTime.TryParse(dateString, out result))
            {
                return result;
            }

            return null;
        }
    }
}