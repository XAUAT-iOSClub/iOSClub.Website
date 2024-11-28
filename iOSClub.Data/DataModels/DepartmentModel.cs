using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iOSClub.Data.DataModels;

public class DepartmentModel
{
    /// <summary>
    /// 部门名称
    /// </summary>
    [Key]
    [Column(TypeName = "varchar(20)")]
    public string Name { get; set; } = "";

    [Column(TypeName = "varchar(32)")] 
    public string? Description { get; set; }

    /// <summary>
    /// 部员
    /// </summary>
    public List<StaffModel> Staffs { get; init; } = [];

    /// <summary>
    /// 项目
    /// </summary>
    public List<ProjectModel> Projects { get; init; } = [];
}