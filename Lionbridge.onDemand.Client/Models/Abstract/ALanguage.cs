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

namespace Lionbridge.onDemand.Client
{
    /// <summary>
    /// Abstract base class for classes representing a Language which is either a source or target for Translation
    /// </summary>
    public abstract class ALanguage : IEqualityComparer<ALanguage>
    {
        #region Private Members

        // Locale code representing the language
        private String languageCode;

        #endregion


        #region Properties

        /// <summary>
        /// A locale code in the format en-us where EN is the 2 character ISO language code and US is the 2 character ISO country code.
        /// </summary>
        public String LanguageCode
        {
            get
            {
                return this.languageCode;
            }
            protected set
            {
                if (value != null && (value.Length != 5 || value[2] != '-'))
                {
                    throw new ArgumentException("Language codes should be in the format: gb-en", languageCode);
                }

                this.languageCode = value;
            }
        }

        #endregion


        #region Constructors

        /// <summary>
        /// Protected empty constructor
        /// </summary>
        protected ALanguage()
        {

        }

        /// <summary>
        /// Constructor given a string language code
        /// </summary>
        /// <param name="languageCode"></param>
        protected ALanguage(String languageCode)
        {
            this.LanguageCode = languageCode;
        }

        /// <summary>
        /// Constructor given a Locale object
        /// </summary>
        /// <param name="locale"></param>
        protected ALanguage(Locale locale)
        {
            this.LanguageCode = locale.Code;
        }

        #endregion


        #region IEqualityComparer

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool Equals(ALanguage x, ALanguage y)
        {
            return x.LanguageCode == y.LanguageCode;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool Equals(String x, ALanguage y)
        {
            return x == y.LanguageCode;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool Equals(ALanguage x, String y)
        {
            return x.LanguageCode == y;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int GetHashCode(ALanguage obj)
        {
            return (char)obj.LanguageCode[0] ^
                   (char)obj.LanguageCode[1] ^
                   (char)obj.LanguageCode[3] ^
                   (char)obj.LanguageCode[4];
        }

        #endregion
    }
}
