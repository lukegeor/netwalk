namespace NetwalkLib
{
    public class Board
    {
        public Spot[,] Spots { get; }

        internal Board(int height, int width)
        {
            Spots = new Spot[height, width];
        }
    }
}