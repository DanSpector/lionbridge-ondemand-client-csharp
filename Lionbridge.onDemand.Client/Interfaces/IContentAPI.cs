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

using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Lionbridge.onDemand.Client
{
    /// <summary>
    /// Interface for classes implementing the onDemand Client API
    /// </summary>
    public interface IContentAPI
    {
        /// <summary>
        /// This interface adds a file to the system.
        /// </summary>
        /// <remarks>Lionbridge onDemand will attempt to detect the language. 
        /// Files are then used to generate quotes. If a file is not used in a quote within 1 hour of it being uploaded it will be deleted from the system. Lionbridge onDemand has a general retention policy of 60 days for all customer content.
        /// Files can only be associated with one project. If you need to translate a file into additional languages, you can upload it again and create a new project out of it.</remarks>
        /// <param name="filePath">The path to the file to add</param>
        /// <returns>A File object or null if the file could not be created.</returns>
        File AddFile(String filePath);

        /// <summary>
        /// This interface adds a file to the system.
        /// </summary>
        /// <remarks>Files are then used to generate quotes. If a file is not used in a quote within 1 hour of it being uploaded it will be deleted from the system. Lionbridge onDemand has a general retention policy of 60 days for all customer content.
        /// Files can only be associated with one project. If you need to translate a file into additional languages, you can upload it again and create a new project out of it.</remarks>
        /// <param name="languageCode">A locale code in the format en-us where EN is the 2 character ISO language code and US is the 2 character ISO country code.</param>
        /// <param name="filePath">The path to the file to add</param>
        /// <returns>A File object or null if the file could not be created.</returns>
        File AddFile(String languageCode, String filePath);

        /// <summary>
        /// This interface adds a file to the system.
        /// </summary>
        /// <remarks>Lionbridge onDemand will attempt to detect the language. 
        /// Files are then used to generate quotes. If a file is not used in a quote within 1 hour of it being uploaded it will be deleted from the system. Lionbridge onDemand has a general retention policy of 60 days for all customer content.
        /// Files can only be associated with one project. If you need to translate a file into additional languages, you can upload it again and create a new project out of it.</remarks>
        /// <param name="fileName">The original name of the file.</param>
        /// <param name="fileData">The file contents</param>
        /// <returns>A File object or null if the file could not be created.</returns>
        File AddFile(String fileName, Byte[] fileData);

        /// <summary>
        /// This interface adds a file to the system.
        /// </summary>
        /// <remarks>Files are then used to generate quotes. If a file is not used in a quote within 1 hour of it being uploaded it will be deleted from the system. Lionbridge onDemand has a general retention policy of 60 days for all customer content.
        /// Files can only be associated with one project. If you need to translate a file into additional languages, you can upload it again and create a new project out of it.</remarks>
        /// <param name="languageCode">A locale code in the format en-us where EN is the 2 character ISO language code and US is the 2 character ISO country code.</param>
        /// <param name="fileName">The original name of the file.</param>
        /// <param name="fileData">The file contents</param>
        /// <returns>A File object or null if the file could not be created.</returns>
        File AddFile(String languageCode, String fileName, Byte[] fileData);

        /// <summary>
        /// This interface adds a file to onDemand by providing an external URL that onDemand can download from. This is a good alternative to the Add File API for cases when the files are very large.
        /// </summary>
        /// <remarks>Lionbridge onDemand will attempt to detect the language. 
        /// Files are then used to generate quotes. If a file is not used in a quote within 1 hour of it being uploaded it will be deleted from the system. Lionbridge onDemand has a general retention policy of 60 days for all customer content.
        /// Files can only be associated with one project. If you need to translate a file into additional languages, you can upload it again and create a new project out of it.</remarks>
        /// <param name="fileName">The original name of the file.</param>
        /// <param name="fileURL">The full URL to the file. The URL must be publicly accessible. It can use use http, https, ftp, or ftps. If the URL requires authentication, credentials must be passed in the URL.></param>
        /// <returns>A File object or null if the file could not be created.</returns>
        File AddFile(String fileName, Uri fileURL);
        
        /// <summary>
        /// This interface adds a file to onDemand by providing an external URL that onDemand can download from. This is a good alternative to the Add File API for cases when the files are very large.
        /// </summary>
        /// <remarks>Files are then used to generate quotes. If a file is not used in a quote within 1 hour of it being uploaded it will be deleted from the system. Lionbridge onDemand has a general retention policy of 60 days for all customer content.
        /// Files can only be associated with one project. If you need to translate a file into additional languages, you can upload it again and create a new project out of it.</remarks>
        /// <param name="languageCode">A locale code in the format en-us where EN is the 2 character ISO language code and US is the 2 character ISO country code.</param>
        /// <param name="fileName">The original name of the file.</param>
        /// <param name="fileURL">The full URL to the file. The URL must be publicly accessible. It can use use http, https, ftp, or ftps. If the URL requires authentication, credentials must be passed in the URL.></param>
        /// <returns>A File object or null if the file could not be created.</returns>
        File AddFile(String languageCode, String fileName, Uri fileURL);

        /// <summary>
        /// This interface adds money to a prepaid balance that can be used to pay for onDemand projects.
        /// </summary>
        /// <param name="addCreditBalance"></param>
        /// <returns>Contains information about the credit balance request including a payment URL. The user must follow this URL to a payment page.</returns>
        AddCreditBalance AddPrepaidBalance(AddCreditBalance addCreditBalance);

        /// <summary>
        /// This interface adds money to a prepaid balance that can be used to pay for onDemand projects.
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="currency"></param>
        /// <returns>Contains information about the credit balance request including a payment URL. The user must follow this URL to a payment page.</returns>
        AddCreditBalance AddPrepaidBalance(Decimal amount, String currency);

        /// <summary>
        /// Adds a new project to onDemand. Should be used in conjunction with Generate Quote to make a purchase. 
        /// </summary>
        /// <param name="projectName"></param>
        /// <param name="service"></param>
        /// <param name="files"></param>
        /// <param name="options"></param>
        /// <param name="referenceFiles"></param>
        /// <returns></returns>
        Project AddProject(String projectName, Service service, IEnumerable<File> files, ProjectOptions options, IEnumerable<File> referenceFiles = null);

        /// <summary>
        /// Adds a new project to onDemand. Should be used in conjunction with Generate Quote to make a purchase. 
        /// </summary>
        /// <param name="projectName"></param>
        /// <param name="service"></param>
        /// <param name="filePaths"></param>
        /// <param name="options"></param>
        /// <param name="referenceFilePaths"></param>
        /// <returns></returns>
        Project AddProject(String projectName, Service service, String[] filePaths, ProjectOptions options, String[] referenceFilePaths = null);

        /// <summary>
        /// Adds a new project to onDemand. Should be used in conjunction with Generate Quote to make a purchase. 
        /// </summary>
        /// <param name="projectName"></param>
        /// <param name="service"></param>
        /// <param name="products"></param>
        /// <param name="options"></param>
        /// <param name="referenceFiles"></param>
        /// <returns></returns>
        Project AddProject(String projectName, Service service, IEnumerable<Product> products, ProjectOptions options, IEnumerable<File> referenceFiles = null);

        /// <summary>
        /// This interface authorizes a quote. Only quotes with a status of “Pending” can be authorized.
        /// </summary>
        /// <param name="quote"></param>
        /// <returns></returns>
        QuoteAuthorization AuthorizeQuote(Quote quote);

        /// <summary>
        /// This interface creates a new Account. Access is restricted to an API account with create merchant privileges.
        /// </summary>
        /// <param name="createAccount"></param>
        /// <returns></returns>
        Account CreateAccount(CreateAccount createAccount);

        /// <summary>
        /// This interface creates a new Account. Access is restricted to an API account with create merchant privileges.
        /// </summary>
        /// <param name="merchantID"></param>
        /// <param name="emailAddress"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="companyName"></param>
        /// <param name="country"></param>
        /// <param name="vatID"></param>
        /// <returns></returns>
        Account CreateAccount(String merchantID, String emailAddress, String firstName, String lastName, String companyName, String country, String vatID);
        
        /// <summary>
        /// Gets or sets the default currency for the current client
        /// </summary>
        String DefaultCurrency { get; set; }
        
        /// <summary>
        /// This interface is used to generate a quote from Files that were uploading using the Add File API. A quote can contain multiple projects.
        /// </summary>
        /// <param name="service"></param>
        /// <param name="files"></param>
        /// <param name="options"></param>
        /// <param name="referenceFiles">If null, will not use reference files</param>
        /// <returns></returns>
        Quote GenerateQuote(Service service, IEnumerable<File> files, QuoteOptions options, IEnumerable<File> referenceFiles = null);
        
        /// <summary>
        /// This interface is used to generate a quote from Product elements, which are are inserted into the generate quote request. A quote can contain multiple projects.
        /// </summary>
        /// <param name="service"></param>
        /// <param name="products"></param>
        /// <param name="options"></param>
        /// <param name="referenceFiles">If null, will not use reference files</param>
        /// <returns></returns>
        Quote GenerateQuote(Service service, IEnumerable<Product> products, QuoteOptions options, IEnumerable<File> referenceFiles = null);
        
        /// <summary>
        /// This interface is used to generate a quote from file paths that will be loaded. A quote can contain multiple projects.
        /// </summary>
        /// <param name="service"></param>
        /// <param name="filePaths"></param>
        /// <param name="options"></param>
        /// <param name="referenceFiles">If null, will not use reference files</param>
        /// <returns></returns>
        Quote GenerateQuote(Service service, String[] filePaths, QuoteOptions options, String[] referenceFilePaths = null);

        /// <summary>
        /// This interface is used to generate a quote from file URLs that will be loaded. A quote can contain multiple projects.
        /// </summary>
        /// <param name="service"></param>
        /// <param name="fileURLs"></param>
        /// <param name="fileNames"></param>
        /// <param name="options"></param>
        /// <param name="referenceFiles">If null, will not use reference files</param>
        /// <returns></returns>
        Quote GenerateQuote(Service service, Uri[] fileURLs, String[] fileNames, QuoteOptions options, IEnumerable<File> referenceFiles = null);

        /// <summary>
        /// This interface is used to generate a quote from Projects that were uploading using the Add Project API. A quote can contain multiple projects.
        /// </summary>
        /// <param name="projects"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        Quote GenerateQuote(IEnumerable<Project> projects, ProjectQuoteOptions options);

        /// <summary>
        /// Returns information about the merchant’s account
        /// </summary>
        /// <returns></returns>
        AccountInformation GetAccountInformation();

        /// <summary>
        /// The Get Estimate API is used to get a rough estimate for a project based on the parameters listed below. 
        /// It is useful for client applications that are capable of counting words and that want to give the end customer a rough estimate of how much a project will cost. 
        /// Please note, the estimate will not be the same as the actual price if the onDemand word counting algorithm produces a different result than the client application.
        /// </summary>
        /// <param name="service"></param>
        /// <param name="unitCount"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        Estimate GetEstimate(Service service, Int32 unitCount, EstimateOptions options);
        
        /// <summary>
        /// Returns a source file
        /// </summary>
        /// <param name="assetID"></param>
        /// <returns></returns>
        byte[] GetFile(String assetID);

        /// <summary>
        /// Returns details about a file
        /// </summary>
        /// <param name="assetID"></param>
        /// <returns></returns>
        File GetFileDetails(String assetID);
        
        /// <summary>
        /// Retrieves the translation of a file
        /// </summary>
        /// <param name="assetID"></param>
        /// <param name="languageCode"></param>
        /// <returns></returns>
        byte[] GetFileTranslation(String assetID, String languageCode);
        
        /// <summary>
        /// Returns a product
        /// </summary>
        /// <param name="productID"></param>
        /// <returns></returns>
        Product GetProduct(int productID);
        
        /// <summary>
        /// Retrieves the translation of an item
        /// </summary>
        /// <param name="assetID"></param>
        /// <param name="languageCode"></param>
        /// <returns></returns>
        ProductTranslation GetProductTranslation(int assetID, String languageCode);
        
        /// <summary>
        /// Retrieves information about a project.
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        Project GetProject(int projectId);
        
        /// <summary>
        /// Returns information about a quote. This API is useful for polling
        /// </summary>
        /// <param name="quoteID"></param>
        /// <returns></returns>
        Quote GetQuote(int quoteID);

        /// <summary>
        /// Provides detailed information about a single service
        /// </summary>
        /// <param name="serviceID"></param>
        /// <returns></returns>
        Service GetService(int serviceID);
        
        /// <summary>
        /// This interface retrieves the terms and conditions.
        /// </summary>
        /// <returns>Returns an XHTML document containing the current terms and conditions</returns>
        XDocument GetTermsAndConditions();
        
        /// <summary>
        /// Returns a list of all files submitted by user.
        /// </summary>
        /// <returns></returns>
        IEnumerable<File> ListFiles();

        /// <summary>
        /// Returns a list of all supported locales
        /// </summary>
        /// <returns></returns>
        IEnumerable<Locale> ListLocales();
        
        /// <summary>
        /// Returns a list of all products submitted from the user account.
        /// </summary>
        /// <returns></returns>
        IEnumerable<Product> ListProducts();
        
        /// <summary>
        /// Returns a list of all a user’s projects.
        /// </summary>
        /// <returns></returns>
        IEnumerable<Project> ListProjects();
        
        /// <summary>
        /// This interface lists translation services that are available through the API.
        /// </summary>
        /// <returns></returns>
        IEnumerable<Service> ListServices();

        /// <summary>
        /// Returns a list of all of the quotes owned by a user.
        /// </summary>
        /// <returns></returns>
        IEnumerable<Quote> ListQuotes();
        
        /// <summary>
        /// This interface rejects a quote that has already been created. This deletes the quote.
        /// </summary>
        /// <param name="quote"></param>
        /// <returns></returns>
        RejectQuoteStatus RejectQuote(Quote quote);
        
        /// <summary>
        /// This interface rejects a quote that has already been created. This deletes the quote.
        /// </summary>
        /// <param name="quoteID"></param>
        /// <returns></returns>
        RejectQuoteStatus RejectQuote(Int32 quoteID);

        /// <summary>
        /// Update the file information in place
        /// </summary>
        /// <param name="file"></param>
        void UpdateFile(File file);
        
        /// <summary>
        /// Updates the project information in place
        /// </summary>
        /// <param name="project"></param>
        void UpdateProject(Project project);
        
        /// <summary>
        /// Updates the quote information in place
        /// </summary>
        /// <param name="quote"></param>
        void UpdateQuote(Quote quote);
    }
}
