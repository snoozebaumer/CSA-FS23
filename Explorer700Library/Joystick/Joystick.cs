// ----------------------------------------------------------------------------
// CSA - C# in Action
// (c) 2023, Christian Jost, HSLU
// ----------------------------------------------------------------------------
using System;
using System.Device.Gpio;
using System.Diagnostics;
using System.Threading;

namespace Explorer700Library
{
    public class Joystick
    {
        #region members & events
        private int centerPin;
        public event EventHandler<KeyEventArgs> JoystickChanged;
        #endregion

        #region constructor & destructor
        public Joystick(Pcf8574 pcf8574, GpioController gpioController)
        {
            Pcf8574 = pcf8574;
            pcf8574.Mask |= 0x0F;
            centerPin = 20;
            GpioController = gpioController;
            GpioController.OpenPin(centerPin, PinMode.InputPullUp);

            // Bugfix da die folgenden Zeile nicht funktioniert:
            // centerPin.InputPullMode = GpioPinResistorPullMode.PullUp;
            // Console.Write("   " + centerPin.Read() + "   ");
            //ProcessStartInfo psi = new ProcessStartInfo();
            //psi.FileName = "/usr/bin/raspi-gpio";
            //psi.Arguments = "set 20 ip pu";
            //Process.Start(psi);

            // Start Polling-Thread
            Thread t = new Thread(Run);
            t.IsBackground = true;
            t.Start();
        }
        #endregion

        #region properties
        private Pcf8574 Pcf8574 { get; set; }

        internal GpioController GpioController { get; }


        /// <summary>
        /// Liest und liefert den Zustand des Joysticks
        /// </summary>
        public Keys Keys
        {
            get
            {
                byte data = Pcf8574.Read();
                data = (byte)((~data) & 0x0F);
                Keys k = (Keys)data; // Low Active
                if (!(bool)GpioController.Read(centerPin)) k |= Keys.Center;
                return k;
            }
        }
        #endregion

        #region methods
        /// <summary>
        /// Pollt alle 50ms den Joystick und generiert ein JoystickChanged Event, falls
        /// sich der Zustand des Joysticks (Taste gedrückt/losgelassen) verändert hat.
        /// </summary>
        private void Run()
        {
            Keys oldState = Keys;
            while (true)
            {
                Thread.Sleep(50);
            }
        }
        #endregion
    }
}
