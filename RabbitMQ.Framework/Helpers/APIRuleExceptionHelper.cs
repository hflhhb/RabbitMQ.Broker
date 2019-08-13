using RabbitMQ.Framework.Definitions;
using RabbitMQ.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.Framework.Helpers
{
    /// <summary>
    /// 
    /// </summary>
    public static class APIRuleExceptionHelper
    {
        public static ApiRuleException NotAuthenticated()
        {
            return new ApiRuleException(APIStatusKindEnum.NotAuthenticated);
        }

        public static ApiRuleException NotAuthorized()
        {
            return new ApiRuleException(APIStatusKindEnum.NotAuthorized);
        }

        public static ApiRuleException SystemError()
        {
            return new ApiRuleException(APIStatusKindEnum.SystemError);
        }

        //public static ApiRuleException SystemError(string reasionMsg)
        //{
        //    return new ApiRuleException(APIStatusKindEnum.SystemError, reasionMsg);
        //}
        public static ApiRuleException UndefinedError()
        {
            return new ApiRuleException(APIStatusKindEnum.Undefined);
        }
        public static ApiRuleException NotFound(string name)
        {
            return new ApiRuleException(APIStatusKindEnum.NotFound, name);
        }
        public static ApiRuleException ArgumentIncorrect(string argumentName)
        {
            var msgFormat = "{0}参数值不正确";
            var message = String.Format(msgFormat, argumentName);
            return new ApiRuleException(APIStatusKindEnum.BadRequest, message);
            //return new ApiRuleException(APIStatusKindEnum.ArgumentIncorrect,argumentName);
        }
        public static ApiRuleException BadRequest(string message)
        {
            return new ApiRuleException(APIStatusKindEnum.BadRequest, message);
        }
    }
}
