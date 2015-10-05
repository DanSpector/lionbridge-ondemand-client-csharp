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
using System.Xml.Linq;

namespace Lionbridge.onDemand.Client
{
    /// <summary>
    /// Contains helper functions for safely getting XElement child values in strongly typed ways
    /// </summary>
    internal static class Serialization
    {
        /// <summary>
        /// Safely get the XElement child value as an Int32
        /// </summary>
        /// <param name="element"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Int32 GetChildValueAsInt32(this XElement element, String name)
        {
            XElement child = element.Element(name);

            if (child != null)
            {
                int result = 0;

                if (Int32.TryParse(child.Value, out result))
                {
                    return result;
                }
            }

            return 0;
        }

        /// <summary>
        /// Safely get the XElement child value as an Int64
        /// </summary>
        /// <param name="element"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Int64 GetChildValueAsInt64(this XElement element, String name)
        {
            XElement child = element.Element(name);

            if (child != null)
            {
                long result = 0;

                if (Int64.TryParse(child.Value, out result))
                {
                    return result;
                }
            }

            return 0;
        }

        /// <summary>
        /// Safely get the XElement child value as a Decimal
        /// </summary>
        /// <param name="element"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Decimal GetChildValueAsDecimal(this XElement element, String name)
        {
            XElement child = element.Element(name);

            if (child != null)
            {
                Decimal result = 0;

                if (Decimal.TryParse(child.Value, NumberStyles.Currency, CultureInfo.InvariantCulture, out result))
                {
                    return result;
                }
            }

            return 0;
        }

        /// <summary>
        /// Safely get the XElement child value as a DateTime
        /// </summary>
        /// <param name="element"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static DateTime GetChildValueAsDateTime(this XElement element, String name)
        {
            XElement child = element.Element(name);

            if (child != null)
            {
                DateTime result = DateTime.MinValue;

                if (DateTime.TryParse(child.Value, out result))
                {
                    return result;
                }
            }

            return DateTime.MinValue;
        }

        /// <summary>
        /// Safely get the XElement child value as a Uri
        /// </summary>
        /// <param name="element"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Uri GetChildValueAsUri(this XElement element, String name)
        {
            XElement child = element.Element(name);

            if (child != null && !String.IsNullOrEmpty(child.Value))
            {
                Uri result = null;
                if (Uri.TryCreate(child.Value, UriKind.Absolute, out result))
                {
                    return result;
                }
            }

            return null;
        }

        /// <summary>
        /// Safely get the XElement child value as a String
        /// </summary>
        /// <param name="element"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static String GetChildValue(this XElement element, String name)
        {
            XElement child = element.Element(name);

            if (child != null)
            {
                return child.Value;
            }

            return "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static String GetChildValueCDATA(this XElement element, String name)
        {
            XElement child = element.Element(name);

            if (child != null)
            {
                StringBuilder builder = new StringBuilder();

                foreach (XCData cdata in child.DescendantNodes().OfType<XCData>())
                {
                    builder.Append(cdata.Value);
                }

                return builder.ToString();
            }

            return "";
        }
    }
}
