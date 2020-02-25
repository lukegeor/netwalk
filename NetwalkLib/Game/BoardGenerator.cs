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

        public BoardGenerator(GameConfig gameConfig)
        {
            _gameConfig = gameConfig;
        }

        public Board GenerateBoard()
        {
            var height = _gameConfig.Height;
            var width = _gameConfig.Width;

            var board = new Board(height, width, new int[height, width]);

            var sets = new Dictionary<(int, int), Set<(int, int)>>();
            var removableWalls = new List<((int Row, int Col) Spot, WallLocation WallLocation)>();
            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
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
            removableWalls.Shuffle();
            
            var r = new Random();
            
            // Remove 2 or 3 walls from the center spot
            var numWallsToRemoveFromCenterSpot = r.Next(2) == 0 ? 2 : 3;
            var wallsToRemoveFromCenter = new List<WallLocation>(Enum.GetValues(typeof(WallLocation))
                .Cast<WallLocation>())
                .Shuffle()
                .Take(numWallsToRemoveFromCenterSpot);
            foreach (var wallLocation in wallsToRemoveFromCenter)
            {
                if (wallLocation == WallLocation.Bottom || wallLocation == WallLocation.Right)
                {
                    TryRemoveWall(
                        (height / 2, width / 2),
                        wallLocation,
                        removableWalls,
                        board.Spots,
                        sets);
                }
                else if (wallLocation == WallLocation.Left)
                {
                    TryRemoveWall(
                        (height / 2, width / 2 - 1),
                        WallLocation.Right,
                        removableWalls,
                        board.Spots,
                        sets);
                }
                else
                {
                    TryRemoveWall(
                        (height / 2 - 1, width / 2),
                        WallLocation.Bottom,
                        removableWalls,
                        board.Spots,
                        sets);
                }
            }

            // Remove as many walls as possible
            while (removableWalls.Count > 0)
            {
                if (removableWalls.First().Spot == (height / 2, width / 2) ||
                    removableWalls.First().Spot == (height / 2 - 1, width / 2) && removableWalls.First().WallLocation == WallLocation.Bottom ||
                    removableWalls.First().Spot == (height / 2, width / 2 - 1) && removableWalls.First().WallLocation == WallLocation.Right)
                {
                    removableWalls.Remove(removableWalls.First());
                }
                else
                {
                    TryRemoveWall(removableWalls.First().Spot, removableWalls.First().WallLocation, removableWalls, board.Spots, sets);
                }
            }
            
            return board;
        }
        
        private bool TryRemoveWall((int Row, int Col) spot1, WallLocation wallToRemove, IList<((int, int), WallLocation)> removableWalls, int[,] spots, Dictionary<(int, int), Set<(int, int)>> sets)
        {
            if (wallToRemove == WallLocation.Top || wallToRemove == WallLocation.Left)
            {
                throw new ArgumentException("Can't pass Top or Left to TryRemoveWall");
            }

            removableWalls.Remove((spot1, wallToRemove));
            (int Row, int Col) spot2 = (spot1.Row + (wallToRemove == WallLocation.Bottom ? 1 : 0),
                spot1.Col + (wallToRemove == WallLocation.Right ? 1 : 0));
            
            if (Equals(sets[spot1], sets[spot2]))
            {
                return false;
            }

            if (Moves(spots[spot1.Row, spot1.Col]) == 3
                || Moves(spots[spot2.Row, spot2.Col]) == 3)
            {
                return false;
            }

            spots[spot1.Row, spot1.Col] |= (int) wallToRemove;
            spots[spot2.Row, spot2.Col] |= (int) (wallToRemove == WallLocation.Bottom ? WallLocation.Top : WallLocation.Left);
            sets[spot1].Merge(sets[spot2]);
            foreach (var mergedSpot in sets[spot2])
            {
                sets[mergedSpot] = sets[spot1];
            }
            return true;
        }

        internal int Moves(int wallSpec)
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