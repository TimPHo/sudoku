using SudokuApi.Models;

namespace SudokuApi.Services
{
    public class SudokuService : ISudokuService
    {    
        // Validates incoming DTO shape (9x9 grid, values 0–9).
        public bool IsRequestDtoValid(SudokuRequestDto dto)
        {
            if (dto.Board == null || dto.Board.Length != 9)
                return false;

            for (int r = 0; r < 9; r++)
            {
                if (dto.Board[r] == null || dto.Board[r].Length != 9)
                    return false;

                for (int c = 0; c < 9; c++)
                {
                    int val = dto.Board[r][c];
                    if (val < 0 || val > 9)
                        return false;
                }
            }

            return true;
        }

        // Validates Sudoku rules (no duplicate numbers in rows, columns, or 3x3 boxes).
        public bool IsBoardValid(SudokuBoard board)
        {
            var cells = board.Cells;

            // rows
            for (int r = 0; r < 9; r++)
            {
                var seen = new HashSet<int>();

                for (int c = 0; c < 9; c++)
                {
                    int val = cells[r, c];
                    if (val == 0) continue;

                    if (!seen.Add(val))
                        return false;
                }
            }

            // columns
            for (int c = 0; c < 9; c++)
            {
                var seen = new HashSet<int>();

                for (int r = 0; r < 9; r++)
                {
                    int val = cells[r, c];
                    if (val == 0) continue;

                    if (!seen.Add(val))
                        return false;
                }
            }

            // 3x3 boxes
            for (int br = 0; br < 3; br++)
            {
                for (int bc = 0; bc < 3; bc++)
                {
                    var seen = new HashSet<int>();

                    for (int r = br * 3; r < br * 3 + 3; r++)
                    {
                        for (int c = bc * 3; c < bc * 3 + 3; c++)
                        {
                            int val = cells[r, c];
                            if (val == 0) continue;

                            if (!seen.Add(val))
                                return false;
                        }
                    }
                }
            }

            return true;
        }

        public SudokuBoard? Solve(SudokuBoard board)
        {
            if (!IsBoardValid(board))
                return null;

            var copy = Copy(board.Cells);

            return TrySolve(copy)
                ? new SudokuBoard { Cells = copy }
                : null;
        }

        // --- Backtracking ---
        private bool TrySolve(int[,] cells)
        {
            var emptyCell = FindEmptyCell(cells);

            if (emptyCell == null)
                return true;

            var (row, col) = emptyCell.Value;

            for (int value = 1; value <= 9; value++)
            {
                if (!IsValidPlacement(cells, row, col, value))
                    continue;

                cells[row, col] = value;

                if (TrySolve(cells))
                    return true;

                cells[row, col] = 0;
            }

            return false;
        }

        private (int Row, int Col)? FindEmptyCell(int[,] cells)
        {
            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    if (cells[row, col] == 0)
                        return (row, col);
                }
            }

            return null;
        }

        private bool IsValidPlacement(int[,] cells, int row, int col, int num)
        {
            for (int i = 0; i < 9; i++)
            {
                if (cells[row, i] == num) return false;
                if (cells[i, col] == num) return false;
            }

            int startRow = (row / 3) * 3;
            int startCol = (col / 3) * 3;

            for (int r = startRow; r < startRow + 3; r++)
            {
                for (int c = startCol; c < startCol + 3; c++)
                {
                    if (cells[r, c] == num) return false;
                }
            }

            return true;
        }

        private int[,] Copy(int[,] source)
        {
            var copy = new int[9, 9];

            for (int r = 0; r < 9; r++)
                for (int c = 0; c < 9; c++)
                    copy[r, c] = source[r, c];

            return copy;
        }
    }



}