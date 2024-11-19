using System.Text;
using AntDesign;
using AntDesign.Charts;
using iOSClub.Data.DataModels;
using iOSClub.WebSite.Client.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;
using Newtonsoft.Json;

namespace iOSClub.WebSite.Client.CentreToolPages.DepartmentPages;

public partial class Department
{
    private readonly ListGridType grid = new()
    {
        Gutter = 16,
        Xs = 1,
        Sm = 2,
        Md = 4,
        Lg = 4,
        Xl = 6,
        Xxl = 3
    };

    [Parameter] public string? Key { get; set; }
    [CascadingParameter] private MemberModel Member { get; set; } = new();
    private List<DepartmentList> Departments { get; set; } = [];
    private MemberList Staffs { get; } = new();

    private readonly List<object> _collegeData = [];
    private readonly List<object> _genderData = [];

    private readonly PieConfig _collegeConfig = new()
    {
        AppendPadding = 10,
        InnerRadius = 0.6,
        Radius = 0.8,
        Padding = "auto",
        AngleField = "value",
        ColorField = "type"
    };

    protected override async Task OnInitializedAsync()
    {
        Key ??= "总览";
        await using var db = await DbFactory.CreateDbContextAsync();

        Departments = await db.Departments
            .Include(x => x.Projects)
            .ThenInclude(x => x.Department)
            .Include(x => x.Staffs)
            .Select(x => new DepartmentList()
            {
                Name = x.Name, Projects = x.Projects,
                Ministers = x.Staffs.Where(y => y.Identity == "Minister").ToList(),
                Member = x.Staffs.Where(y => y.Identity == "Department").ToList()
            })
            .ToListAsync();


        Staffs.Ministers = await db.Staffs.Where(x => x.Identity == "President").ToListAsync();
        var departmentMember = new List<StaffModel>();

        departmentMember.AddRange(Staffs.Ministers);
        foreach (var item in Departments)
        {
            foreach (var minister in item.Ministers)
            {
                minister.Department = new DepartmentModel() { Name = item.Name };
                departmentMember.Add(minister);
            }

            foreach (var model in item.Member)
            {
                model.Department = new DepartmentModel() { Name = item.Name };
                departmentMember.Add(model);
            }
        }

        foreach (var model in departmentMember)
        {
            var student = await db.Students.FirstOrDefaultAsync(x => x.UserId == model.UserId);
            if (student == null) continue;
            var member = MemberModel.CopyFrom(student);
            member.Identity = $"{model.Department?.Name} {MemberModel.IdentityDictionary[model.Identity]}";
            Staffs.Member.Add(member);
        }

        var man = Staffs.Member.Count(x => x.Gender == "男");
        var woman = Staffs.Member.Count(x => x.Gender == "女");

        _genderData.AddRange(new List<object>
        {
            new { type = "男", value = man },
            new { type = "女", value = woman }
        });
        
        _collegeData.AddRange(Staffs.Member.GroupBy(x => x.Academy).OrderBy(x => x.Count()).Select(x => new
        {
            type = x.Key,
            value = x.Count()
        }));

        Staffs.Projects = await db.Projects.Include(x => x.Department).ToListAsync();
    }

    private async Task UploadFiles(InputFileChangeEventArgs e)
    {
        var reader = new StreamReader(e.File.OpenReadStream());
        var result = await reader.ReadToEndAsync();
        reader.Dispose();
        if (string.IsNullOrEmpty(result)) return;
        await using var context = await DbFactory.CreateDbContextAsync();

        var json = JsonConvert.DeserializeObject<List<StaffModel>>(result);

        if (json == null) return;

        foreach (var staff in json)
        {
            if (await context.Staffs.AnyAsync(x => x.UserId == staff.UserId)) continue;
            await context.Staffs.AddAsync(staff);
        }

        StateHasChanged();
    }

    private async Task CsvDownload()
    {
        var p = new List<MemberModel>();
        await using var db = await DbFactory.CreateDbContextAsync();
        foreach (var item in Staffs.Ministers)
        {
            var student = await db.Students.FirstOrDefaultAsync(x => x.UserId == item.UserId);
            if (student == null) continue;
            var member = MemberModel.CopyFrom(student);
            member.Identity = item.Identity;
            p.Add(member);
        }

        p.AddRange(Staffs.Member);
        var jsonString = MemberModel.GetCsv(p);
        var data = Encoding.UTF8.GetBytes(jsonString);
        await Download("部员信息.csv", data);
    }

    private async Task Download(string fileName, byte[] data)
    {
        await JS.InvokeVoidAsync("jsSaveAsFile", fileName, Convert.ToBase64String(data));
    }

    private async Task Delete(StaffModel item, List<StaffModel> list)
    {
        await using var dbContext = await DbFactory.CreateDbContextAsync();
        dbContext.Staffs.Remove(item);
        list.Remove(item);
        await dbContext.SaveChangesAsync();
    }

    private async Task Delete(MemberModel model)
    {
        await using var dbContext = await DbFactory.CreateDbContextAsync();
        var s = await dbContext.Staffs.FirstOrDefaultAsync(x => x.UserId == model.UserId);
        if (s == null) return;
        dbContext.Staffs.Remove(s);
        await dbContext.SaveChangesAsync();
        StateHasChanged();
    }

    private bool _visible;

    private void HandleCancel()
    {
        SearchResult.Clear();
        _visible = false;
    }

    private DepartmentList? _department;
    private string _identity = "";
    private List<StudentModel> SearchResult { get; set; } = [];

    private async Task Search(string s)
    {
        await using var context = await DbFactory.CreateDbContextAsync();
        SearchResult = await context.Students.Where(x => x.UserName.StartsWith(s)).ToListAsync();
        StateHasChanged();
    }

    private async Task Add(StudentModel model)
    {
        var staff = new StaffModel() { Name = model.UserName, UserId = model.UserId };

        await using var context = await DbFactory.CreateDbContextAsync();
        if (_department == null)
        {
            staff.Identity = "President";
        }
        else
        {
            staff.Identity = _identity;
            staff.Department = await context.Departments.FirstOrDefaultAsync(x => x.Name == _department.Name);
        }

        if (await context.Staffs.AnyAsync(x => x.UserId == model.UserId))
        {
            await MessageService.Warn("该用户已存在");
        }
        else
        {
            await context.Staffs.AddAsync(staff);
            await context.SaveChangesAsync();

            if (_department == null)
            {
                Staffs.Ministers.Add(staff);
            }
            else
            {
                if (_identity == "Department")
                {
                    _department.Member.Add(staff);
                }
                else
                {
                    _department.Ministers.Add(staff);
                }
            }

            StateHasChanged();
            await MessageService.Success("添加成功");
        }
    }

    public class DepartmentList
    {
        public string Name { get; set; } = "";
        public List<StaffModel> Ministers { get; init; } = [];
        public List<StaffModel> Member { get; init; } = [];
        public List<ProjectModel> Projects { get; init; } = [];

        public void Update(DepartmentModel model)
        {
            Name = model.Name;
            Ministers.Clear();
            Member.Clear();
            foreach (var item in model.Staffs)
            {
                item.Department = model;
                if (item.Identity == "Minister")
                {
                    Ministers.Add(item);
                }
                else
                {
                    Member.Add(item);
                }
            }

            Projects.Clear();
            Projects.AddRange(model.Projects);
        }

        public static DepartmentList CopyFrom(DepartmentModel model)
        {
            var result = new DepartmentList();
            result.Update(model);
            return result;
        }
    }

    public class MemberList
    {
        public List<StaffModel> Ministers { get; set; } = [];
        public List<MemberModel> Member { get; } = [];
        public List<ProjectModel> Projects { get; set; } = [];
    }

    private void OpenAddMember(DepartmentList? model = null, string identity = "Minister")
    {
        _department = model;
        _identity = identity;
        _visible = true;
    }

    private async Task DeleteAll(List<StaffModel> c)
    {
        await using var dbContext = await DbFactory.CreateDbContextAsync();
        dbContext.Staffs.RemoveRange(c);
        await dbContext.SaveChangesAsync();

        c.Clear();
        StateHasChanged();
    }

    private async Task DeleteDepartment(DepartmentList department)
    {
        await using var dbContext = await DbFactory.CreateDbContextAsync();
        var d = await dbContext.Departments
            .Include(x => x.Staffs)
            .Include(x => x.Projects)
            .FirstOrDefaultAsync(x => x.Name == department.Name);
        if (d == null) return;

        foreach (var item in d.Staffs)
        {
            dbContext.Staffs.Remove(item);
        }

        foreach (var item in d.Projects)
        {
            dbContext.Projects.Remove(item);
        }

        dbContext.Departments.Remove(d);
        await dbContext.SaveChangesAsync();
        Departments.Remove(department);

        await MessageService.Success($"删除成功! 删除部门为 {department.Name}");
        StateHasChanged();
    }

    private void AddProj(string id = "")
    {
        Nav.NavigateTo($"/Centre/EditProject/{id}");
    }

    private async Task DeleteProj(ProjectModel item, List<ProjectModel> list)
    {
        await using var dbContext = await DbFactory.CreateDbContextAsync();
        dbContext.Projects.Remove(item);
        await dbContext.SaveChangesAsync();
        list.Remove(item);
        StateHasChanged();
    }

    private bool _departmentVisible;
    private string _changeDepartmentName = "";

    private DepartmentModel _departmentModel = new();

    private void OpenDepartment(DepartmentList? model = null)
    {
        if (model == null)
        {
            _departmentModel = new DepartmentModel();
            _changeDepartmentName = "";
        }
        else
        {
            _departmentModel = new DepartmentModel() { Name = model.Name };
            _changeDepartmentName = model.Name;
        }

        _departmentVisible = true;
    }

    private async Task AddDepartment()
    {
        await using var context = await DbFactory.CreateDbContextAsync();
        var d = await context.Departments
            .Include(x => x.Projects)
            .Include(x => x.Staffs)
            .FirstOrDefaultAsync(x => x.Name == _changeDepartmentName);
        if (!string.IsNullOrEmpty(_changeDepartmentName))
        {
            if (d == null)
            {
                await MessageService.Warn("未找到该部门");
                return;
            }

            context.Departments.Remove(d);
            await context.Departments.AddAsync(_departmentModel);
            _departmentModel.Projects.AddRange(d.Projects);
            _departmentModel.Staffs.AddRange(d.Staffs);

            var a = Departments.FirstOrDefault(x => x.Name == _changeDepartmentName);
            if (a != null)
            {
                a.Update(_departmentModel);
            }
            else
            {
                Departments.Add(DepartmentList.CopyFrom(_departmentModel));
            }
        }
        else
        {
            if (d != null)
            {
                await MessageService.Warn("该部门已存在");
                return;
            }

            await context.Departments.AddAsync(_departmentModel);

            Departments.Add(DepartmentList.CopyFrom(_departmentModel));
        }

        await context.SaveChangesAsync();
        _departmentModel = new DepartmentModel();
        _departmentVisible = false;
        _changeDepartmentName = "";
        await MessageService.Success("添加成功");
    }

    private bool _operaVisible;
}