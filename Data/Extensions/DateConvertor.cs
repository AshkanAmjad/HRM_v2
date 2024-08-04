using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Extensions
{
    public static class DateConvertor
    {
        //مبدل تاریخ میلادی به شمسی با قالب تاریخ - زمان
        public static string ToShamsi(this DateTime value)
        {
            PersianCalendar pc = new();
            return pc.GetYear(value) + "/" + pc.GetMonth(value).ToString("00") + "/" +
                   pc.GetDayOfMonth(value).ToString("00") + "\r\n" + pc.GetHour(value).ToString("00") + ":" + pc.GetMinute(value).ToString("00") + ":" + pc.GetSecond(value).ToString("00");
        }
    }
}
