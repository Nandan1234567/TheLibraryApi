using Microsoft.EntityFrameworkCore;
using TheLibraryApi.Data;
using TheLibraryApi.DTOs;

namespace TheLibraryApi.Services
{
    public class MemberService : IMemberService
    {


        //di
        private readonly AppDbContext _context;

        public MemberService(AppDbContext context)
        {
            _context = context;
        }


        public async Task<IEnumerable<MemberResponseDto>> GetMembersAsync()

        {
            return await _context.Members
                .Include(m => m.BorrowRecords)
                .Select(m => new MemberResponseDto
                {
                    Id = m.Id,
                    FullName = m.FullName,
                    Email = m.Email,
                    TotalBooksBorrowed = m.BorrowRecords.Count,
                    CurrentlyBorrowing = m.BorrowRecords.Count(br => !br.IsReturned)


                }).ToListAsync();

        }



        public async Task<MemberResponseDto?> GetMemberByIdAsync(int Id)

        {
            return await _context.Members
                .Include(m => m.BorrowRecords)
                .Where(m => m.Id == Id)
                .Select(m => new MemberResponseDto
                {

                    Id = m.Id,
                    FullName = m.FullName,
                    Email = m.Email,
                    TotalBooksBorrowed = m.BorrowRecords.Count,
                    CurrentlyBorrowing = m.BorrowRecords.Count(br => !br.IsReturned)  // count only which are not returned( is returned meaning true ,
                                                                                      // count which are not returned)


                }).FirstOrDefaultAsync();
        }


        public async Task<MemberResponseDto> CreateMemberAsync(CreateMemberDto dto)

        {
            var email = _context.Members.Where(m => m.Email == dto.Email).FirstOrDefault();

            if (email != null)
                throw new Exception($" A memeber with the email {dto.Email}already exists ");

            var member = new Member
            {

                FullName = dto.FullName,
                Email = dto.Email,
                Phone = dto.Phone,
                MembershipDate = DateTime.UtcNow



            };

            _context.Members.Add(member);
            await _context.SaveChangesAsync();


            return new MemberResponseDto
            {
                Id = member.Id,
                FullName = member.FullName,
                Email = member.Email,
                TotalBooksBorrowed = 0,
                CurrentlyBorrowing = 0
            };

        }


        public async Task<MemberResponseDto?> UpdateMemberAsync(int id, UpdateMemberDto dto)

        {

            // lets try what you got on ur minf

            // var member = await _context.Members.FindAsync(id);  // for update for this borrow wedont have  those so 

            var member = await _context.Members
                 .Include(m => m.BorrowRecords)
                 .FirstOrDefaultAsync(m => m.Id == id);
            if (member == null)
                throw new Exception($"memeber with id{id}  not found");

            // here first order compares and give only one output but this vresion we added include 
            // we can use find async but it will not work with include
            // so we need to use this version to include the borrow records to get the count of total books borrowed and currently borrowing



            member.FullName = dto.FullName;
            member.Email = dto.Email;
            member.Phone = dto.Phone;

            // it tracks the changes so we dont need to update it explicitly
            //_context.Members.Update(member);
            await _context.SaveChangesAsync();

            return new MemberResponseDto
            {
                Id = member.Id,
                FullName = member.FullName,
                Email = member.Email,
                TotalBooksBorrowed = member.BorrowRecords.Count,
                CurrentlyBorrowing = member.BorrowRecords.Count(br => !br.IsReturned)
            };

        }

        public async Task<bool> DeleteMemberAsync(int id)
        {
            // if there is conditon checking instead of using find we use .include and firstorder

            // this version is correct, but we need to check if the member has any active borrows before deleting



            var member = await _context.Members
                .Include(m => m.BorrowRecords)
                .FirstOrDefaultAsync(m => m.Id == id);


            if (member == null)
                throw new Exception($"member with id {id} is not found  to delete");


            //  this logic check even returned books and delte preventon
            //if(member.BorrowRecords.Count>0)
            //    throw new Exception($"member with id {id} has active borrows and cannot be deleted");

            // we want to check only active borrows

            if(member.BorrowRecords.Any(br => !br.IsReturned))
                throw new InvalidOperationException ($" member with id {id} has active borrows and cannot be deleted");

            _context.Members.Remove(member);

          await   _context.SaveChangesAsync();

            return true;




            // var member = await _context.Members.FindAsync(id);

            //if(member == null)
            //    throw new Exception($"member with id {id} is not found to delte");

            //_context.Members.Remove(member);

            //await _context.SaveChangesAsync();


            //return true;

        }

    }
}