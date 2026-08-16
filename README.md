<p align="center">
  <img src="https://raw.githubusercontent.com/emuuu/sevDesk.NET/main/icon.png" alt="sevDesk.NET" width="128" />
</p>

<h1 align="center">sevDesk.NET</h1>

A strongly-typed .NET client library for the [sevDesk API](https://my.sevdesk.de/api/v1/). Manage invoices, contacts, vouchers, orders, credit notes, and more — with full async support and dependency injection.

[![NuGet](https://img.shields.io/nuget/v/sevDesk.NET.svg)](https://www.nuget.org/packages/sevDesk.NET)
[![NuGet Downloads](https://img.shields.io/nuget/dt/sevDesk.NET.svg)](https://www.nuget.org/packages/sevDesk.NET)
[![CI](https://github.com/emuuu/sevDesk.NET/actions/workflows/ci.yml/badge.svg)](https://github.com/emuuu/sevDesk.NET/actions/workflows/ci.yml)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![Docs](https://img.shields.io/badge/Docs-GitHub%20Pages-blue)](https://emuuu.github.io/sevDesk.NET/)

**Feature highlights:**

- 20 typed clients covering the entire sevDesk REST API
- Strongly typed models and enums for all resources
- Transaction operations: save invoice/order/voucher with positions atomically
- PDF generation, email sending, status management, and document upload
- Pagination with `SevDeskListResponse<T>` and filtering
- Proper exception hierarchy (`SevDeskAuthenticationException`, `SevDeskNotFoundException`, `SevDeskValidationException`)
- `IHttpClientFactory` integration with automatic auth header injection
- Dependency injection via `IServiceCollection.AddSevDesk()`

## Prerequisites

- A [sevDesk](https://sevdesk.de/) account and API token
- .NET 10.0

## Installation

```bash
dotnet add package sevDesk.NET
```

## Getting Started

### 1. Register services

In `Program.cs`, register sevDesk.NET with your API token:

```csharp
builder.Services.AddSevDesk(options =>
{
    options.ApiToken = "your-api-token";
});
```

For custom base URLs (e.g. self-hosted or proxy):

```csharp
builder.Services.AddSevDesk(options =>
{
    options.ApiToken = "your-api-token";
    options.CustomBaseUrl = "https://my-proxy.example.com/api/v1";
});
```

### 2. Inject and use the client

```csharp
public class InvoiceService
{
    private readonly ISevDeskClient _client;

    public InvoiceService(ISevDeskClient client)
    {
        _client = client;
    }

    public async Task ListRecentInvoicesAsync()
    {
        var result = await _client.Invoices.ListAsync();
        foreach (var invoice in result.Items)
        {
            Console.WriteLine($"{invoice.InvoiceNumber}: {invoice.SumGross} {invoice.Currency}");
        }
    }
}
```

### 3. Configuration via appsettings.json

```json
{
  "SevDesk": {
    "ApiToken": "your-api-token"
  }
}
```

```csharp
builder.Services.AddSevDesk(
    builder.Configuration.GetSection("SevDesk"));
```

## Available Clients

### Financial Documents

| Client | Property | Operations |
|---|---|---|
| Invoices | `client.Invoices` | CRUD, SaveInvoice, ChangeStatus, GetPdf, SendViaEmail, Duplicate, Cancel, MarkAsSent, BookAmount |
| Invoice Positions | `client.InvoicePositions` | CRUD, filter by invoice |
| Orders | `client.Orders` | CRUD, SaveOrder, ChangeStatus, GetPdf, SendViaEmail, Duplicate |
| Order Positions | `client.OrderPositions` | CRUD, filter by order |
| Vouchers | `client.Vouchers` | CRUD, SaveVoucher, BookAmount, MarkAsPaid, MarkAsOpen, UploadFile |
| Voucher Positions | `client.VoucherPositions` | CRUD, filter by voucher |
| Credit Notes | `client.CreditNotes` | CRUD, SaveCreditNote, CreateFromInvoice, GetPdf, SendViaEmail |
| Credit Note Positions | `client.CreditNotePositions` | CRUD, filter by credit note |

### Contacts

| Client | Property | Operations |
|---|---|---|
| Contacts | `client.Contacts` | CRUD, GetNextCustomerNumber |
| Contact Addresses | `client.ContactAddresses` | CRUD, filter by contact |
| Communication Ways | `client.CommunicationWays` | CRUD, filter by contact |
| Accounting Contacts | `client.AccountingContacts` | List, Get, filter by contact |

### Banking

| Client | Property | Operations |
|---|---|---|
| Check Accounts | `client.CheckAccounts` | CRUD, GetBalance |
| Check Account Transactions | `client.CheckAccountTransactions` | CRUD, filter by account |

### Products

| Client | Property | Operations |
|---|---|---|
| Parts | `client.Parts` | CRUD |

### Organization

| Client | Property | Operations |
|---|---|---|
| Tags | `client.Tags` | Create, List, Get, Delete |
| Categories | `client.Categories` | CRUD, filter by object type |
| Documents | `client.Documents` | List, Get, Upload, Download |

### Reference Data

| Client | Property | Operations |
|---|---|---|
| Unities | `client.Unities` | List, Get |
| Tax Rules | `client.TaxRules` | List, Get |
| Currency Exchange Rates | `client.CurrencyExchangeRates` | List, Get |
| Static Countries | `client.StaticCountries` | List, Get |

## Key Operations

### Create an invoice with positions

```csharp
var invoice = await client.Invoices.SaveInvoiceAsync(
    new Invoice
    {
        Contact = new SevDeskObjectReference { Id = 123, ObjectName = "Contact" },
        InvoiceDate = DateTime.Today,
        Header = "Invoice 2024-001",
        TimeToPay = 14
    },
    new[]
    {
        new InvoicePos
        {
            Name = "Consulting",
            Quantity = 10,
            Price = 150.00m,
            Unity = new SevDeskObjectReference { Id = 1, ObjectName = "Unity" },
            TaxRate = 19
        }
    });
```

### Get invoice PDF

```csharp
byte[] pdf = await client.Invoices.GetPdfAsync(invoiceId);
File.WriteAllBytes("invoice.pdf", pdf);
```

### Send invoice via email

```csharp
await client.Invoices.SendViaEmailAsync(
    invoiceId,
    email: "customer@example.com",
    subject: "Your Invoice",
    text: "Please find your invoice attached.");
```

### Pagination

```csharp
var page = await client.Contacts.ListAsync(new PaginationParameters
{
    Limit = 50,
    Offset = 100
});

Console.WriteLine(page.Total is int total
    ? $"Showing {page.Items.Count} of {total} contacts"
    : $"Showing {page.Items.Count} contacts (server reported no total)");
```

`SevDeskListResponse<T>.Total` is `int?`. The API sends `total` only for `countAll=true` — which
every `ListAsync` requests — and not reliably even then, so `null` (no total reported, result set
size unknown) and `0` (an empty result set) are distinct. To page through everything, loop while a
page comes back full and use `Total` only as an early exit.

### Upload a voucher with file

```csharp
await using var stream = File.OpenRead("receipt.pdf");
var document = await client.Vouchers.UploadFileAsync(stream, "receipt.pdf");
```

### Check account balance

```csharp
decimal balance = await client.CheckAccounts.GetBalanceAsync(
    accountId,
    date: DateTime.Today);
```

## Error Handling

sevDesk.NET uses a typed exception hierarchy:

```csharp
try
{
    var invoice = await client.Invoices.GetAsync(id);
}
catch (SevDeskNotFoundException)
{
    // 404 — invoice not found
}
catch (SevDeskAuthenticationException)
{
    // 401 — invalid API token
}
catch (SevDeskValidationException ex)
{
    // 422 — validation error
    Console.WriteLine(ex.RawResponse);
}
catch (SevDeskApiException ex)
{
    // Other API errors
    Console.WriteLine($"{ex.StatusCode}: {ex.Message}");
}
```

| Exception | HTTP Status | When |
|---|---|---|
| `SevDeskAuthenticationException` | 401 | Invalid or missing API token |
| `SevDeskNotFoundException` | 404 | Resource not found |
| `SevDeskValidationException` | 422 | Invalid request data |
| `SevDeskWriteSucceededException` | Various | A `Save…Async` write succeeded but its follow-up failed |
| `SevDeskApiException` | Various | Other API errors |
| `SevDeskException` | — | Base exception (network errors, etc.) |

### Failures After a Successful Write

`SaveInvoiceAsync`, `SaveCreditNoteAsync`, `CreateFromInvoiceAsync`, `SaveOrderAsync` and
`SaveVoucherAsync` post the document and then read it back. Only the post writes. If the read-back
fails, the document already exists and repeating the call would create a duplicate.
`SevDeskWriteSucceededException` makes that outcome distinguishable from a write that never arrived:

```csharp
try
{
    var invoice = await client.Invoices.SaveInvoiceAsync(invoice, positions);
}
catch (SevDeskWriteSucceededException ex)
{
    // Written. Do not send it again.
    // ex.ObjectId is null when even the id could not be read — look the document up.
}
catch (SevDeskApiException)
{
    // Not written. Retrying is safe.
}
```

Each of those methods has a `…ReferenceAsync` counterpart that skips the read-back and returns just
the identifier — one request instead of two:

```csharp
var reference = await client.Invoices.SaveInvoiceReferenceAsync(invoice, positions);
Console.WriteLine(reference.Id);
```

See the [error handling guide](https://sevDesk-NET.github.io/sevDesk.NET/docs/guides/error-handling/)
for the full picture.

## Configuration

### SevDeskOptions

| Property | Type | Default | Description |
|---|---|---|---|
| `ApiToken` | `string` | *(required)* | 32-character API token from sevDesk |
| `CustomBaseUrl` | `string?` | `null` | Override the default API base URL |
| `BaseUrl` | `string` | `https://my.sevdesk.de/api/v1` | Computed base URL (uses CustomBaseUrl if set) |

### Validation

`AddSevDesk()` validates options on registration:
- `ApiToken` must not be empty
- `CustomBaseUrl` (if set) must use HTTPS or be localhost

## License

MIT
