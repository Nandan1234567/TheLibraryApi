namespace TheLibraryApi.Services
{
    using global::TheLibraryApi.Models;
    using Microsoft.Extensions.Configuration;
    using Microsoft.IdentityModel.Tokens;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;


    namespace TheLibraryApi.Services
    {
        // ONE JOB — take a user, return a signed token string
        // Nothing else. No DB. No business logic.

        public class JwtService
        {
            private readonly IConfiguration _config;

            public JwtService(IConfiguration config)
            {
                _config = config;
            }

            public string GenerateToken(AppUser user)
            {
                // Your secret key from appsettings — converted to bytes for signing
                var key = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_config["Jwt:Key"])
                );

                var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                // Claims = data INSIDE the token
                // Anyone can READ these (token is base64, not encrypted)
                // But nobody can FAKE them without your secret key
                var claims = new[]
                {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Role, user.Role)  // ← this powers [Authorize(Roles="Admin")]
            };

                var token = new JwtSecurityToken(
                    issuer: _config["Jwt:Issuer"],
                    audience: _config["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(
                                            Convert.ToDouble(_config["Jwt:ExpiryInMinutes"])),
                    signingCredentials: credentials
                );

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
        }
    }
}
