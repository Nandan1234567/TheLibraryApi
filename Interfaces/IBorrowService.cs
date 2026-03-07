// Interfaces/IBorrowService.cs
using TheLibraryApi.DTOs;

public interface IBorrowService
{


    // borrow book ,  returnbook by id, active borrow, borrow history by member id and  overdue borrows

    Task<BorrowResponseDto> BorrowBookAsync(BorrowBookDto dto);
    Task<BorrowResponseDto> ReturnBookAsync(int BorrowId);


    // enumerable is nothing but iterations used in multi data 
    Task<IEnumerable<BorrowResponseDto>> GetActiveBorrowsAsync();

    Task<IEnumerable<BorrowResponseDto>> GetBorrowHistoryByMemberIdAsync(int memberId);

    Task<IEnumerable<BorrowResponseDto>> GetOverdueBorrowsAsync(int dueDays);

}

