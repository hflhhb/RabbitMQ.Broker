using log4net;
using ObjectDumper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using static RabbitMQ.Framework.Utility.LogManager;

namespace RabbitMQ.Service
{
    public static class ObjectEx
    {
        public static string Dump(this object obj, string title = "")
        {
            var content = "";
            if(title == "Main1")
            {
                content = "-------------------调用开始-----------------";
            }
            if (!string.IsNullOrEmpty(title))
            {
                content += title + " : ";
            }
            content += obj.ToString();

            content = obj.DumpToString(!string.IsNullOrEmpty(title) ? title : "Title");
            content += "; HttpContext：" + HttpContext.Current;

            
            //               

            Debug.WriteLine(content);

            //
            ILog log = LogManager.GetLogger("dump");
            log.Info(content);

            WriteLog("AsyncTest", content);

            return content;
        }
    }
    public class Worker
    {
        public async Task<int> Work()
        {
            Thread.CurrentThread.ManagedThreadId.Dump("Work1");
            
            //var r = await GetTaskOne();
            await Delay(2000);

            var r = await DoWorkAsync(1000);
            Thread.CurrentThread.ManagedThreadId.Dump("Work2");
            
            //go.Dump();
            return r;

        }

        public async Task Delay(int t) {
            Thread.CurrentThread.ManagedThreadId.Dump("Before Delay");
            
            await Task.Delay(t);
            Thread.CurrentThread.ManagedThreadId.Dump("After Delay");
            
        }

        public async Task<int> DoWorkAsync(int t)
        {
            Thread.CurrentThread.ManagedThreadId.Dump("Before DoWorkAsync");
            await Task.Delay(t);

            var rst =  await Task.FromResult(999);

            Thread.CurrentThread.ManagedThreadId.Dump("After DoWorkAsync");
            return rst;
        }

        int go = 1;
        async Task<int> GetTaskOne()
        {
            "GetTaskOne-Start".Dump();
            Thread.CurrentThread.ManagedThreadId.Dump("One1"); //和Main相等
            await Task.Delay(1000);
            "GetTask111111111-Doing......".Dump();
            Thread.CurrentThread.ManagedThreadId.Dump("One2");
            var r = await GetTaskTwo();
            Thread.CurrentThread.ManagedThreadId.Dump("One3");
            "GetTaskOne-End".Dump();

            return r;
        }

        async Task<int> GetTaskTwo()
        {
            "GetTaskTwo-Start".Dump();
            Thread.CurrentThread.ManagedThreadId.Dump("Two1");
            var r = GetTaskThree();
            Thread.CurrentThread.ManagedThreadId.Dump("Two2");
            "GetTask22222222222-Doing.......".Dump();
            await Task.Delay(2000);
            AddGo();
            go.Dump("after: gogogo2222222222");
            "GetTaskTwo-End".Dump();
            Thread.CurrentThread.ManagedThreadId.Dump("Two3");
            var tr = await r;
            tr.Dump("等待的结果");
            return tr;
        }

        async Task<int> GetTaskThree()
        {
            "GetTaskThree-Start".Dump();
            Thread.CurrentThread.ManagedThreadId.Dump("Three1");
            "GetTask33333333-Doing......".Dump();
            await Task.Delay(5000);
            AddGo();
            go.Dump("after: gogogo333333333");
            Thread.CurrentThread.ManagedThreadId.Dump("Three2");
            "GetTaskThree-End".Dump();
            return await Task<int>.FromResult(3);
        }

        void AddGo()
        {
            Thread.CurrentThread.ManagedThreadId.Dump("Add Go ++;当前线程");
            go.Dump("before: gogogo");
            go = go + 1;
            go.Dump("after: gogogo");
        }

        // Define other methods and classes here
    }
}
