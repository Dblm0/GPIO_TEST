using System;
using System.Device.Gpio;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace GPIO_TEST
{
    public interface ITest
    {
        public Task Run();
        public Task Run(CancellationTokenSource cts);
        public void Cancel();
    }
    class Program
    {
        static ITest _tst;
        static Task _t;
        static async Task Main(string[] args)
        {
            using (Process p = Process.GetCurrentProcess())
                p.PriorityClass = ProcessPriorityClass.RealTime;


            AppDomain.CurrentDomain.ProcessExit += Exit;
            Console.CancelKeyPress += Exit;
            _tst = new GpioTest();
            _t = _tst.Run();
            await _t;
        }
        static void Exit(object sender, EventArgs args)
        {
            _tst?.Cancel();
            _t.Wait();
            Console.WriteLine("closing...");
        }
    }
}
