using System;
using Xunit;
using Extensions;
using System.Threading;
using System.Collections.Generic;

namespace Extensions.Test
{
    public class MathExtensions_Tests
    {
        [Fact]
        public void MathExtensions_Test1()
        {
            // Values calculated from excel for the sequence 1,2,3,4,5,6,7,8,9,10
            // Normal Stdev	    2.872
            // Sample Stdev     3.028

            List<double> values = new List<double>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            double normalStDev = Math.Round(values.NormalStdDev(), 3);
            double sampleStDev = Math.Round(values.SampleStdDev(), 3);

            Assert.Equal(normalStDev, (double)2.872 );
            Assert.Equal(sampleStDev, (double)3.028 );
        }



    }
}
