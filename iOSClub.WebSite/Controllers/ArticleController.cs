using iOSClub.Data;
using iOSClub.Data.DataModels;
using iOSClub.WebSite.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iOSClub.WebSite.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ArticleController(
    IDbContextFactory<iOSContext> factory)
    : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<ArticleModel>>> GetArticles()
    {
        await using var context = await factory.CreateDbContextAsync();
        return await context.Articles.OrderBy(x => x.LastWriteTime).ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ArticleModel?>> GetArticle(string id)
    {
        await using var context = await factory.CreateDbContextAsync();
        return await context.Articles.FirstOrDefaultAsync(x => x.Path == id);
    }

    [HttpGet("Rss")]
    public async Task<IActionResult> GetRss()
    {
        var RssModels = await RssArticle.GetArticleAsync();
        var Entries = await WebArticle.GetArticleAsync();
        return Ok(new
        {
            RssModels,
            Entries
        });
    }
}