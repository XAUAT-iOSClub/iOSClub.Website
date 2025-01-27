using System.Text.Json.Nodes;

namespace iOSClub.WebSite.Models;

public class RssModel
{
    public string Url { get; init; } = "";
    public string Image { get; init; } = "";
    public string Title { get; init; } = "";
}

public static class RssArticle
{
    public static async Task<RssModel[]> GetArticleAsync()
    {
        using var client = new HttpClient();
        try
        {
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
        catch
        {
            return [];
        }
    }
}