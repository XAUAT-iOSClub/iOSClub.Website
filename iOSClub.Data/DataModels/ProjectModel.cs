using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iOSClub.Data.DataModels;

public class ProjectModel : DataModel
{
    [Column(TypeName = "varchar(20)")] public string DepartmentName { get; init; } = "";
    [Column(TypeName = "varchar(20)")] public string Title { get; set; } = "";

    [Key] [Column(TypeName = "varchar(33)")]public string Id { get; set; } = "";

    [Column(TypeName = "varchar(512)")] public string Description { get; set; } = "";
    [Column(TypeName = "varchar(20)")] public string? StartTime { get; set; }
    [Column(TypeName = "varchar(20)")] public string? EndTime { get; set; }

    public void Update(ProjectModel model)
    {
        if (!string.IsNullOrEmpty(model.Title)) Title = model.Title;
        if (!string.IsNullOrEmpty(model.Description)) Description = model.Description;
        if (!string.IsNullOrEmpty(model.StartTime)) StartTime = model.StartTime;
        if (!string.IsNullOrEmpty(model.EndTime)) EndTime = model.EndTime;
    }

    public List<StaffModel> Staffs { get; init; } = [];
    public List<TaskModel> Tasks { get; init; } = [];
}