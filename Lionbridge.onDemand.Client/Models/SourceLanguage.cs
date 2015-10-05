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
    /// This class represents a Source Language.
    /// </summary>
    public class SourceLanguage : ALanguage, IXmlSerializable
    {
        #region Constructor(s)

        /// <summary>
        /// Create an empty Source Language 
        /// </summary>
        protected SourceLanguage()
            : base()
        {
            // Nothing to do
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="languageCode"></param>
        public SourceLanguage(String languageCode)
            : base(languageCode)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="locale"></param>
        public SourceLanguage(Locale locale)
            : base(locale)
        {

        }
       
        /// <summary>
        /// Create a Source Language from XML
        /// </summary>
        /// <param name="element">The SourceLanguage parent element</param>
        internal SourceLanguage(XElement element)
            : this()
        {
            if (element != null)
            {
                this.LanguageCode = element.GetChildValue("LanguageCode");
            }
        }
        #endregion


        #region IXmlSerializable

        /// <summary>
        /// Seralize the class data to XML in an onDemand specific way
        /// </summary>
        /// <returns>The XML string which can be passed to onDemand</returns>
        public string ToXmlString()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("<SourceLanguage>");
            builder.Append("<LanguageCode>" + this.LanguageCode + "</LanguageCode>");
            builder.Append("</SourceLanguage>");

            return builder.ToString();
        }

        #endregion
    }
}
