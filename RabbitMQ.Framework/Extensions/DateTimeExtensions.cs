using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.Framework.Extensions
{
    public static class  DateTimeExtensions
    {
        private static readonly DateTime UnixDateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        /// <summary>
        /// 时间转化为时间戳
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static long ToTimestamp(this DateTime datetime)
        {
            var timespan = datetime.ToUniversalTime() - UnixDateTime;

            return Convert.ToInt64(timespan.TotalMilliseconds);
        }
        /// <summary>
        /// 时间戳转化为时间
        /// </summary>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this long timestamp)
        {
            var utcTime = UnixDateTime.AddMilliseconds(timestamp);

            return utcTime.ToLocalTime();
        }
    }
}
