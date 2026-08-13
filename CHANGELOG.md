# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/).

## [2.2.0] - 2026-08-13

### Fixed

- `GetAsync(id)` on every client threw a `JsonException` against the live API. `GET /{Entity}/{id}` answers with `{"objects": [ … ]}` — a single-element array — while the create and update endpoints answer with a bare `{"objects": { … }}`. Only the bare object was accepted. Both shapes are now read; an empty array or a null `objects` raises `SevDeskNotFoundException`, a missing `objects` raises `SevDeskApiException`.
- `AccountingContact.Contact`: the `contact` reference the API returns was dropped during mapping, which made a debitor number unattributable to a contact — the only reason to read the endpoint. Resolving it no longer requires one filtered `ListAsync` call per contact. This also corrects the 2.1.0 note claiming the endpoint returns no contact reference.

### Added

- `Invoice.DeliveryDateUntil`: the end of the service period. The API has always returned `deliveryDateUntil`, but the model exposed only `DeliveryDate`, so a period silently collapsed into a single date. Mapped in both directions.
- `Invoice.EmbeddedContact`: the full contact returned for `embed=contact` was discarded because only the `{id, objectName}` reference was mapped. `Contact` keeps carrying the reference; `EmbeddedContact` carries the expanded object and stays `null` when the embed was not requested. Only the reference is ever written back on create and update.

## [2.1.0] - 2026-08-13

### Added

- `Invoice`: the structured recipient address next to the existing rendered `Address` block — `AddressName`, `AddressName2`, `AddressStreet`, `AddressZip`, `AddressCity`, `AddressCountry` (a `StaticCountry` reference), `AddressParentName`, `AddressParentName2` and `AddressGender`. Previously an invoice import could only recover the recipient from the multi-line text block. Mapped in both directions.
- `Invoice.PaidAmount` and `Invoice.PayDate` (read-only, calculated by the API).
- `Invoice.Positions`: line items requested via `embed=positions` are now mapped instead of being silently dropped, so a full import no longer needs one extra request per invoice. The property stays `null` when the embed was not requested.
- `InvoicePos.PriceNet`, `InvoicePos.PriceGross` and `InvoicePos.PriceTax` (read-only, calculated by the API). `PriceNet` is the authoritative net unit price of a position.
- `InvoiceListFilter.InvoiceDateFrom` and `InvoiceListFilter.InvoiceDateTo`: server-side `startDate` / `endDate` filters on `invoiceDate`, sent as Unix seconds. Makes a resumable historical import that walks backwards in date windows possible.
- `IAccountingContactClient` via `client.AccountingContacts`: read access to `/AccountingContact`, the source of the DATEV debitor and creditor numbers. The endpoint returns no contact reference, only `ContactName`, so resolving an entry to a specific contact goes through the `contactId` argument on `ListAsync`. `DebitorNumber` and `CreditorNumber` are typed as `string` because the API returns them as strings and bookkeeping numbers may carry leading zeros.
- `IStaticCountryClient` via `client.StaticCountries`: read access to `/StaticCountry`, the catalogue the `country` and `addressCountry` references resolve against.

### Notes

- `ISevDeskClient` gained the `AccountingContacts` and `StaticCountries` properties. Custom implementations of that interface must add them; callers using `SevDeskClient` or a mocking framework are unaffected.

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
