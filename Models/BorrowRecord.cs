// Models/BorrowRecord.cs  ← Junction Table (Many-to-Many)
public class BorrowRecord
{
    public int Id { get; set; }
    public DateTime BorrowDate { get; set; }
    public DateTime? ReturnDate { get; set; }   // Nullable = not yet returned
    public bool IsReturned { get; set; }

    // FK to Book
    public int BookId { get; set; }
    public Book Book { get; set; } = null!;

    // FK to Member
    public int MemberId { get; set; }
    public Member Member { get; set; } = null!;
}