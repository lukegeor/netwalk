using System;
using System.Security.Principal;
using System.Text;
using System.Threading;

namespace NetwalkLib
{
    public class Board
    {
        private static readonly char[] _glyphs = new char[]
        {
            (char)197,
            (char)195,
            (char)193,
            (char)192,
            (char)180,
            179,
            217,
            
            'X'
        };
        private int _height;
        private int _width;
        
        public Spot[,] Spots { get; }

        internal Board(int height, int width)
        {
            _height = height;
            _width = width;
            Spots = new Spot[height, width];
        }

        public override string ToString()
        {
            StringBuilder s = new StringBuilder();
            for (int row = 0; row < _height; row++)
            {
                for (int col = 0; col < _width; col++)
                {
                    int sum = 0;
                    sum <<= 1;
                    sum += (Spots[row, col].Walls[(int) WallLocation.Top] != null ? 1 : 0);
                    sum <<= 1;
                    sum += (Spots[row, col].Walls[(int) WallLocation.Right] != null ? 1 : 0);
                    sum <<= 1;
                    sum += (Spots[row, col].Walls[(int) WallLocation.Bottom] != null ? 1 : 0);
                    sum <<= 1;
                    sum += (Spots[row, col].Walls[(int) WallLocation.Left] != null ? 1 : 0);
                    
                }
                s.Append(Environment.NewLine);
            }

            return s.ToString();
        }
    }
}