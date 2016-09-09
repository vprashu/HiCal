using System;
using System.Collections.Generic;
using System.Linq;

namespace HiCalScheduler
{
    public static class Scheduler
    {
        public static MeetingSlot[] CondenseMeetingTimes(MeetingSlot[] meetings)
        {
            if (meetings == null)
            {
                return null;
            }

            var condensedSlots = new Stack<MeetingSlot>();

            Array.Sort(meetings);
            condensedSlots.Push(meetings[0]);

            foreach (var currentMeeting in meetings)
            {
                var condensedMeeting = condensedSlots.Peek();
                if (currentMeeting.startTime > condensedMeeting.endTime)
                {
                    //NonOverLapping
                    condensedSlots.Push(currentMeeting);
                }
                else
                {
                    //Overlapping
                    condensedMeeting.endTime = condensedMeeting.endTime > currentMeeting.endTime ? condensedMeeting.endTime : currentMeeting.endTime;
                }
            }
            return condensedSlots.Reverse().ToArray();
        }
    }
}