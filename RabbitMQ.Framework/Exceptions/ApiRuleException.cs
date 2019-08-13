using RabbitMQ.Framework.Definitions;
using RabbitMQ.Framework.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.Framework.Exceptions
{
    public class ApiRuleException : Exception
    {
        public ApiRuleException()
            : base(string.Empty)
        {

        }
        public ApiRuleException(APIStatusKindEnum kind)
            : base(EnumHelper.GetEnumDescription(kind))
        {
            Kind = kind;
        }
        public ApiRuleException(APIStatusKindEnum kind, string name)
            : base(string.Format(EnumHelper.GetEnumDescription(kind),name))
        {
            Kind = kind;
        }

        public APIStatusKindEnum Kind { get; set; }
    }

}
