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
    /// A Payment shows details about how the quote was paid.
    /// </summary>
    public class Payment
    {

        /// <summary>
        /// PayPal, American Express, Master Card, Visa, Prepaid, Purchase Order, Translation Credit.
        /// </summary>
        public String Type
        {
            get;
            private set;
        }

        /// <summary>
        /// A string describing the funding source such as Amex Charge to card ending in 1234
        /// </summary>
        public String Description
        {
            get;
            private set;
        }

        /// <summary>
        /// Amount paid
        /// </summary>
        public Decimal Amount
        {
            get;
            private set;
        }

        /// <summary>
        /// Three letter currency code of the currency used in the transaction.
        /// </summary>
        public String Currency
        {
            get;
            private set;
        }

        /// <summary>
        /// Private constructor
        /// </summary>
        private Payment()
        {

        }

        /// <summary>
        /// Constructor from XML
        /// </summary>
        /// <param name="element"></param>
        /// <param name="client"></param>
        internal Payment(XElement element)
            : this()
        {
            
            if (element != null)
            {
                this.Type = element.GetChildValue("PaymentType");
                this.Description = element.GetChildValue("PaymentDescription");
                this.Currency = element.GetChildValue("PaymentCurrency");
                this.Amount = element.GetChildValueAsDecimal("PaymentAmount");
            }
        }

        /// <summary>
        /// Creates an Enumerable of Payment from XML
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        internal static IEnumerable<Payment> CreateEnumerable(XElement element)
        {
            List<Payment> result = new List<Payment>();

            foreach (XElement payment in element.Elements("Payment"))
            {
                result.Add(new Payment(payment));
            }

            return result;
        }
    }
}
