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
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lionbridge.onDemand.Client.Tests
{
    [TestClass]
    public class QuoteAuthorizationTest
    {
        [TestMethod]
        public void ConstructorNoPaymentRequiredTest()
        {
            string xml = @"<?xml version='1.0' encoding='UTF-8'?>
                <QuoteAuthorization>
                    <Status>Authorized</Status>
                    <QuoteURL>https://www.quoteurl.com/url</QuoteURL>
                    <Projects>
                        <Project>
                            <ProjectID>123</ProjectID>
                            <ProjectURL>https://www.projecturl.com/url</ProjectURL>
                            <ProjectDueDate>2014-02-11T10:22:46Z</ProjectDueDate>
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
                </QuoteAuthorization>
            ";

            XDocument document = XDocument.Parse(xml);

            var quoteAuthorization = new QuoteAuthorization(document.Element("QuoteAuthorization"), new MockContentAPI());

            Assert.AreEqual("Authorized", quoteAuthorization.Status);
            Assert.AreEqual("https://www.quoteurl.com/url", quoteAuthorization.QuoteURL);

            Assert.AreEqual(1, quoteAuthorization.Projects.Count());

        }

        [TestMethod]
        public void ConstructorPaymentRequiredTest()
        {
            string xml = @"<?xml version='1.0' encoding='UTF-8'?>
                <QuoteAuthorization>
                    <Status>Pending</Status>
                    <PaymentURL>https://www.paymenturl.com/url</PaymentURL>
                    <QuoteURL>https://www.quoteurl.com/url</QuoteURL>
                    <Projects>
                        <Project>
                            <ProjectID>123</ProjectID>
                            <ProjectURL>https://www.projecturl.com/url</ProjectURL>
                            <ProjectDueDate>2014-02-11T10:22:46Z</ProjectDueDate>
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
                </QuoteAuthorization>
            ";

            XDocument document = XDocument.Parse(xml);

            var quoteAuthorization = new QuoteAuthorization(document.Element("QuoteAuthorization"), new MockContentAPI());

            Assert.AreEqual("Pending", quoteAuthorization.Status);
            Assert.AreEqual("https://www.quoteurl.com/url", quoteAuthorization.QuoteURL);
            Assert.AreEqual("https://www.paymenturl.com/url", quoteAuthorization.PaymentURL);

            Assert.AreEqual(1, quoteAuthorization.Projects.Count());

        }

    }
}
