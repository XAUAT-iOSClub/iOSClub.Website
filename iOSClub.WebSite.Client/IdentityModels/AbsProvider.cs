using System.Security.Claims;
using iOSClub.Data.DataModels;
using Microsoft.AspNetCore.Components.Authorization;

namespace iOSClub.WebSite.Client.IdentityModels;

public abstract class AbsProvider: AuthenticationStateProvider
{
    protected readonly ClaimsPrincipal _anonymous = new(new ClaimsIdentity());

    public abstract Task UpdateAuthState(StaffModel? permission);
    
    public async Task Logout()
    {
        await UpdateAuthState(null);
    }
}