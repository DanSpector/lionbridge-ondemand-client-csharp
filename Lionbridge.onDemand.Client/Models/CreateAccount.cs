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
using System.Xml;
using System.Xml.Linq;

namespace Lionbridge.onDemand.Client
{
    /// <summary>
    /// A class to generate a Create Account request and handle the related response.
    /// </summary>
    public class CreateAccount : IXmlSerializable
    {

        #region Properties

        /// <summary>
        /// Merchant system account ID.
        /// </summary>
        public String MerchantID
        {
            get;
            private set;
        }

        /// <summary>
        /// Email address of the primary user.
        /// </summary>
        public String Email
        {
            get;
            private set;
        }

        /// <summary>
        /// First name of the primary user (optional).
        /// </summary>
        public String FirstName
        {
            get;
            private set;
        }

        /// <summary>
        /// Last name of the primary user (optional).
        /// </summary>
        public String LastName
        {
            get;
            private set;
        }

        /// <summary>
        /// Merchant Company.
        /// </summary>
        public String CompanyName
        {
            get;
            private set;
        }

        /// <summary>
        /// Country that the merchant is headquartered in. ISO 3166-1 2 character country code.
        /// </summary>
        public String Country
        {
            get;
            private set;
        }

        /// <summary>
        /// Tax ID for VAT accounting. Required for Irish merchants only.
        /// </summary>
        public String VATID
        {
            get;
            private set;
        }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Private default constructor
        /// </summary>
        private CreateAccount()
        {
        }


        /// <summary>
        /// Construct a new Create Account request
        /// </summary>
        public CreateAccount(String merchantID, String emailAddress, String firstName, String lastName, String companyName, String country, String vatID)
        {
            this.MerchantID = merchantID;
            this.Email = emailAddress;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.CompanyName = companyName;
            this.Country = country;
            this.VATID = vatID;
        }


        #endregion

        #region Public Methods

        /// <summary>
        /// Seralize the class data to XML in an onDemand specific way
        /// </summary>
        /// <returns>The XML string which can be passed to onDemand</returns>
        public String ToXmlString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<CreateAccount>");
            this.AddXmlElementToString(builder, "MerchantID", this.MerchantID);
            this.AddXmlElementToString(builder, "EmailAddress", this.Email);
            this.AddXmlElementToString(builder, "FirstName", this.FirstName);
            this.AddXmlElementToString(builder, "LastName", this.LastName);
            this.AddXmlElementToString(builder, "CompanyName", this.CompanyName);
            this.AddXmlElementToString(builder, "Country", this.Country);
            this.AddXmlElementToString(builder, "VATID", this.VATID);
            builder.Append("</CreateAccount>");

            return builder.ToString();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Create the XML element
        /// </summary>
        /// <param name="builder">The string builder</param>
        /// <param name="elementName">The element name</param>
        /// <param name="value">The element value</param>
        private void AddXmlElementToString(StringBuilder builder, String elementName, String value)
        {
            if (!String.IsNullOrEmpty(value))
            {
                builder.Append(new XElement(elementName, value).ToString(SaveOptions.DisableFormatting));
            }
        }

        #endregion

    }
}
