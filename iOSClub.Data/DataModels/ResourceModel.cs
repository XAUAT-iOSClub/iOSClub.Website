using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iOSClub.Data.DataModels;

public class ResourceModel : DataModel
{
    [Key][Column(TypeName = "varchar(33)")] public string Id { get; set; } = "";

    [Column(TypeName = "varchar(20)")] public string Name { get; set; } = "";

    [Column(TypeName = "varchar(512)")] public string? Description { get; set; }

    [Column(TypeName = "varchar(50)")] public string? Tag { get; set; }
}