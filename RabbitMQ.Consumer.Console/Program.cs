using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Service;
using RabbitMQ.Models;

namespace RabbitMQ.Consumer.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var consumer = new LifeConsumer();
            //无限循环
            //consumer.Listen<OrderSubmitRequest>();
            //以事件的方式
            consumer.ListenInit();
            consumer.ListenByEvent<OrderSubmitRequest>();

            var key = System.Console.ReadLine();
            if(key  == "quit")
            {
                consumer.ListenDestory();
                //Environment.Exit(0);
            }
        }
    }
}
