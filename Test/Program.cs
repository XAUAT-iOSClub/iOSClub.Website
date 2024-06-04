

var markdownStr = File.ReadAllText(@"C:\iOSClub\科技部活动\软著\软著注意事项\软著说明事项.md");
var doc = Markdig.Markdown.Parse(markdownStr);
foreach (var b in doc)
{
    Console.WriteLine(b.ToPositionText());
}