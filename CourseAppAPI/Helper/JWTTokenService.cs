using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CourseAppAPI.Helper
{
    public class JWTTokenService
    {
        private readonly IConfiguration _configuration;
        public JWTTokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Genratetoken(string username)
        {
            //Create claim
            var claims = new[]
            {
                new Claim(ClaimTypes.Name,username),
                new Claim(ClaimTypes.Role,"user")
            };

            //Encode and secure the key
            string? configkey = _configuration.GetValue<string>("Jwt:Key");
            if (!string.IsNullOrEmpty(configkey))
            {
                var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(configkey));


                //Create signing credentials
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                //Generate the token using metadata, claim and signature
                var token = new JwtSecurityToken(
                    issuer: _configuration.GetValue<string>("Jwt:Issuer"),
                    audience: _configuration.GetValue<string>("Jwt:Audience"),
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(Convert.ToDouble(_configuration.GetValue<string>("Jwt:ExpirationMininutes"))),
                    signingCredentials: creds
                    );

                //Serialize and return token
                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            else
            {
                return "JWT Key is not configured properly in appsettings.json";

            }
        }
    }
}
