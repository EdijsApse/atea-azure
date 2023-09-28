using System;

namespace Atea
{
    public class Validator
    {
        public static bool IsValidString(string value)
        {
            return !string.IsNullOrEmpty(value);
        }

        public static bool IsValidDateTime(string value)
        {
            DateTime date;

            DateTime.TryParse(value, out date);

            return date != default;
        }

        public static bool DateIntervalIsValid(DateTime dateFrom, DateTime dateTo)
        {
            return dateFrom < dateTo;
        }
    }
}
