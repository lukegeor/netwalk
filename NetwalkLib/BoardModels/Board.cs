using System;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;

namespace NetwalkLib
{
    public class Board
    {
        private static readonly char[] _glyphs = new char[]
        {
            '┼', // 0
            '┬', // 1
            '┤', // 2
            '┐', // 3
            '┴', // 4
            '─', // 5
            '┘', // 6
            '<', // 7
            '├', // 8
            '┌', // 9
            '│', // 10
            'v', // 11
            '└', // 12
            '>', // 13
            '^', // 14
            'O' // 15
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
                    var sum = Spots[row, col].Walls.Select((w, i) => w != null ? 1 << i : 0).Sum();
                    s.Append(_glyphs[sum]);
                }
                s.Append(Environment.NewLine);
            }

            return s.ToString();
        }
    }
}