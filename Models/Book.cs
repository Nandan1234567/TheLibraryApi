// Models/Book.cs
public class Book
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;

    public string ISBN { get; set; } = string.Empty;    // international book standard no

    public int PublishedYear { get; set; }
    public int TotalCopies { get; set; }
    public int AvailableCopies { get; set; }




    // Foreign Key (Many side of One-to-Many)

    public int AuthorId { get; set; }
    public Author Author { get; set; } = null!;

    // Navigation (Many-to-Many via BorrowRecord)


    public ICollection<BorrowRecord> BorrowRecords { get; set; } = new List<BorrowRecord>();

}