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
    /// Options for configurating the translation
    /// </summary>
    public class TranslationOptions
    {
        #region Public Properties

        /// <summary>
        /// 
        /// </summary>
        public String Currency
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public Uri NotifyCompleteURL
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public Uri NotifyQuoteReadyURL
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public Uri NotifyQuotePaidURL
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public Int32 ServiceID
        {
            get;
            internal set;
        }

        /// <summary>
        /// 
        /// </summary>
        public SourceLanguage SourceLanguage
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<TargetLanguage> TargetLanguages
        {
            get;
            set;
        }

        #endregion


        #region Constructors
        
        /// <summary>
        /// Default Empty Constructor
        /// </summary>
        protected TranslationOptions() 
        {

        }

        /// <summary>
        /// Constructor for use with non-project quotes
        /// </summary>
        /// <param name="sourceLanguage"></param>
        /// <param name="targetLanguages">If null, will use ALL the Service's target languages</param>
        /// <param name="currency">If null, will use the Account's default currency</param>
        /// <param name="notifyCompleteURL"></param>
        /// <param name="notifyQuoteReadyURL"></param>
        /// <param name="notifyQuotePaidURL"></param>
        protected TranslationOptions(String sourceLanguage, IEnumerable<String> targetLanguages = null, String currency = null, Uri notifyCompleteURL = null, Uri notifyQuoteReadyURL = null, Uri notifyQuotePaidURL = null)
            : this (currency, notifyCompleteURL, notifyQuoteReadyURL, notifyQuotePaidURL)
        {
            if (sourceLanguage == null)
            {
                throw new ArgumentNullException("sourceLanguage", "Source Language cannot be null");
            }

            this.SourceLanguage = new SourceLanguage(sourceLanguage);

            if (targetLanguages != null)
            {
                this.TargetLanguages = targetLanguages.Select(p => new TargetLanguage(p));
            }
        }

        /// <summary>
        /// Constructor for use with non-project quotes
        /// </summary>
        /// <param name="sourceLanguageLocale"></param>
        /// <param name="targetLanguages">If null, will use ALL the Service's target languages</param>
        /// <param name="currency">If null, will use the Account's default currency</param>
        /// <param name="notifyCompleteURL"></param>
        /// <param name="notifyQuoteReadyURL"></param>
        /// <param name="notifyQuotePaidURL"></param>
        protected TranslationOptions(Locale sourceLanguageLocale, IEnumerable<Locale> targetLanguageLocales = null, String currency = null, Uri notifyCompleteURL = null, Uri notifyQuoteReadyURL = null, Uri notifyQuotePaidURL = null)
            : this(currency, notifyCompleteURL, notifyQuoteReadyURL, notifyQuotePaidURL)
        {
            if (sourceLanguageLocale == null)
            {
                throw new ArgumentNullException("sourceLanguageLocale", "Source Language cannot be null");
            }

            this.SourceLanguage = new SourceLanguage(sourceLanguageLocale);

            if (targetLanguageLocales != null)
            {
                this.TargetLanguages = targetLanguageLocales.Select(p => new TargetLanguage(p));
            }
        }

        /// <summary>
        /// Constructor for use with project quotes
        /// </summary>
        /// <param name="currency">If null, will use the Account's default currency</param>
        /// <param name="notifyCompleteURL"></param>
        /// <param name="notifyQuoteReadyURL"></param>
        /// <param name="notifyQuotePaidURL"></param>
        protected TranslationOptions(String currency = null, Uri notifyCompleteURL = null, Uri notifyQuoteReadyURL = null, Uri notifyQuotePaidURL = null)
        {
            this.Currency = currency;
            this.NotifyCompleteURL = notifyCompleteURL;
            this.NotifyQuoteReadyURL = notifyQuoteReadyURL;
            this.NotifyQuotePaidURL = notifyQuotePaidURL;
        }

        /// <summary>
        /// Constructor for use when adding project
        /// </summary>
        /// <param name="sourceLanguage"></param>
        /// <param name="targetLanguages">If null, will use ALL the Service's target languages</param>
        protected TranslationOptions(String sourceLanguage, IEnumerable<String> targetLanguages = null, String currency = null)
        {
            if (sourceLanguage == null)
            {
                throw new ArgumentNullException("sourceLanguage", "Source Language cannot be null");
            }

            this.SourceLanguage = new SourceLanguage(sourceLanguage);

            if (targetLanguages != null)
            {
                this.TargetLanguages = targetLanguages.Select(p => new TargetLanguage(p));
            }

            this.Currency = currency;
        }

        /// <summary>
        /// Constructor for use when adding project
        /// </summary>
        /// <param name="sourceLanguage"></param>
        /// <param name="targetLanguages">If null, will use ALL the Service's target languages</param>
        protected TranslationOptions(Locale sourceLanguage, IEnumerable<Locale> targetLanguages = null, String currency = null)
        {
            if (sourceLanguage == null)
            {
                throw new ArgumentNullException("sourceLanguage", "Source Language cannot be null");
            }

            this.SourceLanguage = new SourceLanguage(sourceLanguage);

            if (targetLanguages != null)
            {
                this.TargetLanguages = targetLanguages.Select(p => new TargetLanguage(p));
            }

            this.Currency = currency;
        }
        
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="service"></param>
        internal void Initialize(IContentAPI client, Service service)
        {
            if (client == null)
            {
                throw new ArgumentNullException("client", "Client cannot be null");
            }

            if (service == null)
            {
                throw new ArgumentNullException("service", "Service cannot be null");
            }

            if (this.Currency == null)
            {
                this.Currency = client.DefaultCurrency;
            }

            this.ServiceID = service.ServiceID;

            if (this.TargetLanguages == null)
            {
                this.TargetLanguages = service.TargetLanguages.Select(p => new TargetLanguage(p));
            }

            if (!service.SourceLanguages.Contains(this.SourceLanguage.LanguageCode))
            {
                throw new ArgumentOutOfRangeException("sourceLanguage", this.SourceLanguage.LanguageCode, "Source language must be in the Service's list of source languages");
            }

            if (this.TargetLanguages == null)
            {
                throw new ArgumentNullException("targetLanguages", "Target languages cannot be null");
            }

            if (this.TargetLanguages.Count() == 0)
            {
                throw new ArgumentOutOfRangeException("targetLanguages", "Must include at least one target language");
            }

            foreach (TargetLanguage targetLanguage in this.TargetLanguages)
            {
                if (!service.TargetLanguages.Contains(targetLanguage.LanguageCode))
                {
                    throw new ArgumentOutOfRangeException("targetLanguages", targetLanguage.LanguageCode, "Target lanauges must be in the Service's list of target languages");
                }
            }
   
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        internal void Initialize(IContentAPI client)
        {
            if (client == null)
            {
                throw new ArgumentNullException("client", "Client cannot be null");
            }

            if (this.Currency == null)
            {
                this.Currency = client.DefaultCurrency;
            }
        }


        #region IXmlSerializable

        /// <summary>
        /// Seralize the class data to XML in an onDemand specific way
        /// </summary>
        /// <returns>The XML string which can be passed to onDemand</returns>
        public string ToXmlString()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("<TranslationOptions>");

            builder.Append("<Currency>" + this.Currency + "</Currency>");

            if (this.NotifyCompleteURL != null)
            {
                builder.Append(new XElement("NotifyCompleteURL", this.NotifyCompleteURL).ToString(SaveOptions.DisableFormatting));
            }

            if (this.NotifyQuoteReadyURL != null)
            {
                builder.Append(new XElement("NotifyQuoteReadyURL", this.NotifyQuoteReadyURL).ToString(SaveOptions.DisableFormatting));
            }

            if (this.NotifyQuotePaidURL != null)
            {
                builder.Append(new XElement("NotifyQuotePaidURL", this.NotifyQuotePaidURL).ToString(SaveOptions.DisableFormatting));
            }

            if (this.ServiceID != 0)
            {
                builder.Append("<ServiceID>" + this.ServiceID + "</ServiceID>");
            }

            if (this.SourceLanguage != null)
            {
                builder.Append(this.SourceLanguage.ToXmlString());
            }

            if (this.TargetLanguages != null)
            {
                builder.Append("<TargetLanguages>");
                foreach (TargetLanguage targetLanguage in this.TargetLanguages)
                {
                    builder.Append(targetLanguage.ToXmlString());
                }
                builder.Append("</TargetLanguages>");
            }

            builder.Append("</TranslationOptions>");

            return builder.ToString();
        }

        #endregion
    }
}
