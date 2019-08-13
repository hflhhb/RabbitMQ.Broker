using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.Framework.Models
{
    /// <summary>
    /// 返回结果信息
    /// </summary>
    public class ResponseModel : ResponseModel<object>
    {
    }

    /// <summary>
    /// 返回结果信息
    /// </summary>
    public class ResponseModel<T>
    {
        //Fixed:状态码不全，外部接口调用时映射不到未定义的状态码
        //APIStatusKindEnum
        /// <summary>
        /// 状态码
        /// </summary>
        public int StatusCode { get; set; }
        /// <summa1ry>
        /// 时间戳
        /// </summary>
        public long Timestamp { get; set; }
        /// <summary>
        /// 数据
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// 错误数据
        /// </summary>
        public object ErrorData { get; set; }

        /// <summary>
        /// 错误原因
        /// </summary>
        public string Message { get; set; }
    }
}
