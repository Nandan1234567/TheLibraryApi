namespace TheLibraryApi.DTOs
{
    public class BorrowBookDto
    {
        // it can have one to many realtion with book and member 
        public int BookId { get; set; }

        public int MemberId { get; set; }


    }

    public class BorrowResponseDto
    {

        public int Id { get; set; }

        public string BookTitle { get; set; } = string.Empty;

        public string MemberName { get; set; } = string.Empty;

        public DateTime BorrowDate { get; set; }
        public DateTime ReturnDate { get; set; }

        public bool IsReturned { get; set; }




    }
}
