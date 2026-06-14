// Models/Member.cs
public class Member
{

    // member who buy the books
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public DateTime MembershipDate { get; set; }



    // Navigation (Many-to-Many via BorrowRecord)
    public ICollection<BorrowRecord> BorrowRecords { get; set; } = new List<BorrowRecord>();
}