using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Moq;
using Unosquare.RaspberryIO.Abstractions;

using Unosquare.WiringPi;
using yomo.Navigation;
using Unosquare.RaspberryIO;
using static yomo.Utility.Settings;


namespace YoGoTester
{
    
    public class WheelTests
    {
        int PinFwd = 6;
        int PinRev = 5;
        int PinPwm = 13;
        IWheel left;


        [Fact]
        public void Wheel_SetSpeed_Forward()
        {
            yomo.Utility.Settings.Testing = true;

            left = new Wheel(PinPwm, PinFwd, PinRev);

            left.SetSpeed(20);
            Assert.True(left.Forward);
            Assert.False(left.Reverse);

        }
        [Fact]
        public void Wheel_SetSpeed_Reverse()
        {
            yomo.Utility.Settings.Testing = true;

            left = new Wheel(PinPwm, PinFwd, PinRev);

            left.SetSpeed(-20);
            Assert.False(left.Forward);
            Assert.True(left.Reverse);

        }
        [Fact]
        public void Wheel_SetSpeed_Stop()
        {
            yomo.Utility.Settings.Testing = true;

            left = new Wheel(PinPwm, PinFwd, PinRev);

            left.SetSpeed(0);
            Assert.False(left.Forward);
            Assert.False(left.Reverse);
        }

        [Fact]
        public void Wheel_SetSpeed_50()
        {
            yomo.Utility.Settings.Testing = true;

            left = new Wheel(PinPwm, PinFwd, PinRev);

            left.SetSpeed(50);
            Assert.True(left.Forward);
            Assert.False(left.Reverse);
            Assert.Equal(50, left.Speed);
        }

        [Fact]
        public void Wheel_SetSpeed_50Reverse()
        {
            yomo.Utility.Settings.Testing = true;

            left = new Wheel(PinPwm, PinFwd, PinRev);

            left.SetSpeed(-50);
            Assert.False(left.Forward);
            Assert.True(left.Reverse);
            Assert.Equal(-50, left.Speed);
        }

        [Fact]
        public void Wheel_SetSpeed_Overspeed()
        {
            yomo.Utility.Settings.Testing = true;

            left = new Wheel(PinPwm, PinFwd, PinRev);

            left.SetSpeed(150);
            Assert.True(left.Forward);
            Assert.False(left.Reverse);
            Assert.Equal(100, left.Speed);
        }
        [Fact]
        public void Wheel_SetSpeed_OverspeedReverse()
        {
            left = new Wheel(PinPwm, PinFwd, PinRev);

            left.SetSpeed(-150);
            Assert.False(left.Forward);
            Assert.True(left.Reverse);
            Assert.Equal(-100, left.Speed);
        }

        [Fact]
        public void Wheel_SetSpeed_CalculatedDutyCycle50()
        {
            yomo.Utility.Settings.Testing = true;

            left = new Wheel(PinPwm, PinFwd, PinRev);

            left.SetSpeed(50);
            Assert.True(left.Forward);
            Assert.False(left.Reverse);
            Assert.Equal(50, left.Speed) ;
            Assert.Equal((50 * 8.24 + 200), left.DutyCycle);
        }

        [Fact]
        public void Wheel_SetSpeed_CalculatedDutyCycle100()
        {
            yomo.Utility.Settings.Testing = true;

            left = new Wheel(PinPwm, PinFwd, PinRev);

            left.SetSpeed(100);
            Assert.True(left.Forward);
            Assert.False(left.Reverse);
            Assert.Equal(100, left.Speed);
            Assert.Equal((1024), left.DutyCycle);
        }
    }
}
