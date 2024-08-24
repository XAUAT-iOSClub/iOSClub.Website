using Markdig;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;

string[] Academies =
[
    "信息与控制工程学院",
    "理学院",
    "机电工程学院",
    "管理学院",
    "土木工程学院",
    "环境与市政工程学院",
    "建筑设备科学与工程学院",
    "材料科学与工程学院",
    "冶金工程学院",
    "资源工程学院",
    "城市发展与现代交通学院",
    "文学院",
    "艺术学院",
    "建筑学院",
    "马克思主义学院",
    "公共管理学院",
    "化学与化工学院",
    "体育学院",
    "安德学院",
    "未来技术学院",
    "国际教育学院"
];

foreach (var academy in Academies)
{
    Console.WriteLine(academy);
}


//
// string[] Get(string id){
//     string[] result = new string[5];
//     result[0] = id.Substring(0,2);
//     result[1] = id.Substring(2,2);
//     result[2] = id.Substring(3,2);
//     result[3] = id.Substring(4,2);
//     result[4] = id.Substring(8,2);
//     return result;
// }
// string[] result = Get("1234567890");
// foreach (var s in result)
// {
//     Console.WriteLine(s);
// }

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