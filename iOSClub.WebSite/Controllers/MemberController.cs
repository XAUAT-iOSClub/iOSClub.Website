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

    [TokenActionFilter]
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<List<TodoModel>>> GetTodos()
    {
        await using var context = await factory.CreateDbContextAsync();
        var member = httpContextAccessor.HttpContext?.User.GetUser();
        if (member == null) return NotFound();
        var student = await context.Students.FirstOrDefaultAsync(x => x.UserId == member.UserId);
        if (student == null) return NotFound();
        return await context.Todos.Where(x => x.StudentId == student.UserId).ToListAsync();
    }

    [TokenActionFilter]
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<TodoModel>> AddTodo(TodoModel todoModel)
    {
        await using var context = await factory.CreateDbContextAsync();
        var member = httpContextAccessor.HttpContext?.User.GetUser();
        if (member == null) return NotFound();
        var student = await context.Students.FirstOrDefaultAsync(x => x.UserId == member.UserId);
        if (student == null) return NotFound();
        todoModel.Student = student;
        todoModel.Id = todoModel.ToHash();
        await context.Todos.AddAsync(todoModel);
        await context.SaveChangesAsync();

        return todoModel;
    }

    [TokenActionFilter]
    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        await using var context = await factory.CreateDbContextAsync();
        var member = httpContextAccessor.HttpContext?.User.GetUser();
        if (member == null) return NotFound();
        var student = await context.Students.FirstOrDefaultAsync(x => x.UserId == member.UserId);
        if (student == null) return NotFound();

        var todo = await context.Todos.FirstOrDefaultAsync(x => x.Id == id);
        if (todo == null || todo.StudentId != member.UserId) return NotFound();

        context.Todos.Remove(todo);
        await context.SaveChangesAsync();

        return Ok();
    }

    [TokenActionFilter]
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Update(TodoModel todoModel)
    {
        await using var context = await factory.CreateDbContextAsync();
        var member = httpContextAccessor.HttpContext?.User.GetUser();
        if (member == null) return NotFound();
        var student = await context.Students.FirstOrDefaultAsync(x => x.UserId == member.UserId);
        if (student == null) return NotFound();

        var todo = await context.Todos.FirstOrDefaultAsync(x => x.Id == todoModel.Id);
        if (todo == null) return NotFound();

        todo.Update(todoModel);

        await context.SaveChangesAsync();
        return Ok();
    }

    // PUT: api/Member/5
    [TokenActionFilter]
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Update(MemberModel memberModel)
    {
        await using var context = await factory.CreateDbContextAsync();

        context.Entry(memberModel).State = EntityState.Modified;
        await context.SaveChangesAsync();

        return NoContent();
    }

    #endregion

    #region GetInfo

    [HttpGet]
    public ActionResult<string[]> GetAcademies() => SignRecord.Academies;

    [TokenActionFilter]
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetInfo()
    {
        await using var context = await factory.CreateDbContextAsync();

        var member = httpContextAccessor.HttpContext?.User.GetUser();
        if (member == null) return NotFound();
        var student = await context.Students.FirstOrDefaultAsync(x => x.UserId == member.UserId);
        if (student == null) return NotFound();

        if (member.Identity == "Member")
        {
            return Ok();
        }

        if (member.Identity == "Department")
        {
            return Ok(new
            {
                Tasks = await context.Tasks.Where(x => x.Users.Any(y => y.UserId == member.UserId))
                    .ToArrayAsync(),
                Projects = await context.Projects.Where(x => x.Staffs.Any(y => y.UserId == member.UserId))
                    .ToArrayAsync(),
            });
        }

        if (member.Identity == "Minister")
        {
            var staff = await context.Staffs.FirstOrDefaultAsync(x => x.UserId == member.UserId);

            if (staff == null) return NotFound();

            return Ok(new
            {
                Tasks = await context.Tasks.Where(x => x.Users.Any(y => y.UserId == member.UserId))
                    .ToArrayAsync(),
                Projects = await context.Projects
                    .Where(x => x.Staffs.Any(y => y.UserId == member.UserId) || x.Department == staff.Department)
                    .ToArrayAsync(),
                Resources = await context.Resources.ToArrayAsync(),
            });
        }

        return Ok(new
        {
            Total = await context.Students.CountAsync(),
            StaffsCount = await context.Staffs.CountAsync(),
            Projects = await context.Projects.ToArrayAsync(),
            Tasks = await context.Tasks.ToArrayAsync(),
            Resources = await context.Resources.ToArrayAsync(),
            Departments = await context.Departments.ToArrayAsync()
        });
    }

    #endregion
}