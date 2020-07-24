using System;
using DecimalMath;

namespace ChrisKaczor.HomeMonitor.Weather.Service
{
    public static class Extensions
    {
        public static bool IsBetween<T>(this T item, T start, T end) where T : IComparable
        {
            return item.CompareTo(start) >= 0 && item.CompareTo(end) <= 0;
        }

        public static decimal Truncate(this decimal value, int decimalPlaces)
        {
            return decimal.Truncate(value * DecimalEx.Pow(10, decimalPlaces)) / DecimalEx.Pow(10, decimalPlaces);
        }
    }
}
