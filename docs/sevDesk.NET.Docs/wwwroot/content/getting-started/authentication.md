---
title: Authentication
category: Getting Started
order: 4
description: How sevDesk.NET handles API token authentication.
---

## API Token

sevDesk uses a simple API token for authentication. You can find your token in the sevDesk web app under **Settings > Users > API Token**.

The token is a 32-character hexadecimal string that looks like:

```
a1b2c3d4e5f6a7b8c9d0e1f2a3b4c5d6
```

## How It Works

sevDesk.NET uses a `DelegatingHandler` called `SevDeskAuthHandler` to automatically add the API token to every HTTP request:

```csharp
// This happens automatically — you don't need to do this manually
request.Headers.TryAddWithoutValidation("Authorization", apiToken);
```

The handler is registered automatically when you call `AddSevDesk()`.

## Security Best Practices

1. **Never commit tokens to source control** — Use environment variables or secret managers
2. **Use different tokens for development and production** — Limit the blast radius of a leaked token
3. **Rotate tokens regularly** — Generate a new token in the sevDesk settings if you suspect a leak

```csharp
// Good: read from environment
builder.Services.AddSevDesk(options =>
{
    options.ApiToken = Environment.GetEnvironmentVariable("SEVDESK_API_TOKEN")!;
});

// Bad: hardcoded token
builder.Services.AddSevDesk(options =>
{
    options.ApiToken = "a1b2c3d4e5f6a7b8c9d0e1f2a3b4c5d6"; // Don't do this!
});
```

## Token Permissions

The sevDesk API token grants full access to your sevDesk account. There are no scoped permissions — the token can read and write all data.
