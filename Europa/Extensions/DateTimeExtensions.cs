using System;

namespace Europa.Extensions
{
    public static class DateTimeExtensions
    {
        private static readonly string PatternDateTimeSeconds = "dd/MM/yyyy HH:mm:ss";
        private static readonly string PatternDateTime = "dd/MM/yyyy HH:mm";
        private static readonly string PatternDate = "dd/MM/yyyy";
        private static readonly string PatternTimeSeconds = "HH:mm:ss";
        private static readonly string PatternTime = "HH:mm";

        public static bool IsValid(this DateTime? date)
        {
            if (date == null)
            {
                return false;
            }
            if (date.Value == DateTime.MinValue)
            {
                return false;
            }

            return true;
        }

        public static DateTime AbsoluteStart(this DateTime dateTime)
        {
            return dateTime.Date;
        }

        public static DateTime AbsoluteEnd(this DateTime dateTime)
        {
            return AbsoluteStart(dateTime).AddDays(1).AddTicks(-1);
        }

        public static bool IsUsefulDay(this DateTime date)
        {
            return !IsWeekEnd(date);
        }

        public static bool IsWeekEnd(this DateTime date)
        {
            return date.DayOfWeek == DayOfWeek.Saturday
                || date.DayOfWeek == DayOfWeek.Sunday;
        }

        public static string ToDateTimeSeconds(this DateTime date)
        {
            return date.ToString(PatternDateTimeSeconds);
        }

        public static string ToDateTime(this DateTime date)
        {
            return date.ToString(PatternDateTime);
        }

        public static string ToDate(this DateTime date)
        {
            return date.ToString(PatternDate);
        }

        public static string ToTimeSeconds(this DateTime date)
        {
            return date.ToString(PatternTimeSeconds);
        }

        public static string ToTime(this DateTime date)
        {
            return date.ToString(PatternTime);
        }
    }
}
