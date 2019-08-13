using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.Framework.Definitions
{
    /// <summary>
    /// 发票类型
    /// </summary>
    public enum InvoiceTypeEnum
    {
        [Description("未知")]
        Unknown = 0,
        /// <summary>
        /// 发票类型-个人
        /// </summary>
        [Description("个人")]
        Personal,
        /// <summary>
        /// 发票类型-公司
        /// </summary>
        [Description("公司")]
        Corporate
    }
}
