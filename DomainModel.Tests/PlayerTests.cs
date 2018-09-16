using CowsAndChicken.DomainModel;
using Moq;
using System.Threading;
using Xunit;

namespace CowsAndChicken.DomainModel.Tests
{
    public class PlayerTests
    {
        [Fact]
        public void Player_WhenNoGamesPlayed_NoStats()
        {
            var player = new Player("Annejan", 35);

            Assert.Empty(player.GetStatistics<PlayerEfficiencyStats>().Stats);
            Assert.Null(player.GetStatistics<PlayerSpeedStats>().Stats);
        }

        [Fact]
        public void Player_WhenGamePlayed_StatsAvailable()
        {
            var player = new Player("Annejan", 35);
            var generator = new Mock<INumberGenerator>();
            generator.Setup(g => g.Generate(It.Is<int>(i => i == 4))).Returns(() => 1234);
            player.StartGame(4, generator.Object);

            player.TakeTurn(1234);

            var efficiency = player.GetStatistics<PlayerEfficiencyStats>();
            var speed = player.GetStatistics<PlayerSpeedStats>();

            Assert.NotEmpty(efficiency.Stats);
            Assert.Single(efficiency.Stats);
            Assert.NotNull(efficiency.Stats[4]);
            Assert.NotNull(speed.Stats);
        }

        [Fact]
        public void Player_WhenNextGamePlayed_StatsChange()
        {
            var player = new Player("Annejan", 35);
            var generator = new Mock<INumberGenerator>();
            generator.Setup(g => g.Generate(It.Is<int>(i => i == 4))).Returns(() => 1234);
            player.StartGame(4, generator.Object);
            player.TakeTurn(1234);

            var efficiency = player.GetStatistics<PlayerEfficiencyStats>();
            var speed = player.GetStatistics<PlayerSpeedStats>();

            var avg = efficiency.Stats[4].Average;
            var savg = speed.Stats.Average;

            player.StartGame(4, generator.Object);
            player.TakeTurn(1233);
            Thread.Sleep(200);
            player.TakeTurn(1234);

            Assert.NotEqual(avg, efficiency.Stats[4].Average);
            Assert.NotEqual(avg, speed.Stats.Average);
        }

        [Fact]
        public void Player_WhenGameWithDifferentLengthPlayed_StatsChange()
        {
            var player = new Player("Annejan", 35);
            var generator = new Mock<INumberGenerator>();
            generator.Setup(g => g.Generate(It.Is<int>(i => i == 4))).Returns(() => 1234);
            generator.Setup(g => g.Generate(It.Is<int>(i => i == 5))).Returns(() => 12345);
            player.StartGame(4, generator.Object);
            player.TakeTurn(1234);
            player.StartGame(5, generator.Object);
            player.TakeTurn(12345);

            var efficiency = player.GetStatistics<PlayerEfficiencyStats>();
            var speed = player.GetStatistics<PlayerSpeedStats>();

            Assert.NotNull(efficiency.Stats[4]);
            Assert.NotNull(efficiency.Stats[5]);
        }
    }
}
