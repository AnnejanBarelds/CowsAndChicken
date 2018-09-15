using CowsAndChicken.DomainModel;
using System;
using Xunit;

namespace CowsAndChicken.DomainModel.Tests
{
    public class StatsTests
    {
        [Fact]
        public void Stats_WithIntegerValues_Calculated()
        {
            int[] values = { 1, 1, 2, 3, 4, 6, 7, 8, 8, 8, 9 };

            var stats = new Stats(values);

            Assert.Equal(5.1818, stats.Average, 4);
            Assert.Equal(5.1818, stats.Mean, 4);
            Assert.Equal(6, stats.Median);
            Assert.Equal(1, stats.Min);
            Assert.Equal(9, stats.Max);
        }

        [Fact]
        public void Stats_WithDoubleValues_Calculated()
        {
            double[] values = { 0.8, 1, 2.1, 3.8, 4.9, 6.1, 7.5, 8.1, 8.7, 8.8, 9.1, 9.8 };

            var stats = new Stats(values);

            Assert.Equal(5.8917, stats.Average, 4);
            Assert.Equal(5.8917, stats.Mean, 4);
            Assert.Equal(6.8, stats.Median);
            Assert.Equal(0.8, stats.Min);
            Assert.Equal(9.8, stats.Max);
        }

        [Fact]
        public void Stats_WithEmptyList_ExceptionThrown()
        {
            Assert.Throws<ArgumentException>(() => new Stats(Array.Empty<int>()));
        }

        [Fact]
        public void Stats_WithNullList_ExceptionThrown()
        {
            Assert.Throws<ArgumentException>(() => new Stats((int[])null));
        }
    }
}
