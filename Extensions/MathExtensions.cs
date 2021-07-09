using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Extensions
{
    public static class MathExtensions
    {
        public static double? StdDev(this IEnumerable<double> values, bool useNormalStdDev, int minNumberOfEntries = 10)
        {
            if (values.Count() < minNumberOfEntries)
                return null;

            if (useNormalStdDev)
                return values.NormalStdDev();
            else
                return values.SampleStdDev();
        }

        /// <summary>
        /// SAMPLE Std Dev
        /// https://stackoverflow.com/questions/2253874/standard-deviation-in-linq  
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static double SampleStdDev(this IEnumerable<double> values)
        {

            // ref: http://warrenseen.com/blog/2006/03/13/how-to-calculate-standard-deviation/
            double mean = 0.0;
            double sum = 0.0;
            double stdDev = 0.0;
            int n = 0;
            foreach (double val in values)
            {
                n++;
                double delta = val - mean;
                mean += delta / n;
                sum += delta * (val - mean);
            }
            if (1 < n)
                stdDev = Math.Sqrt(sum / (n - 1));

            return stdDev;
        }

        /// <summary>
        /// NORMAL Std Dev
        /// https://stackoverflow.com/questions/2253874/standard-deviation-in-linq  
        /// Divide by n instead of divide by n-1
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static double NormalStdDev(this IEnumerable<double> values)
        {
            // ref: http://warrenseen.com/blog/2006/03/13/how-to-calculate-standard-deviation/
            double mean = 0.0;
            double sum = 0.0;
            double stdDev = 0.0;
            int n = 0;
            foreach (double val in values)
            {
                n++;
                double delta = val - mean;
                mean += delta / n;
                sum += delta * (val - mean);
            }
            if (1 < n)
                stdDev = Math.Sqrt(sum / n);

            return stdDev;
        }

        //If a data distribution is approximately normal then about 68 percent of the data values are 
        // within one standard deviation of the mean (mathematically, μ ± σ, where μ is the arithmetic mean), 
        // about 95 percent are within two standard deviations (μ ± 2σ), and about 99.7 percent lie within 
        // three standard deviations (μ ± 3σ). This is known as the 68-95-99.7 rule, or the empirical rule.
        

        /// <summary>
        /// Returns the value from the list that is the largest value
        /// within the subset of <see cref="percentile"/> values sorted by
        /// smallest to largest.
        /// </summary>
        /// <remarks>
        /// To get the value, the list is sorted from smallest to largest. 
        /// Then the index of the <see cref="percentile"/> entry is 
        /// calculated and the and the value just before that one is returned.
        /// </remarks>
        /// <param name="values"></param>
        /// <param name="percentile"></param>
        /// <returns></returns>
        public static int? PercentileValue(this IEnumerable<double> values, double percentile, int minNumberOfEntries = 10)
        {
            if (values == null)
            {
                return null;
            }

            if(values.Count() < minNumberOfEntries)
            {
                return null;
            }
            var sortedValues = values.OrderBy(v => v);

            int ordinal = (int)Math.Floor(values.Count() * percentile);
            return (int?)values.ElementAt(ordinal);
        }
    }
}
