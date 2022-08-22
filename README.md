# printnode-net
A C# library for communicating with the PrintNode API

## Getting started
Let's set up your account first.

Create an API key at printnode.com.

Install your PrintNode client to the computer of your choice.

Open the PrintNode client and get the printer ID. Now let's "Hello, world!":

```csharp
// Your PrintNode API key is stored in apiKey
// Your PrintNode printer id is stored in printerId
// Your awesome test PDF is stored as a byte[] in pdfDocumentBytes

PrintNodeConfiguration.ApiKey = apiKey;
var printer = await PrintNodePrinter.GetAsync(printerId);
var printJob = new PrintNodePrintJob
{
    Title = "Hello, world!",
    Content = Convert.ToBase64String(pdfDocumentBytes),
    ContentType = "pdf_base64"
};

await printer.AddPrintJob(printJob);
```

## The API base address can be changed if neccessary (default is https://api.printnode.com)
```csharp
PrintNodeConfiguration.BaseAddress = new Uri("https://companyname-api.printnode.com");
```

## You can set up several computers each with their own PrintNode client, and register them to the same PrintNode account.

```csharp
var computers = await PrintNodeComputer.ListAsync();
```

```json
[
  {
    "id": 11,
    "name": "AnalyticalEngine",
    "inet": null,
    "inet6": null,
    "hostname": null,
    "version": null,
    "jre": null,
    "createTimestamp": "2015-06-28T18:29:19.871Z",
    "state": "disconnected"
  }
]
```

## To get a specific computer
```csharp
var computerId = 12777;
var computer = await PrintNodeComputer.GetAsync(computerId);
```

## To get a list of printers connected to your current account
```csharp
var printers = await PrintNodePrinter.ListAsync();
```

## To get a specific printer
```csharp
var printerId = 38409;
var printer = await PrintNodePrinter.GetAsync(printerId);
```

## To add a job to a printer
```csharp
byte[] pdfDocument = await DownloadPdf("http://test.com/test.pdf");

var printJob = new PrintNodePrintJob
{
    Title = "My cool test print",
    Content = Convert.ToBase64String(pdfDocument),
    ContentType = "pdf_base64"
};

var response = await printer.AddPrintJob(printJob);
```

## To list print jobs for a printer
```csharp
var printerId = 38409;
var printJobs = await PrintNodePrintJob.ListForPrinterAsync(printerId);
```

## To create a child account
```csharp
var childAccount = new PrintNodeChildAccount
{
	FirstName = "First name",
	LastName = "Last name",
	Email = "email@test.com",
	Password = "password",
	CreatorRef = "a cool ref"
};

var response = await childAccount.CreateAsync();

var accountId = response.Id.Value;
```

## To get a list of printers on behalf of a child account
```csharp
using (new PrintNodeDelegatedClientContext(accountId))
{
	var printers = PrintNodePrinter.ListAsync();
}
```