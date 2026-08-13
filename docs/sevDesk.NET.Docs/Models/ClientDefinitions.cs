namespace sevDesk.NET.Docs.Models;

public static class ClientDefinitions
{
    public static readonly IReadOnlyList<ClientDefinition> All =
    [
        // Finanzdokumente
        new("Invoices", "api/invoices", "Create, manage, and send invoices. Supports PDF generation, email sending, and status tracking.", "IInvoiceClient", "Financial Documents"),
        new("Invoice Positions", "api/invoice-positions", "Manage line items on invoices with quantity, price, and tax details.", "IInvoicePosClient", "Financial Documents"),
        new("Orders", "api/orders", "Create and manage offers, order confirmations, and delivery notes.", "IOrderClient", "Financial Documents"),
        new("Order Positions", "api/order-positions", "Manage line items on orders.", "IOrderPosClient", "Financial Documents"),
        new("Vouchers", "api/vouchers", "Record expenses and revenues with file upload support.", "IVoucherClient", "Financial Documents"),
        new("Voucher Positions", "api/voucher-positions", "Manage accounting line items on vouchers.", "IVoucherPosClient", "Financial Documents"),
        new("Credit Notes", "api/credit-notes", "Create credit memos, optionally from existing invoices.", "ICreditNoteClient", "Financial Documents"),
        new("Credit Note Positions", "api/credit-note-positions", "Manage line items on credit notes.", "ICreditNotePosClient", "Financial Documents"),

        // Kontakte
        new("Contacts", "api/contacts", "Manage customers, suppliers, and partners with full CRUD and customer number generation.", "IContactClient", "Contacts"),
        new("Contact Addresses", "api/contact-addresses", "Manage postal addresses for contacts.", "IContactAddressClient", "Contacts"),
        new("Communication Ways", "api/communication-ways", "Manage email, phone, and other communication channels for contacts.", "ICommunicationWayClient", "Contacts"),
        new("Accounting Contacts", "api/accounting-contacts", "Query the debitor and creditor numbers assigned to contacts.", "IAccountingContactClient", "Contacts"),

        // Banking
        new("Check Accounts", "api/check-accounts", "Manage bank accounts (online and offline) with balance queries.", "ICheckAccountClient", "Banking"),
        new("Check Account Transactions", "api/check-account-transactions", "Record and manage bank transactions.", "ICheckAccountTransactionClient", "Banking"),

        // Produkte
        new("Parts", "api/parts", "Manage products and services with pricing and stock tracking.", "IPartClient", "Products"),

        // Organisation
        new("Tags", "api/tags", "Create and assign tags to categorize records.", "ITagClient", "Organization"),
        new("Categories", "api/categories", "Manage categories for organizing documents and contacts.", "ICategoryClient", "Organization"),
        new("Documents", "api/documents", "Upload, download, and manage document files.", "IDocumentClient", "Organization"),

        // Stammdaten
        new("Units", "api/unities", "Query available units of measure (pieces, hours, etc.).", "IUnityClient", "Reference Data"),
        new("Tax Rules", "api/tax-rules", "Query available tax rules and rates.", "ITaxRuleClient", "Reference Data"),
        new("Currency Exchange Rates", "api/currency-exchange-rates", "Query currency exchange rates.", "ICurrencyExchangeRateClient", "Reference Data"),
        new("Static Countries", "api/static-countries", "Query the country catalogue that country references resolve against.", "IStaticCountryClient", "Reference Data"),
    ];

    public static readonly IReadOnlyList<string> GroupOrder =
    [
        "Financial Documents",
        "Contacts",
        "Banking",
        "Products",
        "Organization",
        "Reference Data"
    ];
}

public record ClientDefinition(string Title, string Href, string Description, string InterfaceName, string Group);
