namespace NetwalkLib
{
    public class Spot
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Wall[] Walls { get; set; }
        public bool IsCenterSpot { get; set; }

        public Spot(int x, int y, bool isCenterSpot)
        {
            Walls = new Wall[4];
            X = x;
            Y = y;
            IsCenterSpot = isCenterSpot;
        }

        public override string ToString()
        {
            string s = $"spot ({X}, {Y})";
            if (IsCenterSpot)
            {
                s += " is center spot";
            }

            string WallReport(WallLocation wallLocation)
            {
                string sWall = $" { wallLocation.ToString().ToLowerInvariant() } wall";
                Wall wall = Walls[(int) wallLocation];
                
                if (wall == null)
                {
                    sWall += " is null";
                }
                else
                {
                    if (wall.IsBorder)
                    {
                        sWall += " is border";
                    }
                    sWall += " connects";
                    
                    Spot topOrLeftSpot = wall.Spots[(int) SpotLocation.TopOrLeft];
                    if (topOrLeftSpot == null)
                    {
                        sWall += " nothing";
                    }
                    else
                    {
                        sWall += $" ({topOrLeftSpot.X}, {topOrLeftSpot.Y})";
                    }
                    
                    sWall += " with";

                    Spot bottomOrRightSpot = wall.Spots[(int) SpotLocation.BottomOrRight];
                    if (bottomOrRightSpot == null)
                    {
                        sWall += " nothing";
                    }
                    else
                    {
                        sWall += $" ({bottomOrRightSpot.X}, {bottomOrRightSpot.Y})";
                    }
                }

                return sWall;
            }

            s += WallReport(WallLocation.Top);
            s += WallReport(WallLocation.Right);
            s += WallReport(WallLocation.Bottom);
            s += WallReport(WallLocation.Left);
            
            return s;

        }
    }
}