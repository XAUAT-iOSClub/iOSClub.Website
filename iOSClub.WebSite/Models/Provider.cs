using System.Security.Claims;
using iOSClub.Data.DataModels;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace iOSClub.WebSite.Models;

public class Provider(ProtectedSessionStorage sessionStorage) : AbsProvider
{
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var storageResult = await sessionStorage.GetAsync<StaffModel>("Permission");
        var permission = storageResult.Success ? storageResult.Value : null;
        if (permission == null)
        {
            return await Task.FromResult(new AuthenticationState(_anonymous));
        }
    
        var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
        {
            new(ClaimTypes.Name, permission.Name),
            new(ClaimTypes.Role, permission.Identity),
            new (ClaimTypes.NameIdentifier,permission.UserId)
        }, "Auth"));
            
        return await Task.FromResult(new AuthenticationState(claimsPrincipal));
    }

    public override async Task UpdateAuthState(StaffModel? permission)
    {
        ClaimsPrincipal claimsPrincipal;
        if (permission is not null)
        {
            await sessionStorage.SetAsync("Permission", permission);
            claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
            {
                new(ClaimTypes.Name, permission.Name),
                new(ClaimTypes.Role, permission.Identity),
                new (ClaimTypes.NameIdentifier,permission.UserId)
            }));
        }
        else
        {
            await sessionStorage.DeleteAsync("Permission");
            claimsPrincipal = _anonymous;
        }

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
    }
}