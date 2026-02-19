using sevDesk.NET.Docs.Models;

namespace sevDesk.NET.Docs.Services;

public interface IDocContentService
{
    Task InitializeAsync();
    List<NavSection> GetNavSections();
    List<SearchEntry> Search(string query);
    Task<DocArticle?> GetArticleAsync(string slug);
}
