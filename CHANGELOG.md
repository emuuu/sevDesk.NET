# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/).

## [3.1.0] - 2026-08-16

### Added

- `SevDeskWriteSucceededException`: raised when a write to sevDesk **succeeded** but a later step of the same call failed. The `Save…Async` methods are two requests, not one — they post the document to a `Factory` endpoint and then read it back by its identifier. Only the post writes. Until now every phase of that sequence threw the same exception types, so a caller could not tell "the invoice was never created" from "the invoice was created and reading it back timed out" — and had to choose between losing a document and booking a duplicate one into a live client. The new type is thrown from the moment the API confirms the write, and only then:

  ```csharp
  try
  {
      var invoice = await client.Invoices.SaveInvoiceAsync(invoice, positions);
  }
  catch (SevDeskWriteSucceededException ex)
  {
      // The invoice EXISTS. Do not send it again.
      // ex.ObjectId — the id, or null when even that could not be read
      // ex.IsObjectIdKnown — false means: written, id unknown; look it up, never rewrite
      // ex.ObjectName — "Invoice", "CreditNote", "Order", "Voucher"
      // ex.RawResponse — the raw body of the write response, or null if none arrived
      // ex.InnerException — the failure that followed the write
  }
  catch (SevDeskApiException)
  {
      // Nothing was written. Retrying is safe.
  }
  ```

  `ObjectId` is `null` in the worst case of all: the write went through and its answer could not be read or parsed, so the document exists under an identifier nobody knows. That state is deliberately not conflatable with "not written".

- `SaveInvoiceReferenceAsync`, `SaveCreditNoteReferenceAsync`, `CreateFromInvoiceReferenceAsync`, `SaveOrderReferenceAsync` and `SaveVoucherReferenceAsync`: the same writes without the read-back. They return a `SevDeskObjectReference` carrying the new identifier, in one request instead of two. Callers that only need to know whether and under which id something was created avoid the failing follow-up altogether. The full object stays one `GetAsync(id)` away.

### Changed

- The write to a `Factory` endpoint is now sent with `HttpCompletionOption.ResponseHeadersRead`. Under the default option the handler buffers the entire response body before returning, so a connection dropping mid-body surfaced as a transport error *before* the status code was ever inspected — the created document was then indistinguishable from one that never got written. The status code is now evaluated first, which is what makes that case reportable at all.

- All five factory call sites (`Invoice`, `CreditNote` save and create-from-invoice, `Order`, `Voucher`) share one implementation, so the guarantee holds identically for every document type rather than for invoices alone.

### Notes

- **Backwards compatible for callers.** No signature changed. `SevDeskWriteSucceededException` derives from `SevDeskApiException`, which derives from `SevDeskException`, so an existing `catch (SevDeskApiException)` or `catch (SevDeskException)` around a `Save…Async` call keeps catching exactly what it caught in 3.0.0. Covered by tests.

- **Behaviour change in the failure path.** A failure *after* a confirmed write now arrives as `SevDeskWriteSucceededException` rather than as the underlying exception. Code that specifically caught `SevDeskNotFoundException`, `HttpRequestException` or `OperationCanceledException` around a `Save…Async` call no longer matches on those — the original exception is preserved as `InnerException`. This is the correction: the previous types asserted something the library could not know. A `catch (SevDeskWriteSucceededException)` clause must be placed before any `catch (SevDeskApiException)` clause, which the compiler enforces.

- `ISevDeskClient`'s document client interfaces gained the `…ReferenceAsync` methods. Custom implementations of `IInvoiceClient`, `ICreditNoteClient`, `IOrderClient` or `IVoucherClient` must add them; callers using `SevDeskClient` or a mocking framework are unaffected.

## [3.0.0] - 2026-08-13

### Changed

- **Breaking:** `SevDeskListResponse<T>.Total` is now `int?` instead of `int`. The API sends `total` only for `countAll=true` — which every `ListAsync` requests — and not reliably even then; the recorded `GET /Contact`, `/Category`, `/TaxRule` and `/Unity` responses carry no `total` at all. A missing field silently became the `int` default `0`, indistinguishable from a genuinely empty result set, which is exactly backwards: a caller paginating to completion reads `0` and stops after the first full page. `null` now means the server reported no total and the size of the result set is unknown, while `0` keeps meaning the server reported an empty result set. Both wire forms are still read, the JSON string the API actually sends (`"total": "42"`) and a JSON number (`"total": 42`); an explicit `"total": null` reads as `null`. **Migration:** recompile against 3.0.0. Code that consumed `Total` as an `int` — arithmetic, comparisons, assignment to an `int` — no longer compiles and has to decide what a missing total should mean. Interpolating it into a string still compiles but renders empty instead of `0`. To page through everything, loop while a page comes back full and use `Total` only as an early exit:

  ```csharp
  if (result.Items.Count < pagination.Limit)
      break;

  if (result.Total is int total && allContacts.Count >= total)
      break;
  ```

  `allContacts.Count >= result.Total` on its own is not a substitute: with `Total` null the lifted comparison is always `false`. Neither is `result.Total ?? 0`, which reintroduces the bug the change removes.

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
