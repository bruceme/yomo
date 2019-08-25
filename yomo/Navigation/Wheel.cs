using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unosquare.RaspberryIO;
using Unosquare.RaspberryIO.Abstractions;
using Unosquare.RaspberryIO.Native;
using Unosquare.WiringPi;
using static yomo.Utility.Settings;


namespace yomo.Navigation
{
    public interface IWheel
    {
        int Speed { get; }
        uint DutyRange { get; }
        uint MaxDuty { get; }
        void SetSpeed(int speed);
        bool Forward { get; }
        bool Reverse { get; }
        double DutyCycle { get; }

    }
    public class Wheel : IWheel
    {
        public int Speed { get; private set; }
        public bool Forward { get; private set; }
        public bool Reverse { get; private set; }


        GpioPin PinPwm;
        IGpioPin PinFwd;
        IGpioPin PinRev;


        // pwmFrequency in Hz = 19.2e6 Hz / pwmClock / pwmRange.
        const int PwmClockDivisor = 16; // 1 is 4096, possible values are all powers of 2 starting from 2 to 2048
        const int PwmRange = 1024;
        const int PwmRegister = 0; // (int)(pin.PwmRange * decimal duty); // This goes from 0 to PwmRange-1

        double kFactor = 100.0; // convertion from requested speed to PWM duty
        const uint MinDuty = 200;

        public uint DutyRange { get { return MaxDuty - MinDuty; } }
        public uint MaxDuty { get { return PwmRange; } }
        public double DutyCycle { get; private set; }


        /// <summary>
        ///  Setup the wheel
        /// </summary>
        /// <param name="pinPwm">Pin used for the PWM</param>
        /// <param name="pinFwd">Pin used to enable forward</param>
        /// <param name="pinRev">Pin used to enable reverse</param>
        public Wheel(int pinPwm, int pinFwd, int pinRev)
        {
            if (!Testing)
            {

                //set up pins
                PinFwd = (GpioPin)Pi.Gpio[pinFwd];
                PinFwd.PinMode = GpioPinDriveMode.Output;

                PinRev = (GpioPin)Pi.Gpio[pinRev];
                PinRev.PinMode = GpioPinDriveMode.Output;

                // TODO: Check out:
                // https://raspberrypi.stackexchange.com/questions/4906/control-hardware-pwm-frequency
                // https://stackoverflow.com/questions/20081286/controlling-a-servo-with-raspberry-pi-using-the-hardware-pwm-with-wiringpi

                PinPwm = (GpioPin)Pi.Gpio[pinPwm];
                PinPwm.PinMode = GpioPinDriveMode.PwmOutput;
                PinPwm.PwmMode = PwmMode.MarkSign;

                PinPwm.PwmClockDivisor = PwmClockDivisor;
                PinPwm.PwmRange = PwmRange;
                PinPwm.PwmRegister = PwmRegister;

            }
        }


        /// <summary>
        ///  Let's say that's M/S
        /// </summary>
        /// <param name="speed" range=0-100></param>
        public void SetSpeed(int speed)
        {

            Forward = speed > 0;
            // set as appropriate
            Reverse = !Forward;

            kFactor = (MaxDuty - MinDuty) / 100.0;
            Speed = speed;
            if (Forward)
            {
                Speed = Math.Min(Speed, 100);
            }
            else
            {
                Speed = Math.Max(Speed, -100);
            }
            var pwmDuty = MinDuty + (Math.Abs(Speed) * kFactor);
            DutyCycle = (uint)Math.Min(MaxDuty, pwmDuty);

            Forward = speed > 0;
            // set as appropriate
            Reverse = !Forward;

            if (speed == 0)
            {
                Forward = false;
                Reverse = false;
            }
            if (!Testing)
            {
                PinPwm.PwmRegister = (int)DutyCycle;
                PinFwd.Write(Forward);
                PinRev.Write(Reverse);
            }

        }
    }
}



