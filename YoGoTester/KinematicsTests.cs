using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using yomo.Navigation;

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
        public void Kinematics_CalculateSpeed_Faster()
        {

            yomo.Utility.Settings.Testing = true;
            var k = new Kinematics();

            TimeSpan ts = new TimeSpan(10);
                //DateTime.Now - (DateTime.Now - 1);
            var newSpeed = k.CalculateSpeed(50, 49, ts);
            //todo: not sure what these numbers should be, but i'm pretty sure they're wrong.  the above returns -180, which i think slow the robot
            //event though it's already going slower than desired.  and that's a clamped value from the 250k that was calculated.
            //it appears to be because the processVariableLast is not set when running this way.  it doesn't seem like 
            //it will be healthy to have the the pid controller depending on having a warmed up speed value variance.  and it seems backward.
            //Assert.Equal(50, k.left.Speed);
            //Assert.Equal(50, k.right.Speed);
        }
    }
}
