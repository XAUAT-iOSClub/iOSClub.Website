namespace iOSClub.WebSite.Models;

public class LoginModel
{
    public string Name { get; set; } = "";
    public string Id { get; set; } = "";
}
public class MarkAnchorModel
{
    public int Level { get; set; }
    public string Name { get; set; } = "";
    public string Link { get; set; } = "";
    public List<MarkAnchorModel> Children { get; set; } = [];
}