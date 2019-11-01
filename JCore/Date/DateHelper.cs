using System;

namespace JCore.Date
{
    public enum Period { Day, Week, Month, Year }

    public class DateHelper
    {
        public static void GetPeriod(Period period, DateTime? specialDate, out DateTime beginDate, out DateTime endDate)
        {
            var datetime = specialDate == null ? DateTime.Today : Convert.ToDateTime(specialDate);
            var year = DateTime.Today.Year;
            var month = DateTime.Today.Month;
            switch (period)
            {
                case Period.Day:
                    beginDate = datetime;
                    endDate = datetime;
                    break;
                case Period.Week:
                    var week =(int)datetime.DayOfWeek;
                    if (week == 0)
                    {
                        week = 7;
                    }
                    beginDate = datetime.AddDays(-(week - 1));
                    endDate = beginDate.AddDays(6);
                    break;
                case Period.Month:
                    beginDate=new DateTime(year,month,1);
                    endDate = beginDate.AddMonths(1).AddDays(-1);
                    break;
                case Period.Year:
                    beginDate=new DateTime(year,1,1);
                    endDate=new DateTime(year,12,31);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(period), period, null);
            }
        }
    }
}
