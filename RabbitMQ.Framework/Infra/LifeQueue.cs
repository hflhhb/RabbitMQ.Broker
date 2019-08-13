using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RabbitMQ.Framework.Infra
{
    public class LifeQueue
    {
        #region 变量
        // 用于存放写日志任务的队列
        private readonly ConcurrentQueue<Action> _queue;
        private readonly int _sleepTime;
        #endregion

        // 用于通知是否有新日志要写的“信号器”
        private readonly ManualResetEvent _hasNew;

        #region 构造函数
        public LifeQueue() : this(100)
        {

        }
        public LifeQueue(int sleepTime)
        {
            _queue = new ConcurrentQueue<Action>();
            _hasNew = new ManualResetEvent(false);
            _sleepTime = sleepTime;

            var workThread = new Thread(Process) { IsBackground = true };
            workThread.Start();
        }

        #endregion
        // 
        private void Process()
        {
            while (true)
            {
                // 等待接收信号，阻塞线程。
                _hasNew.WaitOne();

                // 接收到信号后，重置“信号器”，信号关闭。
                _hasNew.Reset();

                // 减少对队列的频繁“进出”操作。
                if (_sleepTime > 0) Thread.Sleep(_sleepTime);

                // 由于执行过程中还可能会有新的任务，所以不能直接对原来的 _queue 进行操作，
                // 先将_queue中的任务复制一份后将其清空，然后对这份拷贝进行操作。
                ConcurrentQueue<Action> queueCopy;
                lock (_queue)
                {
                    queueCopy = new ConcurrentQueue<Action>(_queue);
                    //清空
                    Clear(_queue);
                }

                foreach (var action in queueCopy)
                {
                    action();
                }
            }
        }
        public void Clear<T>(ConcurrentQueue<T> queue)
        {
            T item;
            while (queue.TryDequeue(out item))
            {
            }
        }
        public void Enqueue(Action item)
        {
            lock (_queue)
            {
                // 将任务加到队列
                _queue.Enqueue(item);
            }
            _hasNew.Set();
        }
    }
}
