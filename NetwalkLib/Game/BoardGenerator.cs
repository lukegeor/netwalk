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

            var board = new Board(height, width);
            var sets = new Dictionary<Spot, Set<Spot>>();
            
            var allSpots = new Spot[height * width];
            var wallListSize = (2 * width * height) + width + height;
            var allWalls = new List<Wall>(wallListSize);
            Spot centerSpot = null;

            var s = 0;
            for (var row = 0; row < height; row++)
            {
                for (var col = 0; col < width; col++)
                {
                    var isCenterSpot = row == height / 2 && col == width / 2;
                    var newSpot = new Spot(row, col, isCenterSpot);
                    if (isCenterSpot)
                    {
                        centerSpot = newSpot;
                    }

                    Wall topWall;
                    Wall leftWall;

                    if (row == 0)
                    {
                        topWall = new Wall(true);
                        allWalls.Add(topWall);
                        topWall.Spots[(int) SpotLocation.TopOrLeft] = null;
                        topWall.Spots[(int) SpotLocation.BottomOrRight] = newSpot;
                    }
                    else
                    {
                        topWall = board.Spots[row - 1, col].Walls[(int) WallLocation.Bottom];
                        topWall.Spots[(int) SpotLocation.BottomOrRight] = newSpot;
                    }

                    var bottomWall = new Wall(row == height - 1);
                    allWalls.Add(bottomWall);
                    bottomWall.Spots[(int) SpotLocation.TopOrLeft] = newSpot;

                    if (col == 0)
                    {
                        leftWall = new Wall(true);
                        allWalls.Add(leftWall);
                        leftWall.Spots[(int) SpotLocation.TopOrLeft] = null;
                        leftWall.Spots[(int) SpotLocation.BottomOrRight] = newSpot;
                    }
                    else
                    {
                        leftWall = board.Spots[row, col - 1].Walls[(int) WallLocation.Right];
                        leftWall.Spots[(int) SpotLocation.BottomOrRight] = newSpot;
                    }

                    var rightWall = new Wall(col == width - 1);
                    allWalls.Add(rightWall);
                    rightWall.Spots[(int) SpotLocation.TopOrLeft] = newSpot;

                    newSpot.Walls[(int) WallLocation.Top] = topWall;
                    newSpot.Walls[(int) WallLocation.Right] = rightWall;
                    newSpot.Walls[(int) WallLocation.Bottom] = bottomWall;
                    newSpot.Walls[(int) WallLocation.Left] = leftWall;

                    allSpots[s++] = newSpot;
                    board.Spots[row, col] = newSpot;

                    var newSet = new Set<Spot>(newSpot);
                    sets[newSpot] = newSet;
                }
            }

            if (allWalls.Count != wallListSize)
            {
                throw new ArithmeticException("Walls list was not the right size.");
            }
            
            var removableWalls = allWalls.Where(wall => !wall.IsBorder).ToList().Shuffle();

            bool TryRemoveWall(Wall wallToRemove)
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
            
            var r = new Random();
            // Remove 2 or 3 walls from the center spot
            var numWallsToRemoveFromCenterSpot = r.Next(2) == 0 ? 2 : 3;
            var wallsToRemoveFromCenter = new List<WallLocation>(Enum.GetValues(typeof(WallLocation))
                .Cast<WallLocation>())
                .Shuffle()
                .Take(numWallsToRemoveFromCenterSpot);
            foreach (var wallLocation in wallsToRemoveFromCenter)
            {
                TryRemoveWall(centerSpot?.Walls[(int)wallLocation]);
            }

            // Remove as many walls as possible
            while (removableWalls.Count > 0)
            {
                TryRemoveWall(removableWalls.First());
            }
            
            return board;
        }
    }
}