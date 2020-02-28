using System;
using System.Text;

namespace NetwalkLib
{
    public class Board
    {
        private static readonly char[] Glyphs =
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

        public int[,] Cells { get; }

        public int Height { get; }

        public int Width { get; }

        internal Board(int height, int width, int[,] cells)
        {
            if (cells.GetLength(0) != height)
            {
                throw new ArgumentException($"{nameof(height)} does not match {nameof(cells)} height.");
            }
            
            if (cells.GetLength(1) != width)
            {
                throw new ArgumentException($"{nameof(width)} does not match {nameof(cells)} width.");
            }

            Height = height;
            Width = width;
            Cells = cells;
        }
        
        public static int Rotate(int original)
        {
            return (original >> 1) | ((original & 1) << 3);
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
                        s.Append(Glyphs[Cells[row, col]]);
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
                    if (Cells[row, col] != other.Cells[row, col])
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Height, Width, Cells);
        }
    }
}