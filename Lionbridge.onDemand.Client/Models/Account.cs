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
    /// Information about an Account
    /// </summary>
    public class Account
    {
        #region Properties

        /// <summary>
        /// Merchant system account id
        /// </summary>
        public String MerchantID
        {
            get;
            private set;
        }

        /// <summary>
        /// Email address of the primary user
        /// </summary>
        public String Email
        {
            get;
            private set;
        }

        /// <summary>
        /// First name of the primary user (optional)
        /// </summary>
        public String FirstName
        {
            get;
            private set;
        }

        /// <summary>
        /// Last name of the primary user (optional)
        /// </summary>
        public String LastName
        {
            get;
            private set;
        }

        /// <summary>
        /// Merchant Company
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
        /// String representing the 20 character access key id
        /// </summary>
        public String AccessKeyID
        {
            get;
            private set;
        }

        /// <summary>
        /// String representing the 40 character secret key
        /// </summary>
        public String SecretAccessKey
        {
            get;
            private set;
        }

        /// <summary>
        /// The request status
        /// </summary>
        private String Status
        {
            get;
            set;
        }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create an empty account 
        /// </summary>
        protected Account()
        {
            // Nothing to do
        }


        /// <summary>
        /// Create an account from XML
        /// </summary>
        /// <param name="element"></param>
        internal Account(XElement element)
            : this()
        {
            if (element != null)
            {
                this.MerchantID = element.GetChildValue("MerchantID");

                this.Status = element.GetChildValue("Status");

                this.Email = element.GetChildValue("EmailAddress");

                this.FirstName = element.GetChildValue("FirstName");

                this.LastName= element.GetChildValue("LastName");

                this.CompanyName = element.GetChildValue("CompanyName");

                this.Country = element.GetChildValue("Country");

                this.AccessKeyID = element.GetChildValue("AccessKeyID");

                this.SecretAccessKey = element.GetChildValue("SecretAccessKey");
            }
        }

        #endregion
    }
}
