using iOSClub.Data;
using iOSClub.Data.DataModels;
using iOSClub.WebSite.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iOSClub.WebSite.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class MemberController(
    IDbContextFactory<iOSContext> factory,
    JwtHelper jwtHelper,
    IHttpContextAccessor httpContextAccessor)
    : ControllerBase
{
    #region Visitor

    [TokenActionFilter]
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<MemberModel>> GetData()
    {
        await using var context = await factory.CreateDbContextAsync();
        var member = httpContextAccessor.HttpContext?.User.GetUser();
        if (member == null) return NotFound();
        if (member.Identity == "Founder") return member;
        var student = await context.Students.FirstOrDefaultAsync(x => x.UserId == member.UserId);
        if (student == null) return NotFound();
        var id = member.Identity;
        member = MemberModel.AutoCopy<StudentModel, MemberModel>(student);
        member.Identity = id;

        return member;
    }

    [HttpPost]
    public async Task<ActionResult<string>> SignUp(StudentModel model)
    {
        if (DateTime.Today.Month != 10)
            return NotFound();

        await using var context = await factory.CreateDbContextAsync();
        if (context.Students == null!)
        {
            return Problem("Entity set 'MemberContext.Students'  is null.");
        }

        context.Students.Add(model);
        try
        {
            await context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            if (await context.Students.AnyAsync(e => e.UserId == model.UserId))
                return Conflict();

            throw;
        }

        //return MemberModel.AutoCopy<SignModel, MemberModel>(model);
        return jwtHelper.GetMemberToken(MemberModel.AutoCopy<StudentModel, MemberModel>(model));
    }


    [HttpPost]
    public async Task<ActionResult<string>> Login(LoginModel loginModel)
    {
        await using var context = await factory.CreateDbContextAsync();
        if (context.Students == null!)
            return NotFound();

        var peo = await context.Staffs.FirstOrDefaultAsync(x =>
            x.UserId == loginModel.Id && x.Name == loginModel.Name);

        var id = peo?.Identity ?? "Member";

        var model =
            await context.Students.FirstOrDefaultAsync(x =>
                x.UserId == loginModel.Id && x.UserName == loginModel.Name);

        if (model == null)
        {
            if (peo != null)
            {
                return jwtHelper.GetMemberToken(new MemberModel()
                    { UserName = peo.Name, UserId = peo.UserId, Identity = peo.Identity });
            }

            return NotFound();
        }

        var member = MemberModel.AutoCopy<StudentModel, MemberModel>(model);
        member.Identity = id;

        return jwtHelper.GetMemberToken(member);
    }

    #endregion

    #region Ordinary Members

    // PUT: api/Member/5
    [TokenActionFilter]
    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, MemberModel memberModel)
    {
        if (id != memberModel.UserId)
        {
            return BadRequest();
        }

        await using var context = await factory.CreateDbContextAsync();

        context.Entry(memberModel).State = EntityState.Modified;

        try
        {
            await context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await context.Students.AnyAsync(e => e.UserId == id))
                return NotFound();

            throw;
        }

        return NoContent();
    }

    #endregion

    #region GetInfo

    [HttpGet]
    public ActionResult<string[]> GetAcademies() => SignRecord.Academies;

    #endregion
}