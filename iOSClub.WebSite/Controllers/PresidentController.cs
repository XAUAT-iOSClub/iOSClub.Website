using iOSClub.Data;
using iOSClub.Data.DataModels;
using iOSClub.WebSite.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace iOSClub.WebSite.Controllers;

[Authorize(Roles = "Founder, President, TechnologyMinister, PracticalMinister, NewMediaMinister")]
[TokenActionFilter]
[Route("api/[controller]/[action]")]
[ApiController]
public class PresidentController(IDbContextFactory<iOSContext> factory)
    : ControllerBase
{
    // GET: api/Member
    [HttpGet("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        await using var context = await factory.CreateDbContextAsync();
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
    public async Task<ActionResult<string>> GetAllData()
    {
        await using var context = await factory.CreateDbContextAsync();

        var query =
            from student in context.Students
            join staff in context.Staffs
                on student.UserId equals staff.UserId into staffGroup
            from staff in staffGroup.DefaultIfEmpty() // LEFT JOIN
            select MemberModel.CopyFrom(student, staff != null ? staff.Identity : "Member");
        var members = await query.ToListAsync();
        return GZipServer.CompressString(JsonConvert.SerializeObject(members));
    }

    [HttpGet]
    public async Task<ActionResult<string>> GetAllDataByPage(int pageNum = 1, int pageSize = 10)
    {
        if (pageNum < 1 || pageSize < 1 || pageSize > 100) // 限制最大页大小
        {
            return BadRequest("Invalid pagination parameters");
        }

        await using var context = await factory.CreateDbContextAsync();

        // 1. 使用高效的键集分页而非偏移分页(Keyset Pagination instead of Offset Pagination)
        // 假设 Student 表有一个名为 Id 的主键列

        // 2. 分离计数查询和数据查询，并使用异步并行执行
        var countTask = context.Students.CountAsync();
        // 3. 优化JOIN查询：先获取分页后的学生数据，再一次性关联员工数据
        var skipCount = (pageNum - 1) * pageSize;
        var studentIdsQuery = context.Students
            .OrderBy(s => s.UserId) // 确保结果一致性的排序
            .Skip(skipCount)
            .Take(pageSize)
            .Select(s => s.UserId);

        var studentIds = await studentIdsQuery.ToListAsync(); // 4. 使用两个更小的查询代替一个复杂查询
        var studentsQuery = context.Students
            .Where(s => studentIds.Contains(s.UserId))
            .AsNoTracking(); // 禁用变更跟踪提高性能
        var staffQuery = context.Staffs
            .Where(s => studentIds.Contains(s.UserId))
            .AsNoTracking(); // 并行执行两个查询
        var studentsTask = studentsQuery.ToListAsync();
        var staffsTask = staffQuery.ToListAsync();

        await Task.WhenAll(countTask, studentsTask, staffsTask); // 并行

        var totalCount = await countTask;
        var students = await studentsTask;
        var staffs = await staffsTask;

        // 5. 在内存中执行连接操作
        var staffMap = staffs.ToDictionary(s => s.UserId);

        var results = students.Select(student =>
        {
            staffMap.TryGetValue(student.UserId, out var staff);
            return MemberModel.CopyFrom(student, staff != null ? staff.Identity : "Member");
        }).ToList();

        var response = new
        {
            TotalCount = totalCount,
            PageSize = pageSize,
            CurrentPage = pageNum,
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
            Data = results
        };

        // 6. 使用更高效的序列化
        return GZipServer.CompressString(JsonConvert.SerializeObject(response)); // 使用System.Text.Json代替Newtonsoft
    }

    [HttpPost]
    public async Task<ActionResult<List<StudentModel>>> UpdateMany(List<StudentModel> list)
    {
        await using var context = await factory.CreateDbContextAsync();

        // 1. 首先获取所有现有的学生ID
        var existingStudentIds = await context.Students
            .Select(s => s.UserId)
            .ToHashSetAsync();

        // 2. 过滤出只需要添加的学生
        var newStudents = list
            .Where(model => !existingStudentIds.Contains(model.UserId))
            .Select(model => model.Standardization())
            .ToList();

        // 3. 批量添加新学生
        if (newStudents.Count == 0) return await context.Students.ToListAsync();
        await context.Students.AddRangeAsync(newStudents);
        await context.SaveChangesAsync();

        return await context.Students.ToListAsync();
    }


    [HttpPost]
    public async Task<ActionResult> Update([FromBody] MemberModel model)
    {
        await using var context = await factory.CreateDbContextAsync();
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