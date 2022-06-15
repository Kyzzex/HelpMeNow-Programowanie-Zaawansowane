using System.Security.Cryptography;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace HelpMeNow.Server.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly DataContext _context;
        private readonly IConfiguration configuration;

        public AuthService(DataContext context, IConfiguration configuration)
        {
            _context = context;
            this.configuration = configuration;
        }

        public async Task<Response<string>> Login(string email, string password)    //------------------> Funkcja do logowania
        {
            var response = new Response<string>();
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email.ToLower() == email.ToLower());
            if (user == null)
            {
                response.Success = false;
                response.Message = "Nie znaleziono takiego użytkownika";
            }
            else if (!IsPasswordCorrect(password, user.Password, user.Salt))
            {
                response.Success = false;
                response.Message = "Złe hasło";
            }
            else
            {
                response.Data = TokenCreator(user);
            }
            return response;
        }

        public async Task<Response<int>> Register(User user, string password)   //------------------> Funkcja do rejestracji
        {
            if (await UserExists(user.Email))
            {
                return new Response<int>
                {
                    Success = false,
                    Message = "Taki użytkownik istnieje."
                };
            }

            CreatePasswordHash(password, out byte[] passwordHash, out byte[] Salt);
            user.Salt = Salt;
            user.Password = passwordHash;


            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new Response<int> { Data = user.Id, Message = "Rejestracja udana!" };
        }


        public async Task<bool> UserExists(string email)  //-----------------------> Sprawdzanie czy użytkownik istnieje
        {
            if (await _context.Users.AnyAsync(user => user.Email.ToLower()
                   .Equals(email.ToLower())))
            {
                return true;
            }
            return false;
        }
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] Salt) // -----------------> Tworzenie hasła z funkcją skrótu 
        {
            using (var hmac = new HMACSHA512())
            {
                Salt = hmac.Key;
                passwordHash = hmac
                  
                    .ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
        private bool IsPasswordCorrect(string password, byte[] hashedpassword, byte[] Salt)  //---------------> Sprawdzanie czy hasło jest prawidłowe 
        {
            using (var hmac = new HMACSHA512(Salt))
            {
                var hash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return hash.SequenceEqual(hashedpassword);


            }
        }

        private string TokenCreator(User user)  //--------> Tworzenie tokenu do indentyfikacji użytkownika
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Role, user.Role),

            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8
                .GetBytes(configuration.GetSection("AppSettings:Token").Value));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddDays(2),
                    signingCredentials: credentials);

            var tokeninstance = new JwtSecurityTokenHandler().WriteToken(token);

            return tokeninstance;
        }
    }
}
