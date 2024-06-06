using iOSClub.Data;
using iOSClub.Data.DataModels;
using iOSClub.WebSite.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iOSClub.WebSite.Controllers;

[Authorize(Roles = "Founder, President, TechnologyMinister, PracticalMinister, NewMediaMinister")]
[TokenActionFilter]
[Route("api/[controller]/[action]")]
[ApiController]
public class PresidentController(iOSContext context, IHttpContextAccessor httpContextAccessor)
    : ControllerBase
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    // GET: api/Member
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        if (context.Students == null!)
            return NotFound();

        var memberModel = await context.Students.FindAsync(id);

        if (memberModel == null)
            return NotFound();

        context.Students.Remove(memberModel);
        await context.SaveChangesAsync();

        return NoContent();
    }

    [HttpGet]
    public async Task<List<MemberModel>> GetAllData()
    {
        var list = await context.Students.ToListAsync();
        var newList = new List<MemberModel>();

        list.ForEach(Action);
        return newList;

        async void Action(StudentModel x) => newList.Add(await FromSignToMember(x));
    }

    [HttpPost]
    public async Task<ActionResult> Update([FromBody] MemberModel model)
    {
        if (context.Students == null!)
        {
            return NotFound();
        }
        
        context.Entry(model).State = EntityState.Modified;

        try
        {
            await context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            var a = await context.Students.AnyAsync(x => x.Equals(model));
            if (!a)
                return NotFound();
            throw;
        }

        return NoContent();
    }

    private async Task<MemberModel> FromSignToMember(StudentModel model)
    {
        var member = MemberModel.AutoCopy<StudentModel, MemberModel>(model);
        var m = await context.Staffs.FirstOrDefaultAsync(x => x.UserId == model.UserId && x.Name == model.UserName);
        if (m != null) member.Identity = m.Identity;
        return member;
    }

}