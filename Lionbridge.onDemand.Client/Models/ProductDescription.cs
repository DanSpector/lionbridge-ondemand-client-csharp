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
using System.Xml.Linq;

namespace Lionbridge.onDemand.Client
{
    /// <summary>
    /// The description of the item. The description element can contain sub-elements. HTML that is not well formed XML goes in the Summary
    /// </summary>
    public class ProductDescription : IXmlSerializable
    {
        /// <summary>
        /// Arbitrary XML elements that provide description for the product
        /// This is not for HTML data that is not well formed (Use Summary instead)
        /// </summary>
        public XElement[] ArbitraryElements
        {
            get;
            set;
        }

        /// <summary>
        /// Summary HTML that is not well formed (will be wrapped in CDATA tags).
        /// </summary>
        public String Summary
        {
            get;
            set;
        }

        /// <summary>
        /// Features of the Product represented as a series of key value pairs
        /// </summary>
        public Dictionary<String, String> Features
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public ProductDescription()
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="arbitraryElements"></param>
        /// <param name="features"></param>
        public ProductDescription(XElement[] arbitraryElements, Dictionary<String, String> features = null)
            : this(arbitraryElements, features, null)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="summary"></param>
        public ProductDescription(String summary) 
            : this(null, null, summary)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="features"></param>
        public ProductDescription(Dictionary<String, String> features)
            : this(null, features, null)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="arbitraryElements"></param>
        /// <param name="features"></param>
        /// <param name="summary"></param>
        public ProductDescription(XElement[] arbitraryElements, Dictionary<String, String> features, String summary)
        {
            this.ArbitraryElements = arbitraryElements;
            this.Features = features;
            this.Summary = summary;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        public ProductDescription(XElement element)
        {
            if (element != null)
            {
                XElement featuresElement = element.Element("Features");
                if (featuresElement != null)
                {
                    this.Features = featuresElement.Elements().ToDictionary(p => p.Name.LocalName, q => q.Value);
                }
                else
                {
                    this.Features = null;
                }

                this.Summary = element.GetChildValueCDATA("Summary");

                this.ArbitraryElements = element.Elements().Where(p => p.Name != "Summary" && p.Name != "Features").ToArray();
            }
        }



        #region IXmlSerializable Members

        /// <summary>
        /// Seralize the class data to XML in an onDemand specific way
        /// </summary>
        /// <returns>The XML string which can be passed to onDemand</returns>
        public string ToXmlString()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("<Description>");

            // Arbitrary XML under description
            if (this.ArbitraryElements != null)
            {
                foreach (XElement element in this.ArbitraryElements)
                {
                    builder.Append(new XElement(element).ToString(SaveOptions.DisableFormatting));
                }
            }

            // Arbitrary non-XML "summary" data under description
            if (!String.IsNullOrEmpty(this.Summary))
            {
                builder.Append("<Summary>");
                builder.AppendLine("<![CDATA[");
                builder.AppendLine(this.Summary);
                builder.AppendLine("]]>");
                builder.Append("</Summary>");
            }

            // Features under description
            builder.Append("<Features>");
            if (this.Features != null)
            {
                foreach (var feature in this.Features)
                {
                    builder.Append(new XElement(feature.Key, feature.Value).ToString(SaveOptions.DisableFormatting));
                }
            }
            builder.Append("</Features>");

            builder.Append("</Description>");

            return builder.ToString();
        }

        #endregion
    }
}
