namespace SudokuApi.Models
{
    public class SudokuResponseDto
    {
        public int[][] Board { get; set; } = Array.Empty<int[]>();
        public bool Solved { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}