using System;
using System.Collections.Generic;
using System.Linq;
using NetwalkLib.Utils;

namespace NetwalkLib
{
    public class BoardGenerator
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
            
            var board = CreateInitialBoard(height, width);
            var allSpots = board.Spots.Cast<Spot>().ToList();

            var sets = allSpots.ToDictionary(s => s, s => new Set<Spot>(s));

            var removableWalls = allSpots
                .SelectMany(s => s.Walls)
                .Where(w => !w.IsBorder)
                .Distinct()
                .ToList()
                .Shuffle();
            
            var r = new Random();
            
            // Remove 2 or 3 walls from the center spot
            var numWallsToRemoveFromCenterSpot = r.Next(2) == 0 ? 2 : 3;
            var wallsToRemoveFromCenter = new List<WallLocation>(Enum.GetValues(typeof(WallLocation))
                .Cast<WallLocation>())
                .Shuffle()
                .Take(numWallsToRemoveFromCenterSpot);
            foreach (var wallLocation in wallsToRemoveFromCenter)
            {
                TryRemoveWall(board.Spots[height/2, width/2].Walls[(int)wallLocation], removableWalls, sets);
            }

            // Remove as many walls as possible
            while (removableWalls.Count > 0)
            {
                TryRemoveWall(removableWalls.First(), removableWalls, sets);
            }
            
            return board;
        }

        private Board CreateInitialBoard(int height, int width)
        {
            var spots = new Spot[height, width];
            for (var row = 0; row < height; row++)
            {
                for (var col = 0; col < width; col++)
                {
                    var newSpot = new Spot(row, col, row == height / 2 && col == width / 2);

                    Wall topWall;
                    Wall leftWall;

                    if (row == 0)
                    {
                        topWall = new Wall(true);
                        topWall.Spots[(int) SpotLocation.TopOrLeft] = null;
                        topWall.Spots[(int) SpotLocation.BottomOrRight] = newSpot;
                    }
                    else
                    {
                        topWall = spots[row - 1, col].Walls[(int) WallLocation.Bottom];
                        topWall.Spots[(int) SpotLocation.BottomOrRight] = newSpot;
                    }

                    var bottomWall = new Wall(row == height - 1);
                    bottomWall.Spots[(int) SpotLocation.TopOrLeft] = newSpot;

                    if (col == 0)
                    {
                        leftWall = new Wall(true);
                        leftWall.Spots[(int) SpotLocation.TopOrLeft] = null;
                        leftWall.Spots[(int) SpotLocation.BottomOrRight] = newSpot;
                    }
                    else
                    {
                        leftWall = spots[row, col - 1].Walls[(int) WallLocation.Right];
                        leftWall.Spots[(int) SpotLocation.BottomOrRight] = newSpot;
                    }

                    var rightWall = new Wall(col == width - 1);
                    rightWall.Spots[(int) SpotLocation.TopOrLeft] = newSpot;

                    newSpot.Walls[(int) WallLocation.Top] = topWall;
                    newSpot.Walls[(int) WallLocation.Right] = rightWall;
                    newSpot.Walls[(int) WallLocation.Bottom] = bottomWall;
                    newSpot.Walls[(int) WallLocation.Left] = leftWall;

                    spots[row, col] = newSpot;
                }
            }
            
            return new Board(height, width, spots);
        }
        
        private bool TryRemoveWall(Wall wallToRemove, IList<Wall> removableWalls, Dictionary<Spot, Set<Spot>> sets)
        {
            if (wallToRemove == null)
            {
                throw new ArgumentNullException(nameof(wallToRemove));
            }

            removableWalls.Remove(wallToRemove);
            Spot spot1 = wallToRemove.Spots[(int) SpotLocation.BottomOrRight];
            Spot spot2 = wallToRemove.Spots[(int) SpotLocation.TopOrLeft];
            if (wallToRemove.IsBorder
                || spot1 == null
                || spot2 == null)
            {
                return false;
            }

            if (Equals(sets[spot1], sets[spot2]))
            {
                return false;
            }

            if (spot1.Walls.Count(w => w != null) == 1
                || spot2.Walls.Count(w => w != null) == 1)
            {
                return false;
            }

            var wallLocation1 = Enum.GetValues(typeof(WallLocation)).Cast<WallLocation>().Single(wl => spot1.Walls[(int) wl] == wallToRemove);
            var wallLocation2 = Enum.GetValues(typeof(WallLocation)).Cast<WallLocation>().Single(wl => spot2.Walls[(int) wl] == wallToRemove);
            if ((4 + (int)wallLocation1 - (int)wallLocation2) % 4 != 2)
            {
                throw new ArithmeticException("Attempted to remove wall that did not align on two spots.");
            }
                
            spot1.Walls[(int)wallLocation1] = null;
            spot2.Walls[(int)wallLocation2] = null;
            sets[spot1].Merge(sets[spot2]);
            foreach (var spot in sets[spot2])
            {
                sets[spot] = sets[spot1];
            }
            return true;
        }
    }
}