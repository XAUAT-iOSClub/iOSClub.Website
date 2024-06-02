using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iOSClub.Data.DataModels;

public class FileModel : DataModel
{
    [Column(TypeName = "varchar(64)")]
    public string Path { get; set; } = "";
    [Column(TypeName = "varchar(64)")]
    public string Url { get; set; } = "";
    
    [Column(TypeName = "boolean")]
    public bool IsFolder { get; set; }

    [Key][Column(TypeName = "varchar(33)")] public string Id { get; set; } = "";
}