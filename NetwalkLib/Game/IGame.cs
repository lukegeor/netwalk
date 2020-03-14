using System;

namespace NetwalkLib
{
    public interface IGame
    {
        DateTime StartTime { get; }
        Board SolvedBoard { get; }
        Board PlayingBoard { get; }
        event EventHandler<GameWonEventArgs> GameWonEvent;
        void StartGame();
        void RotateCell(int row, int col);
    }
}