namespace NetwalkLib
{
    public class Board
    {
        public Spot[][] Spots { get; }

        public Board(GameConfig gameConfig)
        {
            Spots = new Spot[gameConfig.Height][gameConfig.Width];
        }
    }
}