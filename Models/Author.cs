// Models/Author.cs


public class Author
{
    public int  Id { get; set; }  // you cna give Guid Id too auto genreate so we use Guid.newGuid()
    public string Name { get; set; } = string.Empty;
    public string Bio { get; set; } = string.Empty;

    public DateTime DateOfBirth { get; set; }


    // navigate proeprty (One-to-Many)

    public ICollection<Book> Books { get; set; } = new List<Book>();



}