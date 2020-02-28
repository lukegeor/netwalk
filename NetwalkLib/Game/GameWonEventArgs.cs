using System;

namespace NetwalkLib
{
    public class GameWonEventArgs : EventArgs
    {
        public TimeSpan WinTime { get; }
        
        public GameWonEventArgs(TimeSpan winTime)
        {
            WinTime = winTime;
        }
    }
}