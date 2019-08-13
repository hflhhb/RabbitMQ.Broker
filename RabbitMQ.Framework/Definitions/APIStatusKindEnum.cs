using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace RabbitMQ.Framework.Definitions
{
    /// <summary>
    /// 状态码
    /// </summary>
    public enum APIStatusKindEnum
    {
        /// <summary>
        /// 未知错误
        /// </summary>
        [Description("未知错误")]
        Undefined,
        /// <summary>
        /// 操作成功 StatusCode=200
        /// </summary>
        [Description("操作成功")]
        Ok = 200,

        //[Description("{0}参数值不正确")]
        //[Obsolete("不再使用，请使用 <see>BadRequest</see>")]
        //ArgumentIncorrect = 499,

        /// <summary>
        /// 错误的请求 StatusCode=400
        /// </summary>
        [Description("{0}")]
        BadRequest = 400,
        /// <summary>
        /// 未登录或账户被锁定 StatusCode=401
        /// </summary>
        [Description("未登录或账户被锁定")]
        NotAuthenticated = 401,
        /// <summary>
        /// 账户权限不足 StatusCode=403
        /// </summary>
        [Description("账户权限不足，无法访问")]
        NotAuthorized = 403,
        /// <summary>
        /// 未找到资源 StatusCode=404
        /// </summary>
        [Description("{0}未找到")]
        NotFound = 404,
        /// <summary>
        /// 系统错误 StatusCode=500
        /// </summary>
        [Description("系统错误")]
        SystemError = 500,
    }
}