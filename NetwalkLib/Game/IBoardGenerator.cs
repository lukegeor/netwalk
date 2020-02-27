namespace NetwalkLib
{
    public interface IBoardGenerator
    {
        Board GenerateBoard();
        Board RotateBoard(Board originalBoard);
    }
}