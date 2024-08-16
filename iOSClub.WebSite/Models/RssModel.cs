using System.Text.Json.Nodes;

namespace iOSClub.WebSite.Models;

public class RssModel
{
    public string Url { get; set; } = "";
    public string Image { get; set; } = "";
    public string Title { get; set; } = "";
}

public static class RssArticle
{
    public static async Task<RssModel[]> GetArticleAsync()
    {
        using var client = new HttpClient();
        var json = await client.GetStringAsync("https://test.xauat.site/feeds/MP_WXS_3226711201.json");
        var a = JsonNode.Parse(json);

        if (a == null) return [];

        return a["items"]?.AsArray()
            .Select(x => new RssModel()
            {
                Url = x?["url"]?.GetValue<string>() ?? "",
                Image = x?["image"]?.GetValue<string>() ?? "",
                Title = x?["title"]?.GetValue<string>() ?? ""
            })
            .ToArray() ?? [];
    }
}