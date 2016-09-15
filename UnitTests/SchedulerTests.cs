using System;
using System.Collections.Generic;
using System.Linq;
using HiCalScheduler;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HiCal
{
    

    [TestClass]
    public class SchedulerTests
    {
        [TestMethod]
        public void AdjacentSlots()
        {
            MeetingSlot[] meetingArray = {
                new MeetingSlot { startTime = 0, endTime = 1 },
                new MeetingSlot { startTime = 1, endTime = 2 },
                new MeetingSlot { startTime = 2, endTime = 6 },
                new MeetingSlot { startTime = 6, endTime = 30 } };

            var actual = Scheduler.CondenseMeetingTimes(meetingArray);

            MeetingSlot[] expected = {
                new MeetingSlot { startTime = 0, endTime = 30 } };

            CollectionAssert.AreEqual(actual, expected, new MeetingSlotComparer());
        }

        [TestMethod]
        public void MultipleHiddenSlots()
        {
            MeetingSlot[] meetingArray = {
                new MeetingSlot { startTime = 0, endTime = 1 },
                new MeetingSlot { startTime = 1, endTime = 2 },
                new MeetingSlot { startTime = 2, endTime = 6 },
                new MeetingSlot { startTime = 21, endTime = 30 } };

            var actual = Scheduler.CondenseMeetingTimes(meetingArray);

            MeetingSlot[] expected = {
                new MeetingSlot { startTime = 0, endTime = 6 },
                new MeetingSlot { startTime = 21, endTime = 30 }};

            CollectionAssert.AreEqual(actual, expected, new MeetingSlotComparer());
        }

        [TestMethod]
        public void NonOverLapping()
        {
            MeetingSlot[] meetingArray = {
                new MeetingSlot { startTime = 0, endTime = 1 },
                new MeetingSlot { startTime = 10, endTime = 12 },
                new MeetingSlot { startTime = 4, endTime = 8 },
                new MeetingSlot { startTime = 21, endTime = 30 } };

            var actual = Scheduler.CondenseMeetingTimes(meetingArray);

            MeetingSlot[] expected = {
                new MeetingSlot { startTime = 0, endTime = 1 },
                new MeetingSlot { startTime = 4, endTime = 8 },
                new MeetingSlot { startTime = 10, endTime = 12 },
                new MeetingSlot { startTime = 21, endTime = 30 } };

            CollectionAssert.AreEqual(actual, expected, new MeetingSlotComparer());
        }

        [TestMethod]
        public void NullInput()
        {
            var condensedMeetings = Scheduler.CondenseMeetingTimes(null);
            List<MeetingSlot> expected = null;
            CollectionAssert.AreEqual(condensedMeetings, expected, new MeetingSlotComparer());
        }

        [TestMethod]
        public void RandomOrderSlots()
        {
            MeetingSlot[] meetingArray = {
                new MeetingSlot { startTime = 3, endTime = 5 },
                new MeetingSlot { startTime = 10, endTime = 12 },
                new MeetingSlot { startTime = 4, endTime = 18 },
                new MeetingSlot { startTime = 9, endTime = 10 },
                new MeetingSlot { startTime = 0, endTime = 1 }};

            var actual = Scheduler.CondenseMeetingTimes(meetingArray);

            MeetingSlot[] expected = {
                new MeetingSlot { startTime = 0, endTime = 1 },
                new MeetingSlot { startTime = 3, endTime = 18 }};

            CollectionAssert.AreEqual(actual, expected, new MeetingSlotComparer());
        }

        [TestMethod]
        public void UsingUnixTimeStamps()
        {
            DateTimeOffset dto = DateTimeOffset.Now;

            MeetingSlot[] meetingArray = {
                new MeetingSlot { startTime = dto.ToUnixTimeSeconds() + (2 * 60 * 60), endTime = dto.ToUnixTimeSeconds() + (6 * 60 * 60) },
                new MeetingSlot { startTime = dto.ToUnixTimeSeconds(), endTime = dto.ToUnixTimeSeconds() + (1 * 60 * 60) },
                new MeetingSlot { startTime = dto.ToUnixTimeSeconds() + (1 * 60 * 60) , endTime = dto.ToUnixTimeSeconds() + (2 * 60 * 60) },
                new MeetingSlot { startTime = dto.ToUnixTimeSeconds() + (21 * 60 * 60), endTime = dto.ToUnixTimeSeconds() + (30 * 60 * 60) } };

            var actual = Scheduler.CondenseMeetingTimes(meetingArray);

            MeetingSlot[] expected = {
                new MeetingSlot { startTime = dto.ToUnixTimeSeconds() , endTime = dto.ToUnixTimeSeconds() + (6 * 60 * 60) },
                new MeetingSlot { startTime = dto.ToUnixTimeSeconds() + (21 * 60 * 60), endTime = dto.ToUnixTimeSeconds() + (30 * 60 * 60) }};

            CollectionAssert.AreEqual(actual, expected, new MeetingSlotComparer());
        }

        [TestMethod]
        public void LotOfSlots()
        {
            int count = 1000000;
            MeetingSlot[] meetingArray = new MeetingSlot[count];
            int startIndex = 1;
            int endIndex = 3;
            MeetingSlot[] expected = {
                new MeetingSlot { startTime = startIndex, endTime = count + endIndex - startIndex }
                };
            for (int i = 0; i < count; i++)
            {
                meetingArray[i] = new MeetingSlot
                {
                    startTime = startIndex++,
                    endTime = endIndex++
                };

            }
            var actual = Scheduler.CondenseMeetingTimes(meetingArray);

            

            CollectionAssert.AreEqual(actual, expected, new MeetingSlotComparer());
        }

    }
}