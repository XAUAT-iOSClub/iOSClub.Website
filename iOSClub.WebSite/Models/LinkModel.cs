using iOSClub.Data;

namespace iOSClub.WebSite.Models;

[Serializable]
public class LinkModel : DataModel
{
    public string Key { get; set; } = "";
    public string Name { get; set; } = "";
    public string? Icon { get; set; } = "";
    public string Url { get; set; } = "";
    public string? Description { get; set; } = "";
    public int Index { get; set; }
}

[Serializable]
public class CategoryModel : DataModel
{
    public string Key { get; set; } = "";

    public string Name { get; set; } = "";
    public string? Description { get; set; } = "";
    public string Icon { get; set; } = "";

    public int Index { get; set; }

    public List<LinkModel> Links { get; set; } = [];
}