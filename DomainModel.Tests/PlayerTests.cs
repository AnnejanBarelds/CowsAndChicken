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

            Assert.Empty(player.Efficiency.Stats);
            Assert.Null(player.Speed.Stats);
        }

        [Fact]
        public void Player_WhenGamePlayed_StatsAvailable()
        {
            var player = new Player("Annejan", 35);
            var generator = new Mock<INumberGenerator>();
            generator.Setup(g => g.Generate(It.Is<int>(i => i == 4))).Returns(() => 1234);
            player.StartGame(4, generator.Object);

            player.TakeTurn(1234);

            Assert.NotEmpty(player.Efficiency.Stats);
            Assert.Single(player.Efficiency.Stats);
            Assert.NotNull(player.Efficiency.Stats[4]);
            Assert.NotNull(player.Speed.Stats);
        }

        [Fact]
        public void Player_WhenNextGamePlayed_StatsChange()
        {
            var player = new Player("Annejan", 35);
            var generator = new Mock<INumberGenerator>();
            generator.Setup(g => g.Generate(It.Is<int>(i => i == 4))).Returns(() => 1234);
            player.StartGame(4, generator.Object);
            player.TakeTurn(1234);
            var avg = player.Efficiency.Stats[4].Average;
            var savg = player.Speed.Stats.Average;

            player.StartGame(4, generator.Object);
            player.TakeTurn(1233);
            Thread.Sleep(200);
            player.TakeTurn(1234);

            Assert.NotEqual(avg, player.Efficiency.Stats[4].Average);
            Assert.NotEqual(avg, player.Speed.Stats.Average);
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

            Assert.NotNull(player.Efficiency.Stats[4]);
            Assert.NotNull(player.Efficiency.Stats[5]);
        }
    }
}
