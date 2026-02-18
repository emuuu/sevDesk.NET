# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/).

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
