using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Extensions
{
    public static class DateConverter
    {
        //مبدل تاریخ میلادی به شمسی با قالب تاریخ - زمان
        public static string ToShamsi(this DateTime value)
        {
            PersianCalendar pc = new();
            return pc.GetYear(value) + "/" + pc.GetMonth(value).ToString("00") + "/" +
                   pc.GetDayOfMonth(value).ToString("00") + "\r\n" + pc.GetHour(value).ToString("00") + ":" + pc.GetMinute(value).ToString("00") + ":" + pc.GetSecond(value).ToString("00");
        }

        //محاسبه سابقه
        public static string CalculateHistory(this DateTime value)
        {
            var history = DateTime.Now - value;
            if (history.Days > 365)
            {
                var years = history.Days / 365;
                var remainingDays = history.Days % 365;
                var months = remainingDays / 30;
                var remainingDaysAfterMonths = remainingDays % 30;
                return $" {remainingDaysAfterMonths} روز - {months} ماه - {years} سال  ";
            }
            else if (history.Days > 30)
            {
                var months = history.Days / 30;
                var remainingDays = history.Days % 30;
                return $"{remainingDays} روز - {months} ماه  ";
            }
            else
            {
                return $"{history.Days} روز";
            }

        }

        //محاسبه سابقه ثبت نام در سامانه به روز
        public static string CalculateDailyWorkHistory(this DateTime value)
        {
            DateTime startTime = value;
            DateTime endTime = DateTime.Now;
            float dailyWorkHistory = ((float)(endTime - startTime).TotalHours) / 24;
            double dailyWorkHistoryRounded = Math.Round(dailyWorkHistory, 2);
            return dailyWorkHistoryRounded.ToString();
        }
    }
}
