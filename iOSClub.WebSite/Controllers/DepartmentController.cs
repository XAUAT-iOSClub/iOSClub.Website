using iOSClub.Data;
using iOSClub.Data.DataModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iOSClub.WebSite.Controllers;

[Authorize]
[TokenActionFilter]
[Route("api/[controller]")]
[ApiController]
public class DepartmentController(
    IDbContextFactory<iOSContext> factory,
    IHttpContextAccessor httpContextAccessor)
    : ControllerBase
{
    [HttpGet("{name?}")]
    public async Task<ActionResult<DepartmentModel>> GetDepartment(string? name)
    {
        var userJwt = httpContextAccessor.HttpContext?.User.GetUser();
        if (userJwt is not { Identity: "Founder" or "President" or "Minister" or "Department" })
            return NotFound("Is Not Data");

        await using var context = await factory.CreateDbContextAsync();

        var user = await context.Staffs.Include(x => x.Department).FirstOrDefaultAsync(x => x.UserId == userJwt.UserId);

        if (user is not { Identity: "Founder" or "President" or "Minister" or "Department" })
            return NotFound("Data is Error");
        var userDepartment = user.Department;
        if (string.IsNullOrEmpty(name))
        {
            if (userDepartment == null)
            {
                return NotFound();
            }

            return userDepartment;
        }

        if (user.Identity is "Founder" or "President" or "Minister")
        {
            var department = await context.Departments.FirstOrDefaultAsync(x => x.Name == name);
            if (department is null) return NotFound();
            return department;
        }

        if (userDepartment is null || userDepartment.Name != name) return NotFound();

        return userDepartment;
    }

    [HttpPost("UpdateDepartment")]
    public async Task<IActionResult> UpdateDepartment([FromBody] DepartmentModel model)
    {
        var userJwt = httpContextAccessor.HttpContext?.User.GetUser();
        if (userJwt is not { Identity: "Founder" or "President" or "Minister"})
            return NotFound("Is Not Data");

        await using var context = await factory.CreateDbContextAsync();

        var user = await context.Staffs.Include(x => x.Department).FirstOrDefaultAsync(x => x.UserId == userJwt.UserId);

        if (user is not { Identity: "Founder" or "President" or "Minister" })
            return NotFound("Data is Error");
        
        var department = await context.Departments.FirstOrDefaultAsync(x => x.Name == model.Name);
        if (department is null)
        {
            return NotFound();
        }
        
        department.Description = model.Description;
        department.Name = model.Name;
        await context.SaveChangesAsync();

        return Ok();
    }

    // [Authorize(Roles = "Founder, President, Minister")]
    // [HttpGet("AddStaff")]
    // public async Task<IActionResult> AddStaff(string departmentName, string userId)
    // {
    //     
    // }
}