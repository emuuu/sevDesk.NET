# sevDeskNET

A .NET client library for the [sevDesk API](https://my.sevdesk.de/api/v1/).

## Installation

```bash
dotnet add package sevDeskNET
```

## Quick Start

```csharp
using Microsoft.Extensions.DependencyInjection;
using sevDeskNET;

var services = new ServiceCollection();

services.AddSevDesk(options =>
{
    options.ApiToken = "your-api-token";
});

await using var provider = services.BuildServiceProvider();
var client = provider.GetRequiredService<ISevDeskClient>();

// List contacts
var contacts = await client.Contacts.ListAsync();
foreach (var contact in contacts.Items)
{
    Console.WriteLine($"{contact.Id}: {contact.Surename} {contact.Familyname}");
}

// Create an invoice
var invoice = await client.Invoices.SaveInvoiceAsync(
    new Invoice { /* ... */ },
    new[] { new InvoicePos { /* ... */ } });
```

## Features

- Full coverage of the sevDesk REST API
- Strongly typed models and enums
- AOT-compatible via JSON source generation
- Proper exception hierarchy with typed exceptions
- Pagination support
- Dependency injection via `IServiceCollection`
- `IHttpClientFactory` integration

## Supported Resources

| Resource | Client Property |
|---|---|
| Contacts | `client.Contacts` |
| Invoices | `client.Invoices` |
| Invoice Positions | `client.InvoicePositions` |
| Orders | `client.Orders` |
| Order Positions | `client.OrderPositions` |
| Vouchers | `client.Vouchers` |
| Voucher Positions | `client.VoucherPositions` |
| Credit Notes | `client.CreditNotes` |
| Credit Note Positions | `client.CreditNotePositions` |
| Parts | `client.Parts` |
| Check Accounts | `client.CheckAccounts` |
| Check Account Transactions | `client.CheckAccountTransactions` |
| Communication Ways | `client.CommunicationWays` |
| Contact Addresses | `client.ContactAddresses` |
| Tags | `client.Tags` |
| Categories | `client.Categories` |
| Unities | `client.Unities` |
| Tax Rules | `client.TaxRules` |
| Currency Exchange Rates | `client.CurrencyExchangeRates` |
| Documents | `client.Documents` |

## License

MIT
