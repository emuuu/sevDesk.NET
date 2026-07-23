using System.Text;

namespace sevDesk.NET.Internal;

internal sealed class QueryBuilder
{
    private readonly List<KeyValuePair<string, string>> _parameters = [];

    internal QueryBuilder Add(string key, string value)
    {
        _parameters.Add(new KeyValuePair<string, string>(key, value));
        return this;
    }

    internal QueryBuilder AddIfNotNull(string key, string? value)
    {
        if (value is not null)
        {
            _parameters.Add(new KeyValuePair<string, string>(key, value));
        }

        return this;
    }

    internal QueryBuilder AddIfNotNull(string key, int? value)
    {
        if (value.HasValue)
        {
            _parameters.Add(new KeyValuePair<string, string>(key, value.Value.ToString()));
        }

        return this;
    }

    internal QueryBuilder AddPagination(PaginationParameters? pagination)
    {
        var p = pagination ?? new PaginationParameters();
        var limit = Math.Clamp(p.Limit, 1, 2000);
        var offset = Math.Max(p.Offset, 0);
        Add("limit", limit.ToString());
        Add("offset", offset.ToString());
        Add("countAll", "true");
        return this;
    }

    internal string Build(string basePath)
    {
        if (_parameters.Count == 0)
        {
            return basePath;
        }

        var sb = new StringBuilder(basePath);
        sb.Append('?');

        for (var i = 0; i < _parameters.Count; i++)
        {
            if (i > 0)
            {
                sb.Append('&');
            }

            sb.Append(Uri.EscapeDataString(_parameters[i].Key));
            sb.Append('=');
            sb.Append(Uri.EscapeDataString(_parameters[i].Value));
        }

        return sb.ToString();
    }
}
