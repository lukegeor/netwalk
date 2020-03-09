using System;

namespace NetwalkLib
{
    public class Game : IGame
    {
        private bool _won;

        public DateTime StartTime { get; }

        public Board SolvedBoard { get; }
        public Board PlayingBoard { get; }

        public event EventHandler<GameWonEventArgs> GameWonEvent;
        
        public Game(IBoardGenerator boardGenerator, GameConfig gameConfig)
        {
            StartTime = DateTime.UtcNow;
            (SolvedBoard, PlayingBoard) = boardGenerator.GenerateBoard(gameConfig);
            _won = false;
        }

        public void RotateCell(int row, int col)
        {
            PlayingBoard.Cells[row, col] = Board.Rotate(PlayingBoard.Cells[row, col]);
            if (!_won && CheckForWin())
            {
                GameWonEvent?.Invoke(this, new GameWonEventArgs(DateTime.UtcNow - StartTime));
                _won = true;
            }
        }

        private bool CheckForWin()
        {
            var active= PlayingBoard.GetActive();
            for (var row = 0; row < active.GetLength(0); row++)
            {
                for (var col = 0; col < active.GetLength(1); col++)
                {
                    if (!active[row, col])
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}