namespace TheLibraryApi.Services
{
    using global::TheLibraryApi.Data;
    using global::TheLibraryApi.DTOs;
    using global::TheLibraryApi.Models;
    using Microsoft.EntityFrameworkCore;

    using static System.Net.Mime.MediaTypeNames;

    namespace TheLibraryApi.Services
    {
        public class AuthService
        {
            private readonly AppDbContext _context;
            private readonly JwtService _jwtService;

            public AuthService(AppDbContext context, JwtService jwtService)
            {
                _context = context;
                _jwtService = jwtService;
            }

            public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
            {
                // Check email already exists
                var exists = await _context.AppUsers
                    .AnyAsync(u => u.Email == dto.Email);

                if (exists)
                    throw new Exception($"Email {dto.Email} is already registered.");

                // WHY BCrypt?
                // Never store plain passwords.
                // BCrypt hashes it — even if your DB is stolen, passwords are safe.
                // Install: Install-Package BCrypt.Net-Next
                var user = new AppUser
                {
                    FullName = dto.FullName,
                    Email = dto.Email,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                    Role = dto.Role ?? "User"  // default to User if not provided
                };

                _context.AppUsers.Add(user);
                await _context.SaveChangesAsync();

                // Generate token immediately after register
                // User is logged in as soon as they register — no extra login step
                var token = _jwtService.GenerateToken(user);

                return new AuthResponseDto
                {
                    Token = token,
                    FullName = user.FullName,
                    Email = user.Email,
                    Role = user.Role
                };
            }

            public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
            {
                // Find user by email
                var user = await _context.AppUsers
                    .FirstOrDefaultAsync(u => u.Email == dto.Email);

                // WHY same error for both cases?
                // If you say "email not found" — attacker knows valid emails.
                // Always give the same vague message for security.
                if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                    throw new Exception("Invalid email or password.");

                var token = _jwtService.GenerateToken(user);

                return new AuthResponseDto
                {
                    Token = token,
                    FullName = user.FullName,
                    Email = user.Email,
                    Role = user.Role
                };
            }
        }
    }

}

