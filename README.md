# Lionbridge onDemand Client
This library is a client SDK which provides a Service wrapper and Object-Oriented mechanism for communicating with the Lionbridge onDemand Content API web service.

## About the Lionbridge Content API
The Lionbridge Content API is a RESTful web service which provides file translation by Lionbridge Technologies, Inc, the world's largest localization firm.  

The API supports all translation quality levels and the broadest range of content types. You can write to a single API for all of your translation needs.

In order to get an account to use the API (a prerequisite for using this SDK) or to learn more, please go to the [Lionbridge Developers Site](http://developers.lionbridge.com/content/).


## Usage

### Prerequisites

In order to use the Client library, you must first sign up for a developer's account on the [Lionbridge Sandbox](https://developer-sandbox.liondemand.com/accounts/register/).

You will then need to use the sandbox endpoint of [https://developer-sandbox.liondemand.com](https://developer-sandbox.liondemand.com) (or your site specific endpoint if you have an enteprise onDemand account) and retrieve your Account Key Id and Account Secret Key.

### Instantiating the ContentAPI

```cs
// Construct the client with your connection information and default currency
IContentAPI client = new ContentAPI(keyId: "yourKeyId", 
                                    secretKey: "yourSecretKey", 
                                    endpoint: new Uri("https://developer-sandbox.liondemand.com"),
                                    defaultCurrency: "USD");
```
You can now use the Content API as a proxy to the service described on the [Lionbridge Content API Docs](http://developers.lionbridge.com/content/docs/).

### Getting the Available Services

```cs
IEnumerable<Service> services = client.ListServices();
```

If you know the name of the service you want, you could get it from the list like this:
```cs
Service service = services.SingleOrDefault(p => p.Name == "Put Service Name Here");
```

### Creating a Quote with Files

```cs
// Decide on Source and Target languages
string sourceLanguage = "en-gb";
string[] targetLanguages = { "fr-fr", "de-de" };

// Get the files that will be processed by the service
String[] filePaths = new String[] { "C:\filepath\file1.ext", "C:\filepath\file2.ext" };

// Tell the service to generate a quote with these files
Quote quote = service.GenerateQuote(filePaths, new QuoteOptions(sourceLanguage, targetLanguages));
```

> Files can be added by file path and will be loaded from disk, or they can be loaded into memory and passed in as `byte[]`.

### Creating a Quote by Creating Projects

```cs
// Decide on Source and Target languages
string sourceLanguage = "en-gb";
string[] targetLanguages = { "fr-fr", "de-de" };

// Get the files that will be part of the project
String[] filePaths = new String[] { "C:\filepath\file1.ext", "C:\filepath\file2.ext" };

// Tell the service to create a project with these files
Project project = service.AddProject("Project Name", filePaths, new ProjectOptions(sourceLanguage, targetLanguages));

// Use the client to generate a quote with this project (can be used with multiple projects)
Quote quote = client.GenerateQuote(new Project[] { project }, new ProjectQuoteOptions());
```
> Files can be added by file path and will be loaded from disk, or they can be loaded into memory and passed in as `byte[]`.

### Creating a Quote by Adding Files, then Create Projects

```cs
// Decide on Source and Target languages
string sourceLanguage = "en-gb";
string[] targetLanguages = { "fr-fr", "de-de" };

// Add the files that will be part of the project

File file1 = client.AddFile(sourceLanguage, "C:\filepath\file1.ext");
File file2 = client.AddFile(sourceLanguage, "C:\filepath\file2.ext");

// Tell the service to create a project with these files
Project project = service.AddProject("Project Name", new File[] {file1, file2}, new ProjectOptions(sourceLanguage, targetLanguages));

// Use the client to generate a quote with this project (can be used with multiple projects)
Quote quote = client.GenerateQuote(new Project[] { project }, new ProjectQuoteOptions());
```
> Files can be added by file path and will be loaded from disk, or they can be loaded into memory and passed in as `byte[]`.

### Creating a Quote with Products

```cs
// Create a list of products to translate
var products = new List<Product>();

// Create a product with 2 SKUs and a complex Description
products.Add(
    new Product(
        title: "Hermes Women's Satchel",
        primaryCategory: 1,
        topLevelCategory: 1,
        categoryPath: "Accessories : Womens : Handbags",
        skus: new SKU[] { 
            new SKU(skuNumber: "44488", 
                    itemSpecifics: new Dictionary<String, String>() {
                        {"Color", "White"},
                        {"Size", "Large"}
                    }
            ),
            new SKU(skuNumber: "44489", 
                    itemSpecifics: new Dictionary<String, String>() {
                        {"Color", "Red"},
                        {"Size", "Large"}
                    }
            )
        },
        description: new ProductDescription(arbitraryElements: new XElement[] {
                    XElement.Parse("<Main>Arbitrary Data</Main>")},
                features: new Dictionary<String, String> {
                    {"Cost", "14000"},
                    {"Country", "France"}
                },
                summary: @"This is an Hermes Fjord leather Birkin in 35cm. Very nice feel. Highly recommended.
                            Material: Vachette Crispe Fjord
                            Includes: keys, lock, clochette, dust bag.
                            Blind stamp : Square A Measurements: [do-not-translate]W:35cm(15.74 inches)x H:30cm 
                            (11.81 inches) x D:21cm (8.26 inches)[/do-not-translate]"
        )
    )
);

// Decide on Source and Target languages
string sourceLanguage = "en-gb";
string[] targetLanguages = { "fr-fr", "de-de" };

// Ask the Service that was chosen to generate a quote for these products with these langauges
Quote quote = service.GenerateQuote(products, new QuoteOptions(sourceLanguage, targetLanguages));
```
### Authorizing a Quote

```cs
// The quote may return but still be calculating... use this method to wait for it to be done calculating, or timeout
// Alternatively, call quote.Update() later and check the Status
quote.WaitWhileCalculating(timeoutInSeconds: 60);

if (quote.Status != "Pending")
{
    // The quote didn't calculate in the time alloted
    // TODO: Error handling here
    return;
}

// Authorize the quote (assuming you've looked at it and approve of it)
QuoteAuthorization quoteAuthorization = quote.Authorize();
``` 
> Alternatively, a Quote can be rejected by calling `quote.Reject()`

### Polling for Completed Files

```cs
// Given the projects on the Quote Authorization, either save them to check on them later (using client.GetProject()), 
// or as here, keep checking their status until they are Complete
foreach (Project project in quoteAuthorization.Projects)
{
    project.Update();

    while (project.Status != "Complete")
    {
        Thread.Sleep(TimeSpan.FromSeconds(30));

        // Update the project to update the Status
        project.Update();
    }

    // The project is now complete.  Get the File translations
    foreach (File file in project.Files)
    {
        // Iterate all Translations for the File
        foreach (KeyValuePair<String, byte[]> translationPair in file.AllTranslations)
        {
            string targetLanguage = translationPair.Key;
            byte[] fileBytes = translationPair.Value;

            // Do whatever you want with the results of the translation
            // ---
             System.IO.File.WriteAllBytes(Path.GetFileNameWithoutExtension(file.Name) + "-" + targetLanguage + Path.GetExtension(file.Name), fileBytes);
            
            // ---

        }
    }
}
```

### Polling for Completed Products

```cs
// Given the projects on the Quote Authorization, either save them to check on them later (using client.GetProject()), 
// or as here, keep checking their status until they are Complete
foreach (Project project in quoteAuthorization.Projects)
{
    project.Update();

    while (project.Status != "Complete")
    {
        Thread.Sleep(TimeSpan.FromSeconds(30));

        // Update the project to update the Status
        project.Update();
    }

    // The project is now complete.  Get the Product translations
    foreach (Product product in project.Products)
    {
        // Iterate all Translations for the Product
        foreach (KeyValuePair<String, ProductTranslation> translationPair in product.AllTranslations)
        {
            var productTranslation = translationPair.Value;

            // Do whatever you want with the results of the translation
            // ---
            Console.WriteLine(productTranslation.Title);
            Console.WriteLine(productTranslation.Description.Summary);
                        
            foreach (String featureKey in productTranslation.Description.Features.Keys)
            {
                Console.WriteLine("Feature:" + featureKey + ", " + productTranslation.Description.Features[featureKey]);
            }

            foreach (XElement element in productTranslation.Description.ArbitraryElements)
            {
                Console.WriteLine(element.ToString());
            }
            // ---

            // Alternatively, you could call productTranslation.GetTranslatedFields() to get a String of XML that represents the translation
        }
    }
}
```

## Contributing

To contribute fixes or additions to this project contact 

Daniel Spector, Software Engineering Director, Lionbridge Technologies, Inc.

[daniel.spector@lionbridge.com](mailto:daniel.spector@lionbridge.com)

## License

Provided under the MIT License.

```cs
/* Copyright (c) 2015 Lionbridge Technologies, Inc.
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), 
 * to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, 
 * and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, 
 * WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 * 
 * */
```

## Trademarks

The trademarks, logos, and service marks (collectively “Trademarks”) appearing on any Lionbridge website (and in particular, included in the  Lionbridge Content API) are the property of Lionbridge Technologies, Inc. and other parties. Nothing contained on the Lionbridge website should be construed as granting any license or right to use any Trademark without the prior written permission of the party that owns the Trademark. In particular, Lionbridge onDemand™, onDemand™,  the Lionbridge onDemand logo, Lionbridge™ and the Lionbridge™ logo are trademarks of Lionbridge Technologies, Inc. Lionbridge® and the Lionbridge® logo are registered trademarks of Lionbridge Technologies, Inc. in the United States and several non-US countries.