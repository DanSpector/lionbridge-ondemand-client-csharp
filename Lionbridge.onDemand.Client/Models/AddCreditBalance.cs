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
    /// Information to generate a payment request and the response
    /// </summary>
    public class AddCreditBalance : IXmlSerializable
    {
        /// <summary>
        /// Amount of of money to add to the balance
        /// </summary>
        /// <remarks>Decimal value should have 2 decimal places</remarks>
        public Decimal Amount
        {
            get;
            private set;
        }

        /// <summary>
        /// Currency used to pay for the project. See glossary for list of valid currencies.
        /// </summary>
        public String Currency
        {
            get;
            private set;
        }

        /// <summary>
        /// URL of payment page
        /// </summary>
        public Uri PaymentURL
        {
            get;
            private set;
        }

        /// <summary>
        /// Private default constructor
        /// </summary>
        private AddCreditBalance()
        {

        }

        /// <summary>
        /// Construct a new credit balance prepaid request
        /// </summary>
        /// <param name="amount">Amount of of money to add to the balance</param>
        /// <param name="currency">Currency used to pay for the project. See glossary for list of valid currencies.</param>
        public AddCreditBalance(Decimal amount, String currency)
            : this()
        {
            this.Amount = amount;
            this.Currency = currency;
        }

        /// <summary>
        /// Creates an AddCreditBalance object from a response
        /// </summary>
        /// <param name="element">The root element</param>
        internal AddCreditBalance(XElement element)
        {
            if (element != null)
            {
                this.Amount = element.GetChildValueAsDecimal("Amount");

                this.Currency = element.GetChildValue("Currency");

                this.PaymentURL = new Uri(element.GetChildValue("PaymentURL"));
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
            builder.Append("<AddCreditBalance>");

            builder.Append("<Amount>");
            builder.Append(this.Amount.ToString(CultureInfo.InvariantCulture));
            builder.Append("</Amount>");

            builder.Append("<Currency>");
            builder.Append(this.Currency);
            builder.Append("</Currency>");

            builder.Append("</AddCreditBalance>");
            return builder.ToString();
        }

        #endregion
    }
}
