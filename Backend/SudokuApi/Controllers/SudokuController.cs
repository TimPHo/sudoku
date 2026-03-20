using Microsoft.AspNetCore.Mvc;
using SudokuApi.Models;
using SudokuApi.Services;
using SudokuApi.Extensions;
namespace SudokuApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SudokuController : ControllerBase
{
    private readonly ILogger<SudokuController> _logger;
    private readonly ISudokuService _sudokuService;

    public SudokuController(
        ILogger<SudokuController> logger,
        ISudokuService sudokuService)
    {
        _logger = logger;
        _sudokuService = sudokuService;
    }

    /// <summary>
    /// Solves a Sudoku puzzle.
    /// </summary>
    /// <remarks>
    /// Example request:
    ///
    ///     {
    ///       "board": [
    ///         [5,3,0,0,7,0,0,0,0],
    ///         [6,0,0,1,9,5,0,0,0],
    ///         [0,9,8,0,0,0,0,6,0],
    ///         [8,0,0,0,6,0,0,0,3],
    ///         [4,0,0,8,0,3,0,0,1],
    ///         [7,0,0,0,2,0,0,0,6],
    ///         [0,6,0,0,0,0,2,8,0],
    ///         [0,0,0,4,1,9,0,0,5],
    ///         [0,0,0,0,8,0,0,7,9]
    ///       ]
    ///     }
    /// </remarks>
    [HttpPost("solve")]
    public ActionResult<SudokuResponseDto> Solve([FromBody] SudokuRequestDto dto)
    {
        if (!_sudokuService.IsRequestDtoValid(dto))
            return BadRequest("Invalid board.");

        var solved = _sudokuService.Solve(dto.ToBoard());

        if (solved == null)
        {
            _logger.LogWarning("Unable to solve Sudoku puzzle. {@Board}", dto.Board);
            return BadRequest("Unable to solve Sudoku puzzle.");
        }

        return Ok(solved.ToResponseDto());
    }
}