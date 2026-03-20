using SudokuApi.Models;

namespace SudokuApi.Extensions
{
    public static class SudokuMappingExtensions
    {
        public static SudokuBoard ToBoard(this SudokuRequestDto dto)
        {
            var board = new SudokuBoard();

            for (int r = 0; r < 9; r++)
            {
                for (int c = 0; c < 9; c++)
                {
                    board.Cells[r, c] = dto.Board[r][c];
                }
            }

            return board;
        }

        public static SudokuResponseDto ToResponseDto(this SudokuBoard board, bool solved = true, string message = "Sudoku solved successfully")
        {
            var dto = new SudokuResponseDto
            {
                Board = new int[9][],
                Solved = solved,
                Message = message
            };

            for (int r = 0; r < 9; r++)
            {
                dto.Board[r] = new int[9];

                for (int c = 0; c < 9; c++)
                {
                    dto.Board[r][c] = board.Cells[r, c];
                }
            }

            return dto;
        }
    }    
}

