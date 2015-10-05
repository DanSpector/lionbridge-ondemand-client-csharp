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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lionbridge.onDemand.Client.Tests
{
    public class MockContentAPI : IContentAPI
    {
        #region IContentAPI Members

        public File AddFile(string filePath)
        {
            throw new NotImplementedException();
        }

        public File AddFile(string languageCode, string filePath)
        {
            throw new NotImplementedException();
        }

        public File AddFile(string fileName, byte[] fileData)
        {
            throw new NotImplementedException();
        }

        public File AddFile(string languageCode, string fileName, byte[] fileData)
        {
            throw new NotImplementedException();
        }

        public File AddFile(string fileName, Uri fileURL)
        {
            throw new NotImplementedException();
        }

        public File AddFile(string languageCode, string fileName, Uri fileURL)
        {
            throw new NotImplementedException();
        }

        public AddCreditBalance AddPrepaidBalance(AddCreditBalance addCreditBalance)
        {
            throw new NotImplementedException();
        }

        public AddCreditBalance AddPrepaidBalance(decimal amount, string currency)
        {
            throw new NotImplementedException();
        }

        public Project AddProject(string projectName, Service service, IEnumerable<File> files, ProjectOptions options, IEnumerable<File> referenceFiles = null)
        {
            throw new NotImplementedException();
        }

        public Project AddProject(string projectName, Service service, string[] filePaths, ProjectOptions options, string[] referenceFilePaths = null)
        {
            throw new NotImplementedException();
        }

        public Project AddProject(string projectName, Service service, IEnumerable<Product> products, ProjectOptions options, IEnumerable<File> referenceFiles = null)
        {
            throw new NotImplementedException();
        }

        public QuoteAuthorization AuthorizeQuote(Quote quote)
        {
            throw new NotImplementedException();
        }

        public Account CreateAccount(CreateAccount createAccount)
        {
            throw new NotImplementedException();
        }

        public Account CreateAccount(string merchantID, string emailAddress, string firstName, string lastName, string companyName, string country, string vatID)
        {
            throw new NotImplementedException();
        }

        public string DefaultCurrency
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public Quote GenerateQuote(Service service, IEnumerable<File> files, QuoteOptions options, IEnumerable<File> referenceFiles = null)
        {
            throw new NotImplementedException();
        }

        public Quote GenerateQuote(Service service, IEnumerable<Product> products, QuoteOptions options, IEnumerable<File> referenceFiles = null)
        {
            throw new NotImplementedException();
        }

        public Quote GenerateQuote(Service service, string[] filePaths, QuoteOptions options, string[] referenceFilePaths = null)
        {
            throw new NotImplementedException();
        }

        public Quote GenerateQuote(Service service, Uri[] fileURLs, string[] fileNames, QuoteOptions options, IEnumerable<File> referenceFiles = null)
        {
            throw new NotImplementedException();
        }

        public Quote GenerateQuote(IEnumerable<Project> projects, ProjectQuoteOptions options)
        {
            throw new NotImplementedException();
        }

        public AccountInformation GetAccountInformation()
        {
            throw new NotImplementedException();
        }

        public Estimate GetEstimate(Service service, int unitCount, EstimateOptions options)
        {
            throw new NotImplementedException();
        }

        public byte[] GetFile(string assetID)
        {
            throw new NotImplementedException();
        }

        public File GetFileDetails(string assetID)
        {
            throw new NotImplementedException();
        }

        public byte[] GetFileTranslation(string assetID, string languageCode)
        {
            throw new NotImplementedException();
        }

        public Product GetProduct(int productID)
        {
            throw new NotImplementedException();
        }

        public ProductTranslation GetProductTranslation(int assetID, string languageCode)
        {
            throw new NotImplementedException();
        }

        public Project GetProject(int projectId)
        {
            throw new NotImplementedException();
        }

        public Quote GetQuote(int quoteID)
        {
            throw new NotImplementedException();
        }

        public Service GetService(int serviceID)
        {
            throw new NotImplementedException();
        }

        public System.Xml.Linq.XDocument GetTermsAndConditions()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<File> ListFiles()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Locale> ListLocales()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Product> ListProducts()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Project> ListProjects()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Service> ListServices()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Quote> ListQuotes()
        {
            throw new NotImplementedException();
        }

        public RejectQuoteStatus RejectQuote(Quote quote)
        {
            throw new NotImplementedException();
        }

        public RejectQuoteStatus RejectQuote(int quoteID)
        {
            throw new NotImplementedException();
        }

        public void UpdateFile(File file)
        {
            throw new NotImplementedException();
        }

        public void UpdateProject(Project project)
        {
            throw new NotImplementedException();
        }

        public void UpdateQuote(Quote quote)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
