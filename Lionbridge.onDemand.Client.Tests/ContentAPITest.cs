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
using System.Net;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lionbridge.onDemand.Client;

namespace Lionbridge.onDemand.Client.Tests
{
    [TestClass]
    public class ContentAPITest
    {

        private const string supportFilesPath = @"..\..\SupportFiles\";

        #region Utility Methods for Testing

        internal static ContentAPI ConstructServiceClient()
        {
            return new ContentAPI(keyId: "aZqpaIZkYRfPFrtUWiyq", secretKey: "pfnjPvrGvmdNLSABtQmarrJcKeFtovQBeVXyzWjW", endpoint: new Uri("https://developer-sandbox.liondemand.com"), defaultCurrency: "USD");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private void CleanupQuoteSetup(ContentAPI client, Quote quote)
        {
            // Never fail (future implementation of ListQuotes will help this)
            try
            {
                RejectQuoteStatus result = client.RejectQuote(quote.QuoteID);

                // Unit test breakpoint line
                if (result == RejectQuoteStatus.Success)
                {
                    result = RejectQuoteStatus.Success;
                }
            }
            catch { }
        }

        #endregion


        #region Account Tests

        /// <summary>
        /// This method verifies that account information can be retrieved
        /// </summary>
        [TestMethod]
        public void AccountInformationTest()
        {
            ContentAPI client = ConstructServiceClient();

            AccountInformation account = client.GetAccountInformation();

            Assert.IsNotNull(account);
            Assert.AreEqual<String>("daniel.spector@lionbridge.com", account.Email);
            Assert.AreEqual<String>("USD", account.Currency);
        }


        /// <summary>
        /// This method verifies that money can be added to an account.
        /// </summary>
        [TestMethod]
        public void AddCreditBalanceTest()
        {
            ContentAPI client = ConstructServiceClient();

            AddCreditBalance acb = client.AddPrepaidBalance(1.01M, "USD");

            Assert.AreEqual(1.01M, acb.Amount);
            Assert.AreEqual("USD", acb.Currency);
            Uri paymentURL = new Uri(@"https://developer-sandbox.liondemand.com/account/c6d694b5521b928283066e8cd8d42f/add_credit_balance/1.01/USD");
            Assert.AreEqual(paymentURL, acb.PaymentURL);
        }


        /// <summary>
        /// This method verifies the CreateAccount constructor
        /// </summary>
        [TestMethod]
        public void CreateAccountConstructorTest()
        {
            ContentAPI client = ConstructServiceClient();

            long ticks = DateTime.Now.Ticks;
            String merchantID = ticks.ToString();
            String email = "test@" + merchantID + ".com";
            String firstName = "Unit";
            String lastName = "Test";
            String company = merchantID + " Inc.";
            String country = "US";
            String vatID = "12334455544";

            Account account = client.CreateAccount(merchantID, email, firstName, lastName, company, country, vatID);
            CreateAccountCommonAsserts(account, merchantID, email, firstName, lastName, company, country);
        }

        /// <summary>
        /// This method verifies that an account can be created without a VAT ID when the merchant is outside of Ireland
        /// </summary>
        [TestMethod]
        public void CreateAccountUSNoVatIDTest()
        {
            ContentAPI client = ConstructServiceClient();

            long ticks = DateTime.Now.Ticks;
            String merchantID = ticks.ToString();
            String email = "test@" + merchantID + ".com";
            String firstName = "Unit";
            String lastName = "Test";
            String company = merchantID + " Inc.";
            String country = "US";
            String vatID = "";

            Account account = client.CreateAccount(merchantID, email, firstName, lastName, company, country, vatID);
            CreateAccountCommonAsserts(account, merchantID, email, firstName, lastName, company, country);
        }

        /// <summary>
        /// This method verifies that an account cannot be created without a VAT ID when the merchant is in Ireland (IE)
        /// </summary>
        [TestMethod]
        //[ExpectedException(typeof(OnDemandClientException))]
        public void CreateAccountIENoVatIDTest()
        {
            ContentAPI client = ConstructServiceClient();

            long ticks = DateTime.Now.Ticks;
            String merchantID = ticks.ToString();
            String email = "test@" + merchantID + ".com";
            String firstName = "Unit";
            String lastName = "Test";
            String company = merchantID + " Inc.";
            String country = "IE";
            String vatID = "";

            // This request should throw an exception because VAT ID's are required in Ireland (IE)
            Account account = client.CreateAccount(merchantID, email, firstName, lastName, company, country, vatID);
            CreateAccountCommonAsserts(account, merchantID, email, firstName, lastName, company, country);
        }

        /// <summary>
        /// This method verifies that you cannot create an account with an invalid country code (XX)
        /// </summary>
        [TestMethod]
        public void CreateAccountBadCountryTest()
        {
            ContentAPI client = ConstructServiceClient();

            long ticks = DateTime.Now.Ticks;
            String merchantID = ticks.ToString();
            String email = "test@" + merchantID + ".com";
            String firstName = "Unit";
            String lastName = "Test";
            String company = merchantID + " Inc.";
            String country = "XX";
            String vatID = "12334455544";

            try
            {
                Account account = client.CreateAccount(merchantID, email, firstName, lastName, company, country, vatID);
                Assert.Fail("Created an account with an invalid invalid country code.");
            }
            catch (OnDemandClientException odce)
            {
                Assert.AreEqual<Int32>(400, odce.ReasonCode);
                Assert.IsNotNull(odce.SimpleMessage);
                Assert.IsNotNull(odce.DetailedMessage);
            }
        }


        /// <summary>
        /// This method verifies that you can't create an account using an email address that already exists for another account
        /// </summary>
        [TestMethod]
        public void CreateAccountEmailConflictTest()
        {
            ContentAPI client = ConstructServiceClient();

            long ticks = DateTime.Now.Ticks;
            String merchantID = ticks.ToString();
            String email = "test@" + merchantID + ".com";
            String firstName = "Unit";
            String lastName = "Test";
            String company = merchantID + " Inc.";
            String country = "US";
            String vatID = "12334455544";

            Account account1 = client.CreateAccount(merchantID, email, firstName, lastName, company, country, vatID);
            Assert.IsNotNull(account1, "The account should have been created.");

            try
            {
                Account account2 = client.CreateAccount(merchantID, email, firstName, lastName, company, country, vatID);
                Assert.Fail("Created an account with a duplicate email address.");
            }
            catch (OnDemandClientException odce)
            {
                Assert.AreEqual<HttpStatusCode>(HttpStatusCode.Conflict, odce.HttpStatusCode);
                Assert.AreEqual<Int32>(404, odce.ReasonCode);
                Assert.IsNotNull(odce.SimpleMessage);
                Assert.IsNotNull(odce.DetailedMessage);
            }
        }

        /// <summary>
        /// This method contains a series of common asserts for the Create Account method.
        /// </summary>
        /// <param name="account">The Account object</param>
        /// <param name="merchantID">Merchant system account ID.</param>
        /// <param name="email">Email address of the primary user.</param>
        /// <param name="firstName">First name of the primary user (optional).</param>
        /// <param name="lastName">Last name of the primary user (optional).</param>
        /// <param name="company">Merchant Company.</param>
        /// <param name="country">Country that the merchant is headquartered in. ISO 3166-1 2 character country code.</param>
        private static void CreateAccountCommonAsserts(Account account, String merchantID, String email, String firstName, String lastName, String company, String country)
        {
            Assert.IsNotNull(account.MerchantID);
            Assert.AreEqual(merchantID, account.MerchantID);
            Assert.AreEqual(email, account.Email);
            Assert.AreEqual(firstName, account.FirstName);
            Assert.AreEqual(lastName, account.LastName);
            Assert.AreEqual(company, account.CompanyName);
            Assert.AreEqual(country, account.Country);
            Assert.AreEqual(20, account.AccessKeyID.Length);
            Assert.AreEqual(40, account.SecretAccessKey.Length);
        }


        #endregion


        #region File Tests

        [TestMethod]
        public void AddFileDocX()
        {
            IContentAPI servicePortal = ConstructServiceClient();

            File file = servicePortal.AddFile("en-us", supportFilesPath + "Use live layout and alignment guides.docx");

            Assert.IsNotNull(file);
            Assert.IsTrue(file.AssetID > 0);
        }


        /// <summary>
        /// This method verifies that an OnDemandClientException is thrown when getting a file if the AssetID references an unknown file.
        /// </summary>
        [TestMethod]
        public void GetFileUnknownTest()
        {
            String assetID = "0";
            ContentAPI client = ConstructServiceClient();

            try
            {
                Byte[] content = client.GetFile(assetID);
                Assert.Fail("A file with an invalid asset ID should not have been retrieved.");
            }
            catch (OnDemandClientException odce)
            {
                Assert.AreEqual<HttpStatusCode>(HttpStatusCode.NotFound, odce.HttpStatusCode);
                Assert.AreEqual<Int32>(0, odce.ReasonCode);
                Assert.IsNotNull(odce.SimpleMessage);
                Assert.IsNotNull(odce.DetailedMessage);
            }
        }

        /// <summary>
        /// This method verifies that an ArgumentException is thrown when getting a file if the AssetID is null.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetFileNullTest()
        {
            String assetID = null;
            ContentAPI client = ConstructServiceClient();
            Byte[] content = client.GetFile(assetID);
        }


        /// <summary>
        /// This method verifies that an ArgumentException is thrown when getting a file if the AssetID is empty.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetFileEmptyTest()
        {
            String assetID = String.Empty;
            ContentAPI client = ConstructServiceClient();
            Byte[] content = client.GetFile(assetID);
        }

        /// <summary>
        /// This method verifies that a OnDemandClientException is thrown when requesting a non-existent translated file.
        /// </summary>
        [TestMethod]
        public void GetFileTranslationTest()
        {
            ContentAPI client = ConstructServiceClient();
            IEnumerable<File> files = client.ListFiles();

            foreach (File file in files)
            {
                int assetID = file.AssetID;
                String languageCode = "en-us";

                try
                {
                    Byte[] content = client.GetFileTranslation(assetID.ToString(), languageCode);
                    Assert.Fail("A file with an invalid translation should not have been retrieved.");
                }
                catch (OnDemandClientException odce)
                {
                    Assert.AreEqual<HttpStatusCode>(HttpStatusCode.NotFound, odce.HttpStatusCode);
                    Assert.AreEqual<Int32>(0, odce.ReasonCode);
                    Assert.IsNotNull(odce.SimpleMessage);
                }

                break;
            }
        }

        /// <summary>
        /// This method verifies that an OnDemandClientException is thrown when getting a translated file if the AssetID references an unknown file.
        /// </summary>
        [TestMethod]
        public void GetFileTranslationUnknownTest()
        {
            String assetID = "0";
            String languageCode = "en-us";
            ContentAPI client = ConstructServiceClient();

            try
            {
                Byte[] content = client.GetFileTranslation(assetID, languageCode);
                Assert.Fail("A file with an invalid Asset ID should not have been retrieved.");
            }
            catch (OnDemandClientException odce)
            {
                Assert.AreEqual<HttpStatusCode>(HttpStatusCode.NotFound, odce.HttpStatusCode);
                Assert.AreEqual<Int32>(0, odce.ReasonCode);
                Assert.IsNotNull(odce.SimpleMessage);
            }
        }

        /// <summary>
        /// This method verifies that an ArgumentNullException is thrown when getting a translated file if the AssetID supplied is null.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetFileTranslationNullTest()
        {
            String assetID = null;
            String languageCode = "en-us";
            ContentAPI client = ConstructServiceClient();
            Byte[] content = client.GetFileTranslation(assetID, languageCode);
        }


        /// <summary>
        /// This method verifies that an ArgumentException is thrown when getting a translated file if the AssetID supplied is empty. 
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetFileTranslationEmptyTest()
        {
            String assetID = String.Empty;
            String languageCode = "en-us";
            ContentAPI client = ConstructServiceClient();
            Byte[] content = client.GetFileTranslation(assetID, languageCode);
        }

        /// <summary>
        /// This method verifies that an ArgumentNullException is thrown when getting a translated file if the AssetID supplied is valid but the language code is null.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetFileTranslationValidNullTest()
        {
            String assetID = "1234";
            String languageCode = null;
            ContentAPI client = ConstructServiceClient();
            Byte[] content = client.GetFileTranslation(assetID, languageCode);
        }


        /// <summary>
        /// This method verifies that an ArgumentException is thrown when getting a translated file if the AssetID supplied is valid but the language code is empty.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetFileTranslationValidEmptyTest()
        {
            String assetID = "1234";
            String languageCode = String.Empty;
            ContentAPI client = ConstructServiceClient();
            Byte[] content = client.GetFileTranslation(assetID, languageCode);
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void ListServicesTest()
        {
            ContentAPI client = ConstructServiceClient();

            IEnumerable<Service> services = client.ListServices();

            Assert.AreEqual(5, services.Count());
            Assert.IsTrue(services.FirstOrDefault().Name == "Pseudo Translation");
            Assert.IsFalse(services.ToArray()[1].Name == "Translation");
            Assert.IsTrue(services.ToArray()[1].Name == "Machine Translation");
            Assert.IsTrue(services.FirstOrDefault().ServiceID == 146);
            Assert.IsTrue(services.ToArray()[1].ServiceID == 144);
        }

        [TestMethod]
        [ExpectedException(typeof(WebException))]
        public void GetProjectsTest()
        {
            ContentAPI client = ConstructServiceClient();
            Project project = client.GetProject(10001);
        }

        [TestMethod]
        public void ListProjectsTest()
        {
            ContentAPI client = ConstructServiceClient();

            IEnumerable<Project> projects = client.ListProjects();
            Assert.IsTrue(projects.Count() > 0);
        }

        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(OnDemandClientException))]
        public void GetNullQuoteTest()
        {
            ContentAPI client = ConstructServiceClient();

            Quote quote = client.GetQuote(0);

            Assert.IsNull(quote);
        }

        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AuthorizeNullQuoteTest()
        {
            ContentAPI client = ConstructServiceClient();

            Quote authorizeQuote = new Quote(0, DateTime.Now, 0, 0, "", 0m, 0m, 0m, null);
            QuoteAuthorization quoteAuthorization = client.AuthorizeQuote(authorizeQuote);

            Assert.IsNull(quoteAuthorization);
        }

        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(OnDemandClientException))]
        public void RejectNullQuoteTest()
        {
            ContentAPI client = ConstructServiceClient();

            RejectQuoteStatus result = client.RejectQuote(0);

            Assert.AreEqual(result, RejectQuoteStatus.BadRequest);
        }

        /// <summary>
        /// Tests that the GetTermsAndConditions returns valid XHTML with an html element, head, and body
        /// </summary>
        [TestMethod]
        public void GetTermsAndConditionsTest()
        {
            ContentAPI client = ConstructServiceClient();

            XDocument terms = client.GetTermsAndConditions();

            Assert.IsNotNull(terms);

            Assert.IsNotNull(terms.Element("html"));
            Assert.IsNotNull(terms.Element("html").Element("head"));
            Assert.IsNotNull(terms.Element("html").Element("head"));

        }

        /// <summary>
        /// This method verifies that requests fail if they don't contain the required authorization data
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(OnDemandClientException))]
        public void AuthorizationTest()        
        {
            ContentAPI client = new ContentAPI(keyId: "aZqpaIZkYRfPFrtUWiyq", secretKey: "", endpoint: new Uri("https://developer-sandbox.liondemand.com"));

            try
            {
                XDocument terms = client.GetTermsAndConditions();
                Assert.Fail("A request without a secret key should not be sucessful.");
            }
            catch (OnDemandClientException odce)
            {
                Assert.AreEqual<HttpStatusCode>(HttpStatusCode.Unauthorized, odce.HttpStatusCode);
                Assert.AreEqual<Int32>(0, odce.ReasonCode);
                Assert.IsNotNull(odce.SimpleMessage);
                Assert.IsNotNull(odce.DetailedMessage);

                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void ListLocalesTest()
        {
            ContentAPI client = ConstructServiceClient();

            IEnumerable<Locale> locales = client.ListLocales();

            Assert.IsNotNull(locales);

            Assert.AreNotEqual(0, locales.Count());
        }
    }
}
