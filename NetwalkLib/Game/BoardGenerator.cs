using System;
using System.Collections.Generic;
using System.Linq;

namespace NetwalkLib
{
    public class BoardGenerator : IBoardGenerator
    {
        public (Board SolvedBoard, Board PlayingBoard) GenerateBoard(GameConfig gameConfig)
        {
            var random = gameConfig.RandomSeed.HasValue ? new Random(gameConfig.RandomSeed.Value) : new Random();
            var height = gameConfig.Height;
            var width = gameConfig.Width;

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
            var numWallsToRemoveFromCenterCell = random.Next(2) == 0 ? 2 : 3;
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
            
            return (board, RotateBoard(board, random));
        }

        private Board RotateBoard(Board originalBoard, Random random)
        {
            var newCells = new int[originalBoard.Height, originalBoard.Width];
            Array.Copy(originalBoard.Cells, newCells, originalBoard.Cells.Length);
            for (var row = 0; row < originalBoard.Height; row++)
            {
                for (var col = 0; col < originalBoard.Width; col++)
                {
                    var rotates = random.Next(4);
                    for (var i = 0; i < rotates; i++)
                    {
                        newCells[row, col] = Board.Rotate(newCells[row, col]);
                    }
                }
            }
            
            return new Board(originalBoard.Height, originalBoard.Width, newCells);
        }
        
        private bool TryRemoveWall((int Row, int Col) cell1, WallLocation wallToRemove, IList<((int, int), WallLocation)> removableWalls, int[,] cells, Dictionary<(int, int), Set<(int, int)>> sets)
        {
            removableWalls.Remove((cell1, wallToRemove));
            (int Row, int Col) cell2 = (cell1.Row + (wallToRemove == WallLocation.Bottom ? 1 : 0),
                cell1.Col + (wallToRemove == WallLocation.Right ? 1 : 0));
            
            if (sets[cell1].Equals(sets[cell2]))
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