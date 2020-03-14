using System;
using System.Linq;
using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;
using NetwalkLib;
using Xunit;
using Xunit.Abstractions;

namespace NetwalkLibTests
{
    public class GameTests
    {
        private readonly Game _game;
        private readonly Mock<IBoardGenerator> _boardGenerator;
        private readonly IFixture _fixture;
        private bool _gameWon;
        
        public GameTests()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _boardGenerator = _fixture.Freeze<Mock<IBoardGenerator>>();
            _fixture.Inject(new GameConfig() { Height = 3, Width = 3 });
            _boardGenerator
                .Setup(bg => bg.GenerateBoard(It.IsAny<GameConfig>()))
                .Returns((
                    new Board(3, 3, new[,] {{9, 9, 9}, {9, 9, 9}, {9, 9, 9}}),
                    new Board(3, 3, new[,] {{3, 3, 3}, {3, 3, 3}, {3, 3, 3}})));
            _game = _fixture.Create<Game>();
            _game.StartGame();
            _gameWon = false;
            _game.GameWonEvent += GameWon;
        }
        
        #region Constructor
        [Fact]
        public void Game_Constructor_Nominal()
        {
            var now = DateTime.UtcNow;
            // Assert
            _boardGenerator.Verify(bg => bg.GenerateBoard(It.IsAny<GameConfig>()));
            Assert.NotNull(_game.SolvedBoard);
            Assert.NotNull(_game.PlayingBoard);
            Assert.True(now - _game.StartTime < TimeSpan.FromSeconds(5));
            Assert.False(_gameWon);
        }
        #endregion
        
        #region RotateCell
        [Theory]
        [InlineData(0, 0)]
        [InlineData(1, 8)]
        [InlineData(2, 1)]
        [InlineData(3, 9)]
        [InlineData(4, 2)]
        [InlineData(5, 10)]
        [InlineData(6, 3)]
        [InlineData(7, 11)]
        [InlineData(8, 4)]
        [InlineData(9, 12)]
        [InlineData(10, 5)]
        [InlineData(11, 13)]
        [InlineData(12, 6)]
        [InlineData(13, 14)]
        [InlineData(14, 7)]
        [InlineData(15, 15)]
        public void Game_RotateCell_Nominal(int preRotated, int postRotatedExpected)
        {
            // Arrange
            _game.PlayingBoard.Cells[0, 0] = preRotated;
            
            // Act
            _game.RotateCell(0, 0);
            
            // Assert
            Assert.Equal(postRotatedExpected, _game.PlayingBoard.Cells[0, 0]);
            Assert.False(_gameWon);
        }

        [Fact]
        public void Game_RotateCell_GameWon()
        {
            // Arrange
            _game.GameWonEvent -= GameWon;
            _boardGenerator.Reset();
            _boardGenerator
                .Setup(bg => bg.GenerateBoard(It.IsAny<GameConfig>()))
                .Returns((
                    new Board(3, 3, new[,] {{9, 9, 9}, {9, 9, 9}, {9, 9, 9}}),
                    new Board(3, 3, new[,] {{6, 14, 8}, {5, 7, 8}, {1, 3, 1}})));

            var game = new Game(_boardGenerator.Object, new GameConfig{ Height = 3, Width = 3});
            game.StartGame();
            game.GameWonEvent += GameWon;
            Assert.False(_gameWon);
            
            // Act
            game.RotateCell(2, 2);
            
            // Assert
            Assert.True(_gameWon);
        }

        [Fact]
        public void Game_RotateCell_GameWonButUnsubscribed()
        {
            _game.GameWonEvent -= GameWon;
            // Act
            for (var row = 0; row < _game.PlayingBoard.Height; row++)
            {
                for (var col = 0; col < _game.PlayingBoard.Width; col++)
                {
                    Assert.False(_gameWon);
                    _game.RotateCell(row, col);
                }
            }
            
            // Assert
            Assert.False(_gameWon);
        }
        #endregion

        private void GameWon(object sender, GameWonEventArgs e)
        {
            _gameWon = true;
        }
    }
}
