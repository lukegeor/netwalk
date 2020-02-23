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
            _gameConfig = new GameConfig {Height = 1, Width = 1};
            _boardGenerator = new BoardGenerator(_gameConfig);
        }
        
        [Fact]
        public void BoardGenerator_GenerateBoard_ToString()
        {
            var board = _boardGenerator.GenerateBoard();
            for (int row = 0; row < _gameConfig.Height; row++)
            {
                for (int col = 0; col < _gameConfig.Width; col++)
                {
                    _testOutputHelper.WriteLine(board.Spots[row, col].ToString());
                }
            }
        }
    }
}
