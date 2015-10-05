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
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lionbridge.onDemand.Client.Tests
{
    [TestClass]
    public class QuoteTest
    {
        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void ConstructorTest()
        {
            Int32 quoteID = 25;
            SourceLanguage sourceLanguage = new SourceLanguage("en-gb");
            TargetLanguage[] targetLanguage = new TargetLanguage[2] { new TargetLanguage("en-us"), 
                                                                      new TargetLanguage("it-it")};
            Int32 totalTranslations = 14;
            Int32 translationCredit = 4;
            String currency = "USD";
            Decimal totalCost = 100.54m;
            Decimal prepaidCredit = 30.4m;
            Decimal amountDue = 1.4m;
            DateTime creationDate = DateTime.Now;

            var projects = new List<Project>();

            var quote = new Quote(quoteID: quoteID, 
                                 creationDate: creationDate, 
                                 totalTranslations: totalTranslations,
                                 translationCredit: translationCredit,
                                 currency: currency, 
                                 totalCost: totalCost, 
                                 prepaidCredit: prepaidCredit, 
                                 amountDue: amountDue,
                                 projects: projects);

            Assert.AreEqual(quote.QuoteID, quoteID);
            Assert.AreEqual(quote.TotalTranslations, totalTranslations);
            Assert.AreEqual(quote.TranslationCredit, translationCredit);
            Assert.AreEqual(quote.Currency, currency);
            Assert.AreEqual(quote.TotalCost, totalCost);
            Assert.AreEqual(quote.PrepaidCredit, prepaidCredit);
            Assert.AreEqual(quote.AmountDue, amountDue);
        }

        [TestMethod]
        public void ProductQuoteResponseTest()
        {
            string xml = @"<?xml version='1.0' encoding='UTF-8'?>
                <Quote>
                    <QuoteID>132</QuoteID>
                    <CreationDate>2014-01-25T10:32:02Z</CreationDate>
                    <Status>Pending</Status>
                    <AuthorizeURL>https://</AuthorizeURL>
                    <RejectURL>https://</RejectURL>
                    <TotalTranslations>2</TotalTranslations>
                    <TranslationCredit>1</TranslationCredit>
                    <TotalCost>1.70</TotalCost>
                    <PrepaidCredit>5.00</PrepaidCredit>
                    <AmountDue>5.00</AmountDue>
                    <Currency>EUR</Currency>

                    <Projects>
                            <Project>
                                <ProjectID>999</ProjectID>
                                <ProjectName>Name of project</ProjectName>
                                <ServiceID>54</ServiceID>
                                <SourceLanguage>
                                    <LanguageCode>en-gb</LanguageCode>
                                </SourceLanguage>
                                <TargetLanguages>
                                            <TargetLanguage>
                                                <LanguageCode>it-it</LanguageCode>
                                            </TargetLanguage>
                                            <TargetLanguage>
                                                <LanguageCode>fr-fr</LanguageCode>
                                            </TargetLanguage>
                                </TargetLanguages>
                                <Products>
                                        <Product>
                                            <AssetID>999</AssetID>
                                            <SKUs>
                                                <SKU>
                                                    <SKUNumber>123</SKUNumber>
                                                </SKU>
                                            </SKUs>
                                            <DueDate>2014-02-11T10:22:46Z</DueDate>
                                        </Product>
                                </Products>
                                <ReferenceFiles>
                                    <ReferenceFile>
                                        <AssetID>12345</AssetID>
                                        <FileName>my-file.txt</FileName>
                                        <URL>https://ondemand.liondemand.com/api/files/12345</URL>
                                        <TargetLanguages />
                                    </ReferenceFile>
                                    <ReferenceFile>
                                        <AssetID>12346</AssetID>
                                        <FileName>my-file.txt</FileName>
                                        <URL>https://ondemand.liondemand.com/api/files/12346</URL>
                                        <TargetLanguages />
                                    </ReferenceFile>
                                </ReferenceFiles>
                            </Project>
                    </Projects>
                </Quote>
            ";

            XDocument document = XDocument.Parse(xml);

            var quote = new Quote(document.Element("Quote"), new MockContentAPI());

            Assert.AreEqual(132, quote.QuoteID);
            Assert.AreEqual(DateTime.Parse("2014-01-25T10:32:02Z"), quote.CreationDate);
            Assert.AreEqual("Pending", quote.Status);
            Assert.AreEqual("https://", quote.AuthorizeURL);
            Assert.AreEqual("https://", quote.RejectURL);
            Assert.AreEqual("", quote.PaymentURL);
            Assert.AreEqual(2, quote.TotalTranslations);
            Assert.AreEqual(1, quote.TranslationCredit);
            Assert.AreEqual(1.70m, quote.TotalCost);
            Assert.AreEqual(5.00m, quote.PrepaidCredit);
            Assert.AreEqual(5.00m, quote.AmountDue);
            Assert.AreEqual("EUR", quote.Currency);

            Assert.IsNotNull(quote.Projects);
            Assert.AreEqual(1, quote.Projects.Count());

            var project = quote.Projects.FirstOrDefault();

            Assert.AreEqual(999, project.ProjectID);
            Assert.AreEqual("Name of project", project.Name);
            Assert.AreEqual(54, project.ServiceID);
            Assert.AreEqual("en-gb", project.SourceLanguage);
            Assert.AreEqual(2, project.TargetLanguages.Count());
            Assert.AreEqual("it-it", project.TargetLanguages.FirstOrDefault());
            Assert.AreEqual("fr-fr", project.TargetLanguages.LastOrDefault());

            Assert.IsNotNull(project.Products);
            Assert.AreEqual(1, project.Products.Count());

            var product = project.Products.FirstOrDefault();

            Assert.AreEqual(999, product.AssetID);
            Assert.AreEqual(1, product.SKUs.Count());
            Assert.AreEqual("123", product.SKUs.FirstOrDefault().SKUNumber);
            Assert.AreEqual(DateTime.Parse("2014-02-11T10:22:46Z"), product.DueDate);

            Assert.IsNotNull(project.ReferenceFiles);
            Assert.AreEqual(2, project.ReferenceFiles.Count());

            Assert.AreEqual(12345, project.ReferenceFiles.FirstOrDefault().AssetID);
            Assert.AreEqual("my-file.txt", project.ReferenceFiles.FirstOrDefault().Name);
            Assert.AreEqual(new Uri("https://ondemand.liondemand.com/api/files/12345"), project.ReferenceFiles.FirstOrDefault().URL);
            Assert.AreEqual(0, project.ReferenceFiles.FirstOrDefault().TargetLanguages.Count());

            Assert.AreEqual(12346, project.ReferenceFiles.LastOrDefault().AssetID);
            Assert.AreEqual("my-file.txt", project.ReferenceFiles.LastOrDefault().Name);
            Assert.AreEqual(new Uri("https://ondemand.liondemand.com/api/files/12346"), project.ReferenceFiles.LastOrDefault().URL);
            Assert.AreEqual(0, project.ReferenceFiles.LastOrDefault().TargetLanguages.Count());
        }

        /// <summary>
        /// If the price is not yet ready, the response will look like:
        /// </summary>
        [TestMethod]
        public void ProductQuoteResponseNotReadyTest()
        {
            string xml = @"<?xml version='1.0' encoding='UTF-8'?>
                <Quote>
                    <QuoteID>132</QuoteID>
                    <CreationDate>2014-01-25T10:32:02Z</CreationDate>
                    <Status>Calculating</Status>
                    <TotalTranslations>2</TotalTranslations>
                    <TranslationCredit>1</TranslationCredit>
                    <TotalCost/>
                    <PrepaidCredit>5.00</PrepaidCredit>
                    <AmountDue/>
                    <Currency>EUR</Currency>

                    <Projects>
                            <Project>
                                <ProjectID>999</ProjectID>
                                <ProjectName>Name of project</ProjectName>
                                <ServiceID>54</ServiceID>
                                <SourceLanguage>
                                    <LanguageCode>en-gb</LanguageCode>
                                </SourceLanguage>
                                <TargetLanguages>
                                            <TargetLanguage>
                                                <LanguageCode>it-it</LanguageCode>
                                            </TargetLanguage>
                                            <TargetLanguage>
                                                <LanguageCode>fr-fr</LanguageCode>
                                            </TargetLanguage>
                                </TargetLanguages>
                                <Products>
                                        <Product>
                                            <AssetID>999</AssetID>
                                            <SKUs>
                                                <SKU>
                                                    <SKUNumber>123</SKUNumber>
                                                </SKU>
                                            </SKUs>
                                        </Product>
                                </Products>
                                <ReferenceFiles>
                                    <ReferenceFile>
                                        <AssetID>12345</AssetID>
                                        <FileName>my-file.txt</FileName>
                                        <URL>https://ondemand.liondemand.com/api/files/12345</URL>
                                        <TargetLanguages />
                                    </ReferenceFile>
                                    <ReferenceFile>
                                        <AssetID>12346</AssetID>
                                        <FileName>my-file.txt</FileName>
                                        <URL>https://ondemand.liondemand.com/api/files/12346</URL>
                                        <TargetLanguages />
                                    </ReferenceFile>
                                </ReferenceFiles>
                            </Project>
                    </Projects>
                </Quote>
            ";

            XDocument document = XDocument.Parse(xml);

            var quote = new Quote(document.Element("Quote"), new MockContentAPI());

            Assert.AreEqual(132, quote.QuoteID);
            Assert.AreEqual(DateTime.Parse("2014-01-25T10:32:02Z"), quote.CreationDate);
            Assert.AreEqual("Calculating", quote.Status);
            Assert.AreEqual("", quote.AuthorizeURL);
            Assert.AreEqual("", quote.RejectURL);
            Assert.AreEqual("", quote.PaymentURL);
            Assert.AreEqual(2, quote.TotalTranslations);
            Assert.AreEqual(1, quote.TranslationCredit);
            Assert.AreEqual(0, quote.TotalCost);
            Assert.AreEqual(5.00m, quote.PrepaidCredit);
            Assert.AreEqual(0, quote.AmountDue);
            Assert.AreEqual("EUR", quote.Currency);

            Assert.IsNotNull(quote.Projects);
            Assert.AreEqual(1, quote.Projects.Count());

            var project = quote.Projects.FirstOrDefault();

            Assert.AreEqual(999, project.ProjectID);
            Assert.AreEqual("Name of project", project.Name);
            Assert.AreEqual(54, project.ServiceID);
            Assert.AreEqual("en-gb", project.SourceLanguage);
            Assert.AreEqual(2, project.TargetLanguages.Count());
            Assert.AreEqual("it-it", project.TargetLanguages.FirstOrDefault());
            Assert.AreEqual("fr-fr", project.TargetLanguages.LastOrDefault());

            Assert.IsNotNull(project.Products);
            Assert.AreEqual(1, project.Products.Count());

            var product = project.Products.FirstOrDefault();

            Assert.AreEqual(999, product.AssetID);
            Assert.AreEqual(1, product.SKUs.Count());
            Assert.AreEqual("123", product.SKUs.FirstOrDefault().SKUNumber);

            Assert.IsNotNull(project.ReferenceFiles);
            Assert.AreEqual(2, project.ReferenceFiles.Count());

            Assert.AreEqual(12345, project.ReferenceFiles.FirstOrDefault().AssetID);
            Assert.AreEqual("my-file.txt", project.ReferenceFiles.FirstOrDefault().Name);
            Assert.AreEqual(new Uri("https://ondemand.liondemand.com/api/files/12345"), project.ReferenceFiles.FirstOrDefault().URL);
            Assert.AreEqual(0, project.ReferenceFiles.FirstOrDefault().TargetLanguages.Count());

            Assert.AreEqual(12346, project.ReferenceFiles.LastOrDefault().AssetID);
            Assert.AreEqual("my-file.txt", project.ReferenceFiles.LastOrDefault().Name);
            Assert.AreEqual(new Uri("https://ondemand.liondemand.com/api/files/12346"), project.ReferenceFiles.LastOrDefault().URL);
            Assert.AreEqual(0, project.ReferenceFiles.LastOrDefault().TargetLanguages.Count());
        }

        [TestMethod]
        public void GetQuoteProductResponseAuthorizedTest()
        {
            string xml = @"<?xml version='1.0' encoding='UTF-8'?>
                <Quote>
                    <QuoteID>132</QuoteID>
                    <CreationDate>2014-01-25T10:32:02Z</CreationDate>
                    <Status>Authorized</Status>
                    <TotalCost>10.00</TotalCost>
                    <Currency>EUR</Currency>
                    <Payments>
                        <Payment>
                            <PaymentType>PayPal</PaymentType>
                            <PaymentDescription>PayPal charge to buyer@example.com</PaymentDescription>
                            <PaymentAmount>10.00</PaymentAmount>
                            <PaymentCurrency>EUR</PaymentCurrency>
                        </Payment>
                    </Payments>
                    <Projects>
                        <Project>
                            <ProjectID>123</ProjectID>
                            <ProjectURL>https://</ProjectURL>
                            <ProjectDueDate>2014-02-11T10:22:46Z</ProjectDueDate>
                            <ServiceID>54</ServiceID>
                            <SourceLanguage>
                                <LanguageCode>en-gb</LanguageCode>
                            </SourceLanguage>
                            <TargetLanguages>
                                <TargetLanguage>
                                    <LanguageCode>it-it</LanguageCode>
                                </TargetLanguage>
                                <TargetLanguage>
                                    <LanguageCode>fr-fr</LanguageCode>
                                </TargetLanguage>
                            </TargetLanguages>
                            <Products>
                                <Product>
                                    <AssetID>999</AssetID>
                                    <SKUs>
                                        <SKU>
                                            <SKUNumber>123</SKUNumber>
                                        </SKU>
                                    </SKUs>
                                </Product>
                            </Products>
                        </Project>
                    </Projects>
                </Quote>
            ";

            XDocument document = XDocument.Parse(xml);

            var quote = new Quote(document.Element("Quote"), new MockContentAPI());

            Assert.AreEqual(132, quote.QuoteID);
            Assert.AreEqual(DateTime.Parse("2014-01-25T10:32:02Z"), quote.CreationDate);
            Assert.AreEqual("Authorized", quote.Status);
            Assert.AreEqual("", quote.AuthorizeURL);
            Assert.AreEqual("", quote.RejectURL);
            Assert.AreEqual("", quote.PaymentURL);
            Assert.AreEqual(0, quote.TotalTranslations);
            Assert.AreEqual(0, quote.TranslationCredit);
            Assert.AreEqual(10.00m, quote.TotalCost);
            Assert.AreEqual(0.00m, quote.PrepaidCredit);
            Assert.AreEqual(0, quote.AmountDue);
            Assert.AreEqual("EUR", quote.Currency);

            Assert.IsNotNull(quote.Payments);
            Assert.AreEqual(1, quote.Payments.Count());

            var payment = quote.Payments.FirstOrDefault();

            Assert.AreEqual("PayPal", payment.Type);
            Assert.AreEqual("PayPal charge to buyer@example.com", payment.Description);
            Assert.AreEqual(10.00m, payment.Amount);
            Assert.AreEqual("EUR", payment.Currency);


            Assert.IsNotNull(quote.Projects);
            Assert.AreEqual(1, quote.Projects.Count());

            var project = quote.Projects.FirstOrDefault();

            Assert.AreEqual(123, project.ProjectID);
            Assert.AreEqual(null, project.URL);
            Assert.AreEqual(DateTime.Parse("2014-02-11T10:22:46Z"), project.DueDate);
            Assert.AreEqual(54, project.ServiceID);
            Assert.AreEqual("en-gb", project.SourceLanguage);
            Assert.AreEqual(2, project.TargetLanguages.Count());
            Assert.AreEqual("it-it", project.TargetLanguages.FirstOrDefault());
            Assert.AreEqual("fr-fr", project.TargetLanguages.LastOrDefault());

            Assert.IsNotNull(project.Products);
            Assert.AreEqual(1, project.Products.Count());

            var product = project.Products.FirstOrDefault();

            Assert.AreEqual(999, product.AssetID);
            Assert.AreEqual(1, product.SKUs.Count());
            Assert.AreEqual("123", product.SKUs.FirstOrDefault().SKUNumber);
        }

        [TestMethod]
        public void FileQuoteResponseTest()
        {
            string xml = @"<?xml version='1.0' encoding='UTF-8'?>
                <Quote>
                    <QuoteID>132</QuoteID>
                    <CreationDate>2014-01-25T10:32:02Z</CreationDate>
                    <Status>Pending</Status>
                    <AuthorizeURL>https://</AuthorizeURL>
                    <RejectURL>https://</RejectURL>
                    <TotalCost>10.00</TotalCost>
                    <PrepaidCredit>5.00</PrepaidCredit>
                    <AmountDue>5.00</AmountDue>
                    <Currency>EUR</Currency>

                    <Projects>
                            <Project>
                                <ProjectID>999</ProjectID>
                                <ProjectName>Name of project</ProjectName>
                                <ServiceID>54</ServiceID>
                                <SourceLanguage>
                                    <LanguageCode>en-gb</LanguageCode>
                                </SourceLanguage>
                                <TargetLanguages>
                                            <TargetLanguage>
                                                <LanguageCode>it-it</LanguageCode>
                                            </TargetLanguage>
                                            <TargetLanguage>
                                                <LanguageCode>fr-fr</LanguageCode>
                                            </TargetLanguage>
                                </TargetLanguages>
                                <Files>
                                        <File>
                                            <AssetID>999</AssetID>
                                            <FileName>example.txt</FileName>
                                        </File>
                                </Files>
                                <ReferenceFiles>
                                    <ReferenceFile>
                                        <AssetID>12345</AssetID>
                                        <FileName>my-file.txt</FileName>
                                        <URL>https://ondemand.liondemand.com/api/files/12345</URL>
                                        <TargetLanguages />
                                    </ReferenceFile>
                                    <ReferenceFile>
                                        <AssetID>12346</AssetID>
                                        <FileName>my-file.txt</FileName>
                                        <URL>https://ondemand.liondemand.com/api/files/12346</URL>
                                        <TargetLanguages />
                                    </ReferenceFile>
                                </ReferenceFiles>
                            </Project>
                    </Projects>
                </Quote>
            ";

            XDocument document = XDocument.Parse(xml);

            var quote = new Quote(document.Element("Quote"), new MockContentAPI());

            Assert.AreEqual(132, quote.QuoteID);
            Assert.AreEqual(DateTime.Parse("2014-01-25T10:32:02Z"), quote.CreationDate);
            Assert.AreEqual("Pending", quote.Status);
            Assert.AreEqual("https://", quote.AuthorizeURL);
            Assert.AreEqual("https://", quote.RejectURL);
            Assert.AreEqual("", quote.PaymentURL);
            Assert.AreEqual(0, quote.TotalTranslations);
            Assert.AreEqual(0, quote.TranslationCredit);
            Assert.AreEqual(10.00m, quote.TotalCost);
            Assert.AreEqual(5.00m, quote.PrepaidCredit);
            Assert.AreEqual(5.00m, quote.AmountDue);
            Assert.AreEqual("EUR", quote.Currency);

            Assert.IsNotNull(quote.Projects);
            Assert.AreEqual(1, quote.Projects.Count());

            var project = quote.Projects.FirstOrDefault();

            Assert.AreEqual(999, project.ProjectID);
            Assert.AreEqual("Name of project", project.Name);
            Assert.AreEqual(54, project.ServiceID);
            Assert.AreEqual("en-gb", project.SourceLanguage);
            Assert.AreEqual(2, project.TargetLanguages.Count());
            Assert.AreEqual("it-it", project.TargetLanguages.FirstOrDefault());
            Assert.AreEqual("fr-fr", project.TargetLanguages.LastOrDefault());

            Assert.IsNotNull(project.Files);
            Assert.AreEqual(1, project.Files.Count());

            Assert.AreEqual(999, project.Files.FirstOrDefault().AssetID);
            Assert.AreEqual("example.txt", project.Files.FirstOrDefault().Name);

            Assert.IsNotNull(project.ReferenceFiles);
            Assert.AreEqual(2, project.ReferenceFiles.Count());

            Assert.AreEqual(12345, project.ReferenceFiles.FirstOrDefault().AssetID);
            Assert.AreEqual("my-file.txt", project.ReferenceFiles.FirstOrDefault().Name);
            Assert.AreEqual(new Uri("https://ondemand.liondemand.com/api/files/12345"), project.ReferenceFiles.FirstOrDefault().URL);
            Assert.AreEqual(0, project.ReferenceFiles.FirstOrDefault().TargetLanguages.Count());

            Assert.AreEqual(12346, project.ReferenceFiles.LastOrDefault().AssetID);
            Assert.AreEqual("my-file.txt", project.ReferenceFiles.LastOrDefault().Name);
            Assert.AreEqual(new Uri("https://ondemand.liondemand.com/api/files/12346"), project.ReferenceFiles.LastOrDefault().URL);
            Assert.AreEqual(0, project.ReferenceFiles.LastOrDefault().TargetLanguages.Count());
        }

        /// <summary>
        /// If the price is not yet ready, the response will look like:
        /// </summary>
        [TestMethod]
        public void FileQuoteResponseNotReadyTest()
        {
            string xml = @"<?xml version='1.0' encoding='UTF-8'?>
                <Quote>
                    <QuoteID>132</QuoteID>
                    <CreationDate>2014-01-25T10:32:02Z</CreationDate>
                    <Status>Calculating</Status>
                    <TotalCost/>
                    <PrepaidCredit>5.00</PrepaidCredit>
                    <AmountDue/>
                    <Currency>EUR</Currency>
                    <Projects>
                        <Project>
                            <Files>
                                <File>
                                    <AssetID>999</AssetID>
                                    <FileName>example.txt</FileName>
                                </File>
                            </Files>
                            <ReferenceFiles>
                                <ReferenceFile>
                                    <AssetID>12345</AssetID>
                                    <FileName>my-file.txt</FileName>
                                    <URL>https://ondemand.liondemand.com/api/files/12345</URL>
                                    <TargetLanguages />
                                </ReferenceFile>
                                <ReferenceFile>
                                    <AssetID>12346</AssetID>
                                    <FileName>my-file.txt</FileName>
                                    <URL>https://ondemand.liondemand.com/api/files/12346</URL>
                                    <TargetLanguages />
                                </ReferenceFile>
                            </ReferenceFiles>
                        </Project>
                    </Projects>
                </Quote>
            ";

            XDocument document = XDocument.Parse(xml);

            var quote = new Quote(document.Element("Quote"), new MockContentAPI());

            Assert.AreEqual(132, quote.QuoteID);
            Assert.AreEqual(DateTime.Parse("2014-01-25T10:32:02Z"), quote.CreationDate);
            Assert.AreEqual("Calculating", quote.Status);
            Assert.AreEqual("", quote.AuthorizeURL);
            Assert.AreEqual("", quote.RejectURL);
            Assert.AreEqual("", quote.PaymentURL);
            Assert.AreEqual(0, quote.TotalTranslations);
            Assert.AreEqual(0, quote.TranslationCredit);
            Assert.AreEqual(0, quote.TotalCost);
            Assert.AreEqual(5.00m, quote.PrepaidCredit);
            Assert.AreEqual(0, quote.AmountDue);
            Assert.AreEqual("EUR", quote.Currency);

            Assert.IsNotNull(quote.Projects);
            Assert.AreEqual(1, quote.Projects.Count());

            var project = quote.Projects.FirstOrDefault();


            Assert.IsNotNull(project.Files);
            Assert.AreEqual(1, project.Files.Count());

            Assert.AreEqual(999, project.Files.FirstOrDefault().AssetID);
            Assert.AreEqual("example.txt", project.Files.FirstOrDefault().Name);

            Assert.IsNotNull(project.ReferenceFiles);
            Assert.AreEqual(2, project.ReferenceFiles.Count());

            Assert.AreEqual(12345, project.ReferenceFiles.FirstOrDefault().AssetID);
            Assert.AreEqual("my-file.txt", project.ReferenceFiles.FirstOrDefault().Name);
            Assert.AreEqual(new Uri("https://ondemand.liondemand.com/api/files/12345"), project.ReferenceFiles.FirstOrDefault().URL);
            Assert.AreEqual(0, project.ReferenceFiles.FirstOrDefault().TargetLanguages.Count());

            Assert.AreEqual(12346, project.ReferenceFiles.LastOrDefault().AssetID);
            Assert.AreEqual("my-file.txt", project.ReferenceFiles.LastOrDefault().Name);
            Assert.AreEqual(new Uri("https://ondemand.liondemand.com/api/files/12346"), project.ReferenceFiles.LastOrDefault().URL);
            Assert.AreEqual(0, project.ReferenceFiles.LastOrDefault().TargetLanguages.Count());
        }

        [TestMethod]
        public void GetQuoteFileResponseNotCalculatedTest()
        {
            string xml = @"<?xml version='1.0' encoding='UTF-8'?>
                <Quote>
                     <QuoteID>132</QuoteID>
                     <CreationDate>2014-01-25T10:32:02Z</CreationDate>
                     <Status>New</Status>
                     <TotalCost/>
                     <Projects>
                         <Project>
                             <ProjectID>123</ProjectID>
                             <ProjectName>Name of project</ProjectName>
                             <ProjectURL>https://www.lionbridge.com</ProjectURL>
                             <ProjectDueDate>2014-02-11T10:22:46Z</ProjectDueDate>
                             <ServiceID>54</ServiceID>
                             <SourceLanguage>
                                 <LanguageCode>en-gb</LanguageCode>
                             </SourceLanguage>
                             <TargetLanguages>
                                 <TargetLanguage>
                                     <LanguageCode>it-it</LanguageCode>
                                 </TargetLanguage>
                                 <TargetLanguage>
                                     <LanguageCode>fr-fr</LanguageCode>
                                 </TargetLanguage>
                             </TargetLanguages>
                             <Files>
                                 <File>
                                     <Status>Analyzing</Status>
                                     <AssetID>999</AssetID>
                                     <FileName>example.txt</FileName>
                                 </File>
                             </Files>
                         </Project>
                     </Projects>
                 </Quote>
            ";

            XDocument document = XDocument.Parse(xml);

            var quote = new Quote(document.Element("Quote"), new MockContentAPI());

            Assert.AreEqual(132, quote.QuoteID);
            Assert.AreEqual(DateTime.Parse("2014-01-25T10:32:02Z"), quote.CreationDate);
            Assert.AreEqual("New", quote.Status);
            Assert.AreEqual("", quote.AuthorizeURL);
            Assert.AreEqual("", quote.RejectURL);
            Assert.AreEqual("", quote.PaymentURL);
            Assert.AreEqual(0, quote.TotalTranslations);
            Assert.AreEqual(0, quote.TranslationCredit);
            Assert.AreEqual(0, quote.TotalCost);
            Assert.AreEqual(0.00m, quote.PrepaidCredit);
            Assert.AreEqual(0, quote.AmountDue);
            Assert.AreEqual("", quote.Currency);


            Assert.IsNotNull(quote.Projects);
            Assert.AreEqual(1, quote.Projects.Count());

            var project = quote.Projects.FirstOrDefault();

            Assert.AreEqual(123, project.ProjectID);
            Assert.AreEqual(new Uri("https://www.lionbridge.com"), project.URL);
            Assert.AreEqual(DateTime.Parse("2014-02-11T10:22:46Z"), project.DueDate);
            Assert.AreEqual(54, project.ServiceID);
            Assert.AreEqual("en-gb", project.SourceLanguage);
            Assert.AreEqual(2, project.TargetLanguages.Count());
            Assert.AreEqual("it-it", project.TargetLanguages.FirstOrDefault());
            Assert.AreEqual("fr-fr", project.TargetLanguages.LastOrDefault());


            Assert.IsNotNull(project.Files);
            Assert.AreEqual(1, project.Files.Count());

            Assert.AreEqual(999, project.Files.FirstOrDefault().AssetID);
            Assert.AreEqual(FileStatus.Analyzing, project.Files.FirstOrDefault().Status);
            Assert.AreEqual("example.txt", project.Files.FirstOrDefault().Name);
        }

        [TestMethod]
        public void ProjectQuoteResponseTest()
        {
            string xml = @"<?xml version='1.0' encoding='UTF-8'?>
                <Quote>
                    <QuoteID>132</QuoteID>
                    <CreationDate>2014-01-25T10:32:02Z</CreationDate>
                    <Status>Pending</Status>
                    <AuthorizeURL>https://</AuthorizeURL>
                    <RejectURL>https://</RejectURL>
                    <TotalCost>10.00</TotalCost>
                    <PrepaidCredit>5.00</PrepaidCredit>
                    <AmountDue>5.00</AmountDue>
                    <Currency>EUR</Currency>

                    <Projects>
                            <Project>
                                <ProjectID>999</ProjectID>
                                <ProjectName>Name of project</ProjectName>
                                <ServiceID>54</ServiceID>
                                <SourceLanguage>
                                    <LanguageCode>en-gb</LanguageCode>
                                </SourceLanguage>
                                <TargetLanguages>
                                            <TargetLanguage>
                                                <LanguageCode>it-it</LanguageCode>
                                            </TargetLanguage>
                                            <TargetLanguage>
                                                <LanguageCode>fr-fr</LanguageCode>
                                            </TargetLanguage>
                                </TargetLanguages>
                            </Project>
                    </Projects>
                </Quote>
            ";

            XDocument document = XDocument.Parse(xml);

            var quote = new Quote(document.Element("Quote"), new MockContentAPI());

            Assert.AreEqual(132, quote.QuoteID);
            Assert.AreEqual(DateTime.Parse("2014-01-25T10:32:02Z"), quote.CreationDate);
            Assert.AreEqual("Pending", quote.Status);
            Assert.AreEqual("https://", quote.AuthorizeURL);
            Assert.AreEqual("https://", quote.RejectURL);
            Assert.AreEqual("", quote.PaymentURL);
            Assert.AreEqual(0, quote.TotalTranslations);
            Assert.AreEqual(0, quote.TranslationCredit);
            Assert.AreEqual(10.00m, quote.TotalCost);
            Assert.AreEqual(5.00m, quote.PrepaidCredit);
            Assert.AreEqual(5.00m, quote.AmountDue);
            Assert.AreEqual("EUR", quote.Currency);

            Assert.IsNotNull(quote.Projects);
            Assert.AreEqual(1, quote.Projects.Count());

            var project = quote.Projects.FirstOrDefault();

            Assert.AreEqual(999, project.ProjectID);
            Assert.AreEqual("Name of project", project.Name);
            Assert.AreEqual(54, project.ServiceID);
            Assert.AreEqual("en-gb", project.SourceLanguage);
            Assert.AreEqual(2, project.TargetLanguages.Count());
            Assert.AreEqual("it-it", project.TargetLanguages.FirstOrDefault());
            Assert.AreEqual("fr-fr", project.TargetLanguages.LastOrDefault());
        }

        [TestMethod]
        public void ProjectQuoteResponseNotReadyTest()
        {
            string xml = @"<?xml version='1.0' encoding='UTF-8'?>
                <Quote>
                    <QuoteID>132</QuoteID>
                    <CreationDate>2014-01-25T10:32:02Z</CreationDate>
                    <Status>Calculating</Status>
                    <TotalCost/>
                    <PrepaidCredit>5.00</PrepaidCredit>
                    <AmountDue/>
                    <Currency>EUR</Currency>

                    <Projects>
                            <Project>
                                <ProjectID>999</ProjectID>
                                <ProjectName>Name of project</ProjectName>
                                <ServiceID>54</ServiceID>
                                <SourceLanguage>
                                    <LanguageCode>en-gb</LanguageCode>
                                </SourceLanguage>
                                <TargetLanguages>
                                            <TargetLanguage>
                                                <LanguageCode>it-it</LanguageCode>
                                            </TargetLanguage>
                                            <TargetLanguage>
                                                <LanguageCode>fr-fr</LanguageCode>
                                            </TargetLanguage>
                                </TargetLanguages>
                            </Project>
                    </Projects>
                </Quote>
            ";

            XDocument document = XDocument.Parse(xml);

            var quote = new Quote(document.Element("Quote"), new MockContentAPI());

            Assert.AreEqual(132, quote.QuoteID);
            Assert.AreEqual(DateTime.Parse("2014-01-25T10:32:02Z"), quote.CreationDate);
            Assert.AreEqual("Calculating", quote.Status);
            Assert.AreEqual("", quote.AuthorizeURL);
            Assert.AreEqual("", quote.RejectURL);
            Assert.AreEqual("", quote.PaymentURL);
            Assert.AreEqual(0, quote.TotalTranslations);
            Assert.AreEqual(0, quote.TranslationCredit);
            Assert.AreEqual(0, quote.TotalCost);
            Assert.AreEqual(5.00m, quote.PrepaidCredit);
            Assert.AreEqual(0, quote.AmountDue);
            Assert.AreEqual("EUR", quote.Currency);

            Assert.IsNotNull(quote.Projects);
            Assert.AreEqual(1, quote.Projects.Count());

            var project = quote.Projects.FirstOrDefault();

            Assert.AreEqual(999, project.ProjectID);
            Assert.AreEqual("Name of project", project.Name);
            Assert.AreEqual(54, project.ServiceID);
            Assert.AreEqual("en-gb", project.SourceLanguage);
            Assert.AreEqual(2, project.TargetLanguages.Count());
            Assert.AreEqual("it-it", project.TargetLanguages.FirstOrDefault());
            Assert.AreEqual("fr-fr", project.TargetLanguages.LastOrDefault());
        }

        [TestMethod]
        public void ListQuotesResponseTest()
        {
            string xml = @"<?xml version='1.0' encoding='UTF-8'?>
                <Quotes>
                   <Quote>
                        <QuoteID>132</QuoteID>
                        <CreationDate>2014-01-25T10:32:02Z</CreationDate>
                        <Status>Pending</Status>
                        <TotalCost>10.00</TotalCost>
                        <PrepaidCredit>5.00</PrepaidCredit>
                        <AmountDue>5.00</AmountDue>
                        <Currency>EUR</Currency>
                        <Projects>
                            <Project>
                                <ProjectID>123</ProjectID>
                                <ProjectURL>https://</ProjectURL>
                                <ProjectDueDate>2014-02-11T10:22:46Z</ProjectDueDate>
                                <ServiceID>54</ServiceID>
                                <SourceLanguage>
                                    <LanguageCode>en-gb</LanguageCode>
                                </SourceLanguage>
                                <TargetLanguages>
                                    <TargetLanguage>
                                        <LanguageCode>it-it</LanguageCode>
                                    </TargetLanguage>
                                    <TargetLanguage>
                                        <LanguageCode>fr-fr</LanguageCode>
                                    </TargetLanguage>
                                </TargetLanguages>
                                <Products>
                                    <Product>
                                        <AssetID>999</AssetID>
                                        <SKUs>
                                            <SKU>
                                                <SKUNumber>123</SKUNumber>
                                            </SKU>
                                        </SKUs>
                                    </Product>
                                </Products>
                            </Project>
                        </Projects>
                    </Quote>
                   <Quote>
                        <QuoteID>133</QuoteID>
                        <CreationDate>2014-01-25T10:32:02Z</CreationDate>
                        <Status>Authorized</Status>
                        <TotalCost>10.00</TotalCost>
                        <Currency>EUR</Currency>
                        <Payments>
                            <Payment>
                                <PaymentType>PayPal</PaymentType>
                                <PaymentDescription>PayPal charge to buyer@example.com</PaymentDescription>
                                <PaymentAmount>10.00</PaymentAmount>
                                <PaymentCurrency>EUR</PaymentCurrency>
                            </Payment>
                        </Payments>
                        <Projects>
                            <Project>
                                <ProjectID>123</ProjectID>
                                <ProjectURL>https://</ProjectURL>
                                <ProjectDueDate>2014-02-11T10:22:46Z</ProjectDueDate>
                                <ServiceID>54</ServiceID>
                                <SourceLanguage>
                                    <LanguageCode>en-gb</LanguageCode>
                                </SourceLanguage>
                                <TargetLanguages>
                                    <TargetLanguage>
                                        <LanguageCode>it-it</LanguageCode>
                                    </TargetLanguage>
                                    <TargetLanguage>
                                        <LanguageCode>fr-fr</LanguageCode>
                                    </TargetLanguage>
                                </TargetLanguages>
                                <Products>
                                    <Product>
                                        <AssetID>999</AssetID>
                                        <SKUs>
                                            <SKU>
                                                <SKUNumber>123</SKUNumber>
                                            </SKU>
                                        </SKUs>
                                    </Product>
                                </Products>
                            </Project>
                        </Projects>
                    </Quote>
                   <Quote>
                        <QuoteID>134</QuoteID>
                        <CreationDate>2014-01-25T10:32:02Z</CreationDate>
                        <Status>Pending</Status>
                        <AuthorizeURL>https://</AuthorizeURL>
                        <RejectURL>https://</RejectURL>
                        <TotalCost>10.00</TotalCost>
                        <PrepaidCredit>5.00</PrepaidCredit>
                        <AmountDue>5.00</AmountDue>
                        <Currency>EUR</Currency>
                        <Projects>
                            <Project>
                                <ProjectID>123</ProjectID>
                                <ProjectName>Name of project</ProjectName>
                                <ProjectURL>https://</ProjectURL>
                                <ProjectDueDate>2014-02-11T10:22:46Z</ProjectDueDate>
                                <ServiceID>54</ServiceID>
                                <SourceLanguage>
                                    <LanguageCode>en-gb</LanguageCode>
                                </SourceLanguage>
                                <TargetLanguages>
                                    <TargetLanguage>
                                        <LanguageCode>it-it</LanguageCode>
                                    </TargetLanguage>
                                    <TargetLanguage>
                                        <LanguageCode>fr-fr</LanguageCode>
                                    </TargetLanguage>
                                </TargetLanguages>
                                <Files>
                                    <File>
                                        <Status>Analyzed</Status>
                                        <AssetID>999</AssetID>
                                        <FileName>example.txt</FileName>
                                    </File>
                                </Files>
                            </Project>
                        </Projects>
                    </Quote>
                </Quotes>
            ";

            XDocument document = XDocument.Parse(xml);

            IEnumerable<Quote> quotes = Quote.CreateEnumerable(document.Element("Quotes"), new MockContentAPI());

            Assert.IsNotNull(quotes);
            Assert.AreEqual(3, quotes.Count());

            // First Quote
            var quote = quotes.ElementAt(0);

            Assert.AreEqual(132, quote.QuoteID);
            Assert.AreEqual(DateTime.Parse("2014-01-25T10:32:02Z"), quote.CreationDate);
            Assert.AreEqual("Pending", quote.Status);
            Assert.AreEqual("", quote.AuthorizeURL);
            Assert.AreEqual("", quote.RejectURL);
            Assert.AreEqual("", quote.PaymentURL);
            Assert.AreEqual(0, quote.TotalTranslations);
            Assert.AreEqual(0, quote.TranslationCredit);
            Assert.AreEqual(10.00m, quote.TotalCost);
            Assert.AreEqual(5.00m, quote.PrepaidCredit);
            Assert.AreEqual(5.00m, quote.AmountDue);
            Assert.AreEqual("EUR", quote.Currency);

            Assert.IsNotNull(quote.Projects);
            Assert.AreEqual(1, quote.Projects.Count());

            var project = quote.Projects.FirstOrDefault();

            Assert.AreEqual(123, project.ProjectID);
            Assert.AreEqual(null, project.URL);
            Assert.AreEqual(DateTime.Parse("2014-02-11T10:22:46Z"), project.DueDate);
            Assert.AreEqual(54, project.ServiceID);
            Assert.AreEqual("en-gb", project.SourceLanguage);
            Assert.AreEqual(2, project.TargetLanguages.Count());
            Assert.AreEqual("it-it", project.TargetLanguages.FirstOrDefault());
            Assert.AreEqual("fr-fr", project.TargetLanguages.LastOrDefault());

            Assert.IsNotNull(project.Products);
            Assert.AreEqual(1, project.Products.Count());

            var product = project.Products.FirstOrDefault();

            Assert.AreEqual(999, product.AssetID);
            Assert.AreEqual(1, product.SKUs.Count());
            Assert.AreEqual("123", product.SKUs.FirstOrDefault().SKUNumber);

            // Second Quote
            quote = quotes.ElementAt(1);

            Assert.AreEqual(133, quote.QuoteID);
            Assert.AreEqual(DateTime.Parse("2014-01-25T10:32:02Z"), quote.CreationDate);
            Assert.AreEqual("Authorized", quote.Status);
            Assert.AreEqual("", quote.AuthorizeURL);
            Assert.AreEqual("", quote.RejectURL);
            Assert.AreEqual("", quote.PaymentURL);
            Assert.AreEqual(0, quote.TotalTranslations);
            Assert.AreEqual(0, quote.TranslationCredit);
            Assert.AreEqual(10.00m, quote.TotalCost);
            Assert.AreEqual(0.00m, quote.PrepaidCredit);
            Assert.AreEqual(0.00m, quote.AmountDue);
            Assert.AreEqual("EUR", quote.Currency);

            Assert.IsNotNull(quote.Payments);
            Assert.AreEqual(1, quote.Payments.Count());

            var payment = quote.Payments.FirstOrDefault();

            Assert.AreEqual("PayPal", payment.Type);
            Assert.AreEqual("PayPal charge to buyer@example.com", payment.Description);
            Assert.AreEqual(10.00m, payment.Amount);
            Assert.AreEqual("EUR", payment.Currency);


            Assert.IsNotNull(quote.Projects);
            Assert.AreEqual(1, quote.Projects.Count());

            project = quote.Projects.FirstOrDefault();

            Assert.AreEqual(123, project.ProjectID);
            Assert.AreEqual(null, project.URL);
            Assert.AreEqual(DateTime.Parse("2014-02-11T10:22:46Z"), project.DueDate);
            Assert.AreEqual(54, project.ServiceID);
            Assert.AreEqual("en-gb", project.SourceLanguage);
            Assert.AreEqual(2, project.TargetLanguages.Count());
            Assert.AreEqual("it-it", project.TargetLanguages.FirstOrDefault());
            Assert.AreEqual("fr-fr", project.TargetLanguages.LastOrDefault());

            Assert.IsNotNull(project.Products);
            Assert.AreEqual(1, project.Products.Count());

            product = project.Products.FirstOrDefault();

            Assert.AreEqual(999, product.AssetID);
            Assert.AreEqual(1, product.SKUs.Count());
            Assert.AreEqual("123", product.SKUs.FirstOrDefault().SKUNumber);


            // Third Quote
            quote = quotes.ElementAt(2);

            Assert.AreEqual(134, quote.QuoteID);
            Assert.AreEqual(DateTime.Parse("2014-01-25T10:32:02Z"), quote.CreationDate);
            Assert.AreEqual("Pending", quote.Status);
            Assert.AreEqual("https://", quote.AuthorizeURL);
            Assert.AreEqual("https://", quote.RejectURL);
            Assert.AreEqual("", quote.PaymentURL);
            Assert.AreEqual(0, quote.TotalTranslations);
            Assert.AreEqual(0, quote.TranslationCredit);
            Assert.AreEqual(10.00m, quote.TotalCost);
            Assert.AreEqual(5.00m, quote.PrepaidCredit);
            Assert.AreEqual(5.00m, quote.AmountDue);
            Assert.AreEqual("EUR", quote.Currency);


            Assert.IsNotNull(quote.Projects);
            Assert.AreEqual(1, quote.Projects.Count());

            project = quote.Projects.FirstOrDefault();

            Assert.AreEqual(123, project.ProjectID);
            Assert.AreEqual(null, project.URL);
            Assert.AreEqual(DateTime.Parse("2014-02-11T10:22:46Z"), project.DueDate);
            Assert.AreEqual(54, project.ServiceID);
            Assert.AreEqual("en-gb", project.SourceLanguage);
            Assert.AreEqual(2, project.TargetLanguages.Count());
            Assert.AreEqual("it-it", project.TargetLanguages.FirstOrDefault());
            Assert.AreEqual("fr-fr", project.TargetLanguages.LastOrDefault());


            Assert.IsNotNull(project.Files);
            Assert.AreEqual(1, project.Files.Count());

            Assert.AreEqual(999, project.Files.FirstOrDefault().AssetID);
            Assert.AreEqual(FileStatus.Analyzed, project.Files.FirstOrDefault().Status);
            Assert.AreEqual("example.txt", project.Files.FirstOrDefault().Name);

        }

        [TestMethod]
        public void ToXmlStringPayAsYouGoTest()
        {
            string projectsXml = @"
            <Projects>
                    <Project>
                        <ProjectID>999</ProjectID>
                        <ProjectName>Name of project</ProjectName>
                        <ServiceID>54</ServiceID>
                        <SourceLanguage>
                            <LanguageCode>en-gb</LanguageCode>
                        </SourceLanguage>
                        <TargetLanguages>
                                    <TargetLanguage>
                                        <LanguageCode>it-it</LanguageCode>
                                    </TargetLanguage>
                                    <TargetLanguage>
                                        <LanguageCode>fr-fr</LanguageCode>
                                    </TargetLanguage>
                        </TargetLanguages>
                        <Products>
                                <Product>
                                    <AssetID>999</AssetID>
                                    <SKUs>
                                        <SKU>
                                            <SKUNumber>123</SKUNumber>
                                        </SKU>
                                    </SKUs>
                                    <DueDate>2014-02-11T10:22:46Z</DueDate>
                                </Product>
                        </Products>
                        <ReferenceFiles>
                            <ReferenceFile>
                                <AssetID>12345</AssetID>
                                <FileName>my-file.txt</FileName>
                                <URL>https://ondemand.liondemand.com/api/files/12345</URL>
                                <TargetLanguages></TargetLanguages>
                            </ReferenceFile>
                            <ReferenceFile>
                                <AssetID>12346</AssetID>
                                <FileName>my-file.txt</FileName>
                                <URL>https://ondemand.liondemand.com/api/files/12346</URL>
                                <TargetLanguages></TargetLanguages>
                            </ReferenceFile>
                        </ReferenceFiles>
                    </Project>
            </Projects>
            ";

            XDocument document = XDocument.Parse(projectsXml);

            IEnumerable<Project> projects = Project.CreateEnumerable(document.Element("Projects"), new MockContentAPI());

            var quote = new Quote(quoteID: 795,
                                  creationDate: DateTime.Parse("2014-06-25T16:39:07Z"),
                                  totalTranslations: 2,
                                  translationCredit: 49984,
                                  totalCost: 0.00m,
                                  prepaidCredit: 118.99m,
                                  amountDue: 0.00m,
                                  currency: "EUR",
                                  projects: projects);

            string xml = @"
                <Quote>
                    <QuoteID>795</QuoteID>
                    <CreationDate>2014-06-25T16:39:07Z</CreationDate>
                    <TotalTranslations>2</TotalTranslations>
                    <TranslationCredit>49984</TranslationCredit>
                    <TotalCost>0.00</TotalCost>
                    <PrepaidCredit>118.99</PrepaidCredit>
                    <AmountDue>0.00</AmountDue>
                    <Currency>EUR</Currency>
                    <Projects>
                            <Project>
                                <ProjectID>999</ProjectID>
                                <ProjectName>Name of project</ProjectName>
                                <ServiceID>54</ServiceID>
                                <SourceLanguage>
                                    <LanguageCode>en-gb</LanguageCode>
                                </SourceLanguage>
                                <TargetLanguages>
                                            <TargetLanguage>
                                                <LanguageCode>it-it</LanguageCode>
                                            </TargetLanguage>
                                            <TargetLanguage>
                                                <LanguageCode>fr-fr</LanguageCode>
                                            </TargetLanguage>
                                </TargetLanguages>
                                <Products>
                                        <Product>
                                            <AssetID>999</AssetID>
                                            <SKUs>
                                                <SKU>
                                                    <SKUNumber>123</SKUNumber>
                                                </SKU>
                                            </SKUs>
                                            <DueDate>2014-02-11T10:22:46Z</DueDate>
                                        </Product>
                                </Products>
                                <ReferenceFiles>
                                    <ReferenceFile>
                                        <AssetID>12345</AssetID>
                                        <FileName>my-file.txt</FileName>
                                        <URL>https://ondemand.liondemand.com/api/files/12345</URL>
                                <TargetLanguages></TargetLanguages>
                                    </ReferenceFile>
                                    <ReferenceFile>
                                        <AssetID>12346</AssetID>
                                        <FileName>my-file.txt</FileName>
                                        <URL>https://ondemand.liondemand.com/api/files/12346</URL>
                                        <TargetLanguages></TargetLanguages>
                                    </ReferenceFile>
                                </ReferenceFiles>
                            </Project>
                    </Projects>
                </Quote>
            ";

            Assert.AreEqual(Regex.Replace(xml, @"\s", ""), Regex.Replace(quote.ToXmlString(), @"\s", ""));
        }

        [TestMethod]
        public void ToXmlStringChargebackTest()
        {
            string projectsXml = @"
            <Projects>
                    <Project>
                        <ProjectID>999</ProjectID>
                        <ProjectName>Name of project</ProjectName>
                        <ServiceID>54</ServiceID>
                        <SourceLanguage>
                            <LanguageCode>en-gb</LanguageCode>
                        </SourceLanguage>
                        <TargetLanguages>
                                    <TargetLanguage>
                                        <LanguageCode>it-it</LanguageCode>
                                    </TargetLanguage>
                                    <TargetLanguage>
                                        <LanguageCode>fr-fr</LanguageCode>
                                    </TargetLanguage>
                        </TargetLanguages>
                        <Products>
                                <Product>
                                    <AssetID>999</AssetID>
                                    <SKUs>
                                        <SKU>
                                            <SKUNumber>123</SKUNumber>
                                        </SKU>
                                    </SKUs>
                                    <DueDate>2014-02-11T10:22:46Z</DueDate>
                                </Product>
                        </Products>
                        <ReferenceFiles>
                            <ReferenceFile>
                                <AssetID>12345</AssetID>
                                <FileName>my-file.txt</FileName>
                                <URL>https://ondemand.liondemand.com/api/files/12345</URL>
                                <TargetLanguages></TargetLanguages>
                            </ReferenceFile>
                            <ReferenceFile>
                                <AssetID>12346</AssetID>
                                <FileName>my-file.txt</FileName>
                                <URL>https://ondemand.liondemand.com/api/files/12346</URL>
                                <TargetLanguages></TargetLanguages>
                            </ReferenceFile>
                        </ReferenceFiles>
                    </Project>
            </Projects>
            ";

            XDocument document = XDocument.Parse(projectsXml);

            IEnumerable<Project> projects = Project.CreateEnumerable(document.Element("Projects"), new MockContentAPI());

            var quote = new Quote(quoteID: 795,
                                  creationDate: DateTime.Parse("2014-06-25T16:39:07Z"),
                                  totalTranslations: 2,
                                  translationCredit: 49984,
                                  totalCost: 0.00m,
                                  prepaidCredit: 118.99m,
                                  amountDue: 0.00m,
                                  currency: "EUR",
                                  projects: projects) 
                                  { InternalBillingCode = "ABCD100001" };

            string xml = @"
                <Quote>
                    <QuoteID>795</QuoteID>
                    <CreationDate>2014-06-25T16:39:07Z</CreationDate>
                    <TotalTranslations>2</TotalTranslations>
                    <TranslationCredit>49984</TranslationCredit>
                    <TotalCost>0.00</TotalCost>
                    <PrepaidCredit>118.99</PrepaidCredit>
                    <AmountDue>0.00</AmountDue>
                    <Currency>EUR</Currency>
                    <InternalBillingCode>ABCD100001</InternalBillingCode>
                    <Projects>
                            <Project>
                                <ProjectID>999</ProjectID>
                                <ProjectName>Name of project</ProjectName>
                                <ServiceID>54</ServiceID>
                                <SourceLanguage>
                                    <LanguageCode>en-gb</LanguageCode>
                                </SourceLanguage>
                                <TargetLanguages>
                                            <TargetLanguage>
                                                <LanguageCode>it-it</LanguageCode>
                                            </TargetLanguage>
                                            <TargetLanguage>
                                                <LanguageCode>fr-fr</LanguageCode>
                                            </TargetLanguage>
                                </TargetLanguages>
                                <Products>
                                        <Product>
                                            <AssetID>999</AssetID>
                                            <SKUs>
                                                <SKU>
                                                    <SKUNumber>123</SKUNumber>
                                                </SKU>
                                            </SKUs>
                                            <DueDate>2014-02-11T10:22:46Z</DueDate>
                                        </Product>
                                </Products>
                                <ReferenceFiles>
                                    <ReferenceFile>
                                        <AssetID>12345</AssetID>
                                        <FileName>my-file.txt</FileName>
                                        <URL>https://ondemand.liondemand.com/api/files/12345</URL>
                                        <TargetLanguages></TargetLanguages>
                                    </ReferenceFile>
                                    <ReferenceFile>
                                        <AssetID>12346</AssetID>
                                        <FileName>my-file.txt</FileName>
                                        <URL>https://ondemand.liondemand.com/api/files/12346</URL>
                                        <TargetLanguages></TargetLanguages>
                                    </ReferenceFile>
                                </ReferenceFiles>
                            </Project>
                    </Projects>
                </Quote>
            ";


            Assert.AreEqual(Regex.Replace(xml, @"\s", ""), Regex.Replace(quote.ToXmlString(), @"\s", ""));
        }

        [TestMethod]
        public void ToXmlStringProvisioningTest()
        {

            var oldCulture = Thread.CurrentThread.CurrentCulture;
            try
            {

                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("de-DE");

                string projectsXml = @"
            <Projects>
                    <Project>
                        <ProjectID>999</ProjectID>
                        <ProjectName>Name of project</ProjectName>
                        <ServiceID>54</ServiceID>
                        <SourceLanguage>
                            <LanguageCode>en-gb</LanguageCode>
                        </SourceLanguage>
                        <TargetLanguages>
                                    <TargetLanguage>
                                        <LanguageCode>it-it</LanguageCode>
                                    </TargetLanguage>
                                    <TargetLanguage>
                                        <LanguageCode>fr-fr</LanguageCode>
                                    </TargetLanguage>
                        </TargetLanguages>
                        <Products>
                                <Product>
                                    <AssetID>999</AssetID>
                                    <SKUs>
                                        <SKU>
                                            <SKUNumber>123</SKUNumber>
                                        </SKU>
                                    </SKUs>
                                    <DueDate>2014-02-11T10:22:46Z</DueDate>
                                </Product>
                        </Products>
                        <ReferenceFiles>
                            <ReferenceFile>
                                <AssetID>12345</AssetID>
                                <FileName>my-file.txt</FileName>
                                <URL>https://ondemand.liondemand.com/api/files/12345</URL>
                                <TargetLanguages></TargetLanguages>
                            </ReferenceFile>
                            <ReferenceFile>
                                <AssetID>12346</AssetID>
                                <FileName>my-file.txt</FileName>
                                <URL>https://ondemand.liondemand.com/api/files/12346</URL>
                                <TargetLanguages></TargetLanguages>
                            </ReferenceFile>
                        </ReferenceFiles>
                    </Project>
            </Projects>
            ";

                XDocument document = XDocument.Parse(projectsXml);

                IEnumerable<Project> projects = Project.CreateEnumerable(document.Element("Projects"), new MockContentAPI());

                var quote = new Quote(quoteID: 795,
                                      creationDate: DateTime.Parse("2014-06-25T16:39:07Z"),
                                      totalTranslations: 2,
                                      translationCredit: 49984,
                                      totalCost: 0.00m,
                                      prepaidCredit: 118.99m,
                                      amountDue: 0.00m,
                                      currency: "EUR",
                                      projects: projects) { InternalBillingCode = "ABCD100001", PurchaseOrderNumber = "001-005-100" };

                string xml = @"
                <Quote>
                    <QuoteID>795</QuoteID>
                    <CreationDate>2014-06-25T16:39:07Z</CreationDate>
                    <TotalTranslations>2</TotalTranslations>
                    <TranslationCredit>49984</TranslationCredit>
                    <TotalCost>0.00</TotalCost>
                    <PrepaidCredit>118.99</PrepaidCredit>
                    <AmountDue>0.00</AmountDue>
                    <Currency>EUR</Currency>
                    <PurchaseOrderNumber>001-005-100</PurchaseOrderNumber>
                    <InternalBillingCode>ABCD100001</InternalBillingCode>
                    <Projects>
                            <Project>
                                <ProjectID>999</ProjectID>
                                <ProjectName>Name of project</ProjectName>
                                <ServiceID>54</ServiceID>
                                <SourceLanguage>
                                    <LanguageCode>en-gb</LanguageCode>
                                </SourceLanguage>
                                <TargetLanguages>
                                            <TargetLanguage>
                                                <LanguageCode>it-it</LanguageCode>
                                            </TargetLanguage>
                                            <TargetLanguage>
                                                <LanguageCode>fr-fr</LanguageCode>
                                            </TargetLanguage>
                                </TargetLanguages>
                                <Products>
                                        <Product>
                                            <AssetID>999</AssetID>
                                            <SKUs>
                                                <SKU>
                                                    <SKUNumber>123</SKUNumber>
                                                </SKU>
                                            </SKUs>
                                            <DueDate>2014-02-11T10:22:46Z</DueDate>
                                        </Product>
                                </Products>
                                <ReferenceFiles>
                                    <ReferenceFile>
                                        <AssetID>12345</AssetID>
                                        <FileName>my-file.txt</FileName>
                                        <URL>https://ondemand.liondemand.com/api/files/12345</URL>
                                        <TargetLanguages></TargetLanguages>
                                    </ReferenceFile>
                                    <ReferenceFile>
                                        <AssetID>12346</AssetID>
                                        <FileName>my-file.txt</FileName>
                                        <URL>https://ondemand.liondemand.com/api/files/12346</URL>
                                        <TargetLanguages></TargetLanguages>
                                    </ReferenceFile>
                                </ReferenceFiles>
                            </Project>
                    </Projects>
                </Quote>
            ";


                Assert.AreEqual(Regex.Replace(xml, @"\s", ""), Regex.Replace(quote.ToXmlString(), @"\s", ""));

            }
            finally
            {
                Thread.CurrentThread.CurrentCulture = oldCulture;
            }
        }
    }
}
