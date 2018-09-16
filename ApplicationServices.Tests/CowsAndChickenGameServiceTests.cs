using CowsAndChicken.ApplicationServices;
using System;
using Xunit;

namespace ApplicationServices.Tests
{
    public class CowsAndChickenGameServiceTests
    {
        [Fact]
        public void CowsAndChickenGameService_OnPassingNullsToConstructor_ExceptionThrown()
        {
            Assert.Throws<ArgumentNullException>(() => new CowsAndChickenGameService(null, null));
        }

        // TODO: Add more tests
    }
}
