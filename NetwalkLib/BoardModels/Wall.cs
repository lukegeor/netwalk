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
    }
}