using SudokuApi.Models;
namespace SudokuApi.Services
{
    public interface ISudokuService
    {
        bool IsRequestDtoValid(SudokuRequestDto dto);
        bool IsBoardValid(SudokuBoard board);
        SudokuBoard? Solve(SudokuBoard board);
    }
}