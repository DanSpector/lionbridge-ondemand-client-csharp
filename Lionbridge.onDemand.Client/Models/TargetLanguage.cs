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
    /// This class represents a Target Language.
    /// </summary>
    public class TargetLanguage : ALanguage, IXmlSerializable
    {
        #region Properties

        /// <summary>
        /// Number of products translated into this language
        /// </summary>
        public int Count
        {
            get;
            set;
        }

        /// <summary>
        /// The status of the translation
        /// </summary>
        public TranslatedFileStatus Status
        {
            get;
            set;
        }

        /// <summary>
        /// URL to retrieve the project information.
        /// </summary>
        public Uri ProjectURL
        {
            get;
            set;
        }

        /// <summary>
        /// URL to download the translated file or product.
        /// </summary>
        public Uri DownloadURL
        {
            get;
            set;
        }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create an empty Target Language 
        /// </summary>
        protected TargetLanguage()
            : base()
        {
            this.Status = TranslatedFileStatus.None;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="languageCode"></param>
        public TargetLanguage(String languageCode)
            : base(languageCode)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="locale"></param>
        public TargetLanguage(Locale locale)
            : base(locale)
        {

        }

        /// <summary>
        /// Create a Target Language from XML
        /// </summary>
        /// <param name="element"></param>
        internal TargetLanguage(XElement element)
            : this()
        {
            if (element != null)
            {
                this.LanguageCode = element.GetChildValue("LanguageCode");

                this.Count = element.GetChildValueAsInt32("Count");

                String fileStatus = element.GetChildValue("Status");
                if (FileStatus.IsDefined(typeof(TranslatedFileStatus), fileStatus))
                {
                    this.Status = (TranslatedFileStatus)(Enum.Parse(typeof(TranslatedFileStatus), fileStatus));
                }

                try
                {
                    String projectUrl = element.GetChildValue("ProjectURL");

                    if (!String.IsNullOrEmpty(projectUrl))
                    {
                        this.ProjectURL = new Uri(projectUrl);
                    }
                }
                catch (UriFormatException)
                {
                    // leave default value
                }

                try
                {
                    String url = element.GetChildValue("DownloadURL");
                    if (String.IsNullOrEmpty(url))
                    {
                        url = element.GetChildValue("URL");
                    }
                    this.DownloadURL = new Uri(url);
                }
                catch (UriFormatException)
                {
                    // leave default value
                }
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

            builder.Append("<TargetLanguage>");
            builder.Append("<LanguageCode>" + this.LanguageCode + "</LanguageCode>");
            builder.Append("</TargetLanguage>");

            return builder.ToString();
        }

        #endregion


        #region Static Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="elements"></param>
        /// <returns></returns>
        public static IEnumerable<TargetLanguage> CreateEnumerable(XElement element)
        {
            List<TargetLanguage> retVal = new List<TargetLanguage>();

            foreach(XElement subElement in element.Elements("TargetLanguage"))
            {
                retVal.Add(new TargetLanguage(subElement));
            }

            return retVal.ToArray();
        }

        #endregion
    }
}
