using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using iOSClub.WebSite.Models;
using Microsoft.IdentityModel.Tokens;

namespace iOSClub.WebSite.Controllers;

public class JwtHelper(IConfiguration configuration)
{
    public string GetMemberToken(MemberModel model)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, model.UserName),
            new Claim(ClaimTypes.Role, model.Identity),
            new Claim(ClaimTypes.NameIdentifier,model.UserId)
        };

        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"]!));
        var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

        var securityToken = new JwtSecurityToken(
            claims: claims,
            notBefore: DateTime.Now, //notBefore
            expires: DateTime.Now.AddHours(2), //expires
            signingCredentials: signingCredentials
        );
        return new JwtSecurityTokenHandler().WriteToken(securityToken);
    }
}