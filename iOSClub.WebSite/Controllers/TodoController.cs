using iOSClub.Data;
using iOSClub.Data.DataModels;
using iOSClub.WebSite.Client.IdentityModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iOSClub.WebSite.Controllers;

[Authorize]
[TokenActionFilter]
[Route("api/[controller]/[action]")]
[ApiController]
public class TodoController(
    IDbContextFactory<iOSContext> factory,
    IHttpContextAccessor httpContextAccessor)
    : ControllerBase
{
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

    [HttpPost]
    public async Task<ActionResult<string>> AddTodo(TodoModel todoModel)
    {
        await using var context = await factory.CreateDbContextAsync();
        var member = httpContextAccessor.HttpContext?.User.GetUser();
        if (member == null) return NotFound();
        var student = await context.Students.FirstOrDefaultAsync(x => x.UserId == member.UserId);
        if (student == null) return NotFound();

        todoModel.StudentId = student.UserId;
        todoModel.Id = todoModel.ToHash();

        await context.Todos.AddAsync(todoModel);
        await context.SaveChangesAsync();
        return todoModel.Id;
    }

    [HttpPost]
    public async Task<IActionResult> Update(TodoModel todoModel)
    {
        await using var context = await factory.CreateDbContextAsync();
        var member = httpContextAccessor.HttpContext?.User.GetUser();
        if (member == null) return NotFound();

        var todo = await context.Todos.FirstOrDefaultAsync(x => x.Id == todoModel.Id);
        todo?.Update(todoModel);
        await context.SaveChangesAsync();

        return Ok();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        await using var context = await factory.CreateDbContextAsync();
        var member = httpContextAccessor.HttpContext?.User.GetUser();
        if (member == null) return NotFound();

        var todo = await context.Todos.FirstOrDefaultAsync(x => x.Id == id);
        if (todo == null || todo.StudentId != member.UserId) return NotFound();
        context.Todos.Remove(todo);
        await context.SaveChangesAsync();
        
        return Ok();
    }
}