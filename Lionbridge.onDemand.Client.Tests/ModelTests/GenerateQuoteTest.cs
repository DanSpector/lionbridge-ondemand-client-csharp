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
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lionbridge.onDemand.Client.Tests
{
    [TestClass]
    public class GenerateQuoteTest
    {
        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void ToXmlStringFileBasedTest()
        {
            var service = new Service() { ServiceID = 54 };

            var files = new List<File>() { new File() { AssetID = 123456 } };

            var targetLanguages = new List<String>() { "it-it", "fr-fr" };

            var quoteOptions = new QuoteOptions("en-gb", targetLanguages, "EUR") { ServiceID = service.ServiceID };

            var referenceFiles = new List<File>() { new File() { AssetID = 12345 }, new File() { AssetID = 12346 } };

            var generateQuote = new GenerateQuote(files, quoteOptions, referenceFiles);

            String xml = @"
                <GenerateQuote>
                    <TranslationOptions>
                        <Currency>EUR</Currency>
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
                    </TranslationOptions>
                    <Files>
                        <File>
                            <AssetID>123456</AssetID>
                        </File>
                    </Files>
                    <ReferenceFiles>
                        <ReferenceFile>
                            <AssetID>12345</AssetID>
                        </ReferenceFile>
                        <ReferenceFile>
                            <AssetID>12346</AssetID>
                        </ReferenceFile>
                    </ReferenceFiles>
                </GenerateQuote>
            ";

            Assert.AreEqual(Regex.Replace(xml, @"\s", ""), generateQuote.ToXmlString());
        }

        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void ToXmlStringProductBasedTest()
        {
            var service = new Service() { ServiceID = 54 };

            var products = new List<Product>() { new Product(title: "The title of the item",
                                                             primaryCategory: 123,
                                                             topLevelCategory: 1,
                                                             categoryPath: "Clothing : Menswear : Shoes",
                                                             skus: new SKU[] {new SKU("1234", new Dictionary<string,string>() {{"Color", "White"}, {"Size", "Large"}})},
                                                             description: new ProductDescription(arbitraryElements: null, 
                                                                 features: new Dictionary<String, String>() { {"Feature1", "Feature 1 & Bold"}, {"Feature2", "Feature 2"} },
                                                             summary: @"This is a summary it can contain HTML markup.
                                                                To tell the translation service to ignore some
                                                                text, <b>wrap</b> it in a
                                                                [do-not-translate]
                                                                do not translate
                                                                [/do-not-translate]
                                                                tag"))};

            var targetLanguages = new List<String>() { "it-it", "fr-fr" };

            var quoteOptions = new QuoteOptions("en-gb", targetLanguages, "EUR") { ServiceID = service.ServiceID };

            var referenceFiles = new List<File>() { new File() { AssetID = 12345 }, new File() { AssetID = 12346 } };

            var generateQuote = new GenerateQuote(products, quoteOptions, referenceFiles);

            String xml = @"
                <GenerateQuote>
                    <TranslationOptions>
                        <Currency>EUR</Currency>
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
                    </TranslationOptions>
                    <Products>
                        <Product>
                            <Title>The title of the item</Title>
                            <PrimaryCategory>123</PrimaryCategory>
                            <TopLevelCategory>1</TopLevelCategory>
                            <CategoryPath>Clothing : Menswear : Shoes</CategoryPath>
                            <Description>
                                <Summary>
                                    <![CDATA[
                                            This is a summary it can contain HTML markup.
                                            To tell the translation service to ignore some
                                            text, <b>wrap</b> it in a
                                            [do-not-translate]
                                            do not translate
                                            [/do-not-translate]
                                            tag
                                            ]]>

                                </Summary>
                                <Features>
                                    <Feature1>Feature 1 &amp; Bold</Feature1>
                                    <Feature2>Feature 2</Feature2>
                                </Features>
                            </Description>
                            <SKUs>
                                <SKU>
                                   <SKUNumber>1234</SKUNumber>
                                    <ItemSpecifics>
                                        <ItemSpecific>
                                            <Name>Color</Name>
                                            <Value>White</Value>
                                        </ItemSpecific>
                                        <ItemSpecific>
                                            <Name>Size</Name>
                                            <Value>Large</Value>
                                        </ItemSpecific>
                                  </ItemSpecifics>
                                </SKU>
                            </SKUs>
                        </Product>
                    </Products>
                    <ReferenceFiles>
                        <ReferenceFile>
                            <AssetID>12345</AssetID>
                        </ReferenceFile>
                        <ReferenceFile>
                            <AssetID>12346</AssetID>
                        </ReferenceFile>
                    </ReferenceFiles>
                </GenerateQuote>
            ";

            Assert.AreEqual(Regex.Replace(xml, @"\s", ""), Regex.Replace(generateQuote.ToXmlString(), @"\s", ""));
        }

        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void ToXmlStringProjectBasedTest()
        {

            var projectQuoteOptions = new ProjectQuoteOptions("EUR");

            var projects = new List<Project>() { new Project() { ProjectID = 123456, Name = "Don't Output"} };

            var generateQuote = new GenerateQuote(projects, projectQuoteOptions);

            String xml = @"
                <GenerateQuote>
                    <TranslationOptions>
                        <Currency>EUR</Currency>
                    </TranslationOptions>
                    <Projects>
                        <Project>
                            <ProjectID>123456</ProjectID>
                        </Project>

                    </Projects>
                </GenerateQuote>
            ";

            Assert.AreEqual(Regex.Replace(xml, @"\s", ""), generateQuote.ToXmlString());
        }
    }

}
