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
    public class ProductDescriptionTest
    {
        [TestMethod]
        public void ContructorFromXmlTest()
        {
            string xml = @"<?xml version='1.0' encoding='UTF-8'?>
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
            ";

            XDocument document = XDocument.Parse(xml);

            var description = new ProductDescription(document.Element("Description"));

            Assert.AreEqual(Regex.Replace(@"This is a summary it can contain HTML markup.
                To tell the translation service to ignore some
                text, <b>wrap</b> it in a
                [do-not-translate]
                do not translate
                [/do-not-translate]
                tag", @"\s", ""), Regex.Replace(description.Summary, @"\s", ""));


            XElement[] arbitrary = new XElement[] { XElement.Parse("<ArbitraryXml1><Whoa>There</Whoa></ArbitraryXml1>"), XElement.Parse("<ArbitraryXml2><Whoa>There</Whoa></ArbitraryXml2>") };
            Dictionary<String, String> features = new Dictionary<String, String>() { {"Feature1", "Feature 1 & Bold"}, {"Feature2", "Feature 2"} };

            for (int i = 0; i < arbitrary.Length; i++)
            {
                Assert.AreEqual(arbitrary[i].ToString(SaveOptions.None), description.ArbitraryElements[i].ToString(SaveOptions.None));
            }

            foreach (string featureKey in features.Keys)
            {
                Assert.AreEqual(features[featureKey], description.Features[featureKey]);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void ToXmlStringTest()
        {

            var description = new ProductDescription(
                arbitraryElements: new XElement[] { XElement.Parse("<ArbitraryXml1><Whoa>There</Whoa></ArbitraryXml1>"), XElement.Parse("<ArbitraryXml2><Whoa>There</Whoa></ArbitraryXml2>") },
                features: new Dictionary<String, String>() { {"Feature1", "Feature 1 & Bold"}, {"Feature2", "Feature 2"} },
                summary: @"This is a summary it can contain HTML markup.
                To tell the translation service to ignore some
                text, <b>wrap</b> it in a
                [do-not-translate]
                do not translate
                [/do-not-translate]
                tag");

            String xml = @"
                
                
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
                    
            ";

            Assert.AreEqual(Regex.Replace(xml, @"\s", ""), Regex.Replace(description.ToXmlString(), @"\s", ""));
        }
    }
}
