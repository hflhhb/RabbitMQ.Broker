using RabbitMQ.Framework.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.Models
{
    public interface ISaleOrderSubmitRequest
    {
        /// <summary>
        /// 会员ID
        /// </summary>
        long MemberId { get; set; }
    }

    /// <summary>
    /// 拆分订单时用到
    /// </summary>
    public class SaleOrderSubmitRequestBase : ISaleOrderSubmitRequest
    {
        /// <summary>
        /// 会员ID
        /// </summary>
        public long MemberId { get; set; }
        /// <summary>
        /// 购买的商品信息
        /// </summary>
        public IEnumerable<SaleOrderSubmitProductInfo> Products { get; set; }
    }

    /// <summary>
    /// 计算运费时用到
    /// </summary>
    public class SaleOrderSubmitRequest : SaleOrderSubmitRequestBase
    {
        /// <summary>
        /// 收货地址ID
        /// </summary>
        public long ShipAddressId { get; set; }

    }

    /// <summary>
    /// 订单创建时前端提交的数据模型
    /// </summary>
    public class OrderSubmitRequest : SaleOrderSubmitRequest
    {
        /// <summary>
        /// 联币抵扣值
        /// </summary>
        public long LianCoinDudect { get; set; }
        /// <summary>
        /// 支付方式 传入的数字时按照ID处理， 字符串按照编码处理
        /// </summary>
        public string PayTerm { get; set; }

        /// <summary>
        /// 发票类型
        /// </summary>
        public InvoiceTypeEnum InvoiceType { get; set; }

        /// <summary>
        /// 发票抬头
        /// </summary>
        public string InvoiceTitle { get; set; }

        /// <summary>
        /// 备注信息
        /// </summary>
        public IEnumerable<SaleOrderSubmitRemarkInfo> RemarkInfos { get; set; }

    }
    /// <summary>
    /// 订单提交时购买的商品信息
    /// </summary>
    public class SaleOrderSubmitProductInfo
    {
        /// <summary>
        /// 购买的商品
        /// </summary>
        public long ProductId { get; set; }
        /// <summary>
        /// 购买数量
        /// </summary>
        public int Quantity { get; set; }
    }
    /// <summary>
    /// 订单提交时发票信息
    /// </summary>
    public class SaleOrderSubmitRemarkInfo
    {

        public long ProductId { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; set; }

        /// <summary>
        /// 是否分配
        /// </summary>
        public bool IsAttach { get; set; }
    }
}
