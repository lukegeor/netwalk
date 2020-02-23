using System;

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
            int height = _gameConfig.Height;
            int width = _gameConfig.Width;

            Board board = new Board(height, width);
            
            Spot[] allSpots = new Spot[height * width];
            Wall[] allWalls = new Wall[(2 * width * height) + width + height];
            int s = 0;
            int w = 0;
            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {
                    Spot newSpot = new Spot(row, col, row == height / 2 && col == width / 2);
                    Wall topWall;
                    Wall leftWall;

                    if (row == 0)
                    {
                        topWall = new Wall(true);
                        allWalls[w++] = topWall;
                        topWall.Spots[(int) SpotLocation.TopOrLeft] = null;
                        topWall.Spots[(int) SpotLocation.BottomOrRight] = newSpot;
                    }
                    else
                    {
                        topWall = board.Spots[row - 1, col].Walls[(int) WallLocation.Bottom];
                        topWall.Spots[(int) SpotLocation.BottomOrRight] = newSpot;
                    }

                    Wall bottomWall = new Wall(row == height - 1);
                    allWalls[w++] = bottomWall;
                    bottomWall.Spots[(int) SpotLocation.TopOrLeft] = newSpot;

                    if (col == 0)
                    {
                        leftWall = new Wall(true);
                        allWalls[w++] = leftWall;
                        leftWall.Spots[(int) SpotLocation.TopOrLeft] = null;
                        leftWall.Spots[(int) SpotLocation.BottomOrRight] = newSpot;
                    }
                    else
                    {
                        leftWall = board.Spots[row, col - 1].Walls[(int) WallLocation.Right];
                        leftWall.Spots[(int) SpotLocation.BottomOrRight] = newSpot;
                    }

                    Wall rightWall = new Wall(col == width - 1);
                    allWalls[w++] = rightWall;
                    rightWall.Spots[(int) SpotLocation.TopOrLeft] = newSpot;

                    newSpot.Walls[(int) WallLocation.Top] = topWall;
                    newSpot.Walls[(int) WallLocation.Right] = rightWall;
                    newSpot.Walls[(int) WallLocation.Bottom] = bottomWall;
                    newSpot.Walls[(int) WallLocation.Left] = leftWall;

                    allSpots[s++] = newSpot;
                    board.Spots[row, col] = newSpot;
                }
            }
            
            return board;
        }
    }
}