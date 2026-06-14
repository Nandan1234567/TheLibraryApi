namespace TheLibraryApi.Models
{
    public class AppUser
    {


        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }  // NEVER store plain password
        public string Role { get; set; }
    }
}
