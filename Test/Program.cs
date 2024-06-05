using Markdig;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;

var info = new FileInfo(@"C:\iOSClub\科技部活动\软著\软著注意事项\软著说明事项.md");
if (info.Exists)
{
    var pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
    var content = await File.ReadAllTextAsync(info.FullName);
    var document = Markdown.Parse(content,pipeline);
    foreach (var node in document.AsEnumerable())
    {
        if (node is HeadingBlock { Level: 1 ,Inline: not null} headingBlock)
        {
            Console.WriteLine(headingBlock.Inline.FirstChild.ToString());
        }
        if (node is not ParagraphBlock { Inline: not null } paragraphBlock) continue;
        foreach (var inline in paragraphBlock.Inline)
        {
            if (inline is LinkInline { IsImage: true } linkInline)
            {
                linkInline.Url = $"wwwroot/ArticleFile/{linkInline.Url}";
            }
        }
    }
}