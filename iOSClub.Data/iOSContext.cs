using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using iOSClub.Data.DataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace iOSClub.Data;

public sealed class iOSContext(DbContextOptions<iOSContext> options) : DbContext(options)
{
    public DbSet<StudentModel> Students { get; init; }
    public DbSet<StaffModel> Staffs { get; init; }
    public DbSet<TaskModel> Tasks { get; init; }
    public DbSet<TodoModel> Todos { get; init; }
    public DbSet<ProjectModel> Projects { get; init; }
    public DbSet<ResourceModel> Resources { get; init; }
    public DbSet<DepartmentModel> Departments { get; init; }

    public DbSet<ArticleModel> Articles { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TodoModel>()
            .HasOne(x => x.Student).WithMany()
            .HasForeignKey(e => e.StudentId)
            .IsRequired();

        modelBuilder.Entity<StaffModel>()
            .HasMany(x => x.Tasks)
            .WithMany(x => x.Users);

        modelBuilder.Entity<StaffModel>()
            .HasMany(x => x.Projects)
            .WithMany(x => x.Staffs);

        modelBuilder.Entity<DepartmentModel>()
            .HasMany(x => x.Staffs)
            .WithOne(x => x.Department)
            .IsRequired(false);

        modelBuilder.Entity<DepartmentModel>()
            .HasMany(x => x.Projects)
            .WithOne(x => x.Department)
            .IsRequired(false);
    }
}

[Serializable]
public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<iOSContext>
{
    public iOSContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<iOSContext>();
        optionsBuilder.UseSqlite("Data Source=Data.db");
        return new iOSContext(optionsBuilder.Options);
    }
}

public static class DataTool
{
    public static string StringToHash(string s)
    {
        var data = Encoding.UTF8.GetBytes(s);
        var hash = MD5.HashData(data);
        var hashStringBuilder = new StringBuilder();
        foreach (var t in hash)
            hashStringBuilder.Append(t.ToString("x2"));
        return hashStringBuilder.ToString();
    }

    public static string ToHash(this object t) => StringToHash(t.ToString()!);

    public static string GetProperties<T>(T t)
    {
        StringBuilder builder = new StringBuilder();
        if (t == null) return builder.ToString();

        var properties = t.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);

        if (properties.Length <= 0) return builder.ToString();

        foreach (var item in properties)
        {
            var name = item.Name;
            var value = item.GetValue(t, null);
            if (item.PropertyType.IsValueType || item.PropertyType.Name.StartsWith("String"))
            {
                builder.Append($"{name}:{value ?? "null"},");
            }
        }

        return builder.ToString();
    }
}

public abstract class DataModel
{
    public override string ToString() => $"{GetType()} : {DataTool.GetProperties(this)}";
}