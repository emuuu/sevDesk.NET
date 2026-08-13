using Microsoft.Extensions.DependencyInjection;
using sevDesk.NET;

// Setup DI
var services = new ServiceCollection();

services.AddSevDesk(options =>
{
    options.ApiToken = "YOUR_SEVDESK_API_TOKEN";
});

await using var provider = services.BuildServiceProvider();

var client = provider.GetRequiredService<ISevDeskClient>();

// -- Contact Example --
Console.WriteLine("=== sevDesk Contact Example ===");
Console.WriteLine("Listing contacts...");
Console.WriteLine("(This would call the sevDesk API in a real scenario)");

try
{
    // Uncomment to call the sevDesk API:
    // var contacts = await client.Contacts.ListAsync();
    // foreach (var contact in contacts.Items)
    // {
    //     Console.WriteLine($"  {contact.Id}: {contact.Surename} {contact.Familyname} ({contact.Name})");
    // }
    // // Total is int? — null when the API reported no total, which is not the same as 0.
    // Console.WriteLine(contacts.Total is int total ? $"Total: {total}" : "Total: not reported");
}
catch (sevDesk.NET.Exceptions.SevDeskException ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}

// -- Invoice Example --
Console.WriteLine();
Console.WriteLine("=== sevDesk Invoice Example ===");
Console.WriteLine("Creating an invoice...");
Console.WriteLine("(This would call the sevDesk API in a real scenario)");

try
{
    // Uncomment to call the sevDesk API:
    // var invoice = await client.Invoices.SaveInvoiceAsync(
    //     new sevDesk.NET.Models.Invoice
    //     {
    //         Contact = new sevDesk.NET.Models.SevDeskObjectReference { Id = 123, ObjectName = "Contact" },
    //         InvoiceDate = DateTime.Now,
    //         Header = "Test Invoice",
    //         TaxType = "default",
    //         Currency = "EUR"
    //     },
    //     new[]
    //     {
    //         new sevDesk.NET.Models.InvoicePos
    //         {
    //             Name = "Consulting",
    //             Quantity = 10,
    //             Price = 150.00m,
    //             TaxRate = 19,
    //             Unity = new sevDesk.NET.Models.SevDeskObjectReference { Id = 1, ObjectName = "Unity" }
    //         }
    //     });
    // Console.WriteLine($"Invoice created: {invoice.InvoiceNumber}");
}
catch (sevDesk.NET.Exceptions.SevDeskException ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}

Console.WriteLine();
Console.WriteLine("Setup complete. Replace API token and uncomment API calls to use.");
