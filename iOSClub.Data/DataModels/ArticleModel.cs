using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iOSClub.Data.DataModels;

public class ArticleModel
{
    [Column(TypeName = "varchar(256)")] public string Title { get; init; } = "";

    [Key]
    [Column(TypeName = "varchar(256)")]
    public string Link { get; init; } = "";

    [Column(TypeName = "varchar(256)")] public string Cover { get; init; } = "";

    [Column(TypeName = "varchar(256)")] public string Digest { get; init; } = "";
}