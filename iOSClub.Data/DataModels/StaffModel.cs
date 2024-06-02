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
    /// President : 社长,副社长,秘书长
    /// TechnologyMinister : 科技部部长/副部长
    /// PracticalMinister : 实践交流部部长/副部长
    /// NewMediaMinister : 新媒体部部长/副部长
    /// TechnologyMember : 科技部成员
    /// PracticalMember : 实践交流部成员
    /// NewMediaMember : 新媒体部成员
    /// Member : 普通成员
    /// </summary>
    [Column(TypeName = "varchar(20)")]
    public string Identity { get; init; } = "Member";

    public List<ProjectModel> Projects { get; init; } = [];
    public List<TaskModel> Tasks { get; init; } = [];
}