using System;
using System.Text;

namespace NetwalkLib
{
    public class Board
    {
        private static readonly char[] Glyphs = new char[]
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

        public int[,] Spots { get; }

        public int Height { get; }

        public int Width { get; }

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

            Height = height;
            Width = width;
            Spots = spots;
        }
        
        public override string ToString()
        {
            var s = new StringBuilder();
            for (var row = 0; row < Height; row++)
            {
                for (var col = 0; col < Width; col++)
                {
                    if (row == Height / 2 && col == Width / 2)
                    {
                        s.Append('C');
                    }
                    else
                    {
                        s.Append(Glyphs[Spots[row, col]]);
                    }
                }
                s.Append(Environment.NewLine);
            }

            return s.ToString();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Board other))
            {
                return false;
            }

            if (Height != other.Height || Width != other.Width)
            {
                return false;
            }

            for (var row = 0; row < Height; row++)
            {
                for (var col = 0; col < Width; col++)
                {
                    if (Spots[row, col] != other.Spots[row, col])
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Height, Width, Spots);
        }
    }
}