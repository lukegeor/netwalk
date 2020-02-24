namespace NetwalkLib
{
    public class Wall
    {
        public Spot[] Spots { get; set; }
        public bool IsBorder { get; set; }

        public Wall(bool isBorder)
        {
            Spots = new Spot[2];
            IsBorder = isBorder;
        }

        public override string ToString()
        {
            string s = "";
            s += Spots[0] != null ? $"({Spots[0].X}, {Spots[0].Y})" : "null";
            s += " - ";
            s += Spots[1] != null ? $"({Spots[1].X}, {Spots[1].Y})" : "null";
            return s;
        }
    }
}