using NetwalkLib;
using Xunit;
using Xunit.Abstractions;

namespace NetwalkLibTests
{
    public class BoardGeneratorTests
    {
        private readonly GameConfig _gameConfig;
        private readonly BoardGenerator _boardGenerator;
        private readonly ITestOutputHelper _testOutputHelper;
        
        public BoardGeneratorTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
            _gameConfig = new GameConfig {Height = 5, Width = 5};
            _boardGenerator = new BoardGenerator(_gameConfig);
        }
        
        [Fact]
        public void BoardGenerator_GenerateBoard_ToString()
        {
            var board = _boardGenerator.GenerateBoard();
            _testOutputHelper.WriteLine(board.ToString());
            for (var row = 0; row < _gameConfig.Height; row++)
            {
                for (var col = 0; col < _gameConfig.Width; col++)
                {
                    _testOutputHelper.WriteLine(board.Spots[row, col].ToString());
                }
            }
        }
    }
}
