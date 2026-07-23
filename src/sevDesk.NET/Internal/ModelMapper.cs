using sevDesk.NET.Internal.ApiModels;
using sevDesk.NET.Models;
using sevDesk.NET.Models.Enums;

namespace sevDesk.NET.Internal;

internal static class ModelMapper
{
    // --- Object Reference ---

    internal static SevDeskObjectReference? ToPublic(ApiObjectReference? api) =>
        api is null ? null : new SevDeskObjectReference { Id = api.Id, ObjectName = api.ObjectName ?? "" };

    internal static ApiObjectReference? ToApi(SevDeskObjectReference? model) =>
        model is null ? null : new ApiObjectReference { Id = model.Id, ObjectName = model.ObjectName };

    // --- DateTime Parsing ---

    internal static DateTime? ParseDateTime(string? value) =>
        value is not null && DateTime.TryParse(value, System.Globalization.CultureInfo.InvariantCulture,
            System.Globalization.DateTimeStyles.None, out var dt) ? dt : null;

    internal static string? FormatDateTime(DateTime? value) =>
        value?.ToString("yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);

    internal static decimal? ParseDecimal(string? value) =>
        value is not null && decimal.TryParse(value, System.Globalization.CultureInfo.InvariantCulture, out var d) ? d : null;

    // --- Bool Parsing (API returns "0"/"1" strings) ---

    internal static bool? ParseBool(string? value) =>
        value is "1" or "true" ? true : value is "0" or "false" ? false : null;

    internal static string? FormatBool(bool? value) =>
        value.HasValue ? (value.Value ? "1" : "0") : null;

    // --- Contact ---

    internal static Contact ToPublic(ApiContact api) => new()
    {
        Id = api.Id,
        CustomerNumber = api.CustomerNumber,
        Surename = api.Surename,
        Familyname = api.Familyname,
        Name = api.Name,
        Name2 = api.Name2,
        Status = api.Status.HasValue ? (ContactStatus)api.Status.Value : null,
        Title = api.Title,
        AcademicTitle = api.AcademicTitle,
        Gender = api.Gender,
        Category = ToPublic(api.Category),
        Description = api.Description,
        VatNumber = api.VatNumber,
        BankAccount = api.BankAccount,
        BankNumber = api.BankNumber,
        DefaultCashbackTime = api.DefaultCashbackTime,
        DefaultCashbackPercent = api.DefaultCashbackPercent,
        DefaultTimeToPay = api.DefaultTimeToPay,
        TaxNumber = api.TaxNumber,
        TaxOffice = api.TaxOffice,
        ExemptVat = ParseBool(api.ExemptVat),
        Birthday = ParseDateTime(api.Birthday),
        DefaultDiscountPercentage = ParseDecimal(api.DefaultDiscountPercentage),
        GovernmentAgency = ParseBool(api.GovernmentAgency),
        Create = ParseDateTime(api.Create),
        Update = ParseDateTime(api.Update)
    };

    internal static ApiContact ToApi(Contact model) => new()
    {
        Id = model.Id,
        CustomerNumber = model.CustomerNumber,
        Surename = model.Surename,
        Familyname = model.Familyname,
        Name = model.Name,
        Name2 = model.Name2,
        Status = model.Status.HasValue ? (int)model.Status.Value : null,
        Title = model.Title,
        AcademicTitle = model.AcademicTitle,
        Gender = model.Gender,
        Category = ToApi(model.Category),
        Description = model.Description,
        VatNumber = model.VatNumber,
        BankAccount = model.BankAccount,
        BankNumber = model.BankNumber,
        DefaultCashbackTime = model.DefaultCashbackTime,
        DefaultCashbackPercent = model.DefaultCashbackPercent,
        DefaultTimeToPay = model.DefaultTimeToPay,
        TaxNumber = model.TaxNumber,
        TaxOffice = model.TaxOffice,
        ExemptVat = FormatBool(model.ExemptVat),
        Birthday = FormatDateTime(model.Birthday),
        DefaultDiscountPercentage = model.DefaultDiscountPercentage?.ToString(System.Globalization.CultureInfo.InvariantCulture),
        GovernmentAgency = FormatBool(model.GovernmentAgency)
    };

    // --- Invoice ---

    internal static Invoice ToPublic(ApiInvoice api) => new()
    {
        Id = api.Id,
        InvoiceNumber = api.InvoiceNumber,
        Contact = ToPublic(api.Contact),
        InvoiceDate = ParseDateTime(api.InvoiceDate),
        DeliveryDate = ParseDateTime(api.DeliveryDate),
        Status = api.Status.HasValue ? (InvoiceStatus)api.Status.Value : null,
        InvoiceType = api.InvoiceType is not null && Enum.TryParse<InvoiceType>(api.InvoiceType, out var it) ? it : null,
        Header = api.Header,
        HeadText = api.HeadText,
        FootText = api.FootText,
        TimeToPay = api.TimeToPay,
        DiscountTime = api.DiscountTime,
        Discount = api.Discount,
        ContactPerson = ToPublic(api.ContactPerson),
        Address = api.Address,
        Currency = api.Currency,
        SumNet = api.SumNet,
        SumGross = api.SumGross,
        SumTax = api.SumTax,
        TaxType = api.TaxType,
        TaxRate = api.TaxRate,
        TaxText = api.TaxText,
        SendDate = ParseDateTime(api.SendDate),
        PaymentMethod = ToPublic(api.PaymentMethod),
        CostCentre = ToPublic(api.CostCentre),
        SendType = api.SendType,
        Origin = ToPublic(api.Origin),
        CustomerInternalNote = api.CustomerInternalNote,
        SmallSettlement = ParseBool(api.SmallSettlement),
        TaxSet = ToPublic(api.TaxSet),
        Create = ParseDateTime(api.Create),
        Update = ParseDateTime(api.Update),
        TaxRule = ToPublic(api.TaxRule)
    };

    internal static ApiInvoice ToApi(Invoice model) => new()
    {
        Id = model.Id,
        InvoiceNumber = model.InvoiceNumber,
        Contact = ToApi(model.Contact),
        InvoiceDate = FormatDateTime(model.InvoiceDate),
        DeliveryDate = FormatDateTime(model.DeliveryDate),
        Status = model.Status.HasValue ? (int)model.Status.Value : null,
        InvoiceType = model.InvoiceType?.ToString(),
        Header = model.Header,
        HeadText = model.HeadText,
        FootText = model.FootText,
        TimeToPay = model.TimeToPay,
        DiscountTime = model.DiscountTime,
        Discount = model.Discount,
        ContactPerson = ToApi(model.ContactPerson),
        Address = model.Address,
        Currency = model.Currency,
        TaxType = model.TaxType,
        TaxRate = model.TaxRate,
        TaxText = model.TaxText,
        SendDate = FormatDateTime(model.SendDate),
        PaymentMethod = ToApi(model.PaymentMethod),
        CostCentre = ToApi(model.CostCentre),
        SendType = model.SendType,
        Origin = ToApi(model.Origin),
        CustomerInternalNote = model.CustomerInternalNote,
        SmallSettlement = FormatBool(model.SmallSettlement),
        TaxSet = ToApi(model.TaxSet),
        TaxRule = ToApi(model.TaxRule)
    };

    // --- InvoicePos ---

    internal static InvoicePos ToPublic(ApiInvoicePos api) => new()
    {
        Id = api.Id,
        Invoice = ToPublic(api.Invoice),
        Part = ToPublic(api.Part),
        Quantity = api.Quantity,
        Price = api.Price,
        Name = api.Name,
        Unity = ToPublic(api.Unity),
        TaxRate = api.TaxRate,
        PositionNumber = api.PositionNumber,
        Text = api.Text,
        Discount = api.Discount,
        Optional = ParseBool(api.Optional),
        SumNet = ParseDecimal(api.SumNet),
        SumGross = ParseDecimal(api.SumGross),
        SumTax = ParseDecimal(api.SumTax),
        Create = ParseDateTime(api.Create),
        Update = ParseDateTime(api.Update)
    };

    internal static ApiInvoicePos ToApi(InvoicePos model) => new()
    {
        Id = model.Id,
        Invoice = ToApi(model.Invoice),
        Part = ToApi(model.Part),
        Quantity = model.Quantity,
        Price = model.Price,
        Name = model.Name,
        Unity = ToApi(model.Unity),
        TaxRate = model.TaxRate,
        PositionNumber = model.PositionNumber,
        Text = model.Text,
        Discount = model.Discount,
        Optional = FormatBool(model.Optional),
        MapAll = true
    };

    // --- Order ---

    internal static Order ToPublic(ApiOrder api) => new()
    {
        Id = api.Id,
        OrderNumber = api.OrderNumber,
        Contact = ToPublic(api.Contact),
        OrderDate = ParseDateTime(api.OrderDate),
        Status = api.Status.HasValue ? (OrderStatus)api.Status.Value : null,
        OrderType = api.OrderType is not null && Enum.TryParse<OrderType>(api.OrderType, out var ot) ? ot : null,
        Header = api.Header,
        HeadText = api.HeadText,
        FootText = api.FootText,
        ContactPerson = ToPublic(api.ContactPerson),
        Address = api.Address,
        Currency = api.Currency,
        SumNet = ParseDecimal(api.SumNet),
        SumGross = ParseDecimal(api.SumGross),
        SumTax = ParseDecimal(api.SumTax),
        TaxType = api.TaxType,
        TaxRate = api.TaxRate,
        TaxText = api.TaxText,
        SendDate = ParseDateTime(api.SendDate),
        DeliveryDate = ParseDateTime(api.DeliveryDate),
        SmallSettlement = ParseBool(api.SmallSettlement),
        TaxSet = ToPublic(api.TaxSet),
        Origin = ToPublic(api.Origin),
        CustomerInternalNote = api.CustomerInternalNote,
        Create = ParseDateTime(api.Create),
        Update = ParseDateTime(api.Update)
    };

    internal static ApiOrder ToApi(Order model) => new()
    {
        Id = model.Id,
        OrderNumber = model.OrderNumber,
        Contact = ToApi(model.Contact),
        OrderDate = FormatDateTime(model.OrderDate),
        Status = model.Status.HasValue ? (int)model.Status.Value : null,
        OrderType = model.OrderType?.ToString(),
        Header = model.Header,
        HeadText = model.HeadText,
        FootText = model.FootText,
        ContactPerson = ToApi(model.ContactPerson),
        Address = model.Address,
        Currency = model.Currency,
        TaxType = model.TaxType,
        TaxRate = model.TaxRate,
        TaxText = model.TaxText,
        SendDate = FormatDateTime(model.SendDate),
        DeliveryDate = FormatDateTime(model.DeliveryDate),
        SmallSettlement = FormatBool(model.SmallSettlement),
        TaxSet = ToApi(model.TaxSet),
        Origin = ToApi(model.Origin),
        CustomerInternalNote = model.CustomerInternalNote
    };

    // --- OrderPos ---

    internal static OrderPos ToPublic(ApiOrderPos api) => new()
    {
        Id = api.Id,
        Order = ToPublic(api.Order),
        Part = ToPublic(api.Part),
        Quantity = api.Quantity,
        Price = api.Price,
        Name = api.Name,
        Unity = ToPublic(api.Unity),
        TaxRate = api.TaxRate,
        PositionNumber = api.PositionNumber,
        Text = api.Text,
        Discount = api.Discount,
        Optional = ParseBool(api.Optional),
        SumNet = ParseDecimal(api.SumNet),
        SumGross = ParseDecimal(api.SumGross),
        SumTax = ParseDecimal(api.SumTax),
        Create = ParseDateTime(api.Create),
        Update = ParseDateTime(api.Update)
    };

    internal static ApiOrderPos ToApi(OrderPos model) => new()
    {
        Id = model.Id,
        Order = ToApi(model.Order),
        Part = ToApi(model.Part),
        Quantity = model.Quantity,
        Price = model.Price,
        Name = model.Name,
        Unity = ToApi(model.Unity),
        TaxRate = model.TaxRate,
        PositionNumber = model.PositionNumber,
        Text = model.Text,
        Discount = model.Discount,
        Optional = FormatBool(model.Optional),
        MapAll = true
    };

    // --- Voucher ---

    internal static Voucher ToPublic(ApiVoucher api) => new()
    {
        Id = api.Id,
        VoucherDate = ParseDateTime(api.VoucherDate),
        Supplier = ToPublic(api.Supplier),
        Status = api.Status.HasValue ? (VoucherStatus)api.Status.Value : null,
        VoucherType = api.VoucherType is not null && Enum.TryParse<VoucherType>(api.VoucherType, out var vt) ? vt : null,
        Description = api.Description,
        PayDate = ParseDateTime(api.PayDate),
        Currency = api.Currency,
        SumNet = ParseDecimal(api.SumNet),
        SumGross = ParseDecimal(api.SumGross),
        SumTax = ParseDecimal(api.SumTax),
        TaxType = api.TaxType,
        CreditDebit = api.CreditDebit,
        Document = ToPublic(api.Document),
        CostCentre = ToPublic(api.CostCentre),
        PaidAmount = api.PaidAmount,
        TaxSet = ToPublic(api.TaxSet),
        Create = ParseDateTime(api.Create),
        Update = ParseDateTime(api.Update)
    };

    internal static ApiVoucher ToApi(Voucher model) => new()
    {
        Id = model.Id,
        VoucherDate = FormatDateTime(model.VoucherDate),
        Supplier = ToApi(model.Supplier),
        Status = model.Status.HasValue ? (int)model.Status.Value : null,
        VoucherType = model.VoucherType?.ToString(),
        Description = model.Description,
        PayDate = FormatDateTime(model.PayDate),
        Currency = model.Currency,
        TaxType = model.TaxType,
        CreditDebit = model.CreditDebit,
        Document = ToApi(model.Document),
        CostCentre = ToApi(model.CostCentre),
        TaxSet = ToApi(model.TaxSet)
    };

    // --- VoucherPos ---

    internal static VoucherPos ToPublic(ApiVoucherPos api) => new()
    {
        Id = api.Id,
        Voucher = ToPublic(api.Voucher),
        AccountingType = ToPublic(api.AccountingType),
        EstimatedAccountingType = ToPublic(api.EstimatedAccountingType),
        Net = api.Net,
        TaxRate = api.TaxRate,
        IsAsset = ParseBool(api.IsAsset),
        SumNet = ParseDecimal(api.SumNet),
        SumGross = ParseDecimal(api.SumGross),
        SumTax = ParseDecimal(api.SumTax),
        Comment = api.Comment,
        Create = ParseDateTime(api.Create),
        Update = ParseDateTime(api.Update)
    };

    internal static ApiVoucherPos ToApi(VoucherPos model) => new()
    {
        Id = model.Id,
        Voucher = ToApi(model.Voucher),
        AccountingType = ToApi(model.AccountingType),
        EstimatedAccountingType = ToApi(model.EstimatedAccountingType),
        Net = model.Net,
        TaxRate = model.TaxRate,
        IsAsset = FormatBool(model.IsAsset),
        Comment = model.Comment,
        MapAll = true
    };

    // --- CreditNote ---

    internal static CreditNote ToPublic(ApiCreditNote api) => new()
    {
        Id = api.Id,
        CreditNoteNumber = api.CreditNoteNumber,
        Contact = ToPublic(api.Contact),
        CreditNoteDate = ParseDateTime(api.CreditNoteDate),
        Status = api.Status.HasValue ? (CreditNoteStatus)api.Status.Value : null,
        Header = api.Header,
        HeadText = api.HeadText,
        FootText = api.FootText,
        ContactPerson = ToPublic(api.ContactPerson),
        Address = api.Address,
        Currency = api.Currency,
        SumNet = ParseDecimal(api.SumNet),
        SumGross = ParseDecimal(api.SumGross),
        SumTax = ParseDecimal(api.SumTax),
        TaxType = api.TaxType,
        TaxRate = api.TaxRate,
        TaxText = api.TaxText,
        TaxSet = ToPublic(api.TaxSet),
        SendDate = ParseDateTime(api.SendDate),
        SmallSettlement = ParseBool(api.SmallSettlement),
        Create = ParseDateTime(api.Create),
        Update = ParseDateTime(api.Update)
    };

    internal static ApiCreditNote ToApi(CreditNote model) => new()
    {
        Id = model.Id,
        CreditNoteNumber = model.CreditNoteNumber,
        Contact = ToApi(model.Contact),
        CreditNoteDate = FormatDateTime(model.CreditNoteDate),
        Status = model.Status.HasValue ? (int)model.Status.Value : null,
        Header = model.Header,
        HeadText = model.HeadText,
        FootText = model.FootText,
        ContactPerson = ToApi(model.ContactPerson),
        Address = model.Address,
        Currency = model.Currency,
        TaxType = model.TaxType,
        TaxRate = model.TaxRate,
        TaxText = model.TaxText,
        TaxSet = ToApi(model.TaxSet),
        SendDate = FormatDateTime(model.SendDate),
        SmallSettlement = FormatBool(model.SmallSettlement)
    };

    // --- CreditNotePos ---

    internal static CreditNotePos ToPublic(ApiCreditNotePos api) => new()
    {
        Id = api.Id,
        CreditNote = ToPublic(api.CreditNote),
        Part = ToPublic(api.Part),
        Quantity = api.Quantity,
        Price = api.Price,
        Name = api.Name,
        Unity = ToPublic(api.Unity),
        TaxRate = api.TaxRate,
        PositionNumber = api.PositionNumber,
        Text = api.Text,
        Discount = api.Discount,
        Optional = ParseBool(api.Optional),
        SumNet = ParseDecimal(api.SumNet),
        SumGross = ParseDecimal(api.SumGross),
        SumTax = ParseDecimal(api.SumTax),
        Create = ParseDateTime(api.Create),
        Update = ParseDateTime(api.Update)
    };

    internal static ApiCreditNotePos ToApi(CreditNotePos model) => new()
    {
        Id = model.Id,
        CreditNote = ToApi(model.CreditNote),
        Part = ToApi(model.Part),
        Quantity = model.Quantity,
        Price = model.Price,
        Name = model.Name,
        Unity = ToApi(model.Unity),
        TaxRate = model.TaxRate,
        PositionNumber = model.PositionNumber,
        Text = model.Text,
        Discount = model.Discount,
        Optional = FormatBool(model.Optional),
        MapAll = true
    };

    // --- Part ---

    internal static Part ToPublic(ApiPart api) => new()
    {
        Id = api.Id,
        Name = api.Name,
        PartNumber = api.PartNumber,
        Text = api.Text,
        Unity = ToPublic(api.Unity),
        Price = api.Price,
        PriceGross = api.PriceGross,
        PriceNet = api.PriceNet,
        TaxRate = api.TaxRate,
        InternalComment = api.InternalComment,
        StockEnabled = ParseBool(api.StockEnabled),
        Stock = api.Stock,
        Category = ToPublic(api.Category),
        Create = ParseDateTime(api.Create),
        Update = ParseDateTime(api.Update)
    };

    internal static ApiPart ToApi(Part model) => new()
    {
        Id = model.Id,
        Name = model.Name,
        PartNumber = model.PartNumber,
        Text = model.Text,
        Unity = ToApi(model.Unity),
        Price = model.Price,
        PriceGross = model.PriceGross,
        PriceNet = model.PriceNet,
        TaxRate = model.TaxRate,
        InternalComment = model.InternalComment,
        StockEnabled = FormatBool(model.StockEnabled),
        Stock = model.Stock,
        Category = ToApi(model.Category)
    };

    // --- CheckAccount ---

    internal static CheckAccount ToPublic(ApiCheckAccount api) => new()
    {
        Id = api.Id,
        Name = api.Name,
        Type = api.Type switch { "online" => CheckAccountType.Online, "offline" => CheckAccountType.Offline, _ => null },
        Iban = api.Iban,
        Bic = api.Bic,
        BankName = api.BankName,
        Currency = api.Currency,
        DefaultAccount = ParseBool(api.DefaultAccount),
        Status = api.Status,
        Create = ParseDateTime(api.Create),
        Update = ParseDateTime(api.Update)
    };

    internal static ApiCheckAccount ToApi(CheckAccount model) => new()
    {
        Id = model.Id,
        Name = model.Name,
        Type = model.Type switch { CheckAccountType.Online => "online", CheckAccountType.Offline => "offline", _ => null },
        Iban = model.Iban,
        Bic = model.Bic,
        BankName = model.BankName,
        Currency = model.Currency,
        DefaultAccount = FormatBool(model.DefaultAccount),
        Status = model.Status
    };

    // --- CheckAccountTransaction ---

    internal static CheckAccountTransaction ToPublic(ApiCheckAccountTransaction api) => new()
    {
        Id = api.Id,
        CheckAccount = ToPublic(api.CheckAccount),
        ValueDate = ParseDateTime(api.ValueDate),
        EntryDate = ParseDateTime(api.EntryDate),
        Amount = api.Amount,
        PayeeName = api.PayeePayerName,
        Purpose = api.PaymtPurpose,
        Status = api.Status,
        Create = ParseDateTime(api.Create),
        Update = ParseDateTime(api.Update)
    };

    internal static ApiCheckAccountTransaction ToApi(CheckAccountTransaction model) => new()
    {
        Id = model.Id,
        CheckAccount = ToApi(model.CheckAccount),
        ValueDate = FormatDateTime(model.ValueDate),
        EntryDate = FormatDateTime(model.EntryDate),
        Amount = model.Amount,
        PayeePayerName = model.PayeeName,
        PaymtPurpose = model.Purpose,
        Status = model.Status
    };

    // --- CommunicationWay ---

    internal static CommunicationWay ToPublic(ApiCommunicationWay api) => new()
    {
        Id = api.Id,
        Contact = ToPublic(api.Contact),
        Type = api.Type is not null && Enum.TryParse<CommunicationWayType>(api.Type, out var cwt) ? cwt : null,
        Value = api.Value,
        Key = ToPublic(api.Key),
        Main = ParseBool(api.Main),
        Create = ParseDateTime(api.Create),
        Update = ParseDateTime(api.Update)
    };

    internal static ApiCommunicationWay ToApi(CommunicationWay model) => new()
    {
        Id = model.Id,
        Contact = ToApi(model.Contact),
        Type = model.Type?.ToString(),
        Value = model.Value,
        Key = ToApi(model.Key),
        Main = FormatBool(model.Main)
    };

    // --- ContactAddress ---

    internal static ContactAddress ToPublic(ApiContactAddress api) => new()
    {
        Id = api.Id,
        Contact = ToPublic(api.Contact),
        Street = api.Street,
        Zip = api.Zip,
        City = api.City,
        Country = ToPublic(api.Country),
        Category = ToPublic(api.Category),
        Name = api.Name,
        Name2 = api.Name2,
        Name3 = api.Name3,
        Name4 = api.Name4,
        Create = ParseDateTime(api.Create),
        Update = ParseDateTime(api.Update)
    };

    internal static ApiContactAddress ToApi(ContactAddress model) => new()
    {
        Id = model.Id,
        Contact = ToApi(model.Contact),
        Street = model.Street,
        Zip = model.Zip,
        City = model.City,
        Country = ToApi(model.Country),
        Category = ToApi(model.Category),
        Name = model.Name,
        Name2 = model.Name2,
        Name3 = model.Name3,
        Name4 = model.Name4
    };

    // --- Tag ---

    internal static Tag ToPublic(ApiTag api) => new()
    {
        Id = api.Id,
        Name = api.Name,
        Object = ToPublic(api.Object),
        ObjectType = api.ObjectType,
        Create = ParseDateTime(api.Create)
    };

    internal static ApiTag ToApi(Tag model) => new()
    {
        Id = model.Id,
        Name = model.Name,
        Object = ToApi(model.Object)
    };

    // --- Category ---

    internal static Category ToPublic(ApiCategory api) => new()
    {
        Id = api.Id,
        Name = api.Name,
        ObjectType = api.ObjectType,
        Priority = api.Priority,
        Code = api.Code,
        Type = api.Type,
        Color = api.Color,
        PostingAccount = api.PostingAccount,
        TranslationCode = api.TranslationCode,
        Create = ParseDateTime(api.Create),
        Update = ParseDateTime(api.Update)
    };

    internal static ApiCategory ToApi(Category model) => new()
    {
        Id = model.Id,
        Name = model.Name,
        ObjectType = model.ObjectType,
        Priority = model.Priority,
        Code = model.Code,
        Type = model.Type,
        Color = model.Color,
        PostingAccount = model.PostingAccount,
        TranslationCode = model.TranslationCode
    };

    // --- Unity ---

    internal static Unity ToPublic(ApiUnity api) => new()
    {
        Id = api.Id,
        Name = api.Name,
        TranslationCode = api.TranslationCode,
        UnitySystem = api.UnitySystem,
        UneceTradeUnitCode = api.UneceTradeUnitCode,
        Create = ParseDateTime(api.Create),
        Update = ParseDateTime(api.Update)
    };

    // --- TaxRule ---

    internal static TaxRule ToPublic(ApiTaxRule api) => new()
    {
        Id = api.Id,
        Name = api.Name,
        Description = api.Description,
        Code = api.Code,
        CountryClient = ToPublic(api.CountryClient),
        CountryContactType = api.CountryContactType
    };

    // --- CurrencyExchangeRate ---

    internal static CurrencyExchangeRate ToPublic(ApiCurrencyExchangeRate api) => new()
    {
        Id = api.Id,
        Currency = api.Currency,
        Rate = api.Rate,
        Date = ParseDateTime(api.Date)
    };

    // --- Document ---

    internal static Document ToPublic(ApiDocument api) => new()
    {
        Id = api.Id,
        Filename = api.Filename,
        Extension = api.Extension,
        Size = api.Size,
        MimeType = api.MimeType,
        Object = ToPublic(api.Object),
        Folder = ToPublic(api.Folder),
        Status = api.Status,
        Create = ParseDateTime(api.Create),
        Update = ParseDateTime(api.Update)
    };
}
