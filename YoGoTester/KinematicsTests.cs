using System;
using System.Collections.Generic;
using System.Text;
using yomo.Navigation;
using Xunit;

namespace YoGoTester
{
    public class KinematicsTests
    {
        [Fact]
        public void Kinematics_Drive_Forward()
        {

            yomo.Utility.Settings.Testing = true;
            var k = new Kinematics();

            k.Drive(50, 50);
            Assert.Equal(50, k.left.Speed);
            Assert.Equal(50, k.right.Speed);
        }

        [Fact]
        public void Kinematics_CalculateSpeed_ALittleFaster()
        {

            yomo.Utility.Settings.Testing = true;
            var k = new Kinematics();

            TimeSpan ts = new TimeSpan(10);

            var newSpeed = k.CalculateSpeed(50, 49, ts);
            Assert.True(newSpeed > 49);

        }
        [Fact]
        public void Kinematics_CalculateSpeed_ALotFaster()
        {

            yomo.Utility.Settings.Testing = true;
            var k = new Kinematics();

            TimeSpan ts = new TimeSpan(10);

            var newSpeed = k.CalculateSpeed(50, 25, ts);
            Assert.True(newSpeed > 25);

        }
        [Fact]
        public void Kinematics_CalculateSpeed_ALittleSlower()
        {

            yomo.Utility.Settings.Testing = true;
            var k = new Kinematics();

            TimeSpan ts = new TimeSpan(10);

            var newSpeed = k.CalculateSpeed(50, 51, ts);
            Assert.True(newSpeed < 51);
        }

        [Fact]
        public void Kinematics_CalculateSpeed_ALotSlower()
        {

            yomo.Utility.Settings.Testing = true;
            var k = new Kinematics();

            TimeSpan ts = new TimeSpan(10);

            var newSpeed = k.CalculateSpeed(50, 75, ts);
            Assert.True(newSpeed < 75);
        }


        [Fact]
        public void Kinematics_CalculateSpeed_Slower_WarmedUp()
        {

            yomo.Utility.Settings.Testing = true;
            var k = new Kinematics();

            TimeSpan ts = new TimeSpan(10);

            var newSpeed = k.CalculateSpeed(50, 75, ts);
            newSpeed = k.CalculateSpeed(50, newSpeed, ts);
            Assert.True(newSpeed < 75);
        }

        [Fact]
        public void Kinematics_CalculateSpeed_Faster_WarmedUp()
        {

            yomo.Utility.Settings.Testing = true;
            var k = new Kinematics();

            TimeSpan ts = new TimeSpan(10);

            var newSpeed = k.CalculateSpeed(50, 25, ts);
            newSpeed = k.CalculateSpeed(50, newSpeed, ts);
            Assert.True(newSpeed > 25);
        }
    }
}
