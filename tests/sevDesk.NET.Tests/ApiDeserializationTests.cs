using System.Text.Json;
using sevDesk.NET.Internal;
using sevDesk.NET.Internal.ApiModels;
using sevDesk.NET.Models.Enums;
using Shouldly;
using Xunit;

namespace sevDesk.NET.Tests;

/// <summary>
/// Tests that feed raw JSON strings matching the real sevDesk API format
/// through the full deserialization + mapping pipeline.
/// The real API returns everything as strings (IDs, numbers, booleans).
/// </summary>
public class ApiDeserializationTests
{
    [Fact]
    public void Contact_DeserializesRealApiResponse()
    {
        var json = """
        {
            "objects": [{
                "id": "12345678",
                "customerNumber": "K-10042",
                "surename": "Max",
                "familyname": "Mustermann",
                "name": "Test GmbH",
                "name2": "Abteilung Einkauf",
                "status": "100",
                "titel": "Herr",
                "academicTitle": "Dr.",
                "gender": "m",
                "category": {"id": "3", "objectName": "Category"},
                "description": "Testkunde",
                "vatNumber": "DE123456789",
                "defaultCashbackTime": "10",
                "defaultCashbackPercent": "2.5",
                "defaultTimeToPay": "14",
                "taxNumber": "12/345/67890",
                "exemptVat": "0",
                "defaultDiscountPercentage": "1",
                "governmentAgency": "0",
                "buyerReference": "991-33333TEST-33",
                "create": "2026-02-19T14:13:11+01:00",
                "update": "2026-02-19T15:00:00+01:00"
            }],
            "total": "1"
        }
        """;

        var response = JsonSerializer.Deserialize(json, SevDeskJsonContext.Default.SevDeskApiListResponseApiContact);

        response.ShouldNotBeNull();
        response.Total.ShouldBe(1);
        response.Objects.ShouldNotBeNull();
        response.Objects!.Count.ShouldBe(1);

        var contact = ModelMapper.ToPublic(response.Objects[0]);

        contact.Id.ShouldBe(12345678);
        contact.CustomerNumber.ShouldBe("K-10042");
        contact.Surename.ShouldBe("Max");
        contact.Familyname.ShouldBe("Mustermann");
        contact.Name.ShouldBe("Test GmbH");
        contact.Name2.ShouldBe("Abteilung Einkauf");
        contact.Status.ShouldBe(ContactStatus.Active);
        contact.Title.ShouldBe("Herr"); // JSON key is "titel"
        contact.AcademicTitle.ShouldBe("Dr.");
        contact.Gender.ShouldBe("m");
        contact.Category.ShouldNotBeNull();
        contact.Category!.Id.ShouldBe(3);
        contact.Category.ObjectName.ShouldBe("Category");
        contact.Description.ShouldBe("Testkunde");
        contact.VatNumber.ShouldBe("DE123456789");
        contact.DefaultCashbackTime.ShouldBe(10);
        contact.DefaultCashbackPercent.ShouldBe(2.5m);
        contact.DefaultTimeToPay.ShouldBe(14);
        contact.TaxNumber.ShouldBe("12/345/67890");
        contact.ExemptVat.ShouldBe(false); // "0" → false
        contact.DefaultDiscountPercentage.ShouldBe(1m); // string "1" → decimal
        contact.GovernmentAgency.ShouldBe(false); // "0" → false
        contact.BuyerReference.ShouldBe("991-33333TEST-33");
        contact.Create.ShouldNotBeNull();
        contact.Update.ShouldNotBeNull();
    }

    [Fact]
    public void Invoice_DeserializesRealApiResponse()
    {
        var json = """
        {
            "objects": [{
                "id": "98765432",
                "invoiceNumber": "RE-2026-0042",
                "contact": {"id": "12345678", "objectName": "Contact"},
                "invoiceDate": "2026-02-19T00:00:00+01:00",
                "deliveryDate": "2026-02-19T00:00:00+01:00",
                "status": "100",
                "invoiceType": "RE",
                "header": "Rechnung",
                "headText": "Sehr geehrte Damen und Herren",
                "footText": "Vielen Dank",
                "timeToPay": "14",
                "discountTime": "7",
                "discount": "0",
                "contactPerson": {"id": "555", "objectName": "SevUser"},
                "address": "Test GmbH\nMusterstr. 1\n12345 Berlin",
                "currency": "EUR",
                "sumNet": "84.02",
                "sumGross": "99.98",
                "sumTax": "15.96",
                "taxType": "default",
                "taxRate": "19",
                "taxText": "Umsatzsteuer 19%",
                "smallSettlement": "0",
                "taxSet": {"id": "1", "objectName": "TaxSet"},
                "paymentMethod": {"id": "42", "objectName": "PaymentMethod"},
                "taxRule": {"id": "1", "objectName": "TaxRule"},
                "einvoiceReference": "RE-2026-0042-EINV",
                "propertyIsEInvoice": "1",
                "create": "2026-02-19T14:13:11+01:00",
                "update": "2026-02-19T14:13:11+01:00"
            }],
            "total": "1"
        }
        """;

        var response = JsonSerializer.Deserialize(json, SevDeskJsonContext.Default.SevDeskApiListResponseApiInvoice);

        response.ShouldNotBeNull();
        response.Total.ShouldBe(1);
        response.Objects.ShouldNotBeNull();

        var invoice = ModelMapper.ToPublic(response.Objects![0]);

        invoice.Id.ShouldBe(98765432);
        invoice.InvoiceNumber.ShouldBe("RE-2026-0042");
        invoice.Contact.ShouldNotBeNull();
        invoice.Contact!.Id.ShouldBe(12345678);
        invoice.InvoiceDate.ShouldNotBeNull();
        invoice.Status.ShouldBe(InvoiceStatus.Draft); // 100 = Draft
        invoice.InvoiceType.ShouldBe(InvoiceType.RE);
        invoice.Header.ShouldBe("Rechnung");
        invoice.TimeToPay.ShouldBe(14); // string "14" → int
        invoice.DiscountTime.ShouldBe(7);
        invoice.Discount.ShouldBe(0m); // string "0" → decimal
        invoice.Currency.ShouldBe("EUR");
        invoice.SumNet.ShouldBe(84.02m); // string "84.02" → decimal via AllowReadingFromString
        invoice.SumGross.ShouldBe(99.98m);
        invoice.SumTax.ShouldBe(15.96m);
        invoice.TaxType.ShouldBe("default");
        invoice.TaxRate.ShouldBe(19m); // string "19" → decimal
        invoice.SmallSettlement.ShouldBe(false); // "0" → false
        invoice.TaxSet.ShouldNotBeNull();
        invoice.TaxSet!.Id.ShouldBe(1);
        invoice.PaymentMethod.ShouldNotBeNull(); // paymentMethod object reference, not a plain string
        invoice.PaymentMethod!.Id.ShouldBe(42);
        invoice.PaymentMethod.ObjectName.ShouldBe("PaymentMethod");
        invoice.TaxRule.ShouldNotBeNull();
        invoice.TaxRule!.Id.ShouldBe(1);
        invoice.EinvoiceReference.ShouldBe("RE-2026-0042-EINV");
        invoice.PropertyIsEInvoice.ShouldBe(true); // "1" → true
        invoice.Create.ShouldNotBeNull();
    }

    [Fact]
    public void Invoice_DeserializesNumericSumsFromPostResponse()
    {
        // POST/PUT responses return sumNet/sumGross/sumTax as JSON numbers instead of strings.
        var json = """
        {
            "objects": [{
                "id": "98765432",
                "invoiceNumber": "RE-2026-0042",
                "sumNet": 84.02,
                "sumGross": 99.98,
                "sumTax": 15.96,
                "einvoiceReference": null,
                "propertyIsEInvoice": null
            }],
            "total": "1"
        }
        """;

        var response = JsonSerializer.Deserialize(json, SevDeskJsonContext.Default.SevDeskApiListResponseApiInvoice);

        response.ShouldNotBeNull();
        response.Objects.ShouldNotBeNull();

        var invoice = ModelMapper.ToPublic(response.Objects![0]);

        invoice.SumNet.ShouldBe(84.02m); // JSON number → decimal
        invoice.SumGross.ShouldBe(99.98m);
        invoice.SumTax.ShouldBe(15.96m);
        invoice.EinvoiceReference.ShouldBeNull();
        invoice.PropertyIsEInvoice.ShouldBeNull();
    }

    [Fact]
    public void CheckAccount_DeserializesRealApiResponse()
    {
        var json = """
        {
            "objects": [{
                "id": "77001",
                "name": "Kasse",
                "type": "offline",
                "currency": "EUR",
                "defaultAccount": "1",
                "status": "100",
                "create": "2026-01-15T10:00:00+01:00",
                "update": "2026-02-19T14:13:11+01:00"
            }],
            "total": "1"
        }
        """;

        var response = JsonSerializer.Deserialize(json, SevDeskJsonContext.Default.SevDeskApiListResponseApiCheckAccount);

        response.ShouldNotBeNull();
        response.Objects.ShouldNotBeNull();

        var account = ModelMapper.ToPublic(response.Objects![0]);

        account.Id.ShouldBe(77001);
        account.Name.ShouldBe("Kasse");
        account.Type.ShouldBe(CheckAccountType.Offline); // "offline" → enum
        account.Currency.ShouldBe("EUR");
        account.DefaultAccount.ShouldBe(true); // "1" → true
        account.Status.ShouldBe(100);
        account.Create.ShouldNotBeNull();
    }

    [Fact]
    public void CheckAccountTransaction_DeserializesRealApiResponse()
    {
        var json = """
        {
            "objects": [{
                "id": "330042",
                "checkAccount": {"id": "77001", "objectName": "CheckAccount"},
                "valueDate": "2026-02-19T00:00:00+01:00",
                "entryDate": "2026-02-19T00:00:00+01:00",
                "amount": "-250",
                "payeePayerName": "Lieferant GmbH",
                "paymtPurpose": "Rechnung RE-2026-0001",
                "status": "100",
                "create": "2026-02-19T14:13:11+01:00",
                "update": "2026-02-19T14:13:11+01:00"
            }],
            "total": "1"
        }
        """;

        var response = JsonSerializer.Deserialize(json, SevDeskJsonContext.Default.SevDeskApiListResponseApiCheckAccountTransaction);

        response.ShouldNotBeNull();
        response.Objects.ShouldNotBeNull();

        var tx = ModelMapper.ToPublic(response.Objects![0]);

        tx.Id.ShouldBe(330042);
        tx.CheckAccount.ShouldNotBeNull();
        tx.CheckAccount!.Id.ShouldBe(77001);
        tx.ValueDate.ShouldNotBeNull();
        tx.EntryDate.ShouldNotBeNull();
        tx.Amount.ShouldBe(-250m); // string "-250" → decimal
        tx.PayeeName.ShouldBe("Lieferant GmbH"); // payeePayerName → PayeeName
        tx.Purpose.ShouldBe("Rechnung RE-2026-0001"); // paymtPurpose → Purpose
        tx.Status.ShouldBe(100);
    }

    [Fact]
    public void Part_DeserializesRealApiResponse()
    {
        var json = """
        {
            "objects": [{
                "id": "44001",
                "name": "Beratungsleistung",
                "partNumber": "P-001",
                "text": "Beratung pro Stunde",
                "unity": {"id": "9", "objectName": "Unity"},
                "price": "49.99",
                "priceGross": "59.49",
                "priceNet": "49.99",
                "taxRate": "19",
                "internalComment": "Standardprodukt",
                "stockEnabled": "0",
                "stock": "100",
                "category": {"id": "5", "objectName": "Category"},
                "create": "2026-01-10T08:30:00+01:00",
                "update": "2026-02-19T14:13:11+01:00"
            }],
            "total": "1"
        }
        """;

        var response = JsonSerializer.Deserialize(json, SevDeskJsonContext.Default.SevDeskApiListResponseApiPart);

        response.ShouldNotBeNull();
        response.Objects.ShouldNotBeNull();

        var part = ModelMapper.ToPublic(response.Objects![0]);

        part.Id.ShouldBe(44001);
        part.Name.ShouldBe("Beratungsleistung");
        part.PartNumber.ShouldBe("P-001");
        part.Text.ShouldBe("Beratung pro Stunde");
        part.Unity.ShouldNotBeNull();
        part.Unity!.Id.ShouldBe(9);
        part.Price.ShouldBe(49.99m); // string "49.99" → decimal
        part.PriceGross.ShouldBe(59.49m);
        part.PriceNet.ShouldBe(49.99m);
        part.TaxRate.ShouldBe(19m); // string "19" → decimal
        part.InternalComment.ShouldBe("Standardprodukt");
        part.StockEnabled.ShouldBe(false); // "0" → false
        part.Stock.ShouldBe(100m); // string "100" → decimal
        part.Category.ShouldNotBeNull();
        part.Category!.Id.ShouldBe(5);
    }

    [Fact]
    public void TaxRule_DeserializesRealApiResponse()
    {
        var json = """
        {
            "objects": [{
                "id": "1",
                "objectName": "TaxRule",
                "name": "Standardregel Inland",
                "description": "Standardregelung fuer Inland",
                "code": "1",
                "countryClient": {"id": "1", "objectName": "StaticCountry"},
                "countryContactType": "default"
            }],
            "total": "1"
        }
        """;

        var response = JsonSerializer.Deserialize(json, SevDeskJsonContext.Default.SevDeskApiListResponseApiTaxRule);

        response.ShouldNotBeNull();
        response.Objects.ShouldNotBeNull();

        var rule = ModelMapper.ToPublic(response.Objects![0]);

        rule.Id.ShouldBe(1);
        rule.Name.ShouldBe("Standardregel Inland");
        rule.Description.ShouldBe("Standardregelung fuer Inland");
        rule.Code.ShouldBe("1");
        rule.CountryClient.ShouldNotBeNull();
        rule.CountryClient!.Id.ShouldBe(1);
        rule.CountryClient.ObjectName.ShouldBe("StaticCountry");
        rule.CountryContactType.ShouldBe("default");
    }

    [Fact]
    public void CurrencyExchangeRate_DeserializesRealApiResponse()
    {
        var json = """
        {
            "objects": [{
                "id": "42",
                "objectName": "CurrencyExchangeRate",
                "currency": "AED",
                "rate": "3.67281",
                "date": "2026-02-19T00:00:00+01:00"
            }],
            "total": "1"
        }
        """;

        var response = JsonSerializer.Deserialize(json, SevDeskJsonContext.Default.SevDeskApiListResponseApiCurrencyExchangeRate);

        response.ShouldNotBeNull();
        response.Objects.ShouldNotBeNull();

        var rate = ModelMapper.ToPublic(response.Objects![0]);

        rate.Id.ShouldBe(42);
        rate.Currency.ShouldBe("AED");
        rate.Rate.ShouldBe(3.67281m); // string "3.67281" → decimal
        rate.Date.ShouldNotBeNull();
    }

    [Fact]
    public void Category_DeserializesRealApiResponse()
    {
        var json = """
        {
            "objects": [{
                "id": "88",
                "name": "Kunde",
                "objectType": "Contact",
                "priority": "3",
                "code": "CUSTOMER",
                "type": "contact",
                "color": "#3498db",
                "translationCode": "CATEGORY_CUSTOMER",
                "create": "2026-01-01T00:00:00+01:00",
                "update": "2026-02-19T14:13:11+01:00"
            }],
            "total": "1"
        }
        """;

        var response = JsonSerializer.Deserialize(json, SevDeskJsonContext.Default.SevDeskApiListResponseApiCategory);

        response.ShouldNotBeNull();
        response.Objects.ShouldNotBeNull();

        var category = ModelMapper.ToPublic(response.Objects![0]);

        category.Id.ShouldBe(88);
        category.Name.ShouldBe("Kunde");
        category.ObjectType.ShouldBe("Contact");
        category.Priority.ShouldBe(3); // string "3" → int
        category.Code.ShouldBe("CUSTOMER");
        category.Type.ShouldBe("contact");
        category.Color.ShouldBe("#3498db");
        category.TranslationCode.ShouldBe("CATEGORY_CUSTOMER");
        category.Create.ShouldNotBeNull();
    }

    [Fact]
    public void Unity_DeserializesRealApiResponse()
    {
        var json = """
        {
            "objects": [{
                "id": "1",
                "name": "Stueck",
                "translationCode": "UNITY_PIECE",
                "unitySystem": "metric",
                "uneceTradeUnitCode": "C62",
                "create": "2026-01-01T00:00:00+01:00",
                "update": "2026-01-01T00:00:00+01:00"
            }],
            "total": "1"
        }
        """;

        var response = JsonSerializer.Deserialize(json, SevDeskJsonContext.Default.SevDeskApiListResponseApiUnity);

        response.ShouldNotBeNull();
        response.Objects.ShouldNotBeNull();

        var unity = ModelMapper.ToPublic(response.Objects![0]);

        unity.Id.ShouldBe(1);
        unity.Name.ShouldBe("Stueck");
        unity.TranslationCode.ShouldBe("UNITY_PIECE");
        unity.UnitySystem.ShouldBe("metric");
        unity.UneceTradeUnitCode.ShouldBe("C62");
        unity.Create.ShouldNotBeNull();
    }

    [Fact]
    public void InvoicePos_DeserializesRealApiResponse()
    {
        var json = """
        {
            "objects": [{
                "id": "556001",
                "invoice": {"id": "98765432", "objectName": "Invoice"},
                "part": {"id": "44001", "objectName": "Part"},
                "quantity": "3",
                "price": "49.99",
                "name": "Beratungsleistung",
                "unity": {"id": "1", "objectName": "Unity"},
                "taxRate": "19",
                "positionNumber": "1",
                "text": "Beratung pro Stunde",
                "discount": "0",
                "optional": "0",
                "sumNet": "149.97",
                "sumGross": "178.46",
                "sumTax": "28.49",
                "create": "2026-02-19T14:13:11+01:00",
                "update": "2026-02-19T14:13:11+01:00"
            }],
            "total": "1"
        }
        """;

        var response = JsonSerializer.Deserialize(json, SevDeskJsonContext.Default.SevDeskApiListResponseApiInvoicePos);

        response.ShouldNotBeNull();
        response.Objects.ShouldNotBeNull();

        var pos = ModelMapper.ToPublic(response.Objects![0]);

        pos.Id.ShouldBe(556001);
        pos.Invoice.ShouldNotBeNull();
        pos.Invoice!.Id.ShouldBe(98765432);
        pos.Part.ShouldNotBeNull();
        pos.Part!.Id.ShouldBe(44001);
        pos.Quantity.ShouldBe(3m);
        pos.Price.ShouldBe(49.99m);
        pos.Name.ShouldBe("Beratungsleistung");
        pos.Unity.ShouldNotBeNull();
        pos.Unity!.Id.ShouldBe(1);
        pos.TaxRate.ShouldBe(19m);
        pos.PositionNumber.ShouldBe(1);
        pos.Text.ShouldBe("Beratung pro Stunde");
        pos.Discount.ShouldBe(0m);
        pos.Optional.ShouldBe(false);
        pos.SumNet.ShouldBe(149.97m);
        pos.SumGross.ShouldBe(178.46m);
        pos.SumTax.ShouldBe(28.49m);
        pos.Create.ShouldNotBeNull();
    }

    [Fact]
    public void Order_DeserializesRealApiResponse()
    {
        var json = """
        {
            "objects": [{
                "id": "220001",
                "orderNumber": "AN-2026-0001",
                "contact": {"id": "12345678", "objectName": "Contact"},
                "orderDate": "2026-02-19T00:00:00+01:00",
                "status": "100",
                "orderType": "AN",
                "header": "Angebot",
                "headText": "Sehr geehrte Damen und Herren",
                "footText": "Mit freundlichen Gruessen",
                "contactPerson": {"id": "555", "objectName": "SevUser"},
                "address": "Test GmbH\nMusterstr. 1\n12345 Berlin",
                "currency": "EUR",
                "sumNet": "500.00",
                "sumGross": "595.00",
                "sumTax": "95.00",
                "taxType": "default",
                "taxRate": "19",
                "taxText": "Umsatzsteuer 19%",
                "sendDate": "2026-02-19T10:00:00+01:00",
                "deliveryDate": "2026-03-01T00:00:00+01:00",
                "smallSettlement": "0",
                "taxSet": {"id": "1", "objectName": "TaxSet"},
                "origin": {"id": "98765432", "objectName": "Invoice"},
                "customerInternalNote": "Eilauftrag",
                "create": "2026-02-19T14:13:11+01:00",
                "update": "2026-02-19T15:00:00+01:00"
            }],
            "total": "1"
        }
        """;

        var response = JsonSerializer.Deserialize(json, SevDeskJsonContext.Default.SevDeskApiListResponseApiOrder);

        response.ShouldNotBeNull();
        response.Objects.ShouldNotBeNull();

        var order = ModelMapper.ToPublic(response.Objects![0]);

        order.Id.ShouldBe(220001);
        order.OrderNumber.ShouldBe("AN-2026-0001");
        order.Contact.ShouldNotBeNull();
        order.Contact!.Id.ShouldBe(12345678);
        order.OrderDate.ShouldNotBeNull();
        order.Status.ShouldBe(OrderStatus.Draft);
        order.OrderType.ShouldBe(OrderType.AN);
        order.Header.ShouldBe("Angebot");
        order.HeadText.ShouldBe("Sehr geehrte Damen und Herren");
        order.FootText.ShouldBe("Mit freundlichen Gruessen");
        order.ContactPerson.ShouldNotBeNull();
        order.ContactPerson!.Id.ShouldBe(555);
        order.Address.ShouldBe("Test GmbH\nMusterstr. 1\n12345 Berlin");
        order.Currency.ShouldBe("EUR");
        order.SumNet.ShouldBe(500.00m);
        order.SumGross.ShouldBe(595.00m);
        order.SumTax.ShouldBe(95.00m);
        order.TaxType.ShouldBe("default");
        order.TaxRate.ShouldBe(19m);
        order.TaxText.ShouldBe("Umsatzsteuer 19%");
        order.SendDate.ShouldNotBeNull();
        order.DeliveryDate.ShouldNotBeNull();
        order.SmallSettlement.ShouldBe(false);
        order.TaxSet.ShouldNotBeNull();
        order.TaxSet!.Id.ShouldBe(1);
        order.Origin.ShouldNotBeNull();
        order.Origin!.Id.ShouldBe(98765432);
        order.CustomerInternalNote.ShouldBe("Eilauftrag");
        order.Create.ShouldNotBeNull();
        order.Update.ShouldNotBeNull();
    }

    [Fact]
    public void OrderPos_DeserializesRealApiResponse()
    {
        var json = """
        {
            "objects": [{
                "id": "660001",
                "order": {"id": "220001", "objectName": "Order"},
                "part": {"id": "44001", "objectName": "Part"},
                "quantity": "5",
                "price": "100.00",
                "name": "Projektplanung",
                "unity": {"id": "1", "objectName": "Unity"},
                "taxRate": "19",
                "positionNumber": "1",
                "text": "Projektplanung und Konzeption",
                "discount": "10",
                "optional": "1",
                "sumNet": "450.00",
                "sumGross": "535.50",
                "sumTax": "85.50",
                "create": "2026-02-19T14:13:11+01:00",
                "update": "2026-02-19T14:13:11+01:00"
            }],
            "total": "1"
        }
        """;

        var response = JsonSerializer.Deserialize(json, SevDeskJsonContext.Default.SevDeskApiListResponseApiOrderPos);

        response.ShouldNotBeNull();
        response.Objects.ShouldNotBeNull();

        var pos = ModelMapper.ToPublic(response.Objects![0]);

        pos.Id.ShouldBe(660001);
        pos.Order.ShouldNotBeNull();
        pos.Order!.Id.ShouldBe(220001);
        pos.Part.ShouldNotBeNull();
        pos.Part!.Id.ShouldBe(44001);
        pos.Quantity.ShouldBe(5m);
        pos.Price.ShouldBe(100.00m);
        pos.Name.ShouldBe("Projektplanung");
        pos.Unity.ShouldNotBeNull();
        pos.Unity!.Id.ShouldBe(1);
        pos.TaxRate.ShouldBe(19m);
        pos.PositionNumber.ShouldBe(1);
        pos.Text.ShouldBe("Projektplanung und Konzeption");
        pos.Discount.ShouldBe(10m);
        pos.Optional.ShouldBe(true); // "1" → true
        pos.SumNet.ShouldBe(450.00m);
        pos.SumGross.ShouldBe(535.50m);
        pos.SumTax.ShouldBe(85.50m);
        pos.Create.ShouldNotBeNull();
    }

    [Fact]
    public void Voucher_DeserializesRealApiResponse()
    {
        var json = """
        {
            "objects": [{
                "id": "880001",
                "voucherDate": "2026-02-15T00:00:00+01:00",
                "supplier": {"id": "12345678", "objectName": "Contact"},
                "status": "100",
                "voucherType": "VOU",
                "description": "Bueromaterial Februar",
                "payDate": "2026-03-01T00:00:00+01:00",
                "currency": "EUR",
                "sumNet": "42.01",
                "sumGross": "49.99",
                "sumTax": "7.98",
                "taxType": "default",
                "creditDebit": "D",
                "document": {"id": "990001", "objectName": "Document"},
                "costCentre": {"id": "10", "objectName": "CostCentre"},
                "paidAmount": "0",
                "taxSet": {"id": "1", "objectName": "TaxSet"},
                "create": "2026-02-15T09:00:00+01:00",
                "update": "2026-02-19T14:13:11+01:00"
            }],
            "total": "1"
        }
        """;

        var response = JsonSerializer.Deserialize(json, SevDeskJsonContext.Default.SevDeskApiListResponseApiVoucher);

        response.ShouldNotBeNull();
        response.Objects.ShouldNotBeNull();

        var voucher = ModelMapper.ToPublic(response.Objects![0]);

        voucher.Id.ShouldBe(880001);
        voucher.VoucherDate.ShouldNotBeNull();
        voucher.Supplier.ShouldNotBeNull();
        voucher.Supplier!.Id.ShouldBe(12345678);
        voucher.Status.ShouldBe(VoucherStatus.Unpaid);
        voucher.VoucherType.ShouldBe(VoucherType.VOU);
        voucher.Description.ShouldBe("Bueromaterial Februar");
        voucher.PayDate.ShouldNotBeNull();
        voucher.Currency.ShouldBe("EUR");
        voucher.SumNet.ShouldBe(42.01m);
        voucher.SumGross.ShouldBe(49.99m);
        voucher.SumTax.ShouldBe(7.98m);
        voucher.TaxType.ShouldBe("default");
        voucher.CreditDebit.ShouldBe("D");
        voucher.Document.ShouldNotBeNull();
        voucher.Document!.Id.ShouldBe(990001);
        voucher.CostCentre.ShouldNotBeNull();
        voucher.CostCentre!.Id.ShouldBe(10);
        voucher.PaidAmount.ShouldBe(0m);
        voucher.TaxSet.ShouldNotBeNull();
        voucher.TaxSet!.Id.ShouldBe(1);
        voucher.Create.ShouldNotBeNull();
        voucher.Update.ShouldNotBeNull();
    }

    [Fact]
    public void VoucherPos_DeserializesRealApiResponse()
    {
        var json = """
        {
            "objects": [{
                "id": "770001",
                "voucher": {"id": "880001", "objectName": "Voucher"},
                "accountingType": {"id": "26", "objectName": "AccountingType"},
                "estimatedAccountingType": {"id": "26", "objectName": "AccountingType"},
                "net": "42.01",
                "taxRate": "19",
                "isAsset": "0",
                "sumNet": "42.01",
                "sumGross": "49.99",
                "sumTax": "7.98",
                "comment": "Bueromaterial",
                "create": "2026-02-15T09:00:00+01:00",
                "update": "2026-02-19T14:13:11+01:00"
            }],
            "total": "1"
        }
        """;

        var response = JsonSerializer.Deserialize(json, SevDeskJsonContext.Default.SevDeskApiListResponseApiVoucherPos);

        response.ShouldNotBeNull();
        response.Objects.ShouldNotBeNull();

        var pos = ModelMapper.ToPublic(response.Objects![0]);

        pos.Id.ShouldBe(770001);
        pos.Voucher.ShouldNotBeNull();
        pos.Voucher!.Id.ShouldBe(880001);
        pos.AccountingType.ShouldNotBeNull();
        pos.AccountingType!.Id.ShouldBe(26);
        pos.EstimatedAccountingType.ShouldNotBeNull();
        pos.EstimatedAccountingType!.Id.ShouldBe(26);
        pos.Net.ShouldBe(42.01m);
        pos.TaxRate.ShouldBe(19m);
        pos.IsAsset.ShouldBe(false); // "0" → false
        pos.SumNet.ShouldBe(42.01m);
        pos.SumGross.ShouldBe(49.99m);
        pos.SumTax.ShouldBe(7.98m);
        pos.Comment.ShouldBe("Bueromaterial");
        pos.Create.ShouldNotBeNull();
        pos.Update.ShouldNotBeNull();
    }

    [Fact]
    public void CreditNote_DeserializesRealApiResponse()
    {
        var json = """
        {
            "objects": [{
                "id": "330001",
                "creditNoteNumber": "GS-2026-0001",
                "contact": {"id": "12345678", "objectName": "Contact"},
                "creditNoteDate": "2026-02-19T00:00:00+01:00",
                "status": "200",
                "header": "Gutschrift",
                "headText": "Wir erstatten Ihnen folgenden Betrag",
                "footText": "Vielen Dank fuer Ihr Verstaendnis",
                "contactPerson": {"id": "555", "objectName": "SevUser"},
                "address": "Test GmbH\nMusterstr. 1\n12345 Berlin",
                "currency": "EUR",
                "sumNet": "84.02",
                "sumGross": "99.98",
                "sumTax": "15.96",
                "taxType": "default",
                "taxRate": "19",
                "taxText": "Umsatzsteuer 19%",
                "taxSet": {"id": "1", "objectName": "TaxSet"},
                "sendDate": "2026-02-19T10:00:00+01:00",
                "smallSettlement": "0",
                "create": "2026-02-19T14:13:11+01:00",
                "update": "2026-02-19T15:00:00+01:00"
            }],
            "total": "1"
        }
        """;

        var response = JsonSerializer.Deserialize(json, SevDeskJsonContext.Default.SevDeskApiListResponseApiCreditNote);

        response.ShouldNotBeNull();
        response.Objects.ShouldNotBeNull();

        var cn = ModelMapper.ToPublic(response.Objects![0]);

        cn.Id.ShouldBe(330001);
        cn.CreditNoteNumber.ShouldBe("GS-2026-0001");
        cn.Contact.ShouldNotBeNull();
        cn.Contact!.Id.ShouldBe(12345678);
        cn.CreditNoteDate.ShouldNotBeNull();
        cn.Status.ShouldBe(CreditNoteStatus.Open);
        cn.Header.ShouldBe("Gutschrift");
        cn.HeadText.ShouldBe("Wir erstatten Ihnen folgenden Betrag");
        cn.FootText.ShouldBe("Vielen Dank fuer Ihr Verstaendnis");
        cn.ContactPerson.ShouldNotBeNull();
        cn.ContactPerson!.Id.ShouldBe(555);
        cn.Address.ShouldBe("Test GmbH\nMusterstr. 1\n12345 Berlin");
        cn.Currency.ShouldBe("EUR");
        cn.SumNet.ShouldBe(84.02m);
        cn.SumGross.ShouldBe(99.98m);
        cn.SumTax.ShouldBe(15.96m);
        cn.TaxType.ShouldBe("default");
        cn.TaxRate.ShouldBe(19m);
        cn.TaxText.ShouldBe("Umsatzsteuer 19%");
        cn.TaxSet.ShouldNotBeNull();
        cn.TaxSet!.Id.ShouldBe(1);
        cn.SendDate.ShouldNotBeNull();
        cn.SmallSettlement.ShouldBe(false);
        cn.Create.ShouldNotBeNull();
        cn.Update.ShouldNotBeNull();
    }

    [Fact]
    public void CreditNotePos_DeserializesRealApiResponse()
    {
        var json = """
        {
            "objects": [{
                "id": "440001",
                "creditNote": {"id": "330001", "objectName": "CreditNote"},
                "part": {"id": "44001", "objectName": "Part"},
                "quantity": "2",
                "price": "49.99",
                "name": "Beratungsleistung",
                "unity": {"id": "1", "objectName": "Unity"},
                "taxRate": "19",
                "positionNumber": "1",
                "text": "Erstattung Beratung",
                "discount": "0",
                "optional": "0",
                "sumNet": "99.98",
                "sumGross": "118.98",
                "sumTax": "19.00",
                "create": "2026-02-19T14:13:11+01:00",
                "update": "2026-02-19T14:13:11+01:00"
            }],
            "total": "1"
        }
        """;

        var response = JsonSerializer.Deserialize(json, SevDeskJsonContext.Default.SevDeskApiListResponseApiCreditNotePos);

        response.ShouldNotBeNull();
        response.Objects.ShouldNotBeNull();

        var pos = ModelMapper.ToPublic(response.Objects![0]);

        pos.Id.ShouldBe(440001);
        pos.CreditNote.ShouldNotBeNull();
        pos.CreditNote!.Id.ShouldBe(330001);
        pos.Part.ShouldNotBeNull();
        pos.Part!.Id.ShouldBe(44001);
        pos.Quantity.ShouldBe(2m);
        pos.Price.ShouldBe(49.99m);
        pos.Name.ShouldBe("Beratungsleistung");
        pos.Unity.ShouldNotBeNull();
        pos.Unity!.Id.ShouldBe(1);
        pos.TaxRate.ShouldBe(19m);
        pos.PositionNumber.ShouldBe(1);
        pos.Text.ShouldBe("Erstattung Beratung");
        pos.Discount.ShouldBe(0m);
        pos.Optional.ShouldBe(false);
        pos.SumNet.ShouldBe(99.98m);
        pos.SumGross.ShouldBe(118.98m);
        pos.SumTax.ShouldBe(19.00m);
        pos.Create.ShouldNotBeNull();
    }

    [Fact]
    public void CommunicationWay_DeserializesRealApiResponse()
    {
        var json = """
        {
            "objects": [{
                "id": "112001",
                "contact": {"id": "12345678", "objectName": "Contact"},
                "type": "EMAIL",
                "value": "max@test-gmbh.de",
                "key": {"id": "1", "objectName": "CommunicationWayKey"},
                "main": "1",
                "create": "2026-02-19T14:13:11+01:00",
                "update": "2026-02-19T14:13:11+01:00"
            }],
            "total": "1"
        }
        """;

        var response = JsonSerializer.Deserialize(json, SevDeskJsonContext.Default.SevDeskApiListResponseApiCommunicationWay);

        response.ShouldNotBeNull();
        response.Objects.ShouldNotBeNull();

        var cw = ModelMapper.ToPublic(response.Objects![0]);

        cw.Id.ShouldBe(112001);
        cw.Contact.ShouldNotBeNull();
        cw.Contact!.Id.ShouldBe(12345678);
        cw.Type.ShouldBe(CommunicationWayType.EMAIL);
        cw.Value.ShouldBe("max@test-gmbh.de");
        cw.Key.ShouldNotBeNull();
        cw.Key!.Id.ShouldBe(1);
        cw.Main.ShouldBe(true); // "1" → true
        cw.Create.ShouldNotBeNull();
        cw.Update.ShouldNotBeNull();
    }

    [Fact]
    public void ContactAddress_DeserializesRealApiResponse()
    {
        var json = """
        {
            "objects": [{
                "id": "223001",
                "contact": {"id": "12345678", "objectName": "Contact"},
                "street": "Musterstr. 1",
                "zip": "12345",
                "city": "Berlin",
                "country": {"id": "1", "objectName": "StaticCountry"},
                "category": {"id": "43", "objectName": "Category"},
                "name": "Test GmbH",
                "name2": "Abteilung Einkauf",
                "name3": "z.Hd. Max Mustermann",
                "name4": "Gebaeude B",
                "create": "2026-02-19T14:13:11+01:00",
                "update": "2026-02-19T15:00:00+01:00"
            }],
            "total": "1"
        }
        """;

        var response = JsonSerializer.Deserialize(json, SevDeskJsonContext.Default.SevDeskApiListResponseApiContactAddress);

        response.ShouldNotBeNull();
        response.Objects.ShouldNotBeNull();

        var addr = ModelMapper.ToPublic(response.Objects![0]);

        addr.Id.ShouldBe(223001);
        addr.Contact.ShouldNotBeNull();
        addr.Contact!.Id.ShouldBe(12345678);
        addr.Street.ShouldBe("Musterstr. 1");
        addr.Zip.ShouldBe("12345");
        addr.City.ShouldBe("Berlin");
        addr.Country.ShouldNotBeNull();
        addr.Country!.Id.ShouldBe(1);
        addr.Country.ObjectName.ShouldBe("StaticCountry");
        addr.Category.ShouldNotBeNull();
        addr.Category!.Id.ShouldBe(43);
        addr.Name.ShouldBe("Test GmbH");
        addr.Name2.ShouldBe("Abteilung Einkauf");
        addr.Name3.ShouldBe("z.Hd. Max Mustermann");
        addr.Name4.ShouldBe("Gebaeude B");
        addr.Create.ShouldNotBeNull();
        addr.Update.ShouldNotBeNull();
    }

    [Fact]
    public void Tag_DeserializesRealApiResponse()
    {
        var json = """
        {
            "objects": [{
                "id": "55001",
                "name": "Wichtig",
                "object": {"id": "12345678", "objectName": "Contact"},
                "objectType": "Contact",
                "create": "2026-02-19T14:13:11+01:00"
            }],
            "total": "1"
        }
        """;

        var response = JsonSerializer.Deserialize(json, SevDeskJsonContext.Default.SevDeskApiListResponseApiTag);

        response.ShouldNotBeNull();
        response.Objects.ShouldNotBeNull();

        var tag = ModelMapper.ToPublic(response.Objects![0]);

        tag.Id.ShouldBe(55001);
        tag.Name.ShouldBe("Wichtig");
        tag.Object.ShouldNotBeNull();
        tag.Object!.Id.ShouldBe(12345678);
        tag.Object.ObjectName.ShouldBe("Contact");
        tag.ObjectType.ShouldBe("Contact");
        tag.Create.ShouldNotBeNull();
    }

    [Fact]
    public void Document_DeserializesRealApiResponse()
    {
        var json = """
        {
            "objects": [{
                "id": "990001",
                "filename": "rechnung-2026-0042.pdf",
                "extension": "pdf",
                "size": "154832",
                "mimeType": "application/pdf",
                "object": {"id": "98765432", "objectName": "Invoice"},
                "folder": {"id": "7", "objectName": "DocumentFolder"},
                "status": "100",
                "create": "2026-02-19T14:13:11+01:00",
                "update": "2026-02-19T14:13:11+01:00"
            }],
            "total": "1"
        }
        """;

        var response = JsonSerializer.Deserialize(json, SevDeskJsonContext.Default.SevDeskApiListResponseApiDocument);

        response.ShouldNotBeNull();
        response.Objects.ShouldNotBeNull();

        var doc = ModelMapper.ToPublic(response.Objects![0]);

        doc.Id.ShouldBe(990001);
        doc.Filename.ShouldBe("rechnung-2026-0042.pdf");
        doc.Extension.ShouldBe("pdf");
        doc.Size.ShouldBe(154832);
        doc.MimeType.ShouldBe("application/pdf");
        doc.Object.ShouldNotBeNull();
        doc.Object!.Id.ShouldBe(98765432);
        doc.Object.ObjectName.ShouldBe("Invoice");
        doc.Folder.ShouldNotBeNull();
        doc.Folder!.Id.ShouldBe(7);
        doc.Folder.ObjectName.ShouldBe("DocumentFolder");
        doc.Status.ShouldBe(100);
        doc.Create.ShouldNotBeNull();
        doc.Update.ShouldNotBeNull();
    }

    [Fact]
    public void ListResponse_DeserializesStringTotal()
    {
        var json = """
        {
            "objects": [
                {"id": "1", "surename": "Max", "familyname": "Mustermann", "status": "100"}
            ],
            "total": "1"
        }
        """;

        var response = JsonSerializer.Deserialize(json, SevDeskJsonContext.Default.SevDeskApiListResponseApiContact);

        response.ShouldNotBeNull();
        response.Total.ShouldBe(1); // string "1" → int via AllowReadingFromString
        response.Objects.ShouldNotBeNull();
        response.Objects!.Count.ShouldBe(1);
    }
}
