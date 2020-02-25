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
            'O', // 0
            '^', // 1
            '>', // 2
            '└', // 3
            'v', // 4
            '│', // 5
            '┌', // 6
            '├', // 7
            '<', // 8
            '┘', // 9
            '─', // 10
            '┴', // 11
            '┐', // 12
            '┤', // 13
            '┬', // 14
            '┼', // 15
        };

        private readonly int _height;
        private readonly int _width;
        private readonly int[,] _spots;

        public int[,] Spots => _spots;

        public int Height => _height;
        
        public int Width => _width;

        internal Board(int height, int width, int[,] spots)
        {
            if (spots.GetLength(0) != height)
            {
                throw new ArgumentException($"{nameof(height)} does not match {nameof(spots)} height.");
            }
            
            if (spots.GetLength(1) != width)
            {
                throw new ArgumentException($"{nameof(width)} does not match {nameof(spots)} width.");
            }

            _height = height;
            _width = width;
            _spots = spots;
        }

        public override string ToString()
        {
            StringBuilder s = new StringBuilder();
            for (int row = 0; row < _height; row++)
            {
                for (int col = 0; col < _width; col++)
                {
                    s.Append(_glyphs[Spots[row, col]]);
                }
                s.Append(Environment.NewLine);
            }

            return s.ToString();
        }
    }
}