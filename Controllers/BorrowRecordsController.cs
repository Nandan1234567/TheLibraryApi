// Controllers/BorrowRecordsController.cs
using Microsoft.AspNetCore.Mvc;
using TheLibraryApi.DTOs;

[ApiController]
[Route("api/borrows")]
public class BorrowRecordsController : ControllerBase
{
    private readonly IBorrowService _borrowService;

    public BorrowRecordsController(IBorrowService borrowService)
    {
        _borrowService = borrowService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllBorrows()
    {
        var borrows = await _borrowService.GetActiveBorrowsAsync();
        return Ok(borrows);
    }

    [HttpGet("history/{memberId}")]
    public async Task<IActionResult> GetMemberHistory(int memberId)
    {
        var history = await _borrowService.GetBorrowHistoryByMemberIdAsync(memberId);
        return Ok(history);
    }

    [HttpGet("overdue/{days}")]
    public async Task<IActionResult> GetOverdueBorrows(int days)
    {
        var overdue = await _borrowService.GetOverdueBorrowsAsync(days);
        return Ok(overdue);
    }


    [HttpPost]
    public async Task<IActionResult> BorrowBook([FromBody] BorrowBookDto dto)
    {
        try
        {
            var record = await _borrowService.BorrowBookAsync(dto);

            return CreatedAtAction(
                nameof(GetBorrowById),
                new { id = record.Id },
                record
            );
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}/return")]
    public async Task<IActionResult> ReturnBook(int id)
    {
        try
        {
            var record = await _borrowService.ReturnBookAsync(id);
            if (record is null) return NotFound();
            return Ok(record);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetBorrowById(int id)
    {
        var records = await _borrowService.GetActiveBorrowsAsync();

        var record = records.FirstOrDefault(x => x.Id == id);

        if (record == null)
            return NotFound();

        return Ok(record);
    }
}