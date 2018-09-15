using CowsAndChicken.DomainModel;
using System;
using Xunit;

namespace CowsAndChicken.DomainModel.Tests
{
    public class NumberGeneratorTests
    {
        [Fact]
        public void NumberGenerator_OnLengthTooSmall_ExceptionThrown()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new NumberGenerator().Generate(3));
        }

        [Fact]
        public void NumberGenerator_OnLengthTooLarge_ExceptionThrown()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new NumberGenerator().Generate(9));
        }

        [Fact]
        public void NumberGenerator_OnAllowableLength_NumberReturned()
        {
            var generator = new NumberGenerator();

            int value1 = generator.Generate(4);
            int value2 = generator.Generate(8);

            Assert.Equal(4, value1.ToString().Length);
            Assert.Equal(8, value2.ToString().Length);
        }
    }
}
