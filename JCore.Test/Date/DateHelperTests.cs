using System;
using JCore.Date;
using NUnit.Framework;

namespace JCore.Tests.Date
{
    [TestFixture]
    public class DateHelperTests
    {

        [Test]
        public void GetPeriod()
        {
            var now = DateTime.Now;
            DateHelper.GetPeriod(Period.Day, null, out var begin, out var end);
            Assert.AreEqual(now.Date,begin.Date);
            Assert.AreEqual(now.Date,end.Date);
            DateHelper.GetPeriod(Period.Week, null, out var begin1, out var end1);
            DateHelper.GetPeriod(Period.Month, null, out var begin2, out var end2);
            DateHelper.GetPeriod(Period.Year, null, out var begin3, out var end3);
            var dt = new DateTime(2018, 10, 1);
            DateHelper.GetPeriod(Period.Day, dt, out var begin4, out var end4);
            DateHelper.GetPeriod(Period.Week, dt, out var begin12, out var end12);
            Assert.AreEqual(dt.Date,begin12.Date);
            Assert.AreEqual(dt.Date.AddDays(6),end12.Date);
            DateHelper.GetPeriod(Period.Month, dt, out var begin22, out var end22);
            Assert.AreEqual(dt.Date,begin22.Date);
            Assert.AreEqual(dt.Date.AddDays(30),end22.Date);
            DateHelper.GetPeriod(Period.Year, dt, out var begin33, out var end33);
            Assert.AreEqual(dt.Date.AddMonths(-9),begin33);
            Assert.AreEqual(dt.Date.AddMonths(2).AddDays(30),end33);
        }
    }
}
