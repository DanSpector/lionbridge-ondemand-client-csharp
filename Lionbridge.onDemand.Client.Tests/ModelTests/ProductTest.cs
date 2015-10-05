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
    public class ProductTest
    {
        /// <summary>
        /// Test the default constructor
        /// </summary>
        [TestMethod]
        public void ProductConstructorTest()
        {
            String title = "Title";
            Int32 primaryCategory = 5;
            Int32 topLevelCategory = 10;
            String categoryPath = "Some Path";
            SKU[] skus = new SKU[5];
            var features = new Dictionary<String, String>() {{"F1", "V1"}, {"F2", "V2"}, {"F3", "V3"}};
            ProductDescription description = new ProductDescription(arbitraryElements: null, features: features);

            Product product = new Product(title, primaryCategory, topLevelCategory, categoryPath, skus, description);
            
            Assert.AreEqual(product.Title, title);
            Assert.AreEqual(product.PrimaryCategory, primaryCategory);
            Assert.AreEqual(product.TopLevelCategory, topLevelCategory);
            Assert.AreEqual(product.CategoryPath, categoryPath);
            Assert.AreEqual(product.SKUs.Count, skus.Length);
            Assert.AreEqual(product.Description.Features.Count, features.Count);

        }

        [TestMethod]
        public void ContructorFromXmlTest()
        {
            string xml = @"<?xml version='1.0' encoding='UTF-8'?>
                <Product>
                    <AssetID>999</AssetID>
                    <SKUs>
                        <SKU>
                            <SKUNumber>123</SKUNumber>
                        </SKU>
                    </SKUs>
                    <DueDate>2014-02-11T10:22:46Z</DueDate>
                </Product>
            ";

            XDocument document = XDocument.Parse(xml);

            var product = new Product(document.Element("Product"), new MockContentAPI());

            Assert.AreEqual(999, product.AssetID);
            Assert.AreEqual(DateTime.Parse("2014-02-11T10:22:46Z"), product.DueDate);
            Assert.IsNotNull(product.SKUs);
            Assert.AreEqual(1, product.SKUs.Count());
            Assert.AreEqual("123", product.SKUs.FirstOrDefault().SKUNumber);

        }

        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void ToXmlStringTest()
        {
            
            var product = new Product(title: "The title of the item",
                                                             primaryCategory: 123,
                                                             topLevelCategory: 1,
                                                             categoryPath: "Clothing : Menswear : Shoes > 12",
                                                             skus: new SKU[] {new SKU("1234", new Dictionary<string,string>() {{"Color", "White"}, {"Size", "Large & Tall"}})},
                                                             description: new ProductDescription(arbitraryElements: null, 
                                                                 features: new Dictionary<String, String>() { {"Feature1", "Feature 1 & Bold"}, {"Feature2", "Feature 2"} },
                                                                 summary: @"This is a summary it can contain HTML markup.
                                                                    To tell the translation service to ignore some
                                                                    text, <b>wrap</b> it in a
                                                                    [do-not-translate]
                                                                    do not translate
                                                                    [/do-not-translate]
                                                                    tag"));

            

            String xml = @"
                
                <Product>
                    <Title>The title of the item</Title>
                    <PrimaryCategory>123</PrimaryCategory>
                    <TopLevelCategory>1</TopLevelCategory>
                    <CategoryPath>Clothing : Menswear : Shoes &gt; 12</CategoryPath>
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
                                    <Value>Large &amp; Tall</Value>
                                </ItemSpecific>
                            </ItemSpecifics>
                        </SKU>
                    </SKUs>
                </Product> 
            ";

            Assert.AreEqual(Regex.Replace(xml, @"\s", ""), Regex.Replace(product.ToXmlString(), @"\s", ""));
        }
    }
}
