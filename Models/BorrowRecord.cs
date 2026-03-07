// Models/BorrowRecord.cs  ← Junction Table (Many-to-Many)
public class BorrowRecord
{
    public int Id { get; set; }
    public DateTime BorrowDate { get; set; }
    public DateTime? ReturnDate { get; set; }   // Nullable = not yet returned
    public bool IsReturned { get; set; }

    // FK to Book  one to many relation wth book    fk =foreign key
    public int BookId { get; set; }
    public Book Book { get; set; } = null!;

    // FK to Member  similarly member
    public int MemberId { get; set; }
    public Member Member { get; set; } = null!;

 // but relation with memeber and book is many to many and its indirect and conncection is borrow
}