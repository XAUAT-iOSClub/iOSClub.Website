using Markdig;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;

string[] Get(string id){
    string[] result = new string[5];
    result[0] = id.Substring(0,2);
    result[1] = id.Substring(2,2);
    result[2] = id.Substring(3,2);
    result[3] = id.Substring(4,2);
    result[4] = id.Substring(8,2);
    return result;
}
string[] result = Get("1234567890");
foreach (var s in result)
{
    Console.WriteLine(s);
}

// var info = new FileInfo(@"C:\iOSClub\科技部活动\软著\软著注意事项\软著说明事项.md");
// if (info.Exists)
// {
//     var pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
//     var content = await File.ReadAllTextAsync(info.FullName);
//     var document = Markdown.Parse(content,pipeline);
//     foreach (var node in document.AsEnumerable())
//     {
//         if (node is HeadingBlock { Level: 1 ,Inline: not null} headingBlock)
//         {
//             Console.WriteLine(headingBlock.Inline.FirstChild.ToString());
//         }
//         if (node is not ParagraphBlock { Inline: not null } paragraphBlock) continue;
//         foreach (var inline in paragraphBlock.Inline)
//         {
//             if (inline is LinkInline { IsImage: true } linkInline)
//             {
//                 linkInline.Url = $"wwwroot/ArticleFile/{linkInline.Url}";
//             }
//         }
//     }
// }