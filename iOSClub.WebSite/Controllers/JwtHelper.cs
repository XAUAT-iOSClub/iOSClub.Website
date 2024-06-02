using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using iOSClub.Data.DataModels;
using iOSClub.WebSite.Models;
using Microsoft.IdentityModel.Tokens;

namespace iOSClub.WebSite.Controllers;

public class JwtHelper(IConfiguration configuration)
{
    public string GetMemberToken(StaffModel model)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, model.Name),
            new Claim(ClaimTypes.Role, model.Identity),
            new Claim(ClaimTypes.PrimarySid,model.UserId)
        };

        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"]!));
        const string algorithm = SecurityAlgorithms.HmacSha256;
        var signingCredentials = new SigningCredentials(secretKey, algorithm);

        var securityToken = new JwtSecurityToken(
            claims: claims,
            notBefore: DateTime.Now, //notBefore
            expires: DateTime.Now.AddSeconds(30), //expires
            signingCredentials: signingCredentials
        );
        var token = new JwtSecurityTokenHandler().WriteToken(securityToken);
        return token;
    }
    
    public string GetMemberToken(MemberModel model)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, model.UserName),
            new Claim(ClaimTypes.Role, model.Identity),
            new Claim(ClaimTypes.PrimarySid,model.UserId)
        };

        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"]!));
        const string algorithm = SecurityAlgorithms.HmacSha256;
        var signingCredentials = new SigningCredentials(secretKey, algorithm);

        var securityToken = new JwtSecurityToken(
            claims: claims,
            notBefore: DateTime.Now, //notBefore
            expires: DateTime.Now.AddSeconds(30), //expires
            signingCredentials: signingCredentials
        );
        var token = new JwtSecurityTokenHandler().WriteToken(securityToken);
        return token;
    }
}