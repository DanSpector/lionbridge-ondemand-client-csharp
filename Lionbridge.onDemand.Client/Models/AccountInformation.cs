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
    public class AccountInformation
    {
        #region Properties

        /// <summary>
        /// Email address associated with the address
        /// </summary>
        public String Email
        {
            get;
            private set;
        }

        /// <summary>
        /// Currency that the merchant is configured to transact in. 
        /// </summary>
        public String Currency
        {
            get;
            private set;
        }

        /// <summary>
        /// Total money spent in the merchant’s currency
        /// </summary>
        public Decimal TotalSpent
        {
            get;
            private set;
        }

        /// <summary>
        /// Amount of pre-paid funds in the merchant’s account.
        /// </summary>
        public Decimal PrepaidCredit
        {
            get;
            private set;
        }

        /// <summary>
        /// Number of translations granted to the merchant.
        /// </summary>
        public Int32 TranslationCredit
        {
            get;
            private set;
        }

        /// <summary>
        /// Number of credit translations used.
        /// </summary>
        public Int32 TranslationCreditUsed
        {
            get;
            private set;
        }

        /// <summary>
        /// Number of Products that have been submitted to the service.
        /// </summary>
        public Int32 ProductCount
        {
            get;
            private set;
        }

        /// <summary>
        /// A collection of target languages that the merchant has translated into
        /// </summary>
        /// <typeparam name="TargetLanguage">A TargetLanguage instance</typeparam>
        /// <returns>A colleciton of target language instances</returns>
        public IEnumerable<TargetLanguage> TargetLanguages
        {
            get;
            private set;
        }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create an empty AccountInformation instance
        /// </summary>
        protected AccountInformation()
        {
            this.TargetLanguages = new List<TargetLanguage>();
        }

        /// <summary>
        /// Create an AccountInformation instance from an XML string
        /// </summary>
        /// <param name="element">An Account element</param>
        internal AccountInformation(XElement element)
            : this()
        {
            if (element != null)
            {
                this.Email = element.GetChildValue("Email");

                this.Currency = element.GetChildValue("Currency");

                this.TotalSpent = element.GetChildValueAsDecimal("TotalSpent");

                this.TranslationCredit = element.GetChildValueAsInt32("TranslationCredit");

                this.TranslationCreditUsed = element.GetChildValueAsInt32("TranslationCreditUsed");

                this.PrepaidCredit = element.GetChildValueAsDecimal("PrepaidCredit");

                this.ProductCount = element.GetChildValueAsInt32("ProductCount");

                XElement targetLanguagesElement = element.Element("TargetLanguages");
                if (targetLanguagesElement != null)
                {
                    List<TargetLanguage> targets = new List<TargetLanguage>();

                    foreach (XElement targetLanguageElement in targetLanguagesElement.Elements("TargetLanguage"))
                    {
                        TargetLanguage targetLanguage = new TargetLanguage(targetLanguageElement);
                        targets.Add(targetLanguage);
                    }

                    this.TargetLanguages = targets;
                }
            }
        }

        #endregion
    }
}
