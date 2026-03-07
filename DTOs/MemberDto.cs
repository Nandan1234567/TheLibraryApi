namespace TheLibraryApi.DTOs
{
    // DTOs/MemberDTOs.cs
    public class CreateMemberDto
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
    }
    public class UpdateMemberDto
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
    }

    public class MemberResponseDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        // from books
        public int TotalBooksBorrowed { get; set; }       // Computed means changed modified
        public int CurrentlyBorrowing { get; set; }        // Computed
    }
}
