namespace NetwalkLib
{
    public interface IBoard
    {
        int[,] Cells { get; }
        int Height { get; }
        int Width { get; }
        bool[,] GetActive();
    }
}