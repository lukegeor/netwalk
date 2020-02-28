using System;

namespace NetwalkLib
{
    public class Game
    {
        public DateTime StartTime { get; }

        public Board SolvedBoard { get; }
        public Board PlayingBoard { get; }

        public event EventHandler<GameWonEventArgs> GameWonEvent;
        
        public Game(IBoardGenerator boardGenerator, GameConfig gameConfig)
        {
            StartTime = DateTime.UtcNow;
            (SolvedBoard, PlayingBoard) = boardGenerator.GenerateBoard(gameConfig);
        }

        public void RotateCell(int row, int col)
        {
            PlayingBoard.Cells[row, col] = Board.Rotate(PlayingBoard.Cells[row, col]);
            if (CheckForWin())
            {
                GameWonEvent?.Invoke(this, new GameWonEventArgs(DateTime.UtcNow - StartTime));
            }
        }

        private bool CheckForWin()
        {
            return SolvedBoard.Equals(PlayingBoard);
        }
    }
}