using System.Security.Claims;
using iOSClub.Data.DataModels;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace iOSClub.WebSite.Models;

public class Provider : AuthenticationStateProvider
{
    private readonly ProtectedSessionStorage _sessionStorage;
    private readonly ClaimsPrincipal _anonymous = new(new ClaimsIdentity());

    public Provider(ProtectedSessionStorage sessionStorage)
    {
        _sessionStorage = sessionStorage;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            var storageResult = await _sessionStorage.GetAsync<StaffModel>("Permission");
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
        catch
        {
            return await Task.FromResult(new AuthenticationState(_anonymous));
        }
    }

    public async Task UpdateAuthState(StaffModel? permission)
    {
        ClaimsPrincipal claimsPrincipal;
        if (permission is not null)
        {
            await _sessionStorage.SetAsync("Permission", permission);
            claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
            {
                new(ClaimTypes.Name, permission.Name),
                new(ClaimTypes.Role, permission.Identity),
                new (ClaimTypes.NameIdentifier,permission.UserId)
            }));
        }
        else
        {
            await _sessionStorage.DeleteAsync("Permission");
            claimsPrincipal = _anonymous;
        }

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
    }
    
}