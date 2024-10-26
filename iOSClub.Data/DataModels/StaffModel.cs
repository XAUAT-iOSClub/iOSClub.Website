using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iOSClub.Data.DataModels;

public class StaffModel
{
    [Key]
    [Column(TypeName = "varchar(10)")]
    public string UserId { get; set; } = "";

    [Column(TypeName = "varchar(50)")] public string Name { get; set; } = "";

    /// <summary>
    /// Founder : 创始人
    /// President : 社长,团支书,秘书长
    /// Minister : 部长
    /// Department : 部员成员
    /// Member : 普通成员
    /// </summary>
    [Column(TypeName = "varchar(20)")]
    public string Identity { get; set; } = "Member";

    public DepartmentModel? Department { get; set; } = new();

    public List<ProjectModel> Projects { get; init; } = [];
    public List<TaskModel> Tasks { get; init; } = [];
}