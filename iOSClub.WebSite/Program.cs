using System.Text;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using iOSClub.Data;
using iOSClub.Data.DataModels;
using iOSClub.WebSite.Client;
using iOSClub.WebSite.Components;
using iOSClub.WebSite.IdentityModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.WebEncoders;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddAntDesign();
builder.Services.AddControllers();

// 身份验证
builder.Services.AddScoped<AuthenticationStateProvider, JwtProvider>();
builder.Services.AddAuthorizationCore();
builder.Services.AddAuthentication(options => { options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme; })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = false, //是否验证Issuer
            ValidateAudience = false, //是否验证Audience
            ValidateIssuerSigningKey = true, //是否验证SecurityKey

            IssuerSigningKey =
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]!)), //SecurityKey
            ValidateLifetime = true, //是否验证失效时间
            ClockSkew = TimeSpan.FromSeconds(30), //过期时间容错值，解决服务器端时间不同步问题（秒）
            RequireExpirationTime = true,
        };
    });

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin() //允许任何来源的主机访问
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services.AddSingleton(new JwtHelper(builder.Configuration));
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<TokenActionFilter>();
builder.Services.AddHttpClient();

// 数据库
var sql = Environment.GetEnvironmentVariable("SQL", EnvironmentVariableTarget.Process);
if (string.IsNullOrEmpty(sql))
{
    builder.Services.AddDbContextFactory<iOSContext>(opt =>
        opt.UseSqlite("Data Source=Data.db",
            o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)));
}
else
{
    builder.Services.AddDbContextFactory<iOSContext>(opt =>
        opt.UseNpgsql(sql,
            o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)));
}

builder.Services.Configure<WebEncoderOptions>(options =>
    options.TextEncoderSettings = new TextEncoderSettings(UnicodeRanges.All));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<iOSContext>();

    try
    {
        context.Database.Migrate();
    }
    catch (Exception e)
    {
        context.Database.EnsureCreated();
        Console.WriteLine(e.Message);
    }

    if (!context.Staffs.Any())
    {
        var user = Environment.GetEnvironmentVariable("USER", EnvironmentVariableTarget.Process);
        var model = new StaffModel() { Identity = "Founder", Name = "root", UserId = "0000000000" };
        var users = user?.Split(',');
        if (!string.IsNullOrEmpty(user) && users != null)
        {
            if (users.Length > 0)
                model.Name = users[0];
            if (users.Length > 1)
                model.UserId = users[1];
        }

        context.Staffs.Add(model);
    }

    if (!context.Departments.Any())
    {
        var software = new DepartmentModel() { Name = "软件部" };
        var hardware = new DepartmentModel() { Name = "硬件部" };
        var practice = new DepartmentModel() { Name = "交流实践部" };
        var newMedia = new DepartmentModel() { Name = "新媒体部" };
        context.Departments.AddRange(software, hardware, practice, newMedia);
        var staffs = await context.Staffs.ToListAsync();
        foreach (var staff in staffs)
        {
            if (staff.Identity.Contains("Technology"))
            {
                staff.Department = software;
                staff.Identity = staff.Identity.Contains("Minister") ? "Minister" : "Department";
            }else if (staff.Identity.Contains("Practical"))
            {
                staff.Department = practice;
                staff.Identity = staff.Identity.Contains("Minister") ? "Minister" : "Department";
            }else if (staff.Identity.Contains("NewMedia"))
            {
                staff.Department = newMedia;
                staff.Identity = staff.Identity.Contains("Minister") ? "Minister" : "Department";
            }
            await context.SaveChangesAsync();
        }
    }

    await context.SaveChangesAsync();
    await context.DisposeAsync();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(_Imports).Assembly);

app.Run();