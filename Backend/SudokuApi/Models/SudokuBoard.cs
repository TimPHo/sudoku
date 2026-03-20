namespace SudokuApi.Models
{
    public class SudokuBoard
    {
        public int[,] Cells { get; set; } = new int[9, 9];
    }
}
