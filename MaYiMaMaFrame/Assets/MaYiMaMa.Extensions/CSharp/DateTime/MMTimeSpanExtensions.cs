using System;

namespace MaYiMaMa.CSharp.Extensions
{
    public static class MMTimeSpanExtensions 
    {
        /// <summary>
        /// TimeSpan转小时:分钟:秒:毫秒
        /// </summary>
        /// <returns></returns>
        public static string ToHHMMSSFFF(this TimeSpan timeSpan)
        {
            return timeSpan.ToString(@"hh\:mm\:ss\.fff");
        }
    }
}

