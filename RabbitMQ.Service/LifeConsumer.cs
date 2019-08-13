using RabbitMQ.Models;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace RabbitMQ.Service
{

    /// <summary>
    /// RabbitMQ 消费者
    /// </summary>
    public class LifeConsumer
    {
        ConnectionFactory _factory = null;
        //Event
        IConnection _connection = null;
        IModel _channel = null;
        long _id = 0;
        int dummyProcessTime = 100; //ms 

        public LifeConsumer()
        {
            _factory = new ConnectionFactory();
            //_factory.HostName = "10.100.6.30";
            //_factory.Port = 5672;
            //_factory.UserName = "life";
            //_factory.Password = "life$rabbitmq";
            //Mode 1
            //_factory.Uri = "amqp://life:life$rabbitmq@10.100.6.30:5672/";
            //Mode 2
            var uri = new Uri("amqp://10.100.6.30:5672/");
            _factory = new ConnectionFactory
            {
                UserName = "life",
                Password = "life$rabbitmq",
                Endpoint = new AmqpTcpEndpoint(uri),
                RequestedHeartbeat = 60, //心跳超时时间
                AutomaticRecoveryEnabled = true  //自动重连
            };
        }

        public void ListenInit()
        {
            _connection = _factory.CreateConnection();
            _channel = _connection.CreateModel();
        }
        public void ListenDestory()
        {
            if(_channel != null && _channel.IsOpen)
            {
                _channel.Close();
            }
            if (_connection != null && _connection.IsOpen)
            {
                _connection.Close();
            }
            //
            _channel?.Dispose();
            _connection?.Dispose();
            //
            _channel = null;
            _connection = null;
        }
        //public void Dispose()
        //{
        //    ListenDestory();
        //}
        public void Listen<T>()
        {
            using (IConnection connection = _factory.CreateConnection())
            {
                using (IModel channel = connection.CreateModel())
                {
                    var queueName = "life.order.submit";
                    var routingKey = queueName;
                    //在MQ上定义一个队列，如果名称相同不会重复创建
                    channel.QueueDeclare(queueName, true, false, false, null);

                    Console.WriteLine("Listening...");

                    //在队列上定义一个消费者
                    QueueingBasicConsumer consumer = new QueueingBasicConsumer(channel);
                    channel.BasicConsume(queueName, false, consumer);
                    long id = 0;
                    while (true)
                    {
                        //阻塞函数，获取队列中的消息
                        BasicDeliverEventArgs ea = consumer.Queue.Dequeue();

                        byte[] body = ea.Body;

                        var result = Encoding.UTF8.GetString(body);

                        var response = JsonConvert.DeserializeObject<T>(result);
                        Console.WriteLine($"Receive a Message, Id:{++id} {Environment.NewLine}   Message:" + JsonConvert.SerializeObject(response));
                        Thread.Sleep(dummyProcessTime);
                        channel.BasicAck(ea.DeliveryTag, false);
                        //using (MemoryStream ms = new MemoryStream(body))
                        //{

                        //    RequestMessage message = (RequestMessage)xs.Deserialize(ms);
                        //    Console.WriteLine("Receive a Message, Id:" + message.MessageId + " Message:" + message.Message);
                        //}
                    }
                }
            }
        }
        public void ListenByEvent<T>(Func<IModel, BasicDeliverEventArgs, T> receiveCallback = null)
        {
            var queueName = "life.order.submit";
            var routingKey = queueName;
            //在MQ上定义一个队列，如果名称相同不会重复创建
            _channel.QueueDeclare(queueName, true, false, false, null);

            Console.WriteLine("Listening...");

            //定义
            EventingBasicConsumer consumer = new EventingBasicConsumer(_channel);
            consumer.Received += Consumer_Received;
            //
            _channel.BasicQos(0, 1, false);
            _channel.BasicConsume(queueName, false, consumer);
                
            
        }

        private void Consumer_Received(object sender, BasicDeliverEventArgs ea)
        {
            var consumer = sender as EventingBasicConsumer;

            byte[] body = ea.Body;

            var result = Encoding.UTF8.GetString(body);
                                                     
            //var response = JsonConvert.DeserializeObject<T>(result);
            //Console.WriteLine($"Receive a Message, Id:{++ _id} {Environment.NewLine}   Message:" + JsonConvert.SerializeObject(response));
            Console.WriteLine($"Receive a Message, Id:{++_id} {Environment.NewLine}   Message:" + result);

            Thread.Sleep(dummyProcessTime);
            _channel.BasicAck(ea.DeliveryTag, false);
        }
    }
}
