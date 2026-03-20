using SudokuApi.Models;
using SudokuApi.Services;
using Xunit;

namespace SudokuApiTest.UnitTests;

public class SudokuServiceTests
{
    private readonly SudokuService _service = new();

    [Fact]
    public void IsRequestDtoValid_ValidBoard_ReturnsTrue()
    {
        var dto = new SudokuRequestDto
        {
            Board = CreateValidPuzzleBoard()
        };

        var result = _service.IsRequestDtoValid(dto);

        Assert.True(result);
    }

    [Fact]
    public void IsRequestDtoValid_NullBoard_ReturnsFalse()
    {
        var dto = new SudokuRequestDto
        {
            Board = null!
        };

        var result = _service.IsRequestDtoValid(dto);

        Assert.False(result);
    }

    [Fact]
    public void IsRequestDtoValid_WrongRowCount_ReturnsFalse()
    {
        var dto = new SudokuRequestDto
        {
            Board = new int[][]
            {
                new[] { 1, 2, 3 }
            }
        };

        var result = _service.IsRequestDtoValid(dto);

        Assert.False(result);
    }

    [Fact]
    public void IsRequestDtoValid_NullRow_ReturnsFalse()
    {
        var board = CreateValidPuzzleBoard();
        board[4] = null!;

        var dto = new SudokuRequestDto
        {
            Board = board
        };

        var result = _service.IsRequestDtoValid(dto);

        Assert.False(result);
    }

    [Fact]
    public void IsRequestDtoValid_WrongColumnCount_ReturnsFalse()
    {
        var board = CreateValidPuzzleBoard();
        board[2] = new[] { 0, 9, 8, 0, 0, 0, 0, 6 };

        var dto = new SudokuRequestDto
        {
            Board = board
        };

        var result = _service.IsRequestDtoValid(dto);

        Assert.False(result);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(10)]
    public void IsRequestDtoValid_ValueOutOfRange_ReturnsFalse(int value)
    {
        var board = CreateValidPuzzleBoard();
        board[0][0] = value;

        var dto = new SudokuRequestDto
        {
            Board = board
        };

        var result = _service.IsRequestDtoValid(dto);

        Assert.False(result);
    }

    [Fact]
    public void IsBoardValid_ValidBoard_ReturnsTrue()
    {
        var board = CreateSudokuBoard(CreateValidPuzzleBoard());

        var result = _service.IsBoardValid(board);

        Assert.True(result);
    }

    [Fact]
    public void IsBoardValid_DuplicateInRow_ReturnsFalse()
    {
        var board = CreateSudokuBoard(CreateValidPuzzleBoard());
        board.Cells[0, 1] = 5;

        var result = _service.IsBoardValid(board);

        Assert.False(result);
    }

    [Fact]
    public void IsBoardValid_DuplicateInColumn_ReturnsFalse()
    {
        var board = CreateSudokuBoard(CreateValidPuzzleBoard());
        board.Cells[1, 0] = 5;

        var result = _service.IsBoardValid(board);

        Assert.False(result);
    }

    [Fact]
    public void IsBoardValid_DuplicateInBox_ReturnsFalse()
    {
        var board = CreateSudokuBoard(CreateValidPuzzleBoard());
        board.Cells[1, 1] = 5;

        var result = _service.IsBoardValid(board);

        Assert.False(result);
    }

    [Fact]
    public void Solve_ValidSolvableBoard_ReturnsSolvedBoard()
    {
        var board = CreateSudokuBoard(CreateValidPuzzleBoard());

        var result = _service.Solve(board);

        Assert.NotNull(result);
        Assert.True(_service.IsBoardValid(result!));
        Assert.DoesNotContain(0, result!.Cells.Cast<int>());
    }

    [Fact]
    public void Solve_InvalidBoard_ReturnsNull()
    {
        var board = CreateSudokuBoard(CreateValidPuzzleBoard());
        board.Cells[0, 1] = 5;

        var result = _service.Solve(board);

        Assert.Null(result);
    }

    private static int[][] CreateValidPuzzleBoard()
    {
        return new int[][]
        {
            new[] { 5, 3, 0, 0, 7, 0, 0, 0, 0 },
            new[] { 6, 0, 0, 1, 9, 5, 0, 0, 0 },
            new[] { 0, 9, 8, 0, 0, 0, 0, 6, 0 },
            new[] { 8, 0, 0, 0, 6, 0, 0, 0, 3 },
            new[] { 4, 0, 0, 8, 0, 3, 0, 0, 1 },
            new[] { 7, 0, 0, 0, 2, 0, 0, 0, 6 },
            new[] { 0, 6, 0, 0, 0, 0, 2, 8, 0 },
            new[] { 0, 0, 0, 4, 1, 9, 0, 0, 5 },
            new[] { 0, 0, 0, 0, 8, 0, 0, 7, 9 }
        };
    }

    private static SudokuBoard CreateSudokuBoard(int[][] source)
    {
        var board = new SudokuBoard();

        for (int r = 0; r < 9; r++)
        {
            for (int c = 0; c < 9; c++)
            {
                board.Cells[r, c] = source[r][c];
            }
        }

        return board;
    }
}