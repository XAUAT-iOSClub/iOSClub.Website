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
public class PresidentController(iOSContext context)
    : ControllerBase
{
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
    public async Task<ActionResult<List<MemberModel>>> GetAllData()
    {
        var list = await context.Students.ToListAsync();
        var staff = await context.Staffs.ToListAsync();

        var members = list.Select(MemberModel.AutoCopy<
            StudentModel, MemberModel>).ToList();

        foreach (var staffModel in staff)
        {
            var member = members.FirstOrDefault(x => x.UserId == staffModel.UserId);
            if (member != null)
            {
                member.Identity = staffModel.Identity;
            }
        }

        return members;
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
}