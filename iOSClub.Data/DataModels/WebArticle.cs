using System.Xml.Serialization;

namespace iOSClub.Data.DataModels;

[XmlRoot(ElementName = "entry")]
public class Entry
{
    [XmlElement(ElementName = "title")] public string Title { get; set; } = "";

    [XmlElement(ElementName = "link")] public Link[] Link { get; set; } = [];
}

[XmlRoot(ElementName = "link")]
public class Link
{
    [XmlAttribute(AttributeName = "href")] public string Href { get; set; } = "";
}

[XmlRoot(ElementName = "feed", Namespace = "http://www.w3.org/2005/Atom"), XmlType("feed")]
[Serializable]
public class Feed
{
    [XmlElement(ElementName = "title")] public string Title { get; set; } = "";

    [XmlElement(ElementName = "link")] public string Link { get; set; } = "";

    [XmlElement(ElementName = "updated")] public DateTime Updated { get; set; }

    [XmlElement(ElementName = "entry")] public List<Entry> Entries { get; set; } = [];
}

public static class WebArticle
{
    public static async Task<List<Entry>> GetArticleAsync()
    {
        const string url = "https://test.xauat.site/feeds/MP_WXS_3226711201.atom";
        var xmlContent = await GetXmlContentAsync(url);

        var serializer = new XmlSerializer(typeof(Feed));
        using var reader = new StringReader(xmlContent);
        var feed = (Feed)serializer.Deserialize(reader)!;
        return feed.Entries;
    }

    private static async Task<string> GetXmlContentAsync(string url)
    {
        using var client = new HttpClient();
        return await client.GetStringAsync(url);
    }
}