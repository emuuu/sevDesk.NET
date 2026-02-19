---
title: Quick Start
category: Getting Started
order: 2
description: Register services and make your first sevDesk API call.
---

## Register Services

In your `Program.cs` or startup configuration, register sevDesk.NET with dependency injection:

```csharp
builder.Services.AddSevDesk(options =>
{
    options.ApiToken = "YOUR_API_TOKEN";
});
```

This registers `ISevDeskClient` and all sub-clients with the DI container, including the HTTP client with authentication.

## Inject and Use

Inject `ISevDeskClient` anywhere in your application:

```csharp
public class MyService
{
    private readonly ISevDeskClient _client;

    public MyService(ISevDeskClient client)
    {
        _client = client;
    }

    public async Task ListContactsAsync()
    {
        var result = await _client.Contacts.ListAsync();
        Console.WriteLine($"Found {result.Total} contacts");

        foreach (var contact in result.Items)
        {
            Console.WriteLine($"  {contact.Surename} {contact.Familyname}");
        }
    }
}
```

## Available Clients

`ISevDeskClient` provides access to 20 typed sub-clients:

| Property | Interface | Description |
|---|---|---|
| `Contacts` | `IContactClient` | Manage customers, suppliers, and partners |
| `Invoices` | `IInvoiceClient` | Create and manage invoices |
| `Orders` | `IOrderClient` | Offers, order confirmations, delivery notes |
| `Vouchers` | `IVoucherClient` | Record expenses and revenues |
| `CreditNotes` | `ICreditNoteClient` | Credit memos |
| `Parts` | `IPartClient` | Products and services |
| `CheckAccounts` | `ICheckAccountClient` | Bank accounts |
| `Tags` | `ITagClient` | Tagging and categorization |
| `Documents` | `IDocumentClient` | File upload and download |
| ... | ... | And 11 more |

See the [API Explorer](/api) for the full list with interactive documentation.
