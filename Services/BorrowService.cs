using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using TheLibraryApi.Data;
using TheLibraryApi.DTOs;

namespace TheLibraryApi.Services
{
    public class BorrowService : IBorrowService
    {

        private readonly AppDbContext _context;

        // you must use constructor i.e same name as a class
        // pass a parameter appdb  and assign to _context this is called dipendceny injection
        public BorrowService(AppDbContext context)

        {

            _context = context;
        }


        // boorw logic ,pas borrow dto, but check if book is avilable, member exist, member has not borrowed more than 5 books,
        // if all ok then create borrow record and update book available copies

        public async Task<BorrowResponseDto> BorrowBookAsync(BorrowBookDto dto)
        {
             
            //var book = await _context.Books.FindAsync(dto.BookId);

            //if(book == null)
            //    throw new Exception("book not found");

            // more simpler syntax

            var book = await _context.Books.FindAsync(dto.BookId)
                ?? throw new Exception ($" book with id {dto.BookId} not found");


            // the enterd book id is  avilable is stored in book variable

            // we have to check if the book is available or not

            if (book.AvailableCopies <= 0)
                throw new Exception("book is not avilable");

            // similarly check the member is exist or not

            var member = await _context.Members.FindAsync(dto.MemberId)
            ??  throw new Exception($"member not found with the id {dto.MemberId}");

            // if member borrowed 5 books then we will not allow to borrow more books

             // this is easy way
            //if (member.BorrowRecords.Count >= 5)
            //    throw new Exception("member has already borrowed 5 books");


            //alternative approch

            var currentBorrows = await _context.BorrowRecords
                 .Where(br => br.MemberId == dto.MemberId && !br.IsReturned)
                 .CountAsync();

//    -Record 1: MemberId = 10, IsReturned = false → included(still borrowed).
//- Record 2: MemberId = 10, IsReturned = true → excluded(already returned).
//- Record 3: MemberId = 20, IsReturned = false → excluded(different member).

//✅ So yes: if IsReturned is false, the! flips it to true, and the && ensures the member matches.
//That’s exactly how you enforce the “count only active borrows” 

            if(currentBorrows>=5)
                throw new Exception("member has already borrowed 5 books and it reached limit");

            // all these condtions is are false then we can create borrow record 

            var borrow = new BorrowRecord
            {

                MemberId = dto.MemberId,
                BookId = dto.BookId,
                BorrowDate = DateTime.UtcNow,
                IsReturned = false,

            };

            book.AvailableCopies--; // decrease the available copies of the book

            //lets save in db

            _context.BorrowRecords.Add(borrow);
            _context.Books.Update(book); // update the book record with new available copies(avilable copy?)
            await _context.SaveChangesAsync();


            // for response we use borrow values not dto values because we need to return the borrow record with id and other details

            return new BorrowResponseDto
            {
                Id = borrow.Id,
                BookTitle = book.Title,
                MemberName = member.FullName,
                BorrowDate = borrow.BorrowDate,
                ReturnDate = borrow.ReturnDate ?? DateTime.MinValue, // if return date is null then set to min value
                IsReturned = borrow.IsReturned



            };

        }


        public async Task<BorrowResponseDto> ReturnBookAsync(int BorrowId)
        {
            var borrowRecord = await _context.BorrowRecords
                .Include(br => br.Book) // include book details to update available copies
                   .Include(br => br.Member)
                   .FirstOrDefaultAsync(br => br.Id == BorrowId);   

            if(borrowRecord == null )
                throw new Exception($"the borrow record with this id {BorrowId} not found");

            if (borrowRecord.IsReturned)
                throw new InvalidOperationException("Book already returned");

            // now we are letting to return book

            borrowRecord.IsReturned = true;
            borrowRecord.ReturnDate = DateTime.UtcNow;

            
            borrowRecord.Book.AvailableCopies++; // increase the available copies of the book


            _context.BorrowRecords.Update(borrowRecord);
            _context.Books.Update(borrowRecord.Book);
            await _context.SaveChangesAsync();



            return new BorrowResponseDto
            {
                Id = borrowRecord.Id,
                BookTitle = borrowRecord.Book.Title,
                MemberName = borrowRecord.Member.FullName,
                BorrowDate = borrowRecord.BorrowDate,
                IsReturned = borrowRecord.IsReturned,

            };


        }




        // list all borrowed guys

        public async Task<IEnumerable<BorrowResponseDto>> GetActiveBorrowsAsync()
        {


         return await _context.BorrowRecords
                .Include(br => br.Book)
                .Include(br => br.Member)
                .Where(br => !br.IsReturned) // only active borrows
                .Select(br => new BorrowResponseDto
                {
                    Id=br.Id,
                    BookTitle = br.Book.Title,
                    MemberName= br.Member.FullName,
                    BorrowDate= br.BorrowDate,
                    IsReturned= br.IsReturned,



                }).ToListAsync();
        }

        // get member borrow history just pass member id and return all the borrow records of that member
        // borrowdate is sorted by descending order means latest borrow record will be first
        public async Task<IEnumerable<BorrowResponseDto>> GetBorrowHistoryByMemberIdAsync(int memberId)
        {

            return await _context.BorrowRecords
                .Include(br => br.Book)
                .Include(br => br.Member)
                .Where(br => br.MemberId == memberId)
                .OrderByDescending(br =>br.BorrowDate)  // newest first
                .Select(br => new BorrowResponseDto
                {
                    Id = br.Id,
                    BookTitle= br.Book.Title,
                    MemberName= br.Member.FullName,
                    BorrowDate = br.BorrowDate,
                    IsReturned= br.IsReturned,



                }).ToListAsync();
        }



        public async Task<IEnumerable<BorrowResponseDto>> GetOverdueBorrowsAsync(int dueDays)
        {
            var dueDate = DateTime.UtcNow.AddDays(-dueDays); // 

            return await _context.BorrowRecords
                .Include(br =>br.Book)
                .Include(br => br.Member)
                .Where(br => !br.IsReturned && br.BorrowDate < dueDate)
                .OrderByDescending(br =>br.BorrowDate) // newest first
                .Select(br => new BorrowResponseDto
                {
                    Id = br.Id,
                    BookTitle = br.Book.Title,
                    MemberName = br.Member.FullName,
                    BorrowDate = br.BorrowDate,
                    IsReturned= br.IsReturned,  


                }).ToListAsync();   

        }




    }
}
