// Controllers/BooksController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheLibraryApi.DTOs;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly IBookService _bookService;

    public BooksController(IBookService bookService)
    {
        _bookService = bookService;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAllBooks()
    {
        var books = await _bookService.GetAllBooksAsync();
        return Ok(books);
    }

    [HttpGet("{id}")]

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetBook(int id)
    {
        var book = await _bookService.GetBookByIdAsync(id);
        if (book is null) return NotFound();
        return Ok(book);
    }

    [HttpPost]
    public async Task<IActionResult> CreateBook([FromBody] CreateBookDto dto)
    {
        try
        {
            var book = await _bookService.CreateBookAsync(dto);
            return CreatedAtAction(nameof(GetBook), new { id = book.Id }, book);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateBook(int id, [FromBody] UpdaateBookDto dto)
    {
        try
        {
            var book = await _bookService.UpdateBookAsync(id, dto);
            if (book is null) return NotFound();
            return Ok(book);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteBook(int id)
    {
        try
        {
            var success = await _bookService.DeleteBookAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
