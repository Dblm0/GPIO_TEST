using System;
using System.Device.Gpio;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GPIO_TEST
{
    class GpioTest : ITest
    {
        CancellationTokenSource _cts;
        GpioController _controller;
        readonly int _outputPinNumber = 19;
        readonly int _inputPinNumber = 26;

        public GpioTest()
        {
            var pinmodes = new (int number, PinMode mode)[]
            {
                (_outputPinNumber, PinMode.Output),
                (_inputPinNumber, PinMode.InputPullDown),
            };

            _controller = new GpioController(PinNumberingScheme.Logical);
            Console.WriteLine("Ready");
            foreach (var pin in pinmodes)
            {
                _controller.OpenPin(pin.number, pin.mode);
            }
            _controller.RegisterCallbackForPinValueChangedEvent(_inputPinNumber, PinEventTypes.Rising, callback);
        }
        void callback(object sender, PinValueChangedEventArgs pinValueChangedEventArgs)
        {
            _controller.Write(_outputPinNumber, PinValue.Low);
        }

        public Task Run()
        {
            return Run(new CancellationTokenSource());
        }
        public Task Run(CancellationTokenSource cts)
        {
            _cts = cts;
            while (!cts.IsCancellationRequested)
            {
                _controller.Write(_outputPinNumber, PinValue.High);
                DelayHelper.DelayMicroseconds(500, false);
            }
            _cts.Cancel();
            return Task.CompletedTask;
        }

        public void Cancel() => _cts?.Cancel();
    }
}
