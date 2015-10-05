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
using System.Xml.XPath;

namespace Lionbridge.onDemand.Client
{
    /// <summary>
    /// 
    /// </summary>
    public class SKU : IXmlSerializable
    {
        /// <summary>
        /// SKU Number
        /// </summary>
        public String SKUNumber
        {
            get;
            set;
        }

        /// <summary>
        /// Contains elements representing specifications.
        /// </summary>
        public Dictionary<String, String> ItemSpecifics
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public SKU()
        {
            this.ItemSpecifics = new Dictionary<string, string>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="skuNumber"></param>
        public SKU(String skuNumber)
            : this()
        {
            this.SKUNumber = skuNumber;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="skuNumber"></param>
        /// <param name="itemSpecifics"></param>
        public SKU(String skuNumber, Dictionary<string, string> itemSpecifics)
        {
            this.SKUNumber = skuNumber;
            this.ItemSpecifics = itemSpecifics;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        internal SKU(XElement element)
            : this()
        {
            XElement skuNumber = element.Element(XName.Get("SKUNumber"));

            if (skuNumber != null)
            {
                this.SKUNumber = skuNumber.Value;
            }

            XElement itemSpecifics = element.Element(XName.Get("ItemSpecifics"));

            if (itemSpecifics != null)
            {
                foreach (XElement nameValue in itemSpecifics.Descendants(XName.Get("ItemSpecific")))
                {
                    XElement name = nameValue.Element(XName.Get("Name"));
                    XElement value = nameValue.Element(XName.Get("Value"));

                    if (name != null && value != null)
                    {
                        if (!this.ItemSpecifics.ContainsKey(name.Value))
                        {
                            this.ItemSpecifics.Add(name.Value, value.Value);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Seralize the class data to XML in an onDemand specific way
        /// </summary>
        /// <returns>The XML string which can be passed to onDemand</returns>
        public String ToXmlString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<SKU>");

            builder.Append(new XElement("SKUNumber", this.SKUNumber).ToString(SaveOptions.DisableFormatting));

            if (this.ItemSpecifics != null && this.ItemSpecifics.Count > 0)
            {
                builder.Append("<ItemSpecifics>");
                foreach (String key in this.ItemSpecifics.Keys)
                {
                    builder.Append("<ItemSpecific>");

                    builder.Append(new XElement("Name", key).ToString(SaveOptions.DisableFormatting));

                    builder.Append(new XElement("Value", this.ItemSpecifics[key]).ToString(SaveOptions.DisableFormatting));

                    builder.Append("</ItemSpecific>");
                }
                builder.Append("</ItemSpecifics>");
            }

            builder.Append("</SKU>");
            return builder.ToString();
        }

        /// <summary>
        /// Returns a collection of child SKUs given a parent element
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        internal static IEnumerable<SKU> CreateEnumerable(XElement element)
        {
            List<SKU> result = new List<SKU>();

            foreach (XElement sku in element.Elements(XName.Get("SKU")))
            {
                result.Add(new SKU(sku));
            }

            return result;
        }
     }
}
