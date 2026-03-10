using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace MaYiMaMa.CSharp.Extensions
{
    public static class MMDateTimeExtensions 
    {
        /// <summary>
        /// Unix时间戳(ms)
        /// </summary>
        public static long UnixTimestamp()
        {
            return new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();
        }

        /// <summary>
        /// 当前时间格式化
        /// </summary>
        public static string NowToHHMMSSFFF()
        {
            return DateTime.Now.ToString("HH:mm:ss.fff");
        }

        /// <summary>
        /// 今天此刻的总毫秒数
        /// </summary>
        public static uint TotalMilliseconds4Today()
        {
            DateTime now = DateTime.Now;
            DateTime todayMidnight = new DateTime(now.Year, now.Month, now.Day);
            TimeSpan timeSinceMidnight = now - todayMidnight;
            uint millisecondsSinceMidnight = (uint)timeSinceMidnight.TotalMilliseconds;
            return millisecondsSinceMidnight;
        }

        /// <summary>
        /// 获取该时间相对于1970-01-01T00:00:00Z的秒数
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static long GetTotalSeconds(this in DateTime dt) => new DateTimeOffset(dt).UtcDateTime.Ticks / 10_000_000L - 62135596800L;

        /// <summary>
        /// 获取该时间相对于1970-01-01T00:00:00Z的毫秒数
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static long GetTotalMilliseconds(this in DateTime dt) => new DateTimeOffset(dt).UtcDateTime.Ticks / 10000L - 62135596800000L;

        /// <summary>
        /// 获取该时间相对于1970-01-01T00:00:00Z的微秒时间戳
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static long GetTotalMicroseconds(this in DateTime dt) => (new DateTimeOffset(dt).UtcTicks - 621355968000000000) / 10;

        [DllImport("Kernel32.dll")]
        private static extern bool QueryPerformanceCounter(out long lpPerformanceCount);

        /// <summary>
        /// 获取该时间相对于1970-01-01T00:00:00Z的纳秒时间戳
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static long GetTotalNanoseconds(this in DateTime dt)
        {
            var ticks = (new DateTimeOffset(dt).UtcTicks - 621355968000000000) * 100;
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                QueryPerformanceCounter(out var timestamp);
                return ticks + timestamp % 100;
            }

            return ticks + Stopwatch.GetTimestamp() % 100;
        }

        /// <summary>
        /// 获取该时间相对于1970-01-01T00:00:00Z的分钟数
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static double GetTotalMinutes(this in DateTime dt) => new DateTimeOffset(dt).Offset.TotalMinutes;

        /// <summary>
        /// 获取该时间相对于1970-01-01T00:00:00Z的小时数
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static double GetTotalHours(this in DateTime dt) => new DateTimeOffset(dt).Offset.TotalHours;

        /// <summary>
        /// 获取该时间相对于1970-01-01T00:00:00Z的天数
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static double GetTotalDays(this in DateTime dt) => new DateTimeOffset(dt).Offset.TotalDays;

        #region P/Invoke 设置本地时间

        [DllImport("kernel32.dll")]
        private static extern bool SetLocalTime(ref SystemTime time);

        [StructLayout(LayoutKind.Sequential)]
        private /*record*/ struct SystemTime
        {
            public short year;
            public short month;
            public short dayOfWeek;
            public short day;
            public short hour;
            public short minute;
            public short second;
            public short milliseconds;
        }

        /// <summary>
        /// 设置本地计算机系统时间，仅支持Windows系统
        /// </summary>
        /// <param name="dt">DateTime对象</param>
        public static void SetLocalTime(this in DateTime dt)
        {
            SystemTime st;
            st.year = (short)dt.Year;
            st.month = (short)dt.Month;
            st.dayOfWeek = (short)dt.DayOfWeek;
            st.day = (short)dt.Day;
            st.hour = (short)dt.Hour;
            st.minute = (short)dt.Minute;
            st.second = (short)dt.Second;
            st.milliseconds = (short)dt.Millisecond;
            SetLocalTime(ref st);
        }

        #endregion P/Invoke 设置本地时间

    }
}

