using sevDesk.NET.Clients;
using sevDesk.NET.Internal;

namespace sevDesk.NET;

/// <summary>
/// Root client implementation for the sevDesk API.
/// </summary>
public class SevDeskClient : ISevDeskClient
{
    /// <inheritdoc />
    public IContactClient Contacts { get; }

    /// <inheritdoc />
    public IInvoiceClient Invoices { get; }

    /// <inheritdoc />
    public IInvoicePosClient InvoicePositions { get; }

    /// <inheritdoc />
    public IOrderClient Orders { get; }

    /// <inheritdoc />
    public IOrderPosClient OrderPositions { get; }

    /// <inheritdoc />
    public IVoucherClient Vouchers { get; }

    /// <inheritdoc />
    public IVoucherPosClient VoucherPositions { get; }

    /// <inheritdoc />
    public ICreditNoteClient CreditNotes { get; }

    /// <inheritdoc />
    public ICreditNotePosClient CreditNotePositions { get; }

    /// <inheritdoc />
    public IPartClient Parts { get; }

    /// <inheritdoc />
    public ICheckAccountClient CheckAccounts { get; }

    /// <inheritdoc />
    public ICheckAccountTransactionClient CheckAccountTransactions { get; }

    /// <inheritdoc />
    public ICommunicationWayClient CommunicationWays { get; }

    /// <inheritdoc />
    public IContactAddressClient ContactAddresses { get; }

    /// <inheritdoc />
    public IAccountingContactClient AccountingContacts { get; }

    /// <inheritdoc />
    public ITagClient Tags { get; }

    /// <inheritdoc />
    public ICategoryClient Categories { get; }

    /// <inheritdoc />
    public IUnityClient Unities { get; }

    /// <inheritdoc />
    public ITaxRuleClient TaxRules { get; }

    /// <inheritdoc />
    public ICurrencyExchangeRateClient CurrencyExchangeRates { get; }

    /// <inheritdoc />
    public IStaticCountryClient StaticCountries { get; }

    /// <inheritdoc />
    public IDocumentClient Documents { get; }

    /// <summary>
    /// Initializes a new instance of <see cref="SevDeskClient"/>.
    /// </summary>
    /// <param name="httpClient">The configured HTTP client for sevDesk API requests.</param>
    public SevDeskClient(HttpClient httpClient)
    {
        ArgumentNullException.ThrowIfNull(httpClient);
        var baseClient = new BaseClient(httpClient);

        Contacts = new ContactClient(baseClient);
        Invoices = new InvoiceClient(baseClient);
        InvoicePositions = new InvoicePosClient(baseClient);
        Orders = new OrderClient(baseClient);
        OrderPositions = new OrderPosClient(baseClient);
        Vouchers = new VoucherClient(baseClient);
        VoucherPositions = new VoucherPosClient(baseClient);
        CreditNotes = new CreditNoteClient(baseClient);
        CreditNotePositions = new CreditNotePosClient(baseClient);
        Parts = new PartClient(baseClient);
        CheckAccounts = new CheckAccountClient(baseClient);
        CheckAccountTransactions = new CheckAccountTransactionClient(baseClient);
        CommunicationWays = new CommunicationWayClient(baseClient);
        ContactAddresses = new ContactAddressClient(baseClient);
        AccountingContacts = new AccountingContactClient(baseClient);
        Tags = new TagClient(baseClient);
        Categories = new CategoryClient(baseClient);
        Unities = new UnityClient(baseClient);
        TaxRules = new TaxRuleClient(baseClient);
        CurrencyExchangeRates = new CurrencyExchangeRateClient(baseClient);
        StaticCountries = new StaticCountryClient(baseClient);
        Documents = new DocumentClient(baseClient);
    }
}
