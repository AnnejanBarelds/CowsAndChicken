using MathNet.Numerics.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CowsAndChicken.DomainModel
{
    public class Stats
    {
        public double Average { get; private set; }

        public double Mean { get; private set; }

        public double Median { get; private set; }

        public double Min { get; private set; }

        public double Max { get; private set; }

        public Stats(IEnumerable<double> values)
        {
            if (values == null || values.Count() == 0)
            {
                throw new ArgumentException("Must be a non-null, non-empty list", nameof(values));
            }

            // Average and mean are the same thing; what was meant here? Included the median as well for now, pending further discussion
            Average = values.Average();
            Mean = values.Mean();
            Median = values.Median();
            Min = values.Min();
            Max = values.Max();
        }

        public Stats(IEnumerable<int> values) :this(values?.Select(i => (double)i)) { }
    }
}
