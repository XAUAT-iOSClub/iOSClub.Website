using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iOSClub.Data.DataModels;

public class ToolModel : DataModel
{
    [Key] [Column(TypeName = "varchar(33)")]public string Id { get; set; } = "";

    [Column(TypeName = "varchar(20)")] public string Name { get; set; } = "";
    [Column(TypeName = "varchar(64)")] public string Url { get; set; } = "";
    [Column(TypeName = "varchar(64)")] public string IconUrl { get; set; } = "";
    [Column(TypeName = "varchar(64)")] public string Description { get; set; } = "";
    [Column(TypeName = "varchar(64)")] public string? Tag { get; set; }

    public void Update(ToolModel model)
    {
        if (!string.IsNullOrEmpty(model.Name)) Name = model.Name;
        if (!string.IsNullOrEmpty(model.Url)) Url = model.Url;
        if (!string.IsNullOrEmpty(model.IconUrl)) IconUrl = model.IconUrl;
        if (!string.IsNullOrEmpty(model.Description)) Description = model.Description;
        Tag = model.Tag;
    }
}