using System;
using System.Linq;
using NetwalkLib;
using Xunit;
using Xunit.Abstractions;

namespace NetwalkLibTests
{
    public class BoardGeneratorTests
    {
        private readonly int _height;
        private readonly int _width;
        private readonly GameConfig _gameConfig;
        private readonly BoardGenerator _boardGenerator;
        
        public BoardGeneratorTests()
        {
            _height = 9;
            _width = 9;
            _gameConfig = new GameConfig {Height = _height, Width = _width};
            _boardGenerator = new BoardGenerator();
        }
        
        #region Constructor
        [Fact]
        public void BoardGenerator_Constructor_Nominal()
        {
        }
        #endregion
        
        #region GenerateBoard
        [Fact]
        public void BoardGenerator_GenerateBoard_Nominal()
        {
            for (var i = 0; i < 200; i++)
            {
                // Act
                var (solvedBoard, playingBoard) = _boardGenerator.GenerateBoard(_gameConfig);

                // Assert
                foreach (var board in new[] {solvedBoard, playingBoard})
                {
                    Assert.Equal(_height, board.Height);
                    Assert.Equal(_width, board.Width);
                    Assert.Equal(_height * _width, board.Cells.Length);
                    var nonNullCenterCellMovesCount = BoardGenerator.Moves(board.Cells[_height / 2, _width / 2]);
                    Assert.True(nonNullCenterCellMovesCount >= 2);
                    Assert.True(nonNullCenterCellMovesCount <= 3);
                }

                for (var row = 0; row < solvedBoard.Height; row++)
                {
                    for (var col = 0; col < solvedBoard.Width; col++)
                    {
                        Assert.Equal(BoardGenerator.Moves(solvedBoard.Cells[row, col]), BoardGenerator.Moves(playingBoard.Cells[row, col]));
                    }
                }
            }
        }

        [Fact]
        public void BoardGenerator_GenerateBoard_WithRandomSeed()
        {
            // Arrange
            var gameConfig = new GameConfig {Height = _height, Width = _width, RandomSeed = 9};
            
            // Act
            // Nothing really to test here except that the method doesn't throw
            _boardGenerator.GenerateBoard(gameConfig);
        }
        #endregion
    }
}
