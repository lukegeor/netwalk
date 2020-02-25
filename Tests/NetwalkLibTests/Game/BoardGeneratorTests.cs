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
            // Act
            var board = _boardGenerator.GenerateBoard();
            
            // Assert
            Assert.Equal(_height, board.Height);
            Assert.Equal(_width, board.Width);
            Assert.Equal(_width * _width, board.Spots.Length);
            var nonNullWallsCount = board.Spots[_height / 2, _width / 2].Walls.Where(w => w != null).Count();
            Assert.True(nonNullWallsCount <= 2);
        }
        #endregion
    }
}
