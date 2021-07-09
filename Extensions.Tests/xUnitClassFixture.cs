using System;
using System.Collections.Generic;
using System.Text;

namespace Extensions.Test
{
    public class xUnitClassFixture : IDisposable
    {
        public int iterationInstance { get; set; }

        public xUnitClassFixture()
        {
            iterationInstance = 0;
        }

        public void incrementIteration()
        {
            iterationInstance++;
        }

        public void Dispose()
        {
            // Clean up global objects
        }
    }
}
