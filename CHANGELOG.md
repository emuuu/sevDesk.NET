# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/).

## [2.0.0] - 2026-07-23

### Changed

- **Breaking:** `Invoice.PaymentMethod` is now a `SevDeskObjectReference` instead of a `string`, matching the actual API response shape (`{ "id": ..., "objectName": "PaymentMethod" }`).
- **Breaking:** `IInvoiceClient.ListAsync` gained a new `InvoiceListFilter? filter = null` parameter, inserted *before* the trailing `CancellationToken`: `ListAsync(PaginationParameters? pagination = null, string? embed = null, InvoiceListFilter? filter = null, CancellationToken ct = default)`. This changes the interface method's signature and is binary-incompatible with 1.x builds, and it breaks source compatibility for callers using positional arguments (e.g. `ListAsync(pagination, embed, ct)` no longer compiles because `ct` now binds to the `filter` parameter). **Migration:** recompile against 2.0.0; switch positional calls to named arguments (`ListAsync(pagination, embed, ct: ct)`) or insert `filter: null` in the third position; custom `IInvoiceClient` implementations must add the new parameter to match the interface.

### Fixed

- `Invoice.SumNet`, `Invoice.SumGross`, and `Invoice.SumTax` now deserialize correctly regardless of whether the API returns them as a JSON string (e.g. `"84.02"`) or a JSON number (e.g. `84.02`), which varies by endpoint.

### Added

- `Invoice.TaxRule` reference field.
- `Invoice.EinvoiceReference` and `Invoice.PropertyIsEInvoice` fields for e-invoicing support.
- `Contact.BuyerReference` field for e-invoicing support.
- `InvoiceListFilter` with `UpdateAfter`, `Status`, and `ContactId` filters, usable via a new optional parameter on `IInvoiceClient.ListAsync`.
- Raised the pagination `Limit` upper bound from 1000 to 2000.

## [1.0.0] - 2026-02-20

### Changed

- Promote to stable 1.0.0 release

## [0.1.0-alpha] - 2026-02-18

### Added

- Initial release with full sevDesk API coverage
- Contact management (CRUD, customer number generation)
- Invoice management (CRUD, save with positions, status changes, PDF, email, duplicate, cancel, book amounts)
- Order management (CRUD, save with positions, status changes, PDF, email, duplicate)
- Voucher management (CRUD, save with positions, book amounts, mark as paid/open, file upload)
- Credit note management (CRUD, save with positions, create from invoice, PDF, email)
- Part management (CRUD)
- Check account management (CRUD, balance queries)
- Check account transaction management (CRUD)
- Communication way management (CRUD)
- Contact address management (CRUD)
- Tag management (create, list, get, delete)
- Category management (CRUD)
- Unity / unit of measure queries
- Tax rule queries
- Currency exchange rate queries
- Document management (list, get, upload, download)
- AOT-compatible JSON source generation
- Typed exception hierarchy (SevDeskException, SevDeskApiException, SevDeskNotFoundException, SevDeskAuthenticationException, SevDeskValidationException)
- IHttpClientFactory integration with DI
- Pagination support
