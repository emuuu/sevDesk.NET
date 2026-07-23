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
    public int Total { get; init; }
}
```

- `Items` — The page of results
- `Total` — The total number of matching records (for calculating total pages)

## Paginating Through All Results

```csharp
var allContacts = new List<Contact>();
var pagination = new PaginationParameters { Limit = 100, Offset = 0 };

while (true)
{
    var result = await client.Contacts.ListAsync(pagination);
    allContacts.AddRange(result.Items);

    if (allContacts.Count >= result.Total)
        break;

    pagination.Offset += pagination.Limit;
}
```

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

Invoices support an additional `InvoiceListFilter` for server-side filtering by update timestamp, status, and contact:

```csharp
var recentOpenInvoices = await client.Invoices.ListAsync(filter: new InvoiceListFilter
{
    UpdateAfter = DateTimeOffset.UtcNow.AddDays(-7),
    Status = InvoiceStatus.Open,
    ContactId = 12345678
});
```
