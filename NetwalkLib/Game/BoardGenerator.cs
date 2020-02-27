using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using NetwalkLib.Utils;

namespace NetwalkLib
{
    public class BoardGenerator : IBoardGenerator
    {
        private readonly GameConfig _gameConfig;
        private readonly Random _random;

        public static int Rotate(int original)
        {
            return (original >> 1) | ((original & 1) << 3);
        }

        public BoardGenerator(GameConfig gameConfig)
        {
            _gameConfig = gameConfig;
            _random = gameConfig.RandomSeed.HasValue ? new Random(gameConfig.RandomSeed.Value) : new Random();
        }

        public Board GenerateBoard()
        {
            var height = _gameConfig.Height;
            var width = _gameConfig.Width;

            var board = new Board(height, width, new int[height, width]);

            var sets = new Dictionary<(int, int), Set<(int, int)>>();
            var removableWalls = new List<((int Row, int Col) Cell, WallLocation WallLocation)>();
            for (var row = 0; row < height; row++)
            {
                for (var col = 0; col < width; col++)
                {
                    sets.Add((row, col), new Set<(int, int)>((row, col)));
                    if (row < height - 1)
                    {
                        removableWalls.Add(((row, col), WallLocation.Bottom));
                    }
                    if (col < width - 1)
                    {
                        removableWalls.Add(((row, col), WallLocation.Right));
                    }
                }
            }
            removableWalls = removableWalls.Shuffle();
            
            // Remove 2 or 3 walls from the center cell
            var numWallsToRemoveFromCenterCell = _random.Next(2) == 0 ? 2 : 3;
            var wallsToRemoveFromCenter = new List<WallLocation>(Enum.GetValues(typeof(WallLocation))
                .Cast<WallLocation>())
                .Shuffle()
                .Take(numWallsToRemoveFromCenterCell);
            foreach (var wallLocation in wallsToRemoveFromCenter)
            {
                if (wallLocation == WallLocation.Bottom || wallLocation == WallLocation.Right)
                {
                    TryRemoveWall(
                        (height / 2, width / 2),
                        wallLocation,
                        removableWalls,
                        board.Cells,
                        sets);
                }
                else if (wallLocation == WallLocation.Left)
                {
                    TryRemoveWall(
                        (height / 2, width / 2 - 1),
                        WallLocation.Right,
                        removableWalls,
                        board.Cells,
                        sets);
                }
                else
                {
                    TryRemoveWall(
                        (height / 2 - 1, width / 2),
                        WallLocation.Bottom,
                        removableWalls,
                        board.Cells,
                        sets);
                }
            }

            // Remove as many walls as possible
            while (removableWalls.Count > 0)
            {
                if (removableWalls.First().Cell == (height / 2, width / 2) ||
                    removableWalls.First().Cell == (height / 2 - 1, width / 2) && removableWalls.First().WallLocation == WallLocation.Bottom ||
                    removableWalls.First().Cell == (height / 2, width / 2 - 1) && removableWalls.First().WallLocation == WallLocation.Right)
                {
                    removableWalls.Remove(removableWalls.First());
                }
                else
                {
                    TryRemoveWall(removableWalls.First().Cell, removableWalls.First().WallLocation, removableWalls, board.Cells, sets);
                }
            }
            
            return board;
        }

        public Board RotateBoard(Board originalBoard)
        {
            var newCells = new int[originalBoard.Height, originalBoard.Width];
            Array.Copy(originalBoard.Cells, newCells, originalBoard.Cells.Length);
            for (var row = 0; row < originalBoard.Height; row++)
            {
                for (var col = 0; col < originalBoard.Width; col++)
                {
                    var rotates = _random.Next(4);
                    for (var i = 0; i < rotates; i++)
                    {
                        newCells[row, col] = Rotate(newCells[row, col]);
                    }
                }
            }
            
            return new Board(originalBoard.Height, originalBoard.Width, newCells);
        }
        
        private bool TryRemoveWall((int Row, int Col) cell1, WallLocation wallToRemove, IList<((int, int), WallLocation)> removableWalls, int[,] cells, Dictionary<(int, int), Set<(int, int)>> sets)
        {
            if (wallToRemove == WallLocation.Top || wallToRemove == WallLocation.Left)
            {
                throw new ArgumentException("Can't pass Top or Left to TryRemoveWall");
            }

            removableWalls.Remove((cell1, wallToRemove));
            (int Row, int Col) cell2 = (cell1.Row + (wallToRemove == WallLocation.Bottom ? 1 : 0),
                cell1.Col + (wallToRemove == WallLocation.Right ? 1 : 0));
            
            if (Equals(sets[cell1], sets[cell2]))
            {
                return false;
            }

            if (Moves(cells[cell1.Row, cell1.Col]) == 3
                || Moves(cells[cell2.Row, cell2.Col]) == 3)
            {
                return false;
            }

            cells[cell1.Row, cell1.Col] |= (int) wallToRemove;
            cells[cell2.Row, cell2.Col] |= (int) (wallToRemove == WallLocation.Bottom ? WallLocation.Top : WallLocation.Left);
            sets[cell1].Merge(sets[cell2]);
            foreach (var mergedCell in sets[cell2])
            {
                sets[mergedCell] = sets[cell1];
            }
            return true;
        }

        internal static int Moves(int wallSpec)
        {
            var sum = 0;
            foreach (var wallLocation in (WallLocation[]) Enum.GetValues(typeof(WallLocation)))
            {
                sum += (wallSpec & (int) wallLocation) / (int) wallLocation;
            }

            return sum;
        }
    }
}