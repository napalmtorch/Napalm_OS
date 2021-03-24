using System;
using System.Collections.Generic;
using System.Text;
using RTC = Cosmos.HAL.RTC;

namespace NapalmOS.Hardware
{
    public static class Clock
    {
        // time getters
        public static byte GetSecond() { return RTC.Second; }
        public static byte GetMinute() { return RTC.Minute; }
        public static byte GetHour() { return RTC.Hour; }
        static int tick, lastTick;

        // get time formatted
        private static string time, sec, min, hr;
        public static string GetTime(bool seconds, bool military)
        {
            tick = GetSecond();
            if (tick != lastTick)
            {
                time = ""; hr = ""; sec = ""; min = "";

                // calculate second
                if (RTC.Second < 10) { sec = "0" + RTC.Second.ToString(); } else { sec = RTC.Second.ToString(); }

                // calculate minute
                if (RTC.Minute < 10) { min = "0" + RTC.Minute.ToString(); } else { min = RTC.Minute.ToString(); }

                // calculate hour
                if (military) { hr = RTC.Hour.ToString(); }
                else
                { if (RTC.Hour > 12) { hr = (RTC.Hour - 12).ToString(); } }
                if (RTC.Hour < 10) { hr = "0" + hr; }
                if (hr == "0" || hr == "00") { hr = "12"; }

                // set time
                time = hr + ":" + min;
                if (seconds) { time += ":" + sec; }
                if (!military)
                {
                    if (RTC.Hour > 12) { time += " PM"; } else { time += " AM"; }
                }

                lastTick = tick;
            }

            // return time
            return time;
        }

        // date getters
        public static byte GetDay() { return RTC.DayOfTheMonth; }
        public static byte GetDayOfWeek() { return RTC.DayOfTheWeek; }
        public static byte GetMonth() { return RTC.Month; }
        public static byte GetYear() { return RTC.Year; }
    }
}
