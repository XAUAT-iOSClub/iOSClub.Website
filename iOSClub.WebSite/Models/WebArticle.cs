using System.Xml.Serialization;

namespace iOSClub.WebSite.Models;

[XmlRoot(ElementName = "entry")]
public class Entry
{
    [XmlElement(ElementName = "title")] public string Title { get; set; } = "";

    [XmlElement(ElementName = "link")] public Link[] Link { get; set; } = [];
    [XmlElement(ElementName = "updated")] public DateTime Updated { get; set; }
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
        using var client = new HttpClient();
        var xmlContent = await client.GetStringAsync("https://test.xauat.site/feeds/all.atom");

        var serializer = new XmlSerializer(typeof(Feed));
        using var reader = new StringReader(xmlContent);
        var feed = (Feed)serializer.Deserialize(reader)!;
        return feed.Entries;
    }
}