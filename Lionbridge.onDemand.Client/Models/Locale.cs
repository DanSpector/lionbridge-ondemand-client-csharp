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
    /// Represents a Language Locale
    /// </summary>
    public class Locale
    {
        /// <summary>
        /// The name of the language locale
        /// </summary>
        public String Name
        {
            get;
            private set;
        }

        /// <summary>
        /// The ISO code of the language locale
        /// </summary>
        public String Code
        {
            get;
            private set;
        }

        /// <summary>
        /// Default Constructor
        /// </summary>
        private Locale()
        {

        }

        /// <summary>
        /// Constructor from XML
        /// </summary>
        /// <param name="element"></param>
        internal Locale(XElement element)
        {
            if (element != null)
            {
                this.Name = element.GetChildValue("Name");
                this.Code = element.GetChildValue("Code");
            }
        }

        /// <summary>
        /// Creates an enumerable of Locale from XML
        /// </summary>
        /// <param name="element"></param>
        /// <param name="client"></param>
        /// <returns></returns>
        internal static IEnumerable<Locale> CreateEnumerable(XElement element)
        {
            List<Locale> result = new List<Locale>();

            foreach (XElement locale in element.Elements("Locale"))
            {
                result.Add(new Locale(locale));
            }

            return result;
        }
    }
}
