using iOSClub.Data;
using iOSClub.Data.DataModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iOSClub.WebSite.Controllers;

[Authorize]
[TokenActionFilter]
[Route("api/[controller]/[action]")]
[ApiController]
public class ProjectController(iOSContext context, IHttpContextAccessor httpContextAccessor)
    : ControllerBase
{
    #region GetValue

    [HttpGet]
    [Authorize(Roles = "Founder, President, TechnologyMinister, PracticalMinister, NewMediaMinister")]
    public async Task<ActionResult<List<ProjectModel>>> GetAllData()
    {
        return await context.Projects
            .Include(x => x.Staffs)
            .Include(x => x.Tasks)
            .ToListAsync();
    }

    [HttpGet]
    public async Task<ActionResult<List<ProjectModel>>> GetYourProjects()
    {
        var member = httpContextAccessor.HttpContext?.User.GetUser();
        if (member == null) return NotFound();
        if (member.Identity is "Founder" or "Member") return new List<ProjectModel>();
        var staff = await context.Staffs.Include(x => x.Projects)
            .ThenInclude(x => x.Tasks)
            .FirstOrDefaultAsync(x => x.UserId == member.UserId);
        if (staff == null) return NotFound();
        return staff.Projects;
    }

    [HttpGet]
    public async Task<ActionResult<List<TaskModel>>> GetYourTasks()
    {
        var member = httpContextAccessor.HttpContext?.User.GetUser();
        if (member == null) return NotFound();
        if (member.Identity is "Founder" or "Member") return new List<TaskModel>();
        var staff = await context.Staffs.Include(x => x.Tasks)
            .FirstOrDefaultAsync(x => x.UserId == member.UserId);
        if (staff == null) return NotFound();
        return staff.Tasks;
    }

    #endregion

    #region Project

    [HttpPost]
    [Authorize(Roles = "Founder, President, TechnologyMinister, PracticalMinister, NewMediaMinister")]
    public async Task<ActionResult<ProjectModel>> CreateOrUpdateProject([FromBody] ProjectModel model)
    {
        var member = httpContextAccessor.HttpContext?.User.GetUser();
        if (member == null || string.IsNullOrEmpty(member.UserId)) return NotFound();
        if (member.Identity is "Founder" or "Member") return NotFound();
        var staff = await context.Staffs.FirstOrDefaultAsync(x => x.UserId == member.UserId);
        if (staff == null) return NotFound();
        var proj = await context.Projects.FirstOrDefaultAsync(x => x.Id == model.Id);
        if (proj == null)
        {
            model.Staffs.Add(staff);
            model.Id = model.ToHash();
            await context.Projects.AddAsync(model);
        }
        else
        {
            proj.Update(model);
        }

        await context.SaveChangesAsync();
        return model;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> DeleteProject(string id)
    {
        var member = httpContextAccessor.HttpContext?.User.GetUser();
        if (member == null || string.IsNullOrEmpty(member.UserId)) return NotFound();
        if (member.Identity is "Member") return NotFound();

        var staff = await context.Staffs.FirstOrDefaultAsync(x => x.UserId == member.UserId);
        if (staff == null) return NotFound();

        var project = await context.Projects.FirstOrDefaultAsync(x => x.Id == id);

        if (project == null) return NotFound();

        if (staff.Identity != "Founder" && staff.Identity != "President" &&
            !staff.Identity.Contains(project.Description)) return NoContent();

        context.Projects.Remove(project);
        await context.SaveChangesAsync();

        return Ok();
    }

    [HttpGet("{id}/{projId}")]
    [Authorize(Roles = "Founder, President, TechnologyMinister, PracticalMinister, NewMediaMinister")]
    public async Task<ActionResult> LetChangeProject(string id, string projId)
    {
        var member = httpContextAccessor.HttpContext?.User.GetUser();
        if (member == null || string.IsNullOrEmpty(member.UserId)) return NotFound();
        if (member.Identity is "Member") return NotFound();

        var staff = await context.Staffs.FirstOrDefaultAsync(x => x.UserId == id);
        if (staff == null) return NotFound();

        if (!staff.Identity.Contains("Member")) return NotFound("您没有权限");

        var proj = await context.Projects.Include(x => x.Staffs).FirstOrDefaultAsync(x => x.Id == projId);
        if (proj == null) return NotFound();

        if (proj.Staffs.Any(x => x.UserId == id))
        {
            proj.Staffs.Remove(staff);
        }
        else
        {
            proj.Staffs.Add(staff);
        }

        await context.SaveChangesAsync();
        return Ok();
    }

    #endregion

    #region Task

    [HttpPost]
    public async Task<ActionResult<TaskModel>> CreateOrUpdateTask([FromBody] TaskModel model)
    {
        var member = httpContextAccessor.HttpContext?.User.GetUser();
        if (member == null || string.IsNullOrEmpty(member.UserId)) return NotFound();
        if (member.Identity is "Member") return NotFound();

        var staff = await context.Staffs.FirstOrDefaultAsync(x => x.UserId == member.UserId);
        if (staff == null) return NotFound();

        var project = await context.Projects.FirstOrDefaultAsync(x => x.Id == model.Project.Id);

        if (project == null) return NotFound();

        if (string.IsNullOrEmpty(model.Id))
        {
            model.Id = model.ToHash();
            model.Users.Add(staff);
            project.Tasks.Add(model);
        }
        else
        {
            var task = await context.Tasks.FirstOrDefaultAsync(x => x.Id == model.Id);
            if (task == null) return NotFound();

            task.Update(model);
        }

        await context.SaveChangesAsync();
        return model;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> DeleteTask(string id)
    {
        var member = httpContextAccessor.HttpContext?.User.GetUser();
        if (member == null || string.IsNullOrEmpty(member.UserId)) return NotFound();
        if (member.Identity is "Member") return NotFound();

        var staff = await context.Staffs.Include(x => x.Tasks).FirstOrDefaultAsync(x => x.UserId == member.UserId);
        if (staff == null) return NotFound();

        var task = staff.Tasks.FirstOrDefault(x => x.Id == id);

        if (task == null) return NotFound();

        staff.Tasks.Remove(task);
        await context.SaveChangesAsync();
        return Ok();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TaskModel>> JoinTask(string id)
    {
        var member = httpContextAccessor.HttpContext?.User.GetUser();
        if (member == null || string.IsNullOrEmpty(member.UserId)) return NotFound();
        if (member.Identity is "Member") return NotFound();

        var staff = await context.Staffs.Include(x => x.Tasks).FirstOrDefaultAsync(x => x.UserId == member.UserId);
        if (staff == null) return NotFound();

        var task = await context.Tasks.Include(x => x.Users).FirstOrDefaultAsync(x => x.Id == id);

        if (task != null) return NotFound("您已添加到项目中");

        task = await context.Tasks.Include(x => x.Users).FirstOrDefaultAsync(x => x.Id == id);
        if (task == null) return NotFound();

        task.Users.Add(staff);
        await context.SaveChangesAsync();
        return task;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> LeaveTask(string id)
    {
        var member = httpContextAccessor.HttpContext?.User.GetUser();
        if (member == null || string.IsNullOrEmpty(member.UserId)) return NotFound();
        if (member.Identity is "Member") return NotFound();

        var staff = await context.Staffs.Include(x => x.Tasks).FirstOrDefaultAsync(x => x.UserId == member.UserId);
        if (staff == null) return NotFound();

        var task = await context.Tasks.Include(x => x.Users).FirstOrDefaultAsync(x => x.Id == id);

        if (task == null) return NotFound("您未曾在这个计划中");

        task.Users.Remove(staff);
        await context.SaveChangesAsync();
        return Ok();
    }

    [HttpGet("{id}/{taskId}")]
    [Authorize(Roles = "Founder, President, TechnologyMinister, PracticalMinister, NewMediaMinister")]
    public async Task<ActionResult> LetChangeTask(string id, string taskId)
    {
        var member = httpContextAccessor.HttpContext?.User.GetUser();
        if (member == null || string.IsNullOrEmpty(member.UserId)) return NotFound();
        if (member.Identity is "Member") return NotFound();

        var staff = await context.Staffs.FirstOrDefaultAsync(x => x.UserId == id);
        if (staff == null) return NotFound();

        if (!staff.Identity.Contains("Member")) return NotFound("您没有权限");

        var task = await context.Tasks.Include(x => x.Users).FirstOrDefaultAsync(x => x.Id == taskId);
        if (task == null) return NotFound();

        if (task.Users.Any(x => x.UserId == id))
        {
            task.Users.Remove(staff);
        }
        else
        {
            task.Users.Add(staff);
        }

        await context.SaveChangesAsync();
        return Ok();
    }

    #endregion
}