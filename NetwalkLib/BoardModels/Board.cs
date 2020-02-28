using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace NetwalkLib
{
    public class Board : IBoard
    {
        internal static readonly char[] Glyphs =
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
            'Θ', // 0
            '↑', // 1
            '→', // 2
            '╚', // 3
            '↓', // 4
            '║', // 5
            '╔', // 6
            '╠', // 7
            '←', // 8
            '╝', // 9
            '═', // 10
            '╩', // 11
            '╗', // 12
            '╣', // 13
            '╦', // 14
            '╬', // 15
        };

        public int[,] Cells { get; }

        public int Height { get; }

        public int Width { get; }

        internal Board(int height, int width, int[,] cells)
        {
            if (height % 2 == 0 || height < 3)
            {
                throw new ArgumentException($"{nameof(height)} must be an odd number >= 3.", nameof(height));
            }
            
            if (width % 2 == 0 || width < 3)
            {
                throw new ArgumentException($"{nameof(width)} must be an odd number >= 3.", nameof(width));
            }
            
            if (cells.GetLength(0) != height)
            {
                throw new ArgumentException($"{nameof(height)} does not match {nameof(cells)} height.", nameof(height));
            }
            
            if (cells.GetLength(1) != width)
            {
                throw new ArgumentException($"{nameof(width)} does not match {nameof(cells)} width.", nameof(width));
            }

            Height = height;
            Width = width;
            Cells = cells;
        }
        
        public static int Rotate(int original)
        {
            return (original >> 1) | ((original & 1) << 3);
        }

        public bool[,] GetActive()
        {
            var active = new bool[Height, Width];
            var searchStack = new Stack<(int Row, int Col)>();
            searchStack.Push((Height / 2, Width / 2));

            while (searchStack.Count > 0)
            {
                var thisCell = searchStack.Pop();
                active[thisCell.Row, thisCell.Col] = true;

                foreach (var thisCellDirection in (WallLocation[]) Enum.GetValues(typeof(WallLocation)))
                {
                    WallLocation otherCellDirection = WallLocation.Top;
                    (int Row, int Col) otherCell = thisCell;
                    switch (thisCellDirection)
                    {
                        case WallLocation.Top:
                        {
                            if (thisCell.Row == 0)
                            {
                                continue;
                            }

                            otherCellDirection = WallLocation.Bottom;
                            otherCell.Row--;
                            break;
                        }
                        case WallLocation.Right:
                        {
                            if (thisCell.Col == Width - 1)
                            {
                                continue;
                            }

                            otherCellDirection = WallLocation.Left;
                            otherCell.Col++;
                            break;
                        }
                        case WallLocation.Bottom:
                        {
                            if (thisCell.Row == Height - 1)
                            {
                                continue;
                            }

                            otherCellDirection = WallLocation.Top;
                            otherCell.Row++;
                            break;
                        }
                        case WallLocation.Left:
                        {
                            if (thisCell.Col == 0)
                            {
                                continue;
                            }

                            otherCellDirection = WallLocation.Right;
                            otherCell.Col--;
                            break;
                        }
                    }

                    var thisCellValue = Cells[thisCell.Row, thisCell.Col];
                    var otherCellValue = Cells[otherCell.Row, otherCell.Col];
                    if ((thisCellValue & (int) thisCellDirection) != 0
                        && (otherCellValue & (int) otherCellDirection) != 0
                        && (!active[otherCell.Row, otherCell.Col]))
                    {
                        searchStack.Push(otherCell);
                    }
                }
            }
            
            return active;
        }

        public override string ToString()
        {
            var s = new StringBuilder();
            var active = GetActive();
            for (var row = 0; row < Height; row++)
            {
                for (var col = 0; col < Width; col++)
                {
                    s.Append(Glyphs[Cells[row, col] | (active[row, col] ? 16 : 0 )]);
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