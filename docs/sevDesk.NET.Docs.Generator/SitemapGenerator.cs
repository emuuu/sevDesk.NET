using System.Text;

namespace sevDesk.NET.Docs.Generator;

public class SitemapGenerator
{
    private readonly string _baseUrl;

    private static readonly string[] ClientSlugs =
    [
        "contacts", "invoices", "invoice-positions", "orders", "order-positions",
        "vouchers", "voucher-positions", "credit-notes", "credit-note-positions",
        "parts", "check-accounts", "check-account-transactions",
        "communication-ways", "contact-addresses", "tags", "categories",
        "unities", "tax-rules", "currency-exchange-rates", "documents"
    ];

    public SitemapGenerator(string baseUrl)
    {
        _baseUrl = baseUrl.TrimEnd('/');
    }

    public async Task GenerateAsync(string wwwrootPath, List<ContentIndexEntry> entries)
    {
        await GenerateSitemap(wwwrootPath, entries);
        await GenerateRobotsTxt(wwwrootPath);
    }

    private async Task GenerateSitemap(string wwwrootPath, List<ContentIndexEntry> entries)
    {
        var sb = new StringBuilder();
        sb.AppendLine("""<?xml version="1.0" encoding="UTF-8"?>""");
        sb.AppendLine("""<urlset xmlns="http://www.sitemaps.org/schemas/sitemap/0.9">""");

        // Root
        AppendUrl(sb, $"{_baseUrl}/", "1.0");

        // Doc pages
        foreach (var entry in entries)
        {
            AppendUrl(sb, $"{_baseUrl}/docs/{entry.Slug}", "0.8");
        }

        // API Explorer
        AppendUrl(sb, $"{_baseUrl}/api", "0.7");

        // Client pages
        foreach (var slug in ClientSlugs)
        {
            AppendUrl(sb, $"{_baseUrl}/api/{slug}", "0.5");
        }

        sb.AppendLine("</urlset>");

        var outputPath = Path.Combine(wwwrootPath, "sitemap.xml");
        await File.WriteAllTextAsync(outputPath, sb.ToString());

        var totalUrls = 1 + entries.Count + 1 + ClientSlugs.Length;
        Console.WriteLine($"  Generated sitemap.xml with {totalUrls} URLs");
    }

    private static void AppendUrl(StringBuilder sb, string loc, string priority)
    {
        sb.AppendLine("  <url>");
        sb.AppendLine($"    <loc>{loc}</loc>");
        sb.AppendLine($"    <priority>{priority}</priority>");
        sb.AppendLine("  </url>");
    }

    private async Task GenerateRobotsTxt(string wwwrootPath)
    {
        var content = $"""
            User-agent: *
            Allow: /
            Sitemap: {_baseUrl}/sitemap.xml
            """;

        var outputPath = Path.Combine(wwwrootPath, "robots.txt");
        await File.WriteAllTextAsync(outputPath, content);
        Console.WriteLine("  Generated robots.txt");
    }
}
