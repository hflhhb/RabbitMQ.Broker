using Nito.AsyncEx;
using RabbitMQ.Broker.Extensions;
using RabbitMQ.Broker.Models;
using RabbitMQ.Framework.Helpers;
using RabbitMQ.Framework.Utility;
using RabbitMQ.Models;
using RabbitMQ.Service;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace RabbitMQ.Broker.Controllers
{
    public class SaleOrdersController : ApiController
    {

        [HttpPost]
        public IHttpActionResult Submit([FromBody]OrderSubmitRequest submitRequest)
        {
            if (submitRequest == null)
            {
                throw APIRuleExceptionHelper.ArgumentIncorrect("订单提交");
            }
            //submitRequest.MemberId = this.GetMemberId();
            if (submitRequest.Products == null || !submitRequest.Products.Any())
            {
                throw APIRuleExceptionHelper.BadRequest("Products");
            }
            Stopwatch sw = Stopwatch.StartNew();
            //IWorkSession submitSession = GetWorkSession();
            Producer p = new Producer();
            p.Send(submitRequest);
            //var submitResponse = await _saleOrderSvr.SubmitSaleOrderAPI(submitSession, submitRequest);
            sw.Stop();
            LogManager.WriteLog("API", "Broker：" + sw.ElapsedMilliseconds + "ms");

            //
            return Ok(this.Success(new[] { new { OrderId = 0, OrerNo = "9999999999999999" } }));
        }

        [HttpGet]
        public async Task<IHttpActionResult> BuyNow()
        {
            Thread.CurrentThread.ManagedThreadId.Dump("Main1");
            var w = new Worker();
            w.Work();  //遇到await 返回，异步函数不继续走，得不到最终结果
            //var r = await w.Work();
            //var r = Task.Run(() => w.Work());
            Thread.CurrentThread.ManagedThreadId.Dump("Main2");
            return Ok(new
            {
                Method = "BuyNow",
                Process = " w.Work()",
                Result = (int?)null,
                Description = "遇到await 返回，异步函数不继续走，得不到最终结果"
            });
        }

        [HttpGet]
        public async Task<IHttpActionResult> BuyNow2()
        {
            Thread.CurrentThread.ManagedThreadId.Dump("Main1");
            var w = new Worker();
            //w.Work();
            var r = await w.Work(); //正常流程，正确用法，等待异步函数，得到最终结果，System.Web.HttpContext可以一直保持
            //var r = Task.Run(() => w.Work());
            Thread.CurrentThread.ManagedThreadId.Dump("Main2");
            return Ok(new
            {
                Method = "BuyNow2",
                Process = "var r = await w.Work()",
                Result = r,
                Description = "正常流程，正确用法，等待异步函数，得到最终结果，System.Web.HttpContext可以一直保持"
            });
        }

        [HttpGet]
        public async Task<IHttpActionResult> BuyNow3()
        {
            Thread.CurrentThread.ManagedThreadId.Dump("Main1");
            var w = new Worker();
            //w.Work();
            //var r = await w.Work();
            //流程不等待Task内方法，走完主流程直接退出，Task里任务继续执行。
            //  可以得到结果，注意Task里面的主流程里面的Context都不存在了，
            //  不捕获HttpContext,并且多开线程，有线程上下文切换
            var r = Task.Run(() => w.Work()); 
            Thread.CurrentThread.ManagedThreadId.Dump("Main2");

            return Ok(new
            {
                Method = "BuyNow3",
                Process = "var r = Task.Run(() => w.Work());",
                Result = r,
                Description = "流程不等待Task内方法，走完主流程直接退出，Task里任务继续执行。" + 
                "\n\r可以得到结果，注意Task里面的主流程里面的Context都不存在了，" +
                "\n\r不捕获HttpContext,并且多开线程，有线程上下文切换。"
            });
        }
        [HttpGet]
        public async Task<IHttpActionResult> BuyNow31()
        {
            Thread.CurrentThread.ManagedThreadId.Dump("Main1");
            var w = new Worker();
            //w.Work();
            //var r = await w.Work();
            //流程不等待Task内方法，走完主流程直接退出，Task里任务继续执行。
            //  可以得到结果，注意Task里面的主流程里面的Context都不存在了，
            //  不捕获HttpContext,并且多开线程，有线程上下文切换
            var r = Task.Run(async () => await w.Work());
            Thread.CurrentThread.ManagedThreadId.Dump("Main2");

            return Ok(new
            {
                Method = "BuyNow31",
                Process = "var r = Task.Run(async () => await w.Work());",
                Result = r,
                Description = ""
            });
        }

        [HttpGet]
        public async Task<IHttpActionResult> BuyNow4()
        {
            Thread.CurrentThread.ManagedThreadId.Dump("Main1");
            var w = new Worker();
            //w.Work();
            //var r = await w.Work();
            var r = await Task.Run(() => w.Work());
            Thread.CurrentThread.ManagedThreadId.Dump("Main2");
            return Ok(new
            {
                Method = "BuyNow4",
                Process = "var r = await Task.Run(() => w.Work());",
                Result = r,
                Description = "流程等待Task内方法，" +
                 "\n\r可以得到结果，注意Task里面的主流程里面的Context都不存在了，" +
                 "\n\r不捕获HttpContext,线程数和await w.Work(); 一样"
            });
        }

        [HttpGet]
        public async Task<IHttpActionResult> BuyNow5()
        {
            Thread.CurrentThread.ManagedThreadId.Dump("Main1");
            var w = new Worker();
            //w.Work();
            //var r = await w.Work();
            var r = AsyncContext.Run(() => w.Work()); //和同步方法一样
            Thread.CurrentThread.ManagedThreadId.Dump("Main2");
            return Ok(new
            {
                Method = "BuyNow5",
                Process = "var r = AsyncContext.Run(() => w.Work());",
                Result = r,
                Description = "和同步方法一样，Context 有，正确结果，线程和主线程一样"
            });
        }

        [HttpGet]
        public async Task<IHttpActionResult> BuyNow6()
        {
            Thread.CurrentThread.ManagedThreadId.Dump("Main1");
            //HttpContext.Current.Dump("HttpContext");
            var w = new Worker();
            //w.Work();
            //var r = await w.Work();
            var r = AsyncContext.Run(() => Task.Run(() => w.Work()));
            Thread.CurrentThread.ManagedThreadId.Dump("Main2");
            return Ok(new
            {
                Method = "BuyNow6",
                Process = "var r = AsyncContext.Run(() => Task.Run(() => w.Work()));",
                Result = r,
                Description = "和var r = await Task.Run(() => w.Work());一样，可以得到结果，Task里面的Context不存在，区别是多开线程 "
            });
        }
    }
}
