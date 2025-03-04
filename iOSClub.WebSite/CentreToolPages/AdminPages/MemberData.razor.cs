﻿using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using AntDesign;
using AntDesign.Charts;
using iOSClub.Data;
using iOSClub.Data.DataModels;
using iOSClub.WebSite.Models;
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
    [Inject] [NotNull] public NavigationManager? Nav { get; set; }


    #region Data Download

    private async Task JsonDownload()
    {
        var options = new JsonSerializerOptions();
        options.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
        await using var context = await DbFactory.CreateDbContextAsync();
        var Models = await context.Students.AsNoTracking().ToArrayAsync();
        var jsonString = JsonSerializer.Serialize(Models, options);
        var data = Encoding.UTF8.GetBytes(jsonString);

        await Download("data.json", data);
    }

    private async Task Download(string fileName, byte[] data)
    {
        await JS.InvokeVoidAsync("jsSaveAsFile", fileName, Convert.ToBase64String(data));
        HandleCancel();
    }

    private async Task CsvDownload()
    {
        await using var context = await DbFactory.CreateDbContextAsync();
        var Models = await context.Students.AsNoTracking().ToListAsync();
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

    private List<AcademyCount> _collegeData = [];

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
    private string GenderWord { get; set; } = "";
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

    private static IEnumerable<string> SearchItems => ["姓名", "学号", "学院"];
    private string SearchItem { get; set; } = "姓名";

    private string SearchValue { get; set; } = "";

    private async Task GetData(string s)
    {
        SearchValue = s;
        var context = await DbFactory.CreateDbContextAsync();

        await ShowChange(context);

        await context.DisposeAsync();
    }

    private async Task GetDataFromItem(string s)
    {
        SearchItem = s;
        if (string.IsNullOrEmpty(SearchValue)) return;
        var context = await DbFactory.CreateDbContextAsync();

        await ShowChange(context);

        await context.DisposeAsync();
    }

    #endregion

    #region Data

    private List<StudentModel> ShowData { get; set; } = [];
    private int PageSize { get; set; } = 10;
    private int PageIndex { get; set; } = 1;

    private async Task ShowChange(iOSContext context)
    {
        if (string.IsNullOrEmpty(SearchValue))
        {
            ShowData = await context.Students.OrderBy(x => x.UserId).Skip((PageIndex - 1) * PageSize).Take(PageSize)
                .ToListAsync();
            Total = await context.Students.CountAsync();
            return;
        }

        switch (SearchItem)
        {
            case "姓名":
                Total = await context.Students.Where(x => x.UserName.StartsWith(SearchValue)).CountAsync();
                if ((PageIndex - 1) * PageSize >= Total)
                    PageIndex = 1;
                ShowData = await context.Students.Where(x => x.UserName.StartsWith(SearchValue))
                    .OrderBy(x => x.UserId).Skip((PageIndex - 1) * PageSize).Take(PageSize).ToListAsync();
                break;
            case "学号":
                Total = await context.Students.Where(x => x.UserId.StartsWith(SearchValue)).CountAsync();
                if ((PageIndex - 1) * PageSize >= Total)
                    PageIndex = 1;
                ShowData = await context.Students.Where(x => x.UserId.StartsWith(SearchValue))
                    .OrderBy(x => x.UserId).Skip((PageIndex - 1) * PageSize).Take(PageSize).ToListAsync();
                break;
            case "学院":
                Total = await context.Students.Where(x => x.Academy.StartsWith(SearchValue)).CountAsync();
                if ((PageIndex - 1) * PageSize >= Total)
                    PageIndex = 1;
                ShowData = await context.Students.Where(x => x.Academy.StartsWith(SearchValue))
                    .OrderBy(x => x.UserId).Skip((PageIndex - 1) * PageSize).Take(PageSize).ToListAsync();
                break;
        }
    }

    private async Task PageIndexChanged(int s)
    {
        PageIndex = s;
        await using var context = await DbFactory.CreateDbContextAsync();
        await ShowChange(context);
    }

    private async Task PageSizeChanged(int s)
    {
        PageSize = s;
        await using var context = await DbFactory.CreateDbContextAsync();
        await ShowChange(context);
    }

    private List<StudentModel> DeleteModels { get; } = [];
    [CascadingParameter] private MemberModel Member { get; set; } = new();
    private int Total { get; set; }

    #endregion

    protected override async Task OnInitializedAsync()
    {
        if (ShowData.Count != 0) return;
        await using var context = await DbFactory.CreateDbContextAsync();
        ShowData = await context.Students.Take(PageSize).ToListAsync();
        Total = await context.Students.CountAsync();
        _yearData.AddRange([
            new { year = "2019学年", value = 33 }, new { year = "2020学年", value = 1 },
            new { year = "2021学年", value = 274 }, new { year = "2022学年", value = 329 }
        ]);
        //_yearList = [231, 429];
        if (Total > 430)
        {
            var (year, month, _) = DateTime.Today;
            for (var i = year - 2024; i >= 0; i--)
            {
                var date = $"{year - i}-09-01";
                var a = year - i - 2005;
#if DEBUG
                var v = await context.Database
                    .SqlQuery<int>($"""
                                    SELECT COUNT(*) as "Value" FROM "Students" WHERE "JoinTime" < {date} AND CAST(SUBSTRING("UserId", 1, 2) AS INTEGER) > {a}
                                    """).FirstAsync();
#else
                var v = await context.Database
                    .SqlQuery<int>($"""
                                    SELECT COUNT(*) as "Value" FROM "Students" WHERE "JoinTime" < cast({date} as DATE) AND CAST(SUBSTRING("UserId" FROM 1 FOR 2) AS INTEGER) > {a}
                                    """).FirstAsync();
#endif

                //_yearList.Add(v);

                _yearData.Add(new
                {
                    year = $"{year - i - 1}学年", value = v
                });
            }

            if (month >= 9)
            {
#if DEBUG
                var v = await context.Database
                    .SqlQuery<int>($"""
                                    SELECT COUNT(*) as "Value" FROM "Students" WHERE CAST(SUBSTRING("UserId", 1, 2) AS INTEGER) > {year - 2004}
                                    """).FirstAsync();
#else
                var v = await context.Database
                    .SqlQuery<int>($"""
                                    SELECT COUNT(*) as "Value" FROM "Students" WHERE CAST(SUBSTRING("UserId" FROM 1 FOR 2) AS INTEGER) > {year - 2004}
                                    """).FirstAsync();
#endif
                _yearData.Add(new { year = $"{year}学年", value = v });
                //_yearList.Add(v);
            }

            //GradientDescent(years);
        }

        _collegeData = await context.Database
            .SqlQuery<AcademyCount>(
                $"SELECT \"Academy\" AS type, COUNT(*) AS value FROM \"Students\" GROUP BY \"Academy\" ORDER BY COUNT(*) DESC")
            .ToListAsync();

        context.Database
            .SqlQuery<AcademyCount>(
                $"SELECT SUBSTRING(\"UserId\", 1, 2) AS type, COUNT(*) AS value FROM \"Students\" GROUP BY SUBSTRING(\"UserId\", 1, 2) ORDER BY type")
            .ForEach(grade =>
                _gradeData.Add(new { 年级 = grade.Type + "级", 人数 = grade.Value }));

        context.Database
            .SqlQuery<AcademyCount>(
                $"SELECT \"PoliticalLandscape\" AS type, COUNT(*) AS value FROM \"Students\" GROUP BY \"PoliticalLandscape\"")
            .ForEach(l
                => _landscapeData.Add(new { type = l.Type, sales = l.Value }));

        var man = await context.Students.CountAsync(x => x.Gender == "男");
        var woman = await context.Students.CountAsync(x => x.Gender == "女");

        _genderData.AddRange(new List<object>
        {
            new { type = "男", value = man },
            new { type = "女", value = woman }
        });

        GenderWord = $"社团现有男生{man}人，女生{woman}人，比例为{(double)man / woman:P}，男生占总人数{(double)man / (man + woman):P}";
    }

    private void Flushed()
    {
        Nav.Refresh(true);
    }

    private async Task Delete(StudentModel model)
    {
        var context = await DbFactory.CreateDbContextAsync();
        context.Students.Remove(model);
        await context.SaveChangesAsync();
        DeleteModels.Add(model);
        await context.DisposeAsync();
        await ShowChange(context);
    }


    private async Task Recover()
    {
        var context = await DbFactory.CreateDbContextAsync();
        foreach (var item in DeleteModels)
        {
            await context.Students.AddAsync(item);
        }

        DeleteModels.Clear();
        await context.SaveChangesAsync();
        await context.DisposeAsync();

        Flushed();
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

        var b = await context.Students.ToListAsync();

        foreach (var model in list.Where(model => model.IsStandardization()))
        {
            if (b.Any(x => x.UserId == model.UserId)) continue;
            var m = model.Standardization();
            await context.Students.AddAsync(m);
            await context.SaveChangesAsync();
        }

        Total = await context.Students.CountAsync();
        await ShowChange(context);
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
        await context.DisposeAsync();
        Flushed();
    }

    [Serializable]
    public class AcademyCount
    {
        public string Type { get; set; } = "";
        public int Value { get; set; }
    }


    private async Task CollegeMore(string college)
    {
        SearchItem = "学院";
        // SearchValue = college;
        await GetData(college);
        ActiveKey = "1";
    }

    private string ActiveKey { get; set; } = "1";
}