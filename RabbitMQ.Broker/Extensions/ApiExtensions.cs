using RabbitMQ.Framework.Definitions;
using RabbitMQ.Framework.Extensions;
using RabbitMQ.Framework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace RabbitMQ.Broker.Extensions
{
    public static class ApiExtensions
    {
        /// <summary>
        /// 处理成功
        /// </summary>
        /// <param name="me"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ResponseModel Success(this ApiController me, object data = null)
        {
            return new ResponseModel
            {
                StatusCode = (int)APIStatusKindEnum.Ok,
                Timestamp = DateTime.Now.ToTimestamp(),
                Data = data
            };
        }

        /// <summary>
        /// 处理失败
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static ResponseModel Error(this ApiController controller, string message)
        {
            return new ResponseModel
            {
                StatusCode = (int)APIStatusKindEnum.SystemError,
                Timestamp = DateTime.Now.ToTimestamp(),
                Data = null,
                Message = message
            };
        }
    }
}
