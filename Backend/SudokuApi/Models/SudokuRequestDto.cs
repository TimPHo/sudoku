namespace SudokuApi.Models
{
    public class SudokuRequestDto
    {
        public int[][] Board { get; set; } = Array.Empty<int[]>();
    }

}