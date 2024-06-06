using iOSClub.Data;
using iOSClub.Data.DataModels;
using iOSClub.WebSite.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iOSClub.WebSite.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class MemberController(iOSContext context, JwtHelper jwtHelper, IHttpContextAccessor httpContextAccessor)
    : ControllerBase
{
    #region Visitor

    [TokenActionFilter]
    [Authorize]
    [HttpGet]
    public ActionResult<MemberModel> GetData()
    {
        var member = httpContextAccessor.HttpContext?.User.GetUser();
        if (member == null) return NotFound();
        return member;
    }

    [HttpPost]
    public async Task<ActionResult<string>> SignUp(StudentModel model)
    {
        if (DateTime.Today.Month != 10)
            return NotFound();

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
            if (await MemberModelExists(model.UserId))
                return Conflict();

            throw;
        }

        //return MemberModel.AutoCopy<SignModel, MemberModel>(model);
        return jwtHelper.GetMemberToken(MemberModel.AutoCopy<StudentModel, MemberModel>(model));
    }


    [HttpPost]
    public async Task<ActionResult<string>> Login(LoginModel loginModel)
    {
        if (context.Students == null!)
            return NotFound();

        var peo = await context.Staffs.FirstOrDefaultAsync(x =>
            x.UserId == loginModel.Id && x.Name == loginModel.Name);

        if (peo != null)
            return jwtHelper.GetMemberToken(new MemberModel()
                { UserName = peo.Name, UserId = peo.UserId, Identity = peo.Identity });
        var model =
            await context.Students.FirstOrDefaultAsync(x =>
                x.UserId == loginModel.Id && x.UserName == loginModel.Name);

        if (model == null)
            return NotFound();
        return jwtHelper.GetMemberToken(MemberModel.AutoCopy<StudentModel, MemberModel>(model));
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

        context.Entry(memberModel).State = EntityState.Modified;

        try
        {
            await context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await MemberModelExists(id))
                return NotFound();

            throw;
        }

        return NoContent();
    }

    #endregion

    private async Task<bool> MemberModelExists(string id)
    {
        return await context.Students.AnyAsync(e => e.UserId == id);
    }
}