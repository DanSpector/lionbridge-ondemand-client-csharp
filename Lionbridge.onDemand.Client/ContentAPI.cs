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
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Xml.Linq;

namespace Lionbridge.onDemand.Client
{
    /// <summary>
    /// ContentAPI implementation
    /// </summary>
    public class ContentAPI : IContentAPI
    {
        public const String VERSION = "2015-02-23";

        public const String DETECT_LANGUAGE = "detect-language";

        #region Public Properties

        /// <summary>
        /// Client's Access Key ID
        /// </summary>
        public String KeyId
        {
            get;
            private set;
        }

        /// <summary>
        /// Client's Secret Access Key
        /// </summary>
        public String SecretKey
        {
            get;
            private set;
        }

        /// <summary>
        /// Base URL of onDemand instance to communicate with
        /// </summary>
        public Uri EndPoint
        {
            get;
            private set;
        }

        /// <summary>
        /// Default Currency for transactions
        /// </summary>
        /// <remarks>If DefaultCurrency is null, Quote generation will fail without a Currency specified</remarks>
        public String DefaultCurrency
        {
            get;
            set;
        }

        #endregion Public Properties

        #region Constructors

        /// <summary>
        /// Constructs a new service to communicate
        /// </summary>
        /// <param name="keyId">Client's Access Key ID</param>
        /// <param name="secretKey">Client's Secret Access Key</param>
        /// <param name="endpoint">Base URL of onDemand instance to communicate with</param>
        /// <param name="defaultCurrency">Default Currency for transactions</param>
        public ContentAPI(String keyId, String secretKey, Uri endpoint, String defaultCurrency = null)
        {
            this.KeyId = keyId;
            this.SecretKey = secretKey;
            this.EndPoint = endpoint;
            this.DefaultCurrency = defaultCurrency;
        }

        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// Creates a new Account. Access is restricted to an API account with create merchant privileges.
        /// </summary>
        /// <param name="merchantID">Merchant system account ID</param>
        /// <param name="emailAddress">Email address of the primary user</param>
        /// <param name="firstName">First name of the primary user</param>
        /// <param name="lastName">Last name of the primary user</param>
        /// <param name="companyName">Merchant Company</param>
        /// <param name="country">Country that the merchant is headquartered in. ISO 3166-1 2 character country code</param>
        /// <param name="vatID">Tax ID for VAT accounting. Required for Irish merchants only</param>
        /// <returns>An Account object</returns>
        public Account CreateAccount(String merchantID, String emailAddress, String firstName, String lastName, String companyName, String country, String vatID = null)
        {
            CreateAccount createAccountRequest = new CreateAccount(merchantID, emailAddress, firstName, lastName, companyName, country, vatID);
            Account account = this.CreateAccount(createAccountRequest);
            return account;
        }

        /// <summary>
        /// Creates a new Account. Access is restricted to an API account with create merchant privileges.
        /// </summary>
        /// <param name="createAccount">An XML request to create an account</param>
        /// <returns>An Account object</returns>
        public Account CreateAccount(CreateAccount createAccount)
        {
            Account result = null;

            Uri uri = new Uri(this.EndPoint.AbsoluteUri + "api/account/create");

            HttpWebRequest request = this.CreateRequestPOST(uri, createAccount);

            using (HttpWebResponse response = request.GetResponseWithoutException() as HttpWebResponse)
            {
                if (response.StatusCode == HttpStatusCode.Created)
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        XDocument document = XDocument.Load(reader);

                        result = new Account(document.Element("Account"));
                    }
                }
                else
                {
                    this.HandleError(response);
                }
            }

            return result;
        }

        /// <summary>
        /// Returns information about the merchant’s account
        /// </summary>
        /// <returns>An Account object</returns>
        public AccountInformation GetAccountInformation()
        {
            AccountInformation result = null;

            Uri uri = new Uri(this.EndPoint.AbsoluteUri + "api/account/info");

            HttpWebRequest request = this.CreateRequestGET(uri);

            using (HttpWebResponse response = request.GetResponseWithoutException() as HttpWebResponse)
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        XDocument document = XDocument.Load(reader);

                        result = new AccountInformation(document.Element("Account"));
                    }
                }
                else
                {
                    this.HandleError(response);
                }
            }

            return result;
        }

        /// <summary>
        /// Adds money to a prepaid balance that can be used to pay for onDemand projects
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="currency"></param>
        /// <returns>Contains information about the credit balance request including a payment URL. The user must follow this URL to a payment page.</returns>
        public AddCreditBalance AddPrepaidBalance(Decimal amount, String currency)
        {
            return this.AddPrepaidBalance(new AddCreditBalance(amount, currency));
        }

        /// <summary>
        /// Adds money to a prepaid balance that can be used to pay for onDemand projects
        /// </summary>
        /// <param name="addCreditBalance"></param>
        /// <returns>Contains information about the credit balance request including a payment URL. The user must follow this URL to a payment page.</returns>
        public AddCreditBalance AddPrepaidBalance(AddCreditBalance addCreditBalance)
        {
            AddCreditBalance result = null;

            Uri uri = new Uri(this.EndPoint.AbsoluteUri + "api/account/credit-balance/add");

            HttpWebRequest request = this.CreateRequestPOST(uri, addCreditBalance);

            using (HttpWebResponse response = request.GetResponseWithoutException() as HttpWebResponse)
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        XDocument document = XDocument.Load(reader);

                        result = new AddCreditBalance(document.Element("AddCreditBalance"));
                    }
                }
                else
                {
                    this.HandleError(response);
                }
            }

            return result;
        }

        /// <summary>
        /// This interface is used to generate a quote from Product elements, which are are inserted into the generate quote request. A quote can contain multiple projects.
        /// </summary>
        /// <param name="service"></param>
        /// <param name="products"></param>
        /// <param name="options"></param>
        /// <param name="referenceFiles">If null, will not use reference files</param>
        /// <returns></returns>
        public Quote GenerateQuote(Service service, IEnumerable<Product> products, QuoteOptions options, IEnumerable<File> referenceFiles = null)
        {
            // Check the service
            if (service == null)
            {
                throw new ArgumentNullException("service", "Must specify a Service to generate a quote");
            }

            if (!service.AcceptsProducts)
            {
                throw new ArgumentException("This service does not accept products.  Please use GenerateQuote with Files", "service");
            }

            if (options == null)
            {
                throw new ArgumentNullException("options", "Must specify options to generate a quote");
            }

            options.Initialize(this, service);

            Quote result = null;

            Uri uri = new Uri(this.EndPoint.AbsoluteUri + "api/quote/generate");

            HttpWebRequest request = this.CreateRequestPOST(uri, new GenerateQuote(products, options, referenceFiles));

            using (HttpWebResponse response = request.GetResponseWithoutException() as HttpWebResponse)
            {
                if (response.StatusCode == HttpStatusCode.Created)
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        XDocument document = XDocument.Load(reader);

                        result = new Quote(document.Element("Quote"), this);
                    }
                }
                else
                {
                    this.HandleError(response);
                }
            }

            return result;
        }

        /// <summary>
        /// This interface is used to generate a quote from file paths that will be loaded. A quote can contain multiple projects.
        /// </summary>
        /// <param name="service"></param>
        /// <param name="filePaths"></param>
        /// <param name="options"></param>
        /// <param name="referenceFilePaths">If null, will not use reference files</param>
        /// <returns></returns>
        public Quote GenerateQuote(Service service, string[] filePaths, QuoteOptions options, string[] referenceFilePaths = null)
        {
            // Check the service
            if (service == null)
            {
                throw new ArgumentNullException("service", "Must specify a Service to generate a quote");
            }

            if (!service.AcceptsFiles)
            {
                throw new ArgumentException("This service does not accept files.  Please use GenerateQuote with Products", "service");
            }

            if (options == null)
            {
                throw new ArgumentNullException("options", "Must specify options to generate a quote");
            }

            options.Initialize(this, service);

            // Check that the file extensions are valid
            foreach (String filePath in filePaths)
            {
                if (!service.AcceptsExtension(Path.GetExtension(filePath)))
                {
                    throw new ArgumentOutOfRangeException("filePaths", filePath, "Service does not accept files with this extension");
                }
            }

            // Upload the files
            List<File> addedFiles = new List<File>();

            foreach (String filePath in filePaths)
            {
                addedFiles.Add(this.AddFile(options.SourceLanguage.LanguageCode, filePath));
            }

            // Upload the reference
            List<File> referenceFiles = null;

            if (referenceFilePaths != null)
            {
                referenceFiles = new List<File>();
                foreach (String referenceFilePath in referenceFilePaths)
                {
                    referenceFiles.Add(this.AddFile(options.SourceLanguage.LanguageCode, referenceFilePath));
                }
            }

            return this.GenerateQuote(service, addedFiles, options, referenceFiles);
        }

        /// <summary>
        /// This interface is used to generate a quote from file URLs that will be loaded. A quote can contain multiple projects.
        /// </summary>
        /// <param name="service"></param>
        /// <param name="fileURLs"></param>
        /// <param name="fileNames"></param>
        /// <param name="options"></param>
        /// <param name="referenceFiles">If null, will not use reference files</param>
        /// <returns></returns>
        public Quote GenerateQuote(Service service, Uri[] fileURLs, String[] fileNames, QuoteOptions options, IEnumerable<File> referenceFiles = null)
        {
            // Check the service
            if (service == null)
            {
                throw new ArgumentNullException("service", "Must specify a Service to generate a quote");
            }

            if (fileURLs == null || fileNames == null)
            {
                throw new ArgumentNullException("files", "Must specify the fileURLs and fileNames");
            }

            if (!service.AcceptsFiles)
            {
                throw new ArgumentException("This service does not accept files.  Please use GenerateQuote with Products", "service");
            }

            if (options == null)
            {
                throw new ArgumentNullException("options", "Must specify options to generate a quote");
            }

            options.Initialize(this, service);

            // Check that the file extensions are valid
            foreach (String fileName in fileNames)
            {
                if (!service.AcceptsExtension(Path.GetExtension(fileName)))
                {
                    throw new ArgumentOutOfRangeException("fileNames", fileName, "Service does not accept files with this extension");
                }
            }

            // Upload the files
            List<File> addedFiles = new List<File>();

            for (int i = 0; i < fileNames.Length && i < fileURLs.Length; i++)
            {
                addedFiles.Add(this.AddFile(options.SourceLanguage.LanguageCode, fileNames[i], fileURLs[i]));
            }

            return this.GenerateQuote(service, addedFiles, options, referenceFiles);
        }


        /// <summary>
        /// This interface is used to generate a quote from Files that were uploading using the Add File API. A quote can contain multiple projects.
        /// </summary>
        /// <param name="service"></param>
        /// <param name="files"></param>
        /// <param name="options"></param>
        /// <param name="referenceFiles">If null, will not use reference files</param>
        /// <returns></returns>
        public Quote GenerateQuote(Service service, IEnumerable<File> files, QuoteOptions options, IEnumerable<File> referenceFiles = null)
        {
            // Check the service
            if (service == null)
            {
                throw new ArgumentNullException("service", "Must specify a Service to generate a quote");
            }

            if (!service.AcceptsFiles)
            {
                throw new ArgumentException("This service does not accept files.  Please use GenerateQuote with Products", "service");
            }

            if (options == null)
            {
                throw new ArgumentNullException("options", "Must specify options to generate a quote");
            }

            options.Initialize(this, service);

            // Now Generate the Quote based on the uplaoded files
            Quote result = null;

            Uri uri = new Uri(this.EndPoint.AbsoluteUri + "api/quote/generate");

            HttpWebRequest request = this.CreateRequestPOST(uri, new GenerateQuote(files, options, referenceFiles));

            using (HttpWebResponse response = request.GetResponseWithoutException() as HttpWebResponse)
            {
                if (response.StatusCode == HttpStatusCode.Created)
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        XDocument document = XDocument.Load(reader);

                        result = new Quote(document.Element("Quote"), this);
                    }
                }
                else
                {
                    this.HandleError(response);
                }
            }

            return result;
        }

        /// <summary>
        /// This interface is used to generate a quote from Projects that were uploading using the Add Project API. A quote can contain multiple projects.
        /// </summary>
        /// <param name="projects"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public Quote GenerateQuote(IEnumerable<Project> projects, ProjectQuoteOptions options)
        {
            Quote result = null;

            if (options == null)
            {
                throw new ArgumentNullException("options", "Must specify options to generate a quote");
            }

            Uri uri = new Uri(this.EndPoint.AbsoluteUri + "api/quote/generate");

            options.Initialize(this);

            HttpWebRequest request = this.CreateRequestPOST(uri, new GenerateQuote(projects, options));

            using (HttpWebResponse response = request.GetResponseWithoutException() as HttpWebResponse)
            {
                if (response.StatusCode == HttpStatusCode.Created)
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        XDocument document = XDocument.Load(reader);

                        result = new Quote(document.Element("Quote"), this);
                    }
                }
                else
                {
                    this.HandleError(response);
                }
            }

            return result;
        }

        /// <summary>
        /// This interface authorizes a quote. Only quotes with a status of “Pending” can be authorized.
        /// </summary>
        /// <param name="quote"></param>
        /// <returns></returns>
        public QuoteAuthorization AuthorizeQuote(Quote quote)
        {
            if (quote.Status != "Pending")
            {
                throw new ArgumentException("Only quotes with a status of Pending can be authorized", "quote");
            }

            QuoteAuthorization result = null;

            Uri uri = new Uri(this.EndPoint.AbsoluteUri + "api/quote/" + quote.QuoteID + "/authorize");

            HttpWebRequest request = this.CreateRequestPOST(uri, quote);
            
            using (HttpWebResponse response = request.GetResponseWithoutException() as HttpWebResponse)
            {
                if (response.StatusCode == HttpStatusCode.Accepted || response.StatusCode == HttpStatusCode.PaymentRequired)
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        XDocument document = XDocument.Load(reader);

                        if (document.Element("QuoteAuthorization") != null)
                        {
                            result = new QuoteAuthorization(document.Element("QuoteAuthorization"), this);
                        }
                        //else if (document.Element("Quote") != null)
                        //{
                        //    result = new QuoteAuthorization(document.Element("Quote"), this);
                        //}
                    }
                }
                else
                {
                    this.HandleError(response);
                }
            }

            return result;
        }

        /// <summary>
        /// Returns a list of all of the quotes owned by a user.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Quote> ListQuotes()
        {
            IEnumerable<Quote> result = null;

            Uri uri = new Uri(this.EndPoint.AbsoluteUri + "api/quote");

            HttpWebRequest request = this.CreateRequestGET(uri);

            using (HttpWebResponse response = request.GetResponseWithoutException() as HttpWebResponse)
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        XDocument document = XDocument.Load(reader);

                        result = Quote.CreateEnumerable(document.Element("Quotes"), this);
                    }
                }
                else
                {
                    this.HandleError(response);
                }
            }

            return result;
        }

        /// <summary>
        /// Updates information about a quote. This is useful for polling
        /// </summary>
        /// <param name="quote"></param>
        /// <returns></returns>
        public void UpdateQuote(Quote quote)
        {
            Uri uri = new Uri(this.EndPoint.AbsoluteUri + "api/quote/" + quote.QuoteID);

            HttpWebRequest request = this.CreateRequestGET(uri);

            using (HttpWebResponse response = request.GetResponseWithoutException() as HttpWebResponse)
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        XDocument document = XDocument.Load(reader);

                        quote.UpdateFromXElement(document.Element("Quote"));
                    }
                }
                else
                {
                    this.HandleError(response);
                }
            }
        }

        /// <summary>
        /// Returns information about a quote. This is useful for polling
        /// </summary>
        /// <param name="quoteID"></param>
        /// <returns></returns>
        public Quote GetQuote(int quoteID)
        {
            Quote result = null;

            Uri uri = new Uri(this.EndPoint.AbsoluteUri + "api/quote/" + quoteID);

            HttpWebRequest request = this.CreateRequestGET(uri);

            using (HttpWebResponse response = request.GetResponseWithoutException() as HttpWebResponse)
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        XDocument document = XDocument.Load(reader);

                        result = new Quote(document.Element("Quote"), this);
                    }
                }
                else
                {
                    this.HandleError(response);
                }
            }

            return result;
        }

        /// <summary>
        /// Rejects a quote that has already been created. This deletes the quote.
        /// </summary>
        /// <param name="quote"></param>
        /// <returns></returns>
        public RejectQuoteStatus RejectQuote(Quote quote)
        {
            return this.RejectQuote(quote.QuoteID);
        }

        /// <summary>
        /// Rejects a quote that has already been created. This deletes the quote.
        /// </summary>
        /// <param name="quoteId"></param>
        /// <returns></returns>
        public RejectQuoteStatus RejectQuote(Int32 quoteID)
        {
            RejectQuoteStatus result = RejectQuoteStatus.Unknown;

            Uri uri = new Uri(this.EndPoint.AbsoluteUri + "api/quote/" + quoteID + "/reject");

            HttpWebRequest request = this.CreateRequestPOST(uri);

            using (HttpWebResponse response = request.GetResponseWithoutException() as HttpWebResponse)
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        XDocument document = XDocument.Load(reader);

                        XElement element = document.Element("RejectQuote");

                        switch (element.GetChildValue("status"))
                        {
                            case "200":
                                result = RejectQuoteStatus.Success;
                                break;
                            case "400":
                                result = RejectQuoteStatus.BadRequest;
                                break;
                            case "401":
                                result = RejectQuoteStatus.Unauthorized;
                                break;
                            case "409":
                                result = RejectQuoteStatus.Conflict;
                                break;
                            default:
                                result = RejectQuoteStatus.Unknown;
                                break;
                        }
                    }
                }
                else
                {
                    this.HandleError(response);
                    result = RejectQuoteStatus.CommunicationError;  // Unlikely, in case above line changes
                }
            }

            return result;
        }

        /// <summary>
        /// Returns a list of all a user’s projects.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Project> ListProjects()
        {
            IEnumerable<Project> result = null;

            Uri uri = new Uri(this.EndPoint.AbsoluteUri + "api/projects");

            HttpWebRequest request = this.CreateRequestGET(uri);

            using (HttpWebResponse response = request.GetResponseWithoutException() as HttpWebResponse)
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        XDocument document = XDocument.Load(reader);

                        result = Project.CreateEnumerable(document.Element("Projects"), this);
                    }
                }
                else
                {
                    this.HandleError(response);
                }
            }

            return result;
        }

        /// <summary>
        /// Returns an XHTML document containing the current terms and conditions
        /// </summary>
        /// <returns>The terms and conditions stored in an XDocument</returns>
        public XDocument GetTermsAndConditions()
        {
            XDocument result = null;

            Uri uri = new Uri(this.EndPoint.AbsoluteUri + "api/terms");

            HttpWebRequest request = this.CreateRequestGET(uri);

            using (HttpWebResponse response = request.GetResponseWithoutException() as HttpWebResponse)
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        result = XDocument.Load(reader);
                    }
                }
                else
                {
                    this.HandleError(response);
                }
            }

            return result;
        }

        /// <summary>
        /// This interface adds a file to the system.
        /// </summary>
        /// <remarks>Lionbridge onDemand will attempt to detect the language. 
        /// Files are then used to generate quotes. If a file is not used in a quote within 1 hour of it being uploaded it will be deleted from the system. Lionbridge onDemand has a general retention policy of 60 days for all customer content.
        /// Files can only be associated with one project. If you need to translate a file into additional languages, you can upload it again and create a new project out of it.</remarks>
        /// <param name="filePath">The path to the file to add</param>
        /// <returns>A File object or null if the file could not be created.</returns>
        public File AddFile(String filePath)
        {
            return this.AddFile(DETECT_LANGUAGE, filePath);
        }

        /// <summary>
        /// This interface adds a file to the system.
        /// </summary>
        /// <remarks>Files are then used to generate quotes. If a file is not used in a quote within 1 hour of it being uploaded it will be deleted from the system. Lionbridge onDemand has a general retention policy of 60 days for all customer content.
        /// Files can only be associated with one project. If you need to translate a file into additional languages, you can upload it again and create a new project out of it.</remarks>
        /// <param name="languageCode">A locale code in the format en-us where EN is the 2 character ISO language code and US is the 2 character ISO country code.</param>
        /// <param name="filePath">The path to the file to add</param>
        /// <returns>A File object or null if the file could not be created.</returns>
        public File AddFile(String languageCode, String filePath)
        {
            return this.AddFile(languageCode, Path.GetFileName(filePath), ExtensionMethods.ReadAllBytes(filePath));
        }

        /// <summary>
        /// This interface adds a file to the system.
        /// </summary>
        /// <remarks>Lionbridge onDemand will attempt to detect the language. 
        /// Files are then used to generate quotes. If a file is not used in a quote within 1 hour of it being uploaded it will be deleted from the system. Lionbridge onDemand has a general retention policy of 60 days for all customer content.
        /// Files can only be associated with one project. If you need to translate a file into additional languages, you can upload it again and create a new project out of it.</remarks>
        /// <param name="fileName">The original name of the file.</param>
        /// <param name="fileData">The file contents</param>
        /// <returns>A File object or null if the file could not be created.</returns>
        public File AddFile(String fileName, Byte[] fileData)
        {
            return this.AddFile(DETECT_LANGUAGE, fileName, fileData);
        }

        /// <summary>
        /// Add a file to the system.
        /// </summary>
        /// <param name="languageCode">A locale code in the format en-us where EN is the 2 character ISO language code and US is the 2 character ISO country code.</param>
        /// <param name="fileName">The original name of the file.</param>
        /// <param name="fileData">The file contents</param>
        /// <returns>A File object or null if the file could not be created.</returns>
        public File AddFile(String languageCode, String fileName, Byte[] fileData)
        {
            File file = null;

            String escapedFileName = Uri.EscapeDataString(fileName);

            Uri uri = new Uri(this.EndPoint.AbsoluteUri + "api/files/add/" + languageCode + "/" + escapedFileName);

            HttpWebRequest request = this.CreateRequestFilePOST(uri, escapedFileName, fileData);

            using (HttpWebResponse response = request.GetResponseWithoutException() as HttpWebResponse)
            {
                if (response.StatusCode == HttpStatusCode.Created)
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        XDocument document = XDocument.Load(reader);

                        XElement element = document.Element("File");
                        file = new File(element, this);
                    }
                }
                else
                {
                    this.HandleError(response);
                }
            }

            return file;
        }

        /// <summary>
        /// This interface adds a file to onDemand by providing an external URL that onDemand can download from. This is a good alternative to the Add File API for cases when the files are very large.
        /// </summary>
        /// <remarks>Lionbridge onDemand will attempt to detect the language. 
        /// Files are then used to generate quotes. If a file is not used in a quote within 1 hour of it being uploaded it will be deleted from the system. Lionbridge onDemand has a general retention policy of 60 days for all customer content.
        /// Files can only be associated with one project. If you need to translate a file into additional languages, you can upload it again and create a new project out of it.</remarks>
        /// <param name="fileName">The original name of the file.</param>
        /// <param name="fileURL">The full URL to the file. The URL must be publicly accessible. It can use use http, https, ftp, or ftps. If the URL requires authentication, credentials must be passed in the URL.></param>
        /// <returns>A File object or null if the file could not be created.</returns>
        public File AddFile(String fileName, Uri fileURL)
        {
            return this.AddFile(DETECT_LANGUAGE, fileName, fileURL);
        }

        /// <summary>
        /// This interface adds a file to onDemand by providing an external URL that onDemand can download from. This is a good alternative to the Add File API for cases when the files are very large.
        /// </summary>
        /// <remarks>Files are then used to generate quotes. If a file is not used in a quote within 1 hour of it being uploaded it will be deleted from the system. Lionbridge onDemand has a general retention policy of 60 days for all customer content.
        /// Files can only be associated with one project. If you need to translate a file into additional languages, you can upload it again and create a new project out of it.</remarks>
        /// <param name="languageCode">A locale code in the format en-us where EN is the 2 character ISO language code and US is the 2 character ISO country code.</param>
        /// <param name="fileName">The original name of the file.</param>
        /// <param name="fileURL">The full URL to the file. The URL must be publicly accessible. It can use use http, https, ftp, or ftps. If the URL requires authentication, credentials must be passed in the URL.></param>
        /// <returns>A File object or null if the file could not be created.</returns>
        public File AddFile(String languageCode, String fileName, Uri fileURL)
        {
            File file = null;

            String escapedFileName = Uri.EscapeDataString(fileName);

            Uri uri = new Uri(this.EndPoint.AbsoluteUri + "api/files/add_by_reference/" + languageCode + "/" + escapedFileName);

            HttpWebRequest request = this.CreateRequestPOST(uri, File.ToXmlStringByURL(fileURL));

            using (HttpWebResponse response = request.GetResponseWithoutException() as HttpWebResponse)
            {
                if (response.StatusCode == HttpStatusCode.Created)
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        XDocument document = XDocument.Load(reader);

                        XElement element = document.Element("File");
                        file = new File(element, this);
                    }
                }
                else
                {
                    this.HandleError(response);
                }
            }

            return file;
        }

        /// <summary>
        /// Returns a list of all files submitted by user.
        /// </summary>
        /// <returns>A list of all files submitted by a user</returns>
        public IEnumerable<File> ListFiles()
        {
            IEnumerable<File> fileList = new List<File>();

            Uri uri = new Uri(this.EndPoint.AbsoluteUri + "api/files");

            HttpWebRequest request = this.CreateRequestGET(uri);

            using (HttpWebResponse response = request.GetResponseWithoutException() as HttpWebResponse)
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        XDocument document = XDocument.Load(reader);
                        XElement files = document.Element("Files");
                        fileList = File.CreateEnumerable(files, this);
                    }
                }
                else
                {
                    this.HandleError(response);
                }
            }

            IEnumerable<File> result = fileList.ToArray<File>();

            return fileList;
        }

        /// <summary>
        /// Adds a new project to onDemand. Should be used in conjunction with Generate Quote to make a purchase. 
        /// </summary>
        /// <param name="projectName"></param>
        /// <param name="service"></param>
        /// <param name="files"></param>
        /// <param name="options"></param>
        /// <param name="referenceFiles"></param>
        /// <returns></returns>
        public Project AddProject(String projectName, Service service, IEnumerable<File> files, ProjectOptions options, IEnumerable<File> referenceFiles = null)
        {
            // Check the service
            if (service == null)
            {
                throw new ArgumentNullException("service", "Must specify a Service to add a project");
            }

            if (!service.AcceptsFiles)
            {
                throw new ArgumentException("This service does not accept files.  Please use AddProject with Products", "service");
            }

            if (options == null)
            {
                throw new ArgumentNullException("options", "Must specify project options to add a project");
            }

            options.Initialize(this, service);

            // Now Generate the project based on the uplaoded files
            Project result = null;

            Uri uri = new Uri(this.EndPoint.AbsoluteUri + "api/projects/add");

            HttpWebRequest request = this.CreateRequestPOST(uri, new AddProject(projectName, files, options, referenceFiles));

            using (HttpWebResponse response = request.GetResponseWithoutException() as HttpWebResponse)
            {
                if (response.StatusCode == HttpStatusCode.Created)
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        XDocument document = XDocument.Load(reader);

                        result = new Project(document.Element("Project"), this);
                    }
                }
                else
                {
                    this.HandleError(response);
                }
            }

            return result;
        }

        /// <summary>
        /// Adds a new project to onDemand. Should be used in conjunction with Generate Quote to make a purchase. 
        /// </summary>
        /// <param name="projectName"></param>
        /// <param name="service"></param>
        /// <param name="filePaths"></param>
        /// <param name="options"></param>
        /// <param name="referenceFilePaths"></param>
        /// <returns></returns>
        public Project AddProject(String projectName, Service service, String[] filePaths, ProjectOptions options, String[] referenceFilePaths = null)
       {
            // Check the service
            if (service == null)
            {
                throw new ArgumentNullException("service", "Must specify a Service to add a project");
            }

            if (!service.AcceptsFiles)
            {
                throw new ArgumentException("This service does not accept files.  Please use AddProject with Products", "service");
            }

            if (options == null)
            {
                throw new ArgumentNullException("options", "Must specify project options to add a project");
            }

            options.Initialize(this, service);

            // Check that the file extensions are valid
            foreach (String filePath in filePaths)
            {
                if (!service.AcceptsExtension(Path.GetExtension(filePath)))
                {
                    throw new ArgumentOutOfRangeException("fileNames", filePath, "Service does not accept files with this extension");
                }
            }

            // Upload the files
            List<File> addedFiles = new List<File>();

            foreach (String filePath in filePaths)
            {
                addedFiles.Add(this.AddFile(options.SourceLanguage.LanguageCode, filePath));
            }

            // Upload the reference
            List<File> referenceFiles = null;

            if (referenceFilePaths != null)
            {
                referenceFiles = new List<File>();
                foreach (String referenceFilePath in referenceFilePaths)
                {
                    referenceFiles.Add(this.AddFile(options.SourceLanguage.LanguageCode, referenceFilePath));
                }
            }

            return this.AddProject(projectName, service, addedFiles, options, referenceFiles);
        }

        /// <summary>
        /// Adds a new project to onDemand. Should be used in conjunction with Generate Quote to make a purchase. 
        /// </summary>
        /// <param name="projectName"></param>
        /// <param name="service"></param>
        /// <param name="products"></param>
        /// <param name="options"></param>
        /// <param name="referenceFiles"></param>
        /// <returns></returns>
        public Project AddProject(String projectName, Service service, IEnumerable<Product> products, ProjectOptions options, IEnumerable<File> referenceFiles = null)
        {
            // Check the service
            if (service == null)
            {
                throw new ArgumentNullException("service", "Must specify a Service to add a project");
            }

            // Check the service
            if (service == null)
            {
                throw new ArgumentNullException("service", "Must specify a Service to add a project");
            }

            if (!service.AcceptsProducts)
            {
                throw new ArgumentException("This service does not accept projdcuts.  Please use AddProject with files", "service");
            }

            if (options == null)
            {
                throw new ArgumentNullException("options", "Must specify project options to add a project");
            }


            if (options == null)
            {
                throw new ArgumentNullException("options", "Must specify project options to add a project");
            }

            options.Initialize(this, service);

            Project result = null;

            Uri uri = new Uri(this.EndPoint.AbsoluteUri + "api/projects/add");

            HttpWebRequest request = this.CreateRequestPOST(uri, new AddProject(projectName, products, options, referenceFiles));

            using (HttpWebResponse response = request.GetResponseWithoutException() as HttpWebResponse)
            {
                if (response.StatusCode == HttpStatusCode.Created)
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        XDocument document = XDocument.Load(reader);

                        result = new Project(document.Element("Project"), this);
                    }
                }
                else
                {
                    this.HandleError(response);
                }
            }

            return result;
        }


        /// <summary>
        /// Updates the project information in place
        /// </summary>
        /// <param name="project"></param>
        public void UpdateProject(Project project)
        {
            Uri uri = new Uri(this.EndPoint.AbsoluteUri + "api/projects/" + project.ProjectID.ToString());

            HttpWebRequest request = CreateRequestGET(uri);

            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        XDocument document = XDocument.Load(reader);

                        project.UpdateFromXElement(document.Element("Project"));
                    }
                }
                else
                {
                    this.HandleError(response);
                }
            }
        }

        /// <summary>
        /// Returns information about a project.
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public Project GetProject(Int32 projectId)
        {
            Project result = null;

            Uri uri = new Uri(this.EndPoint.AbsoluteUri + "api/projects/" + projectId.ToString());

            HttpWebRequest request = CreateRequestGET(uri);

            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        XDocument document = XDocument.Load(reader);

                        result = new Project(document.Element("Project"), this);
                    }
                }
                else
                {
                    this.HandleError(response);
                }
            }

            return result;
        }

        /// <summary>
        /// The Get Estimate API is used to get a rough estimate for a project based on the parameters listed below. 
        /// It is useful for client applications that are capable of counting words and that want to give the end customer a rough estimate of how much a project will cost. 
        /// Please note, the estimate will not be the same as the actual price if the onDemand word counting algorithm produces a different result than the client application.
        /// </summary>
        /// <param name="service"></param>
        /// <param name="unitCount"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public Estimate GetEstimate(Service service, Int32 unitCount, EstimateOptions options)
        {
            Estimate result = null;

            if (service == null)
            {
                throw new ArgumentNullException("service", "Must specify a Service to generate an estimate");
            }

            if (options == null)
            {
                throw new ArgumentNullException("options", "Must specify estimate options to get an estimate");
            }

            options.Initialize(this, service);

            String targetLanguagesCSV = String.Join(",", options.TargetLanguages.Select(p => p.LanguageCode).ToArray());

            Uri uri = new Uri(this.EndPoint.AbsoluteUri + String.Format("api/estimate?service_id={0}&unit_count={1}&currency={2}&source_lang={3}&target_lang={4}",
                service.ServiceID, unitCount, options.Currency, options.SourceLanguage, targetLanguagesCSV));

            HttpWebRequest request = CreateRequestGET(uri);

            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        XDocument document = XDocument.Load(reader);

                        result = new Estimate(document.Element("Estimate"), this);
                    }
                }
                else
                {
                    this.HandleError(response);
                }
            }

            return result;
        }

        /// <summary>
        /// Returns details about a file
        /// </summary>
        /// <param name="assetID"></param>
        /// <returns></returns>
        public File GetFileDetails(string assetID)
        {
            File result = null;

            Uri uri = new Uri(this.EndPoint.AbsoluteUri + "api/files/" + assetID + "/details");

            HttpWebRequest request = this.CreateRequestGET(uri);

            using (HttpWebResponse response = request.GetResponseWithoutException() as HttpWebResponse)
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        XDocument document = XDocument.Load(reader);

                        result = new File(document.Element("File"), this);
                    }
                }
                else
                {
                    this.HandleError(response);
                }
            }

            return result;
        }

        /// <summary>
        /// Update the file information in place
        /// </summary>
        /// <param name="file"></param>
        public void UpdateFile(File file)
        {
            Uri uri = new Uri(this.EndPoint.AbsoluteUri + "api/files/" + file.AssetID + "/details");

            HttpWebRequest request = this.CreateRequestGET(uri);

            using (HttpWebResponse response = request.GetResponseWithoutException() as HttpWebResponse)
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        XDocument document = XDocument.Load(reader);

                        file.UpdateFromXElement(document.Element("File"));
                    }
                }
                else
                {
                    this.HandleError(response);
                }
            }

        }

        /// <summary>
        /// Returns a source file
        /// </summary>
        /// <param name="assetID">The asset ID of the file</param>
        /// <returns>The contents of the file or null if the file cannot be found</returns>
        /// <exception cref="ArgumentNullException">if the asset ID is null</exception>
        /// <exception cref="ArgumentException">if the asset ID is empty</exception>
        public Byte[] GetFile(String assetID)
        {
            if (String.IsNullOrEmpty(assetID))
            {
                if (assetID == null)
                {
                    throw new ArgumentNullException("assetID cannot be null");
                }
                throw new ArgumentException("asset ID cannot be empty");
            }

            Byte[] fileContent = this.GetFileCommon(assetID);

            return fileContent;
        }


        /// <summary>
        /// Retrieves the translation of a file
        /// </summary>
        /// <param name="assetID">The asset ID of the file</param>
        /// <param name="languageCode">A language code</param>
        /// <returns>The contents of the translated file or null if the file cannot be found</returns>
        /// <exception cref="ArgumentNullException">if the asset ID is null</exception>
        /// <exception cref="ArgumentException">if the asset ID is empty</exception>
        public Byte[] GetFileTranslation(String assetID, String languageCode)
        {
            if (String.IsNullOrEmpty(assetID))
            {
                if (assetID == null)
                {
                    throw new ArgumentNullException("assetID cannot be null");
                }
                throw new ArgumentException("asset ID cannot be empty");
            }

            if (String.IsNullOrEmpty(languageCode))
            {
                if (languageCode == null)
                {
                    throw new ArgumentNullException("languageCode cannot be null");
                }
                throw new ArgumentException("languageCode cannot be empty");
            }

            Byte[] fileContent = this.GetFileCommon(assetID, languageCode);

            return fileContent;
        }

        /// <summary>
        /// Returns a list of all supported locales
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Locale> ListLocales()
        {
            IEnumerable<Locale> result = null;

            Uri uri = new Uri(this.EndPoint.AbsoluteUri + "api/locales");

            HttpWebRequest request = this.CreateRequestGET(uri);

            using (HttpWebResponse response = request.GetResponseWithoutException() as HttpWebResponse)
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        XDocument document = XDocument.Load(reader);

                        result = Locale.CreateEnumerable(document.Element("Locales"));
                    }
                }
                else
                {
                    this.HandleError(response);
                }
            }

            return result;
        }

        /// <summary>
        /// This interface lists translation services that are available through the API.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Service> ListServices()
        {
            IEnumerable<Service> result = null;

            Uri uri = new Uri(this.EndPoint.AbsoluteUri + "api/services");

            HttpWebRequest request = this.CreateRequestGET(uri);

            using (HttpWebResponse response = request.GetResponseWithoutException() as HttpWebResponse)
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        XDocument document = XDocument.Load(reader);

                        result = Service.CreateEnumerable(document.Element("Services"), this);
                    }
                }
                else
                {
                    this.HandleError(response);
                }
            }

            return result;
        }

        /// <summary>
        /// Provides detailed information about a single service
        /// </summary>
        /// <param name="serviceID"></param>
        /// <returns></returns>
        public Service GetService(int serviceID)
        {
            Service result = null;

            Uri uri = new Uri(this.EndPoint.AbsoluteUri + "api/services/" + serviceID);

            HttpWebRequest request = this.CreateRequestGET(uri);

            using (HttpWebResponse response = request.GetResponseWithoutException() as HttpWebResponse)
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        XDocument document = XDocument.Load(reader);

                        result = new Service(document.Element("Service"), this);
                    }
                }
                else
                {
                    this.HandleError(response);
                }
            }

            return result;
        }

        /// <summary>
        /// Returns a list of all products submitted from the user account.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Product> ListProducts()
        {
            IEnumerable<Product> result = null;

            Uri uri = new Uri(this.EndPoint.AbsoluteUri + "api/products");

            HttpWebRequest request = this.CreateRequestGET(uri);

            using (HttpWebResponse response = request.GetResponseWithoutException() as HttpWebResponse)
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        XDocument document = XDocument.Load(reader);

                        result = Product.CreateEnumerable(document.Element("Products"), this);
                    }
                }
                else
                {
                    this.HandleError(response);
                }
            }

            return result;
        }

        /// <summary>
        /// Returns a product
        /// </summary>
        /// <param name="productID"></param>
        /// <returns></returns>
        public Product GetProduct(Int32 productID)
        {
            Product result = null;

            Uri uri = new Uri(this.EndPoint.AbsoluteUri + "api/products/" + productID.ToString());
            
            HttpWebRequest request = CreateRequestGET(uri);

            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        XDocument document = XDocument.Load(reader);

                        XElement element = document.Element("Product");
                        result = new Product(element, this);
                    }
                }
                else
                {
                    this.HandleError(response);
                }
            }

            return result;
        }

        /// <summary>
        /// Retrieves the translation of an item
        /// </summary>
        /// <param name="assetID"></param>
        /// <param name="languageCode"></param>
        /// <returns></returns>
        public ProductTranslation GetProductTranslation(Int32 assetID, String languageCode)
        {
            ProductTranslation result = null;

            Uri uri = new Uri(this.EndPoint.AbsoluteUri + "api/products/" + assetID.ToString()+ "/" + languageCode);
            
            HttpWebRequest request = CreateRequestGET(uri);

            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        XDocument document = XDocument.Load(reader);

                        XElement element = document.Element("Translation");

                        result = new ProductTranslation(element);
                    }
                }
                else
                {
                    this.HandleError(response);
                }
            }

            return result;
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Private method to deal with parsing an error response
        /// </summary>
        /// <param name="response"></param>
        private void HandleError(HttpWebResponse response)
        {
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                XElement errorElement = null;

                if (!reader.EndOfStream)
                {
                    try
                    {
                        XDocument document = XDocument.Load(reader);

                        errorElement = document.Descendants("Error").FirstOrDefault();
                    }
                    catch { }
                }

                Error error = new Error(response.StatusCode, errorElement);

                throw error.GenerateException();

            }
        }

        /// <summary>
        /// Create a GET request to get an object from the API
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        private HttpWebRequest CreateRequestGET(Uri uri)
        {
            return this.CreateRequest(uri, "GET");
        }

        /// <summary>
        /// Create a POST request that sends an object to the API
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private HttpWebRequest CreateRequestPOST(Uri uri, IXmlSerializable value = null)
        {
            HttpWebRequest request = this.CreateRequest(uri, "POST");

            if (value != null)
            {
                using (StreamWriter writer = new StreamWriter(request.GetRequestStream()))
                {
                    writer.Write(value.ToXmlString());
                }
            }
            else
            {
                request.ContentLength = 0;
            }

            return request;
        }

        /// <summary>
        /// Create a POST request that sends a string of data to the API
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="postData"></param>
        /// <returns></returns>
        private HttpWebRequest CreateRequestPOST(Uri uri, String postData)
        {
            HttpWebRequest request = this.CreateRequest(uri, "POST");

            if (postData != null)
            {
                using (StreamWriter writer = new StreamWriter(request.GetRequestStream()))
                {
                    writer.Write(postData);
                }
            }
            else
            {
                request.ContentLength = 0;
            }

            return request;
        }

        /// <summary>
        /// Create a POST request that sends the contents of a file
        /// </summary>
        /// <param name="uri">The URI to send the data to</param>
        /// <param name="fileName">The file name</param>
        /// <param name="fileData">The file contents</param>
        /// <returns></returns>
        private HttpWebRequest CreateRequestFilePOST(Uri uri, String fileName, Byte[] fileData)
        {
            HttpWebRequest request = this.CreateRequest(uri, "POST");
            request.ContentType = Utility.GetContentTypeHeader(fileName);

            if (fileData != null)
            {
                using (BinaryWriter writer = new BinaryWriter(request.GetRequestStream()))
                {
                    writer.Write(fileData);
                }
            }
            else
            {
                request.ContentLength = 0;
            }

            return request;
        }

        /// <summary>
        /// Create a PUT request that sends an object to the API
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        private HttpWebRequest CreateRequestPUT(Uri uri, IXmlSerializable value = null)
        {
            HttpWebRequest request = this.CreateRequest(uri, "PUT");

            if (value != null)
            {
                using (StreamWriter writer = new StreamWriter(request.GetRequestStream()))
                {
                    writer.Write(value.ToXmlString());
                }
            }
            else
            {
                request.ContentLength = 0;
            }

            return request;
        }

        /// <summary>
        /// Create a DELETE request that sends an object to the API
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private HttpWebRequest CreateRequestDELETE(Uri uri, IXmlSerializable value = null)
        {
            HttpWebRequest request = this.CreateRequest(uri, "DELETE");

            if (value != null)
            {
                using (StreamWriter writer = new StreamWriter(request.GetRequestStream()))
                {
                    writer.Write(value.ToXmlString());
                }
            }
            else
            {
                request.ContentLength = 0;
            }

            return request;
        }

        /// <summary>
        /// Create a request to the API
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        private HttpWebRequest CreateRequest(Uri uri, String method)
        {
            HttpWebRequest request = WebRequest.Create(uri) as HttpWebRequest;
            request.Method = method;
            request.ContentType = "text/xml";
            request.Accept = "text/xml";

            string timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffffff");
            
            request.Headers.Add("x-lod-timestamp", timestamp);
            request.Headers.Add("x-lod-version", ContentAPI.VERSION);
            request.Headers.Add("Authorization", this.GenerateAuthorizationHeader(method, "/" + uri.GetComponents(UriComponents.Path, UriFormat.UriEscaped), timestamp, ContentAPI.VERSION));

            return request;
        }

        /// <summary>
        /// Generate the authorization header that the API needs
        /// </summary>
        /// <param name="method"></param>
        /// <param name="resource"></param>
        /// <param name="timestamp"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        private String GenerateAuthorizationHeader(String method, String resource, String timestamp, String version)
        {
            StringBuilder toEncode = new StringBuilder();

            toEncode.Append(method);
            toEncode.Append(":");
            toEncode.Append(resource);
            toEncode.Append(":");
            toEncode.Append(this.SecretKey);
            toEncode.Append(":");
            toEncode.Append(timestamp);
            toEncode.Append(":");
            toEncode.Append(version);
            toEncode.Append(":");
            toEncode.Append("text/xml");

            HashAlgorithm hash = new SHA256Managed();

            byte[] encoded = hash.ComputeHash(Encoding.ASCII.GetBytes(toEncode.ToString()));

            string signature = Convert.ToBase64String(encoded);

            StringBuilder authorization = new StringBuilder();

            authorization.Append("LOD1-BASE64-SHA256 ");
            authorization.Append("KeyID=");
            authorization.Append(this.KeyId);
            authorization.Append(",Signature=");
            authorization.Append(signature);
            authorization.Append(",SignedHeaders=x-lod-timestamp;x-lod-version;accept");

            return authorization.ToString();
        }

        /// <summary>
        /// Retrieves the content of a file
        /// </summary>
        /// <param name="assetID">The asset ID of the file</param>
        /// <param name="languageCode">A language code.  For example: en-us. Required only when retrieving a translated file.</param>
        /// <returns>The contents of the file as a byte array or null if the file cannot be found</returns>
        private Byte[] GetFileCommon(String assetID, String languageCode = null)
        {
            Byte[] fileContent = null;
            Byte[] buffer = new Byte[4096];

            String uriAddress = this.EndPoint.AbsoluteUri + "api/files/" + assetID;
            if (!String.IsNullOrEmpty(languageCode))
            {
                uriAddress += "/" + languageCode;
            }
            Uri uri = new Uri(uriAddress);
            HttpWebRequest request = this.CreateRequestGET(uri);

            using (HttpWebResponse response = request.GetResponseWithoutException() as HttpWebResponse)
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        using (MemoryStream memoryStream = new MemoryStream())
                        {
                            int count = 0;
                            do
                            {
                                count = responseStream.Read(buffer, 0, buffer.Length);
                                memoryStream.Write(buffer, 0, count);

                            } while (count != 0);

                            fileContent = memoryStream.ToArray();

                        }
                    }
                }
                else
                {
                    this.HandleError(response);
                }
            }
            return fileContent;
        }


        #endregion Private Methods
    }
}
