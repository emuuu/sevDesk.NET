# sevDesk.NET

A strongly-typed .NET client library for the [sevDesk API](https://my.sevdesk.de/api/v1/). Manage invoices, contacts, vouchers, orders, credit notes, and more — with full async support and dependency injection.

## Quick Start

```bash
dotnet add package sevDesk.NET
```

Register the client in your DI container:

```csharp
builder.Services.AddSevDesk(options =>
{
    options.ApiToken = "your-api-token";
});
```

Or bind from configuration:

```csharp
builder.Services.AddSevDesk(
    builder.Configuration.GetSection("SevDesk"));
```

Inject and use:

```csharp
public class InvoiceService(ISevDeskClient client)
{
    public async Task ListRecentInvoicesAsync()
    {
        var result = await client.Invoices.ListAsync();
        foreach (var invoice in result.Items)
            Console.WriteLine($"{invoice.InvoiceNumber}: {invoice.SumGross} {invoice.Currency}");
    }
}
```

## Features

- **20 typed clients** covering the entire sevDesk REST API
- **Strongly typed models and enums** for all resources
- **Transaction operations** — save invoice/order/voucher with positions atomically
- **PDF generation**, email sending, status management, and document upload
- **Pagination** with `SevDeskListResponse<T>` and filtering — `Total` is `int?`, so "the server reported no total" stays distinct from "the result set is empty"
- **Typed exception hierarchy** — `SevDeskAuthenticationException`, `SevDeskNotFoundException`, `SevDeskValidationException`, and `SevDeskWriteSucceededException` so "written, follow-up failed" never looks like "not written"
- **`IHttpClientFactory`** integration with automatic auth header injection

## Available Clients

| Area | Clients |
|---|---|
| Financial Documents | `Invoices`, `InvoicePositions`, `Orders`, `OrderPositions`, `Vouchers`, `VoucherPositions`, `CreditNotes`, `CreditNotePositions` |
| Contacts | `Contacts`, `ContactAddresses`, `CommunicationWays`, `AccountingContacts` |
| Banking | `CheckAccounts`, `CheckAccountTransactions` |
| Products | `Parts` |
| Organization | `Tags`, `Categories`, `Documents` |
| Reference Data | `Unities`, `TaxRules`, `CurrencyExchangeRates`, `StaticCountries` |

All clients are accessible via `ISevDeskClient` (e.g. `client.Invoices`, `client.Contacts`).

## Error Handling

```csharp
try
{
    var invoice = await client.Invoices.GetAsync(id);
}
catch (SevDeskNotFoundException) { /* 404 */ }
catch (SevDeskAuthenticationException) { /* 401 */ }
catch (SevDeskValidationException ex) { /* 422 */ }
catch (SevDeskApiException ex) { /* other API errors */ }
```

The `Save…Async` methods post the document and then read it back. If the read-back fails, the
document already exists and repeating the call would create a duplicate — so that outcome is raised
as `SevDeskWriteSucceededException`, carrying the id of the document that was written:

```csharp
try
{
    var invoice = await client.Invoices.SaveInvoiceAsync(invoice, positions);
}
catch (SevDeskWriteSucceededException ex) { /* written — do not send it again */ }
catch (SevDeskApiException) { /* not written — retrying is safe */ }
```

Use `SaveInvoiceReferenceAsync` and its siblings to skip the read-back and get just the identifier.

## Documentation

For full documentation, examples, and API reference visit the [project site](https://emuuu.github.io/sevDesk.NET/).

Source code and issue tracker: [GitHub](https://github.com/emuuu/sevDesk.NET)
