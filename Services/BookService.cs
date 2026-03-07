using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop.Infrastructure;
using System.Net;
using System.Runtime.ExceptionServices;
using System.Runtime.Intrinsics.X86;
using System.Text.RegularExpressions;
using TheLibraryApi.Data;
using TheLibraryApi.DTOs;
using static System.Reflection.Metadata.BlobBuilder;

namespace TheLibraryApi.Services
{
    public class BookService : IBookService
    {


        // Dependency injection of the AppDbContext to interact with the database

        public readonly AppDbContext _context;

        public BookService(AppDbContext context)
        {
            _context = context;
        }


        public async Task<IEnumerable<BookResponseDto>> GetAllBooksAsync()
        {
            // Retrieve all books from the database and project them to BookResponseDto


            return await _context.Books
            .Include(b => b.Author)
            .Select(b => new BookResponseDto
            {
                Id = b.Id,
                Title = b.Title,
                ISBN = b.ISBN,
                PublishedYear = b.PublishedYear,
                AvailableCopies = b.AvailableCopies,
                TotalCopies = b.TotalCopies,
                AuthorName = b.Author.Name

            }).ToListAsync();

        }

        // may be null so reffering here , id can not match  so we need to return null if not found  ?
        public async Task<BookResponseDto?> GetBookByIdAsync(int id)
        {

            return await _context.Books
                .Include(b => b.Author)
                .Where(b => b.Id == id)
                .Select(b => new BookResponseDto
                {
                    Id = b.Id,
                    Title = b.Title,
                    ISBN = b.ISBN,
                    PublishedYear = b.PublishedYear,
                    AvailableCopies = b.AvailableCopies,
                    TotalCopies = b.TotalCopies,
                    AuthorName = b.Author.Name


                }).FirstOrDefaultAsync();

        }


        public async Task<IEnumerable<BookResponseDto>> GetBooksByAuthorAsync(int authorId)
        {
            return await _context.Books
                .Include(b => b.Author)
                .Where(b => b.AuthorId == authorId)
                .Select(b => new BookResponseDto
                {
                    Id = b.Id,
                    Title = b.Title,
                    ISBN = b.ISBN,
                    PublishedYear = b.PublishedYear,
                    AvailableCopies = b.AvailableCopies,
                    AuthorName = b.Author.Name
                }).ToListAsync();
        }


        public async Task<IEnumerable<BookResponseDto>> GetSearchBooksAsync(string keyword)

        {

            return await _context.Books
                .Include(b => b.Author)
                // Search for books where the title, author's name, or ISBN contains the keyword typed by string keyword
                .Where(b => b.Title.Contains(keyword) || b.Author.Name.Contains(keyword)
                || b.ISBN.Contains(keyword))
                .Select(b => new BookResponseDto
                {

                    Id = b.Id,
                    Title = b.Title,
                    ISBN = b.ISBN,
                    PublishedYear = b.PublishedYear,
                    AvailableCopies = b.AvailableCopies,
                    AuthorName = b.Author.Name

                }).ToListAsync();


        }


        public async Task<BookResponseDto> CreateBookAsync(CreateBookDto dto)
        {
            var author = await _context.Authors.FindAsync(dto.AuthorId)
                ?? throw new Exception($" author with id{dto.AuthorId} not found");

            var book = new Book
            {
                Title = dto.Title,
                ISBN = dto.ISBN,
                PublishedYear = dto.PublishedYear,
                TotalCopies = dto.TotalCopies,
                AvailableCopies = dto.TotalCopies,  // intially, all copies are available
                AuthorId = dto.AuthorId


            };

            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            // this is a not a list of books so return just one so we no use of .seelct(and those stuff) and .response

            return new BookResponseDto
            {
                Id = book.Id,
                Title = book.Title,
                ISBN = book.ISBN,
                PublishedYear = book.PublishedYear,
                AvailableCopies = book.AvailableCopies,
                TotalCopies = book.TotalCopies,
                AuthorName = author.Name
            };


        }


        public async Task<BookResponseDto > UpdateBookAsync(int id, UpdaateBookDto dto)

        {

            var book = await _context.Books
      .Include(b => b.Author)
      .FirstOrDefaultAsync(b => b.Id == id);



            if(book == null)
                    throw new Exception($"Book with id {id} not found");


            //cant reduce total copies below borrowed copies

            var borrowedCopies = book.TotalCopies- book.AvailableCopies;

            if(book.TotalCopies < borrowedCopies)
                throw new InvalidOperationException($"total copies cannot be less than borrowed copies ({borrowedCopies})");

            // update the book properties

            

            var diff= dto.totalCopies - book.TotalCopies;

            book.Title = dto.Title;

            book.TotalCopies = dto.totalCopies; 

            //  i  did not understand it
            book.AvailableCopies += diff; // adjust available copies based on the change in total copies

            // update the book in the database
            _context.Books.Update(book);
            _context.SaveChanges();

            return new BookResponseDto
            {
                Id = book.Id,
                Title = book.Title,
                ISBN = book.ISBN,
                PublishedYear = book.PublishedYear,
                TotalCopies = book.TotalCopies,
                AvailableCopies = book.AvailableCopies,
                AuthorName = book.Author.Name
            };

        }

        public async Task<bool> DeleteBookAsync(int id)
        {

            var book = await _context.Books
                .Include(b => b.Author)
                .Where(b => b.Id == id)
                .FirstOrDefaultAsync();

            if (book == null)
                return false;

            if (book.AvailableCopies < book.TotalCopies)
                throw new InvalidOperationException("Cannot delete a book that has borrowed copies.");



            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return true;
        }

    }

}

//-Method: GetBooksAfterYearAsync(int year)
//What: Return all books published after a given year.
//Hint: .Where + .Select. (Interface)
//- Method: GetAvailableBooksAsync()
//What: Return all books with AvailableCopies > 0.
//Hint: .Where + .Select. (Interface)
//- Method: GetBookByIdAsync(int id)
//What: Return a single book by ID.
//Hint: .FirstOrDefaultAsync. (Concrete class only)
//-Method: GetBooksPerYearAsync()
//What: Return each year with count of books.
//Hint: .GroupBy + g.Key + g.Count(). (Interface)
//- Method: GetLatestBookPerAuthorAsync()
//What: Return most recent book for each author.
//Hint: Group by author, then .OrderByDescending(...).FirstOrDefault(). (Interface)
//- Method: GetProlificAuthorsAsync(int minBooks)
//What: Return authors with more than minBooks books.
//Hint: Group by author, filter groups with .Where(g => g.Count() > minBooks). (Interface)
//- Method: GetAuthorStatsAsync()
//What: For each author, return name, total books, in-stock count, latest book title.
//Hint: Group by author, project multiple aggregates. (Interface)

//👉 So:
//-Use interface injection for standard API endpoints.
//- Use concrete class i
