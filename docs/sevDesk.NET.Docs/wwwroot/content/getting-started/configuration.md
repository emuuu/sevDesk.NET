---
title: Configuration
category: Getting Started
order: 3
description: Configure SevDeskOptions for API token, base URL, and more.
---

## SevDeskOptions

The `SevDeskOptions` class controls how sevDesk.NET connects to the API:

```csharp
builder.Services.AddSevDesk(options =>
{
    // Required: your sevDesk API token (32-character hex string)
    options.ApiToken = "your-api-token-here";

    // Optional: override the base URL (default: https://my.sevdesk.de/api/v1)
    options.CustomBaseUrl = "https://my.sevdesk.de/api/v1";
});
```

## Configuration Properties

| Property | Type | Required | Default | Description |
|---|---|---|---|---|
| `ApiToken` | `string` | Yes | — | Your 32-character sevDesk API token |
| `CustomBaseUrl` | `string?` | No | `null` | Custom base URL (must be HTTPS or localhost) |
| `BaseUrl` | `string` | — | `https://my.sevdesk.de/api/v1` | Resolved base URL (uses CustomBaseUrl if set) |

## Validation

`AddSevDesk` validates your configuration at startup:

- `ApiToken` is required and must not be empty
- `CustomBaseUrl`, if set, must use HTTPS (or `http://localhost` for development)

## Using appsettings.json

You can bind options from configuration:

```json
{
  "SevDesk": {
    "ApiToken": "your-api-token-here"
  }
}
```

```csharp
builder.Services.AddSevDesk(options =>
{
    builder.Configuration.GetSection("SevDesk").Bind(options);
});
```

## Environment Variables

For production, use environment variables or a secret manager:

```csharp
builder.Services.AddSevDesk(options =>
{
    options.ApiToken = builder.Configuration["SEVDESK_API_TOKEN"]
        ?? throw new InvalidOperationException("SEVDESK_API_TOKEN not configured");
});
```
