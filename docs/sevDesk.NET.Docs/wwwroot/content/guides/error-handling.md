---
title: Error Handling
category: Guides
order: 2
description: Exception hierarchy and error handling patterns in sevDesk.NET.
---

## Exception Hierarchy

sevDesk.NET uses a typed exception hierarchy for API errors:

```
SevDeskException (base)
  └── SevDeskApiException (API errors)
        ├── SevDeskAuthenticationException (401 Unauthorized)
        ├── SevDeskNotFoundException (404 Not Found)
        └── SevDeskValidationException (422 Unprocessable Entity)
```

## SevDeskException

All exceptions inherit from `SevDeskException`, which provides:

```csharp
public class SevDeskException : Exception
{
    public HttpStatusCode? StatusCode { get; }
    public string? RawResponse { get; }
}
```

## Handling Errors

```csharp
try
{
    var invoice = await client.Invoices.GetAsync(12345);
}
catch (SevDeskNotFoundException)
{
    Console.WriteLine("Invoice not found");
}
catch (SevDeskAuthenticationException)
{
    Console.WriteLine("Invalid API token");
}
catch (SevDeskValidationException ex)
{
    Console.WriteLine($"Validation error: {ex.Message}");
    Console.WriteLine($"Raw response: {ex.RawResponse}");
}
catch (SevDeskApiException ex)
{
    Console.WriteLine($"API error ({ex.StatusCode}): {ex.Message}");
}
catch (SevDeskException ex)
{
    Console.WriteLine($"General error: {ex.Message}");
}
```

## Common Error Scenarios

| Exception | HTTP Status | Common Causes |
|---|---|---|
| `SevDeskAuthenticationException` | 401 | Invalid or expired API token |
| `SevDeskNotFoundException` | 404 | Resource does not exist or was deleted |
| `SevDeskValidationException` | 422 | Invalid data (missing required fields, wrong format) |
| `SevDeskApiException` | Various | Rate limiting, server errors, etc. |
