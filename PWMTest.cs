using System;
using System.Collections.Generic;
using System.Device.Pwm;
using System.Threading;
using System.Threading.Tasks;

namespace GPIO_TEST
{
    class PWMTest : ITest
    {
        public PWMTest()
        {

        }
        CancellationTokenSource _cts;
        double _dutyCycle = 0.1;
        public async Task Run(CancellationTokenSource cts)
        {
            _cts = cts;
            using var pwm = PwmChannel.Create(0, 0, 1_000_000, _dutyCycle);
            pwm.Stop();
            pwm.Start();

            Console.WriteLine("starting...");
            while (!_cts.IsCancellationRequested)
            {
                await Task.Delay(5);
                _dutyCycle = (_dutyCycle + 1e-2) % 1;
                pwm.DutyCycle = _dutyCycle;
            }
            pwm.Stop();
        }

        public Task Run()
        {
            return Run(new CancellationTokenSource());
        }

        public void Cancel() => _cts?.Cancel();
    }
}