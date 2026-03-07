// Interfaces/IMemberService.cs
using TheLibraryApi.DTOs;

public interface IMemberService
{

    // all memeber, by id, create, update and delete    

    Task<IEnumerable<MemberResponseDto>> GetMembersAsync();

    Task<MemberResponseDto ?> GetMemberByIdAsync(int id);

    Task<MemberResponseDto> CreateMemberAsync(CreateMemberDto dto);

    Task<MemberResponseDto ?> UpdateMemberAsync(int id, UpdateMemberDto dto);

    Task<bool> DeleteMemberAsync(int id);

}