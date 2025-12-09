using E_Ticaret.Domain.Entities;
using E_Ticaret.Infrastructure.Abstractions;
using E_Ticaret.Infrastructure.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace E_Ticaret.Persistence.Services;
    internal sealed class JwtProvider(IOptions<Jwt> jwt) : IJwtProvider
    {


        public  Task<string> CreateTokenAsync(AppUser appUser,string PasswordHash,CancellationToken cancellationToken = default)
        {

            Claim[] claims = new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, appUser.Id.ToString()),
                new Claim("NameLastname",string.Join(" ",appUser.FirstName,appUser.LastName)),
                new Claim("Email",appUser.Email)
            };
            
            JwtSecurityToken jwtSecurityToken = new(

                issuer: jwt.Value.Issuer,
                audience: jwt.Value.Audience,
                claims: claims,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Value.SecretKey)), SecurityAlgorithms.HmacSha512));

            JwtSecurityTokenHandler handler = new();
            string token = handler.WriteToken(jwtSecurityToken);
            return Task.FromResult(token);
        
        }
    }
