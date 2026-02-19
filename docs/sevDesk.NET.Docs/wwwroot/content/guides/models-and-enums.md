---
title: Models & Enums
category: Guides
order: 3
description: Overview of all model types and enums in sevDesk.NET.
---

## Model Types

sevDesk.NET provides strongly-typed models for all API entities. All model properties use `init` accessors for immutability.

### Core Documents

| Model | Description |
|---|---|
| `Invoice` | Full invoice with contact, dates, amounts, and tax details |
| `InvoicePos` | Invoice line item with quantity, price, and tax |
| `Order` | Offers, order confirmations, and delivery notes |
| `OrderPos` | Order line item |
| `Voucher` | Expense or revenue record |
| `VoucherPos` | Voucher accounting line item |
| `CreditNote` | Credit memo |
| `CreditNotePos` | Credit note line item |

### Supporting Types

| Model | Description |
|---|---|
| `Contact` | Customer, supplier, or partner |
| `ContactAddress` | Postal address for a contact |
| `CommunicationWay` | Email, phone, or other contact method |
| `Part` | Product or service with pricing |
| `CheckAccount` | Bank account (online or offline) |
| `CheckAccountTransaction` | Bank transaction |
| `Tag` | Label for categorizing records |
| `Category` | Category for organizing documents |
| `Document` | Uploaded file |
| `Unity` | Unit of measure |
| `TaxRule` | Tax rate definition |
| `CurrencyExchangeRate` | Exchange rate record |

### Object References

Related entities use `SevDeskObjectReference` instead of full objects:

```csharp
public class SevDeskObjectReference
{
    public int Id { get; init; }
    public string ObjectName { get; init; }
}
```

## Enums

### Document Status Enums

| Enum | Values |
|---|---|
| `InvoiceStatus` | `Draft (100)`, `Open (200)`, `Paid (1000)` |
| `OrderStatus` | `Draft (100)`, `Delivered (200)`, `Rejected (300)`, `Accepted (500)`, `Calculated (1000)` |
| `VoucherStatus` | `Draft (50)`, `Unpaid (100)`, `Paid (1000)` |
| `CreditNoteStatus` | `Draft (100)`, `Open (200)`, `Paid (1000)` |
| `ContactStatus` | `Inactive (0)`, `Active (100)` |

### Type Enums

| Enum | Values |
|---|---|
| `InvoiceType` | `RE`, `WKR`, `SR`, `TR`, `ER`, `AR` |
| `OrderType` | `AN` (Offer), `AB` (Order confirmation), `LI` (Delivery note) |
| `VoucherType` | `VOU` (Purchase), `RV` (Revenue) |
| `CheckAccountType` | `Online (0)`, `Offline (1)` |
| `CommunicationWayType` | `EMAIL`, `PHONE`, `WEB`, `MOBILE` |
| `CommunicationWayKey` | `Work (1)`, `Private (2)`, `Fax (3)`, `Mobile (4)`, `InvoiceEmail (5)`, `Autobox (6)`, `Newsletter (7)`, `Empty (8)` |
