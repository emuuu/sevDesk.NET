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
        ├── SevDeskValidationException (422 Unprocessable Entity)
        └── SevDeskWriteSucceededException (written, follow-up failed)
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

## Failures After a Successful Write

`SaveInvoiceAsync`, `SaveCreditNoteAsync`, `CreateFromInvoiceAsync`, `SaveOrderAsync` and
`SaveVoucherAsync` do two things: they post the document to a `Factory` endpoint, and then read the
created document back by its identifier. Only the post writes. Once sevDesk has answered it with a
success status, **the document exists and sending the call again would create a second one** — a
duplicate invoice in a live client, not a retry.

If anything fails after that point, `SevDeskWriteSucceededException` says so:

```csharp
try
{
    var invoice = await client.Invoices.SaveInvoiceAsync(invoice, positions);
}
catch (SevDeskWriteSucceededException ex)
{
    // The invoice EXISTS in sevDesk. Do not send it again.
    if (ex.IsObjectIdKnown)
    {
        // Everything failed after the id was known — pick up from there.
        MarkAsSent(ex.ObjectId!.Value);
    }
    else
    {
        // Worst case: written, id unknown. Look it up before writing anything else.
        // ex.RawResponse holds the raw factory answer, if one arrived at all.
        await ReconcileByInvoiceNumberAsync(invoice.InvoiceNumber);
    }
}
catch (SevDeskApiException)
{
    // Nothing was written. Retrying is safe.
    await RetryLaterAsync();
}
```

The distinction is the point: without it, a timeout on the read-back is indistinguishable from a
timeout on the write itself, and a caller can only guess between losing a document and booking it
twice.

| Property | Meaning |
|---|---|
| `ObjectName` | The type that was written, e.g. `"Invoice"` |
| `ObjectId` | The identifier, or `null` when it could not be determined |
| `IsObjectIdKnown` | `false` means: written, id unknown — look it up, never rewrite |
| `RawResponse` | The raw body of the **write** response, or `null` if it never arrived |
| `InnerException` | The failure that followed the write |

A cancelled read-back is reported the same way, with an `OperationCanceledException` as the
`InnerException`. Cancellation before or during the write itself propagates unwrapped, because
nothing was written in that case.

### Avoiding the Case Entirely

Every one of those methods has a counterpart that stops after the write and returns just the
reference — one request instead of two, and no read-back that could fail:

```csharp
SevDeskObjectReference reference = await client.Invoices.SaveInvoiceReferenceAsync(invoice, positions);
Console.WriteLine(reference.Id);

// Fetch the full object only if and when it is actually needed:
var full = await client.Invoices.GetAsync(reference.Id);
```

| Read-back variant | Reference-only variant |
|---|---|
| `Invoices.SaveInvoiceAsync` | `Invoices.SaveInvoiceReferenceAsync` |
| `CreditNotes.SaveCreditNoteAsync` | `CreditNotes.SaveCreditNoteReferenceAsync` |
| `CreditNotes.CreateFromInvoiceAsync` | `CreditNotes.CreateFromInvoiceReferenceAsync` |
| `Orders.SaveOrderAsync` | `Orders.SaveOrderReferenceAsync` |
| `Vouchers.SaveVoucherAsync` | `Vouchers.SaveVoucherReferenceAsync` |

These can still raise `SevDeskWriteSucceededException`, but only in the one remaining case: the write
succeeded and its answer could not be read or parsed, so `ObjectId` is `null`.

## Common Error Scenarios

| Exception | HTTP Status | Common Causes |
|---|---|---|
| `SevDeskAuthenticationException` | 401 | Invalid or expired API token |
| `SevDeskNotFoundException` | 404 | Resource does not exist or was deleted |
| `SevDeskValidationException` | 422 | Invalid data (missing required fields, wrong format) |
| `SevDeskWriteSucceededException` | Various | A `Save…Async` write succeeded but its follow-up failed |
| `SevDeskApiException` | Various | Rate limiting, server errors, etc. |
