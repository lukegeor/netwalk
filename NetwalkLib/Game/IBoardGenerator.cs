namespace NetwalkLib
{
    public interface IBoardGenerator
    {
        (Board SolvedBoard, Board PlayingBoard) GenerateBoard(GameConfig gameConfig);
    }
}