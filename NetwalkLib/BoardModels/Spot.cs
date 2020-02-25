using System.Linq;

namespace NetwalkLib
{
    public class Spot
    {
        public int X { get; }
        public int Y { get; }
        public Wall[] Walls { get; }
        public bool IsCenterSpot { get; }

        public Spot(int x, int y, bool isCenterSpot)
        {
            Walls = new Wall[4];
            X = x;
            Y = y;
            IsCenterSpot = isCenterSpot;
        }

        public override string ToString()
        {
            var s = $"spot ({X}, {Y})";
            if (IsCenterSpot)
            {
                s += " is center spot";
            }

            s += $" has {Walls.Count(w => w != null)} walls";

            string WallReport(WallLocation wallLocation)
            {
                var sWall = $" { wallLocation.ToString().ToLowerInvariant() } wall";
                var wall = Walls[(int) wallLocation];
                
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
                    
                    var topOrLeftSpot = wall.Spots[(int) SpotLocation.TopOrLeft];
                    if (topOrLeftSpot == null)
                    {
                        sWall += " nothing";
                    }
                    else
                    {
                        sWall += $" ({topOrLeftSpot.X}, {topOrLeftSpot.Y})";
                    }
                    
                    sWall += " with";

                    var bottomOrRightSpot = wall.Spots[(int) SpotLocation.BottomOrRight];
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