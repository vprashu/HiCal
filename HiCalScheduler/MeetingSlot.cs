using System;
using System.Collections.Generic;

namespace HiCalScheduler
{
    public class MeetingSlot : IComparable<MeetingSlot>
    {
        public long endTime;
        public long startTime;

        public int CompareTo(MeetingSlot other)
        {
            if (other == null)
                return 1;
            return startTime >= other.startTime ? 1 : -1;
        }
    }

    public class MeetingSlotComparer : Comparer<MeetingSlot>
    {
        public override int Compare(MeetingSlot x, MeetingSlot y)
        {
            return x.startTime == y.startTime && x.endTime == y.endTime ? 0 : 1;
        }
    }
}