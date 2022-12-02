using System;

namespace SekaiTools.UI.DateTimeRangeSelect
{
    public struct DateTimeRange
    {
        public DateTime startTime;   
        public DateTime endTime;

        public DateTimeRange(DateTime startTime, DateTime endTime)
        {
            this.startTime = startTime;
            this.endTime = endTime;
        }
    }
}