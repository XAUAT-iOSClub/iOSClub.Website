using System.Collections.Frozen;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using AntDesign;
using AntDesign.Charts;
using iOSClub.Data;
using iOSClub.Data.DataModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace iOSClub.WebSite.CentreToolPages.AdminPages;

public partial class MemberData
{
    [Inject] [NotNull] public IJSRuntime? JS { get; set; }
    [Inject] [NotNull] public IDbContextFactory<iOSContext>? DbFactory { get; set; }

    private bool _runningStyle;

    private bool RunningStyle
    {
        get => _runningStyle;
        set
        {
            _runningStyle = value;
            StateHasChanged();
        }
    }

    #region Data Download

    private async Task JsonDownload()
    {
        var options = new JsonSerializerOptions();
        options.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
        var jsonString = JsonSerializer.Serialize(Models, options);
        var data = Encoding.UTF8.GetBytes(jsonString);

        await Download("data.json", data);
    }

    private async Task Download(string fileName, byte[] data)
    {
        RunningStyle = true;
        await JS.InvokeVoidAsync("jsSaveAsFile", fileName, Convert.ToBase64String(data));
        HandleCancel();
        RunningStyle = false;
    }

    private async Task CsvDownload()
    {
        var jsonString = StudentModel.GetCsv(Models);
        var data = Encoding.UTF8.GetBytes(jsonString);
        await Download("data.csv", data);
    }

    #endregion

    #region Data Chart

    private readonly List<object> _yearData = [];

    private readonly LineConfig _yearConfig = new()
    {
        Padding = "auto",
        XField = "year",
        YField = "value",
        Smooth = true,
        YAxis = new ValueAxis
        {
            Nice = true
        },
        Label = new Label
        {
            Visible = false
        }
    };

    private readonly List<object> _collegeData = [];

    private readonly PieConfig _collegeConfig = new()
    {
        AppendPadding = 10,
        InnerRadius = 0.6,
        Radius = 0.8,
        Padding = "auto",
        AngleField = "value",
        ColorField = "type"
    };

    private readonly List<object> _gradeData = [];

    private readonly BarConfig _gradeConfig = new()
    {
        XField = "人数",
        YField = "年级",
        Label = new BarViewConfigLabel
        {
            Visible = true
        }
    };

    private readonly List<object> _genderData = [];

    private readonly List<object> _landscapeData = [];

    private readonly ColumnConfig _landscapeConfig = new()
    {
        Padding = "auto",
        XField = "type",
        YField = "sales",
        Meta = new
        {
            Type = new
            {
                Alias = "政治面貌"
            },
            Sales = new
            {
                Alias = "人数"
            }
        }
    };

    #endregion

    #region Search

    private static IEnumerable<string> SearchItems => new[] { "姓名", "学号", "学院" };
    private string SearchItem { get; set; } = "姓名";

    private string _searchValue = "";

    private string SearchValue
    {
        get => _searchValue;
        set
        {
            _searchValue = value;
            GetData();
        }
    }

    private async void GetData()
    {
        if (string.IsNullOrEmpty(_searchValue))
        {
            Models = [];
            return;
        }

        RunningStyle = true;
        var context = await DbFactory.CreateDbContextAsync();
        Models = SearchItem switch
        {
            "姓名" => await context.Students.Where(x => x.UserName.StartsWith(_searchValue)).ToListAsync(),
            "学号" => await context.Students.Where(x => x.UserId.StartsWith(_searchValue)).ToListAsync(),
            "学院" => await context.Students.Where(x => x.Academy.StartsWith(_searchValue)).ToListAsync(),
            _ => Models
        };
        RunningStyle = false;

        await context.DisposeAsync();
    }

    #endregion


    private List<StudentModel> DeleteModels { get; } = [];

    private List<StudentModel> _models = [];

    private int Total { get; set; }

    private List<StudentModel> Models
    {
        get => _models;
        set
        {
            _models = value;
            Total = value.Count;
            StateHasChanged();
        }
    }

    private string GenderWord { get; set; } = "";

    protected override async Task OnInitializedAsync()
    {
        if (Models.Count != 0) return;
        await using var context = await DbFactory.CreateDbContextAsync();
        Models = await context.Students.AsNoTracking().ToListAsync();
        var a = Models.ToFrozenSet();
        if (context.Students.Count() < 430) return;
        _yearData.AddRange([new { year = "2021 - 2022", value = 231 }, new { year = "2022 - 2023", value = 429 }]);
        var (year, month, _) = DateTime.Today;
        for (var i = year - 2024; i >= 0; i--)
        {
            _yearData.Add(new
            {
                year = $"{year - i - 1} - {year - i}", value = a.Count(s =>
                    DateTime.Parse(s.JoinTime) < DateTime.Parse($"{year - i}-09-01"))
            });
        }

        if (month >= 9)
        {
            _yearData.Add(new { year = $"{year} - {year+1}", value = a.Count });
        }

        a.GroupBy(x => x.Academy)
            .OrderBy(x => x.Count())
            .ForEach(college
                => _collegeData.Add(new { type = college.Key, value = college.Count() }));

        a.GroupBy(x => x.UserId[..2])
            .OrderBy(x => x.Key)
            .ForEach(grade
                => _gradeData.Add(new { 年级 = grade.Key, 人数 = grade.Count() }));

        context.Students.GroupBy(x => x.PoliticalLandscape)
            .ForEach(l
                => _landscapeData.Add(new { type = l.Key, sales = l.Count() }));

        var man = a.Count(x => x.Gender == "男");
        var woman = a.Count(x => x.Gender == "女");

        _genderData.AddRange(new List<object>
        {
            new { type = "男", value = man },
            new { type = "女", value = woman }
        });

        GenderWord = $"社团现有男生{man}人，女生{woman}人，比例为{(double)man / woman:P}，男生占总人数{(double)man / (man + woman):P}";
    }

    private async Task Flushed()
    {
        await using var context = await DbFactory.CreateDbContextAsync();
        Models = await context.Students.ToListAsync();
    }

    private async Task Delete(StudentModel model)
    {
        var context = await DbFactory.CreateDbContextAsync();
        context.Students.Remove(model);
        await context.SaveChangesAsync();
        Models.Remove(model);
        DeleteModels.Add(model);
        await context.DisposeAsync();
    }


    private async Task Recover()
    {
        var context = await DbFactory.CreateDbContextAsync();
        foreach (var item in DeleteModels)
        {
            await context.Students.AddAsync(item);
            Models.Add(item);
        }

        DeleteModels.Clear();
        await context.SaveChangesAsync();
        await context.DisposeAsync();
    }

    private bool Visible;
    private StudentModel ChangeModel { get; set; } = new();

    private void CloseTable()
    {
        Visible = false;
        ChangeModel = new StudentModel();
    }

    private void OpenTable(StudentModel model)
    {
        Visible = true;
        ChangeModel = model;
    }

    private async Task UploadFiles(InputFileChangeEventArgs e)
    {
        var reader = new StreamReader(e.File.OpenReadStream());
        var result = await reader.ReadToEndAsync();
        reader.Dispose();
        if (string.IsNullOrEmpty(result)) return;
        var list = JsonConvert.DeserializeObject<List<StudentModel>>(result)!;
        var context = await DbFactory.CreateDbContextAsync();

        RunningStyle = true;

        foreach (var model in list)
        {
            if (await context.Students.AnyAsync(x => x.UserId == model.UserId)) continue;
            await context.Students.AddAsync(model.Standardization());
        }

        await context.SaveChangesAsync();

        RunningStyle = false;
        Models = await context.Students.ToListAsync();
        await context.DisposeAsync();
    }

    private void FileDownload() => _visible = true;

    private bool _visible;

    private void HandleCancel() => _visible = false;

    private async Task RemoveAll()
    {
        var context = await DbFactory.CreateDbContextAsync();
        await context.Students.ExecuteDeleteAsync();
        await context.SaveChangesAsync();
        Models.Clear();
        await context.DisposeAsync();
    }
}