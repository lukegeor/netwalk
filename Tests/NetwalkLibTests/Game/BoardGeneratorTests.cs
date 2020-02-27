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
        private readonly ITestOutputHelper _testOutputHelper;
        
        public BoardGeneratorTests(ITestOutputHelper testOutputHelper)
        {
            _height = 9;
            _width = 9;
            _testOutputHelper = testOutputHelper;
            _gameConfig = new GameConfig {Height = _height, Width = _width};
            _boardGenerator = new BoardGenerator(_gameConfig);
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
            //for (var i = 0; i < 200; i++)
            {
                // Act
                var board = _boardGenerator.GenerateBoard();
                _testOutputHelper.WriteLine(board.ToString());

                // Assert
                Assert.Equal(_height, board.Height);
                Assert.Equal(_width, board.Width);
                Assert.Equal(_height * _width, board.Spots.Length);
                var nonNullCenterSpotMovesCount = BoardGenerator.Moves(board.Spots[_height / 2, _width / 2]);
                Assert.True(nonNullCenterSpotMovesCount >= 2);
                Assert.True(nonNullCenterSpotMovesCount <= 3);
            }
        }
        #endregion
        
        #region RotateBoard
        [Fact]
        public void BoardGenerator_RotateBoard_Nominal()
        {
            // Act
            var board = _boardGenerator.GenerateBoard();
            var rotatedBoard = _boardGenerator.RotateBoard(board);
            
            // Assert
            Assert.Equal(board.Height, rotatedBoard.Height);
            Assert.Equal(board.Width, rotatedBoard.Width);
            for (var row = 0; row < board.Height; row++)
            {
                for (var col = 0; col < board.Width; col++)
                {
                    Assert.Equal(BoardGenerator.Moves(board.Spots[row, col]), BoardGenerator.Moves(rotatedBoard.Spots[row, col]));
                }
            }
        }
        #endregion
    }
}
