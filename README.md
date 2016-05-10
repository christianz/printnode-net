# printnode-net
A C# library for communicating with the PrintNode API

## Getting started
Let's set up your account first.

Create an API key at printnode.com.

Add the following to your `<appsettings` :
```xml
<add key="PrintNodeApiKey" value="<your API key>"/>
```

Install your PrintNode client to the computer of your choice.

Let's make sure your computer has been registered:

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
    ContentType = "raw_pdf"
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