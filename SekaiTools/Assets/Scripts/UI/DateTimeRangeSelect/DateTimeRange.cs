using System;

namespace SekaiTools
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

        public override bool Equals(object obj)
        {
            if (obj is DateTimeRange)
            {
                DateTimeRange dtr = (DateTimeRange)obj;
                return startTime.Equals(dtr.startTime) && endTime.Equals(dtr.endTime);
            }

            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            int hashCode = 1558290729;
            hashCode = hashCode * -1521134295 + startTime.GetHashCode();
            hashCode = hashCode * -1521134295 + endTime.GetHashCode();
            return hashCode;
        }
    }
}