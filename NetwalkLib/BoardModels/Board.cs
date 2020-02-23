namespace NetwalkLib
{
    public class Board
    {
        public Spot[,] Spots { get; }

        public Board(int height, int width)
        {
            Spots = new Spot[height, width];
        }
    }
}