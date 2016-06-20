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
using Lionbridge.onDemand.Client;

namespace Lionbridge.onDemand.Client
{
    /// <summary>
    /// This class represents a File object
    /// </summary>
    public class File : IXmlSerializable
    {

        #region Properties

        /// <summary>
        /// Internal onDemand ID for the file asset.
        /// </summary>
        public Int32 AssetID
        {
            get;
            internal set;
        }

        /// <summary>
        /// The current state in the onDemand File Lifecycle
        /// </summary>
        public FileStatus Status
        {
            get;
            private set;
        }

        /// <summary>
        /// Internal onDemand ID of the project that this asset has been associated with. If no project has been associated, this element will be empty.
        /// </summary>
        public Int32 ProjectID
        {
            get;
            internal set;
        }

        /// <summary>
        /// Original name of the file.
        /// </summary>
        public String Name
        {
            get;
            private set;
        }

        /// <summary>
        /// URL to download the file.
        /// </summary>
        public Uri URL
        {
            get;
            private set;
        }

        /// <summary>
        /// DateTime that the file was uploaded
        /// </summary>
        public DateTime UploadDate
        {
            get;
            private set;
        }

        /// <summary>
        /// Contains attributes of the source language
        /// </summary>
        public SourceLanguage SourceLanguage
        {
            get;
            private set;
        }

        /// <summary>
        /// The list of target languages 
        /// </summary>
        public TargetLanguage[] TargetLanguages
        {
            get;
            private set;
        }

        /// <summary>
        /// An enumerable list of translations for this file by language
        /// </summary>
        public IEnumerable<KeyValuePair<String, Byte[]>> AllTranslations
        {
            get
            {
                foreach (TargetLanguage targetLanguage in this.TargetLanguages)
                {
                    if (!this.TranslationsMap.ContainsKey(targetLanguage.LanguageCode))
                    {
                        this.GetTranslation(targetLanguage.LanguageCode);
                    }
                }

                return this.TranslationsMap;
            }
        }

        /// <summary>
        /// Dictionary of languages to translated files
        /// </summary>
        private Dictionary<String, Byte[]> TranslationsMap
        {
            get;
            set;
        }

        /// <summary>
        /// Reference to the sdk client that retrieved this object
        /// </summary>
        private IContentAPI Client
        {
            get;
            set;
        }


        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create an empty File instance
        /// </summary>
        internal File()
        {
            this.TargetLanguages = new TargetLanguage[0];
            this.TranslationsMap = new Dictionary<string, byte[]>();
        }

        /// <summary>
        /// Create a File instance from an XML string
        /// </summary>
        /// <param name="element">A File element</param>
        public File(XElement element, IContentAPI client)
            : this()
        {

            this.Client = client;

            this.UpdateFromXElement(element);

        }

        /// <summary>
        /// Update an existing file object from XML
        /// </summary>
        /// <param name="element"></param>
        internal void UpdateFromXElement(XElement element)
        {
            if (element != null)
            {
                this.AssetID = element.GetChildValueAsInt32("AssetID");

                String fileStatus = element.GetChildValue("Status");

                if (!String.IsNullOrEmpty(fileStatus) && Enum.IsDefined(typeof(FileStatus), fileStatus.Replace(" ", "")))
                {
                    this.Status = (FileStatus)(Enum.Parse(typeof(FileStatus), fileStatus.Replace(" ", "")));
                }

                this.URL = element.GetChildValueAsUri("URL");


                this.ProjectID = element.GetChildValueAsInt32("ProjectID");
                
                this.Name = element.GetChildValue("Name");
                this.Name = String.IsNullOrEmpty(this.Name) ? element.GetChildValue("FileName") : this.Name;

                this.UploadDate = element.GetChildValueAsDateTime("UploadDate");

                this.SourceLanguage = new SourceLanguage(element.Element("SourceLanguage"));

                XElement targetLanguagesElement = element.Element("TargetLanguages");
                if (targetLanguagesElement != null)
                {
                    List<TargetLanguage> targetLanguages = new List<TargetLanguage>();

                    foreach (XElement targetLanguageElement in targetLanguagesElement.Elements("TargetLanguage"))
                    {
                        TargetLanguage targetLanguage = new TargetLanguage(targetLanguageElement);
                        targetLanguages.Add(targetLanguage);
                    }

                    this.TargetLanguages = targetLanguages.ToArray<TargetLanguage>();
                }

            }
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Create a collection of Files.
        /// </summary>
        /// <param name="parentElement">A parent element to one or more child File elements</param>
        /// <returns>A collection of Files</returns>
        internal static IEnumerable<File> CreateEnumerable(XElement parentElement, IContentAPI client)
        {
            List<File> fileList = new List<File>();

            if (parentElement != null)
            {
                var elements = from el in parentElement.Elements() where (el.Name == "File" || el.Name == "ReferenceFile") select el;

                foreach (XElement file in elements)
                {
                    File childFile = new File(file, client);
                    fileList.Add(childFile);
                }
            }

            IEnumerable<File> files = fileList.ToArray<File>();

            return files;
        }

        
        /// <summary>
        /// Seralize the class data to XML in an onDemand specific way using a URL to represent its location
        /// </summary>
        /// <param name="url">The URL where the file can be retrieved</param>
        /// <returns>The XML string which can be passed to onDemand</returns>
        internal static string ToXmlStringByURL(Uri url)
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("<File>");
            builder.Append(new XElement("URL", url).ToString(SaveOptions.DisableFormatting));
            builder.Append("</File>");

            return builder.ToString();
        }

        #endregion


        #region IXmlSerializable Members

        /// <summary>
        /// Seralize the class data to XML in an onDemand specific way
        /// </summary>
        /// <returns>The XML string which can be passed to onDemand</returns>
        public string ToXmlString()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("<File>");
            builder.Append("<AssetID>");
            builder.Append(this.AssetID);
            builder.Append("</AssetID>");
            builder.Append("</File>");

            return builder.ToString();
        }

        #endregion

        /// <summary>
        /// Seralize the class data to XML in an onDemand specific way with all of its properties
        /// </summary>
        /// <param name="isReferenceFile">Is the file a Reference file rather than a Translatable file</param>
        /// <param name="includeExtended">Include all the extended properties</param>
        /// <returns>The XML string which can be passed to onDemand</returns></returns>
        public string ToXmlStringExtended(bool isReferenceFile = true, bool includeExtended = false)
        {
            StringBuilder builder = new StringBuilder();

            builder.Append((isReferenceFile) ? "<ReferenceFile>" : "<File>");
            
            builder.Append("<AssetID>");
            builder.Append(this.AssetID);
            builder.Append("</AssetID>");

            if (includeExtended)
            {
                builder.Append(new XElement("FileName", this.Name).ToString(SaveOptions.DisableFormatting));

                builder.Append(new XElement("URL", this.URL).ToString(SaveOptions.DisableFormatting));

                if (this.ProjectID != 0)
                {
                    builder.Append("<ProjectID>");
                    builder.Append(this.ProjectID);
                    builder.Append("</ProjectID>");
                }
                
                builder.Append("<TargetLanguages>");
                if (this.TargetLanguages != null)
                {
                    foreach (var targetLanguage in this.TargetLanguages)
                    {
                        targetLanguage.ToXmlString();
                    }
                }
                builder.Append("</TargetLanguages>");
            }

            builder.Append((isReferenceFile) ? "</ReferenceFile>" : "</File>");

            return builder.ToString();
        }

        /// <summary>
        /// Update an existing file object
        /// </summary>
        /// <returns></returns>
        public void Update()
        {
            if (this.Client == null)
            {
                throw new InvalidOperationException("The file does not have an APIClient to communicate with");
            }

            this.Client.UpdateFile(this);
        }

        /// <summary>
        /// Retrieve the translation of this file for a particular language
        /// </summary>
        /// <param name="languageCode"></param>
        /// <returns>The file as an array of bytes</returns>
        public byte[] GetTranslation(String languageCode)
        {
            TargetLanguage targetLanguage = this.TargetLanguages.SingleOrDefault(p => p.LanguageCode == languageCode);

            if (targetLanguage == null)
            {
                throw new ArgumentException(string.Format("LanguageCode {0} is not one of the target languages for this file", languageCode), "languageCode");
            }

            if (this.TranslationsMap.ContainsKey(languageCode))
            {
                return this.TranslationsMap[languageCode];
            }

            //if (targetLanguage.Status == FileStatus.Translated)
            //{
                if (this.Client == null)
                {
                    throw new InvalidOperationException("The file does not have an APIClient to communicate with");
                }

                byte[] bytes = this.Client.GetFileTranslation(this.AssetID.ToString(), languageCode);

                this.TranslationsMap.Add(languageCode, bytes);

                return bytes;
            //}

            //return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="languageCode"></param>
        /// <returns></returns>
        public Boolean AcceptTranslation(String languageCode)
        {
            TargetLanguage targetLanguage = this.TargetLanguages.SingleOrDefault(p => p.LanguageCode == languageCode);

            if (targetLanguage == null)
            {
                throw new ArgumentException(string.Format("LanguageCode {0} is not one of the target languages for this file", languageCode), "languageCode");
            }

            if (this.Client == null)
            {
                throw new InvalidOperationException("The file does not have an APIClient to communicate with");
            }

            return this.Client.AcceptFileTranslation(this.AssetID.ToString(), languageCode);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="languageCode"></param>
        /// <param name="fileRejection"></param>
        /// <returns></returns>
        public Boolean RejectTranslation(String languageCode, FileRejection fileRejection)
        {
            TargetLanguage targetLanguage = this.TargetLanguages.SingleOrDefault(p => p.LanguageCode == languageCode);

            if (targetLanguage == null)
            {
                throw new ArgumentException(string.Format("LanguageCode {0} is not one of the target languages for this file", languageCode), "languageCode");
            }

            if (this.Client == null)
            {
                throw new InvalidOperationException("The file does not have an APIClient to communicate with");
            }

            var file = this.Client.RejectFileTranslation(this.AssetID.ToString(), languageCode, fileRejection);

            return file != null;
        }

        /// <summary>
        /// Serves as a hash function for a particular type
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            if (this.AssetID > 0)
            {
                return this.AssetID;
            }

            return base.GetHashCode();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as File);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(File other)
        {
            // Check for null
            if (ReferenceEquals(other, null))
            {
                return false;
            }

            // Check for same reference
            if (ReferenceEquals(this, other))
            {
                return true;
            }

            // Check for same Id and same Values
            return this.AssetID == other.AssetID;
        }
        
    }
}
