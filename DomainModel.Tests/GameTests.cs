using CowsAndChicken.DomainModel;
using Moq;
using Xunit;

namespace CowsAndChicken.DomainModel.Tests
{
    public class GameTests
    {
        [Fact]
        public void Game_OnCorrectGuess_RaisesEvent()
        {
            object sender = null;

            var generator = new Mock<INumberGenerator>();
            generator.Setup(g => g.Generate(It.Is<int>(i => i == 4))).Returns(() => 1234);
            var game = new Game(4, generator.Object);
            game.GameCompleted += (o, e) => sender = o;
            game.ProcessTurn(1234);

            generator.Verify();
            Assert.Same(sender, game);
        }

        [Fact]
        public void Game_OnInCorrectGuess_DoesNotRaiseEvent()
        {
            object sender = null;

            var generator = new Mock<INumberGenerator>();
            generator.Setup(g => g.Generate(It.Is<int>(i => i == 4))).Returns(() => 1234);
            var game = new Game(4, generator.Object);
            game.GameCompleted += (o, e) => sender = o;
            game.ProcessTurn(1233);

            generator.Verify();
            Assert.Null(sender);
        }

        [Fact]
        public void Game_OnCorrectGuess_IsCompleted()
        {
            var generator = new Mock<INumberGenerator>();
            generator.Setup(g => g.Generate(It.Is<int>(i => i == 4))).Returns(() => 1234);
            var game = new Game(4, generator.Object);

            game.ProcessTurn(1234);

            generator.Verify();
            Assert.Equal(GameStatus.Completed, game.Status);
        }

        [Fact]
        public void Game_OnInCorrectGuess_IsStarted()
        {
            var generator = new Mock<INumberGenerator>();
            generator.Setup(g => g.Generate(It.Is<int>(i => i == 4))).Returns(() => 1234);
            var game = new Game(4, generator.Object);

            game.ProcessTurn(1233);

            generator.Verify();
            Assert.Equal(GameStatus.Started, game.Status);
        }

        [Fact]
        public void Game_OnGuess_ReturnsCorrectResult()
        {
            var generator = new Mock<INumberGenerator>();
            generator.Setup(g => g.Generate(It.Is<int>(i => i == 4))).Returns(() => 1234);
            var game = new Game(4, generator.Object);

            var result = game.ProcessTurn(1233);

            generator.Verify();
            Assert.Equal(3, result.NrOfCows);
            Assert.Equal(1, result.NrOfChickens);
        }
    }
}
