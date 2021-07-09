using System;
using Xunit;
using Extensions;
using System.Threading;

namespace Extensions.Test
{
    public class DateTimeExtensions_Tests
    {

        [Theory]
        [InlineData(200)]
        [InlineData(1000)]
        [InlineData(4000)]
        public void DateTimeExtensions_Test1(int value)
        {
            
            DateTime dt = DateTime.UtcNow;
            Thread.Sleep(value);
            DateTime dt2 = DateTime.UtcNow;

            string elapsedTime = dt.GetElapsedSecondsForDisplay();
            int totalTime = int.Parse(elapsedTime.Substring(0,elapsedTime.IndexOf(".")));

            Assert.Equal(totalTime, value / 1000 );
        }



    }
}
