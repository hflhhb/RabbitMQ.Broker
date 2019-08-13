using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.Service
{
    /// <summary>
    /// RabbitMQ 生产者
    /// </summary>
    public class Producer
    {
        ConnectionFactory _factory = null;
        public Producer()
        {
            _factory = new ConnectionFactory();
            _factory.HostName = "10.100.6.30";
            _factory.UserName = "life";
            _factory.Password = "life$rabbitmq";
            //_factory.ho
            //ConnectionFactory factory = new ConnectionFactory();
            //factory.Uri = "amqp://user:pass@hostName:port/vhost";
        }


        /// <summary>
        /// exchange 四种类型(direct, fanout, topic, header) :
        ///   direct : 
        ///     模式下不需要将Exchange进行任何绑定(binding)操作, 消息传递时需要一个“RouteKey”，可以简单的理解为要发送到的队列名字。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="msg"></param>
        public void Send<T>(T msg)
        {
            try
            {
                using (var connection = _factory.CreateConnection())
                {
                    using (var channel = connection.CreateModel())
                    {
                        var exchangeName = "life.order";
                        var queueName = "life.order.submit";
                        var routingKey = queueName;
                        //设置交换器的类型
                        channel.ExchangeDeclare(exchangeName, "direct");
                        //声明一个队列，设置队列是否持久化，排他性，与自动删除
                        channel.QueueDeclare(queueName, true, false, false, null);
                        //绑定消息队列，交换器，routingkey
                        channel.QueueBind(queueName, exchangeName, routingKey);
                        var properties = channel.CreateBasicProperties();
                        //队列持久化
                        properties.Persistent = true;
                        var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(msg));
                        //发送信息
                        channel.BasicPublish(exchangeName, routingKey, properties, body);
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
