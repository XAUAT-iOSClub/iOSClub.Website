using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace iOSClub.Data.DataModels;

public class ProjectModel : DataModel
{
    public DepartmentModel? Department { get; set; } = new();
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

    [JsonIgnore]
    public List<StaffModel> Staffs { get; init; } = [];
    
    [JsonIgnore]
    public List<TaskModel> Tasks { get; init; } = [];
}