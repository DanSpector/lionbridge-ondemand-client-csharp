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
    public class ProductTranslationTest
    {
        [TestMethod]
        public void ContructorFromXmlTest()
        {
            string xml = @"<?xml version='1.0' encoding='UTF-8'?>
                <Translation>
                    <SKUs>
                        <SKU>
                            <SKUNumber>123</SKUNumber>
                        </SKU>
                    </SKUs>
                    <AssetID>9999</AssetID>
                    <SourceTitle>Men&apos;s Pants</SourceTitle>
                    <Service>14</Service>
                    <Language>de-de</Language>
                    <TranslatedFields>
                        <Title>Here&apos;s the title</Title>
                        <Description>
                            <ArbitraryXml1><Whoa>There</Whoa></ArbitraryXml1>
                            <ArbitraryXml2><Whoa>There</Whoa></ArbitraryXml2>
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
                        <PrimaryCategory>123</PrimaryCategory>
                        <SKUs>
                            <SKU>
                                <SKUNumber>123</SKUNumber>
                                <ItemSpecifics>
                                    <ItemSpecific>
                                        <SourceName>Colour</SourceName>
                                        <Name>Culeur</Name>
                                        <Value>Blanc</Value>
                                    </ItemSpecific>
                                    <ItemSpecific>
                                        <SourceName>Size</SourceName>
                                        <Name>Taille</Name>
                                        <Value>Grande</Value>
                                    </ItemSpecific>
                                </ItemSpecifics>
                            </SKU>
                        </SKUs>
                    </TranslatedFields>
                </Translation>
            ";

            XDocument document = XDocument.Parse(xml);

            var translation = new ProductTranslation(document.Element("Translation"));

            Assert.AreEqual(Regex.Replace(@"This is a summary it can contain HTML markup.
                To tell the translation service to ignore some
                text, <b>wrap</b> it in a
                [do-not-translate]
                do not translate
                [/do-not-translate]
                tag", @"\s", ""), Regex.Replace(translation.Description.Summary, @"\s", ""));


            XElement[] arbitrary = new XElement[] { XElement.Parse("<ArbitraryXml1><Whoa>There</Whoa></ArbitraryXml1>"), XElement.Parse("<ArbitraryXml2><Whoa>There</Whoa></ArbitraryXml2>") };

            var features = new Dictionary<String, String>() { { "Feature1", "Feature 1 & Bold" }, { "Feature2", "Feature 2" } };

            for (int i = 0; i < arbitrary.Length; i++)
            {
                Assert.AreEqual(arbitrary[i].ToString(SaveOptions.None), translation.Description.ArbitraryElements[i].ToString(SaveOptions.None));
            }

            foreach (string featureKey in features.Keys)
            {
                Assert.AreEqual(features[featureKey], translation.Description.Features[featureKey]);
            }

            Assert.AreEqual(9999, translation.AssetID);
            Assert.AreEqual("Here's the title", translation.Title);
            Assert.AreEqual("Men's Pants", translation.SourceTitle);
            Assert.AreEqual(123, translation.PrimaryCategory);
            Assert.AreEqual(14, translation.Service);
            Assert.AreEqual("de-de", translation.Language);
            Assert.AreEqual(1, translation.SourceSKUs.Length);
            Assert.AreEqual(1, translation.SKUs.Length);
            Assert.AreEqual("123", translation.SKUs.First().SKUNumber);
            Assert.AreEqual(2, translation.SKUs.First().ItemSpecifics.Count);
            Assert.AreEqual("Culeur", translation.SKUs.First().ItemSpecifics.First().Key);
            Assert.AreEqual("Blanc", translation.SKUs.First().ItemSpecifics.First().Value);

        
        }
    }
}
