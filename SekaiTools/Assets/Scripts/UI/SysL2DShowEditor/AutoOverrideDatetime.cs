using SekaiTools.SystemLive2D;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SekaiTools.UI.SysL2DShowEditor
{
    public static class AutoOverrideDatetime
    {
        public class SingleDate
        {
            public int month;
            public int day;
            public string name;

            public SingleDate(int month, int day, string name)
            {
                this.month = month;
                this.day = day;
                this.name = name;
            }
        }

        public class DateRange
        {
            public int startMonth;
            public int startDay;
            public int endMonth;
            public int endDay;
            public string name;

            public DateRange(int startMonth, int startDay, int endMonth, int endDay, string name)
            {
                this.startMonth = startMonth;
                this.startDay = startDay;
                this.endMonth = endMonth;
                this.endDay = endDay;
                this.name = name;
            }
        }

        public readonly static SingleDate[] holidays = new SingleDate[]
        {
            new SingleDate(2,14,"情人节"),
            new SingleDate(3,14,"白色情人节"),
            new SingleDate(4,1,"愚人节"),
            new SingleDate(7,7,"七夕"),
            new SingleDate(10,31,"万圣节"),
            new SingleDate(12,25,"圣诞节")
        };

        public readonly static DateRange[] longHolidays = new DateRange[]
            {
                new DateRange(12,30,12,31,"年末"),
                new DateRange(1,1,1,3,"新年")
            };

        public readonly static DateRange[] seasons = new DateRange[]
        {
            new DateRange(2,28,5,31,"春季"),
            new DateRange(5,31,8,31,"夏季"),
            new DateRange(8,31,11,30,"秋季"),
            new DateRange(11,30,2,28,"冬季")
        };

        public static readonly Func<DateTimeRange[],string>[] strGetFunctions = new Func<DateTimeRange[], string>[]
            {
                OverrideBirthday,
                OverrideAnniversary,
                OverrideHoliday,
                OverrideLongHoliday,
                OverrideSeason
            };

        public static string GetOverrideText(SysL2DShow sysL2DShow)
        {
            DateTimeRange[] dateTimeRanges = GetDateTimeRanges(sysL2DShow);
            foreach (var func in strGetFunctions)
            {
                string ot = func(dateTimeRanges);
                if (!string.IsNullOrEmpty(ot))
                    return ot;
            }
            return null;
        }

        public static bool IsSame(this DateTime[] dateTimes)
        {
            if (dateTimes.Length <= 0)
                return false;
            DateTime dateTimeFirst = dateTimes[0];
            foreach (var dt in dateTimes)
            {
                if (!dt.Equals(dateTimeFirst))
                    return false;
            }
            return true;
        }

        public static bool EqualsMonthAndDay(DateTime a, DateTime b)
        {
            if (a.Month == b.Month
                && a.Day == b.Day)
                return true;
            return false;
        }

        public static bool EqualsMonthAndDay(DateTime a, int month,int day)
        {
            if (a.Month == month
                && a.Day == day)
                return true;
            return false;
        }

        public static DateTimeRange[] GetDateTimeRanges(SysL2DShow sysL2DShow)
        {
            HashSet<DateTimeRange> dateTimeRanges = new HashSet<DateTimeRange>(
                sysL2DShow.systemLive2D.masterSystemLive2Ds.Select((msl2d)=>
                {
                    return new DateTimeRange(ExtensionTools.UnixTimeMSToDateTimeTST(msl2d.publishedAt),
                        ExtensionTools.UnixTimeMSToDateTimeTST(msl2d.closedAt));
                }));
            return dateTimeRanges.ToArray();
        }

        public static string OverrideBirthday(DateTimeRange[] dateTimeRanges)
        {
            if (dateTimeRanges.Length == 1)
            {
                DateTimeRange dateTimeRange = dateTimeRanges[0];
                for (int i = 1; i < 21; i++)
                {
                    ConstData.CharacterInfo characterInfo = ConstData.characters[i];
                    if (EqualsMonthAndDay(dateTimeRange.startTime, characterInfo.birthday.month, characterInfo.birthday.day)
                        && EqualsMonthAndDay(dateTimeRange.startTime, dateTimeRange.endTime))
                    {
                        return $"※{dateTimeRange.startTime.Year}{characterInfo.namae}生日语音";
                    }
                }
            }
            return null;
        }

        public static string OverrideAnniversary(DateTimeRange[] dateTimeRanges)
        {
            if (dateTimeRanges.Length == 1)
            {
                DateTimeRange dateTimeRange = dateTimeRanges[0];
                for (int i = 21; i < 27; i++)
                {
                    ConstData.CharacterInfo characterInfo = ConstData.characters[i];
                    if (EqualsMonthAndDay(dateTimeRange.startTime, characterInfo.birthday.month, characterInfo.birthday.day)
                        && EqualsMonthAndDay(dateTimeRange.startTime, dateTimeRange.endTime))
                    {
                        return $"※{dateTimeRange.startTime.Year}{characterInfo.namae}纪念日语音";
                    }
                }
            }
            return null;
        }

        public static string OverrideHoliday(DateTimeRange[] dateTimeRanges)
        {
            if (dateTimeRanges.Length != 1)
                return null;
            DateTimeRange dateTimeRange = dateTimeRanges[0];
            foreach (var holiday in holidays)
            {
                if (EqualsMonthAndDay(dateTimeRange.startTime, dateTimeRange.endTime)
                    && EqualsMonthAndDay(dateTimeRange.startTime, holiday.month, holiday.day))
                    return $"※{dateTimeRange.startTime.Year}{holiday.name}语音";
            }
            return null;
        }

        public static string OverrideLongHoliday(DateTimeRange[] dateTimeRanges)
        {
            if (dateTimeRanges.Length != 1)
                return null;
            DateTimeRange dateTimeRange = dateTimeRanges[0];
            foreach (var longHoliday in longHolidays)
            {
                if(EqualsMonthAndDay(dateTimeRange.startTime,longHoliday.startMonth,longHoliday.startDay)
                    &&EqualsMonthAndDay(dateTimeRange.endTime,longHoliday.endMonth,longHoliday.endDay))
                    return $"※{dateTimeRange.startTime.Year}{longHoliday.name}语音";
            }
            return null;
        }

        public static string OverrideSeason(DateTimeRange[] dateTimeRanges)
        {
            foreach (var season in seasons)
            {
                HashSet<int> years = new HashSet<int>();
                bool breakFlag = false;
                foreach (var dateTimeRange in dateTimeRanges)
                {
                    if (EqualsMonthAndDay(dateTimeRange.startTime, season.startMonth, season.startDay)
                        && EqualsMonthAndDay(dateTimeRange.endTime, season.endMonth, season.endDay))
                    {
                        years.Add(dateTimeRange.startTime.Year);
                        continue;
                    }
                    else
                    {
                        breakFlag = true;
                        break;
                    }
                }
                if (!breakFlag)
                {
                    return $"※{string.Join("、", years)}{season.name}语音";
                }
            }
            return null;
        }
    }
}