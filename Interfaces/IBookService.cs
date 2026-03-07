// Interfaces/IBookService.cs
using TheLibraryApi.DTOs;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

public interface IBookService
{



    // methods  for book are
    // getall book, byy id , by author ,searchbook,create, update and delete

    //- Task<T> means: “I will eventually give you a T, but don’t block the thread while I fetch it.”


    // task is a way to handle asynchoronus operation use keyword asynca awiait , and
    // it is a generic class that can return any type of data

    // ienumerable for plural data and bookresponse for single data


      
    // after method name use async 
    Task<IEnumerable<BookResponseDto>> GetAllBooksAsync();

//    ? (nullable)
//- BookResponseDto? means the method might return null.
//- Example: If the client asks for id = 999 and no book exists, you return null.

    Task<BookResponseDto?> GetBookByIdAsync(int id);

    Task<IEnumerable<BookResponseDto>> GetBooksByAuthorAsync(int authorId);


  //  - Example: If a user types "Harry Potter", that string goes into keyword.

  //eg searched  i - is: “I may return many.”
    Task<IEnumerable<BookResponseDto>> GetSearchBooksAsync(string keyword);

    Task<BookResponseDto> CreateBookAsync(CreateBookDto dto);

    Task<BookResponseDto> UpdateBookAsync(int id, UpdaateBookDto dto);

    Task<bool> DeleteBookAsync(int id);





}