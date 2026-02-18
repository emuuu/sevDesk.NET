using sevDeskNET.Clients;

namespace sevDeskNET;

/// <summary>
/// Root client for the sevDesk API providing access to all sub-clients.
/// </summary>
public interface ISevDeskClient
{
    /// <summary>Gets the client for managing contacts.</summary>
    IContactClient Contacts { get; }

    /// <summary>Gets the client for managing invoices.</summary>
    IInvoiceClient Invoices { get; }

    /// <summary>Gets the client for managing invoice positions.</summary>
    IInvoicePosClient InvoicePositions { get; }

    /// <summary>Gets the client for managing orders.</summary>
    IOrderClient Orders { get; }

    /// <summary>Gets the client for managing order positions.</summary>
    IOrderPosClient OrderPositions { get; }

    /// <summary>Gets the client for managing vouchers.</summary>
    IVoucherClient Vouchers { get; }

    /// <summary>Gets the client for managing voucher positions.</summary>
    IVoucherPosClient VoucherPositions { get; }

    /// <summary>Gets the client for managing credit notes.</summary>
    ICreditNoteClient CreditNotes { get; }

    /// <summary>Gets the client for managing credit note positions.</summary>
    ICreditNotePosClient CreditNotePositions { get; }

    /// <summary>Gets the client for managing parts (products/services).</summary>
    IPartClient Parts { get; }

    /// <summary>Gets the client for managing check accounts (bank accounts).</summary>
    ICheckAccountClient CheckAccounts { get; }

    /// <summary>Gets the client for managing check account transactions.</summary>
    ICheckAccountTransactionClient CheckAccountTransactions { get; }

    /// <summary>Gets the client for managing communication ways.</summary>
    ICommunicationWayClient CommunicationWays { get; }

    /// <summary>Gets the client for managing contact addresses.</summary>
    IContactAddressClient ContactAddresses { get; }

    /// <summary>Gets the client for managing tags.</summary>
    ITagClient Tags { get; }

    /// <summary>Gets the client for managing categories.</summary>
    ICategoryClient Categories { get; }

    /// <summary>Gets the client for querying units of measure.</summary>
    IUnityClient Unities { get; }

    /// <summary>Gets the client for querying tax rules.</summary>
    ITaxRuleClient TaxRules { get; }

    /// <summary>Gets the client for querying currency exchange rates.</summary>
    ICurrencyExchangeRateClient CurrencyExchangeRates { get; }

    /// <summary>Gets the client for managing documents.</summary>
    IDocumentClient Documents { get; }
}
