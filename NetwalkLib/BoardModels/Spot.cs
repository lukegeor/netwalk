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

        public string ToString()
        {
            string s = $"spot ({X}, {Y})";

            string WallReport(WallLocation wallLocation)
            {
                s += $" { wallLocation } wall";

                if (Walls[wallLocation] == null)
                {
                    s += " is null";
                }
                else
                {
                    s += " connects";
                    if (Walls[wallLocation].Spots[SpotLocation.TopOrLeft] == null)
                    {
                        s += " nothing";
                    }
                    else
                    {
                        s += $" ({Walls[wallLocation].Spots[SpotLocation.TopOrLeft].X}, {Walls[wallLocation].Spots[SpotLocation.TopOrLeft].Y})";
                    }
                    s += " with";
                    if (Walls[wallLocation].Spots[SpotLocation.BottomOrRight] == null)
                    {
                        s += " nothing";
                    }
                    else
                    {
                        s += $" ({Walls[wallLocation].Spots[SpotLocation.BottomOrRight].X}, {Walls[wallLocation].Spots[SpotLocation.BottomOrRight].Y})";
                    }
                }
            }

            s += WallReport(WallLocation.Top);
            s += WallReport(WallLocation.Right);
            s += WallReport(WallLocation.Bottom);
            s += WallReport(WallLocation.Left);
            
            return s;

        }
    }
}