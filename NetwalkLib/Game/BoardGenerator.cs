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
            Spot[,] spots = new Spot[height, width];
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
                    Wall rightWall;
                    Wall bottomWall;
                    Wall leftWall;

                    if (row == 0)
                    {
                        topWall = new Wall(true);
                        allWalls[w++] = topWall;
                        topWall.Spots[SpotLocation.TopOrLeft] = null;
                        topWall.Spots[SpotLocation.BottomOrRight] = newSpot;
                    }
                    else
                    {
                        topWall = spots[row - 1, col].Walls[WallLocation.Bottom];
                        topWall.Spots[SpotLocation.BottomOrRight] = newSpot;
                    }

                    bottomWall = new Wall(row == height - 1);
                    allWalls[w++] = bottomWall;
                    bottomWall.Spots[SpotLocation.TopOrLeft] = newSpot;

                    if (col == 0)
                    {
                        leftWall = new Wall(true);
                        allWalls[w++] = leftWall;
                        leftWall.Spots[SpotLocation.TopOrLeft] = null;
                        leftWall.Spots[SpotLocation.BottomOrRight] = newSpot;
                    }
                    else
                    {
                        leftWall = spots[row, col - 1].Walls[WallLocation.Right];
                        leftWall.Spots[SpotLocation.BottomOrRight] = newSpot;
                    }

                    rightWall = new Wall(col == width - 1);
                    allWalls[w++]; = rightWall;
                    rightWall.Spots[SpotLocation.TopOrLeft] = newSpot;

                    allSpots[s++] = newSpot;
                    spots[row, col] = newSpot;
                }
            }
        }
    }
}