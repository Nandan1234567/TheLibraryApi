using Microsoft.EntityFrameworkCore;
using TheLibraryApi.Models;

namespace TheLibraryApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Author> Authors { get; set; }

        public DbSet<Book> Books { get; set; }

        public DbSet<BorrowRecord> BorrowRecords { get; set; }

        public DbSet<Member> Members { get; set; }


        // jwt 

        public DbSet<AppUser> AppUsers { get; set; }  // ← add this

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // we wrote book one to many to author  and this vice versa is true so we no need to write it again in dbcontext explicit realtion

            modelBuilder.Entity<Book>()
                .HasOne(b => b.Author)
                .WithMany(a => a.Books)
                .HasForeignKey(b => b.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);

            // first you have to write one side eg book than think from book perspective and then to link both u thnk of passing book id

            // borrow record ==> book one to many

            modelBuilder.Entity<BorrowRecord>()
                .HasOne(br => br.Book)  // there is a differnce between has one and  with one  other side of realtion also contain  
                .WithMany(b => b.BorrowRecords)
                .HasForeignKey(br => br.BookId);

            // borrow record ==> member one to many


            modelBuilder.Entity<BorrowRecord>()
                .HasOne(br => br.Member)
                .WithMany(m => m.BorrowRecords)
                .HasForeignKey(br => br.MemberId);



            // we wrote book  one to many to borrow and similarly for model 
            // book and member are many to many indirect

            // seed data which are raw 


            // seed data for authors
            modelBuilder.Entity<Author>().HasData(
                new Author
                {
                    Id = 1,
                    Name = "J.K. Rowling",
                    Bio = " Software Engineer",
                    DateOfBirth = new DateTime(1980, 7, 31)


                },

                new Author
                {
                    Id = 2,
                    Name = "George R.R. Martin",
                    Bio = "C# developer",
                    DateOfBirth = new DateTime(1990, 12, 12)

                },

                new Author

                {
                    Id = 3,
                    Name = "Agatha Christie",
                    Bio = "Java developer",
                    DateOfBirth = new DateTime(1999, 1, 23)
                },
                new Author

                {
                    Id = 5,
                    Name = "Agatsya",
                    Bio = " developer",
                    DateOfBirth = new DateTime(1999, 1, 23)
                }

                );


            // seed data for books

            modelBuilder.Entity<Book>().HasData(




                new Book
                {
                    Id = 1,
                    Title = "Harry Potter and the Sorcerer's Stone",
                    ISBN = "978-0439708180",
                    PublishedYear = 1998,
                    TotalCopies = 12,
                    AvailableCopies = 12,
                    AuthorId = 2


                },

                new Book
                {
                    Id = 2,
                    Title = "A Game of Thrones",
                    ISBN = "978-0553103540",
                    PublishedYear = 1996,
                    TotalCopies = 34,
                    AvailableCopies = 34,
                    AuthorId = 1
                },

                new Book
                {
                    Id = 3,
                    Title = "The Slient Patient ",
                    ISBN = "978-0553103540",
                    PublishedYear = 2023,
                    TotalCopies = 4,
                    AvailableCopies = 4,
                    AuthorId = 3




                },

                new Book
                {
                    Id = 4,
                    Title = "The Girl On The Train",
                    ISBN = "978-0553103540",
                    PublishedYear = 2023,
                    TotalCopies = 4,
                    AvailableCopies = 4,
                    AuthorId = 1,

                }
                );


        }
    }

}

