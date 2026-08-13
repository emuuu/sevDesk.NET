---
title: Pagination
category: Guides
order: 1
description: How to paginate through large result sets with PaginationParameters.
---

## PaginationParameters

All `ListAsync` methods accept an optional `PaginationParameters` object:

```csharp
var result = await client.Contacts.ListAsync(new PaginationParameters
{
    Limit = 50,   // Number of items per page (1-2000, default: 100)
    Offset = 0    // Number of items to skip
});
```

## SevDeskListResponse

List methods return a `SevDeskListResponse<T>` containing the items and total count:

```csharp
public class SevDeskListResponse<T>
{
    public IReadOnlyList<T> Items { get; init; }
    public int? Total { get; init; }
}
```

- `Items` — The page of results
- `Total` — The total number of matching records (for calculating total pages), or `null` when the
  API did not report one

### `Total` is nullable, and `null` is not `0`

Every `ListAsync` requests `countAll=true`, which is what makes the API send `total` at all — but
it does not send it reliably on every response. The two cases are deliberately distinct:

| `Total` | Meaning |
|---|---|
| `null` | The server reported no total. The size of the result set is unknown. |
| `0` | The server reported an empty result set. |

Collapsing the two — treating a missing total as `0` — makes a full page of results look like the
end of the data.

## Paginating Through All Results

Drive the loop off the page size and use `Total` only as an early exit when the server supplied
one:

```csharp
var allContacts = new List<Contact>();
var pagination = new PaginationParameters { Limit = 100, Offset = 0 };

while (true)
{
    var result = await client.Contacts.ListAsync(pagination);
    allContacts.AddRange(result.Items);

    // A short page is the end of the data, with or without a total.
    if (result.Items.Count < pagination.Limit)
        break;

    // A reported total lets us stop one request earlier.
    if (result.Total is int total && allContacts.Count >= total)
        break;

    pagination.Offset += pagination.Limit;
}
```

Checking `allContacts.Count >= result.Total` on its own is not enough: with `Total` null the
comparison is always `false`, so the loop would only ever end on the short-page check — and
substituting `0` for a missing total would end it after the first page.

## Filtering

Some clients support filtering in their `ListAsync` methods:

```csharp
// Invoice positions for a specific invoice
var positions = await client.InvoicePositions.ListAsync(invoiceId: 12345);

// Communication ways for a specific contact
var comms = await client.CommunicationWays.ListAsync(contactId: 67890);

// Categories for a specific object type
var categories = await client.Categories.ListAsync(objectType: "Invoice");
```

Invoices support an additional `InvoiceListFilter` for server-side filtering by update timestamp, status, contact, and invoice date:

```csharp
var recentOpenInvoices = await client.Invoices.ListAsync(filter: new InvoiceListFilter
{
    UpdateAfter = DateTimeOffset.UtcNow.AddDays(-7),
    Status = InvoiceStatus.Open,
    ContactId = 12345678
});
```

`InvoiceDateFrom` and `InvoiceDateTo` filter on the invoice date rather than the update
timestamp. They make a historical import resumable: walk backwards one date window at a
time and remember the window you finished.

```csharp
var q1 = await client.Invoices.ListAsync(filter: new InvoiceListFilter
{
    InvoiceDateFrom = new DateTimeOffset(2026, 1, 1, 0, 0, 0, TimeSpan.Zero),
    InvoiceDateTo = new DateTimeOffset(2026, 3, 31, 23, 59, 59, TimeSpan.Zero)
});
```

## Embedding related objects

`ListAsync` and `GetAsync` accept an `embed` argument that pulls related objects into the
same response. For invoices this turns a full import from one request per invoice into a
single paginated pass:

```csharp
var invoices = await client.Invoices.ListAsync(
    new PaginationParameters { Limit = 2000 },
    embed: "positions");

foreach (var invoice in invoices.Items)
{
    // Positions is null unless embed: "positions" was requested
    foreach (var position in invoice.Positions ?? [])
    {
        Console.WriteLine($"{position.Name}: {position.Quantity} x {position.PriceNet}");
    }
}
```
