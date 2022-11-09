using System;


namespace VZ.MoneyFlow.Models.Extensions
{
    public static class DateTimeConverter
    {
        public static DateTime ConvertToDateTime(long utcExpiryTime)
        {
            var dateTimeVal = DateTime.UnixEpoch;
            dateTimeVal = dateTimeVal.AddSeconds(utcExpiryTime).ToUniversalTime();
            return dateTimeVal;
        }
    }
}
