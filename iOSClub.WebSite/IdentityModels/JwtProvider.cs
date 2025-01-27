using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using iOSClub.Data.DataModels;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.JSInterop;

namespace iOSClub.WebSite.IdentityModels;

public class JwtProvider(IJSRuntime js, IConfiguration configuration) : AbsProvider
{
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        // 从本地存储或 Cookie 中获取 JWT 令牌
        var token = await js.InvokeAsync<string>("localStorageHelper.getItem", "jwt");

        if (string.IsNullOrEmpty(token))
            return await Task.FromResult(new AuthenticationState(_anonymous));

        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);
        
        var claimsIdentity = new ClaimsIdentity(jwtToken.Claims, "Jwt");
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

        return await Task.FromResult(new AuthenticationState(claimsPrincipal));
    }

    public override async Task UpdateAuthState(StaffModel? permission)
    {
        ClaimsPrincipal claimsPrincipal;
        if (permission is not null)
        {
            var claims = new Claim[]
            {
                new(ClaimTypes.Name, permission.Name),
                new(ClaimTypes.Role, permission.Identity),
                new(ClaimTypes.NameIdentifier, permission.UserId)
            };

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"]!));
            var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var securityToken = new JwtSecurityToken(
                claims: claims,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: signingCredentials
            );
            var token = new JwtSecurityTokenHandler().WriteToken(securityToken);

            await js.InvokeVoidAsync("localStorageHelper.setItem", "jwt", token);

            claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims, "Jwt"));
        }
        else
        {
            claimsPrincipal = _anonymous;
            await js.InvokeVoidAsync("localStorageHelper.removeItem", "jwt");
        }

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
    }
    
    public async Task<StaffModel?> GetPermission()
    {
        var token = await js.InvokeAsync<string>("localStorageHelper.getItem", "jwt");
        
        if (string.IsNullOrEmpty(token))
            return null;

        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);
        
        var name = jwtToken.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
        var identity = jwtToken.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value;
        var id = jwtToken.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        
        if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(identity) || string.IsNullOrEmpty(id)) return null;
        
        return new StaffModel()
        {
            Name = name,
            Identity = identity,
            UserId = id
        };
    }
}