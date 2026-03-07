// Controllers/MembersController.cs
using Microsoft.AspNetCore.Mvc;
using TheLibraryApi.DTOs;

[ApiController]
[Route("api/members")]
public class MembersController : ControllerBase
{
    private readonly IMemberService _memberService;

    public MembersController(IMemberService memberService)
    {
        _memberService = memberService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllMembers()
    {
        var members = await _memberService.GetMembersAsync();
        return Ok(members);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetMember(int id)
    {
        var member = await _memberService.GetMemberByIdAsync(id);
        if (member is null) return NotFound();
        return Ok(member);
    }

    [HttpPost]
    public async Task<IActionResult> CreateMember([FromBody] CreateMemberDto dto)
    {
        try
        {
            var member = await _memberService.CreateMemberAsync(dto);
            return CreatedAtAction(nameof(GetMember), new { id = member.Id }, member);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMember(int id)
    {
        try
        {
            var success = await _memberService.DeleteMemberAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}