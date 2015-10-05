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
    /// Representation of a Product with Title, Description and other properties to be translated
    /// </summary>
    public class Product : IXmlSerializable
    {
        #region Properties

        /// <summary>
        /// Internal onDemand ID for the Product asset.
        /// </summary>
        public Int32 AssetID
        {
            get;
            private set;
        }

        /// <summary>
        /// The title of the product
        /// </summary>
        public String Title
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public ProductDescription Description
        {
            get;
            protected set;
        }

        /// <summary>
        /// ID of the product’s primary category
        /// </summary>
        public Int32 PrimaryCategory
        {
            get;
            protected set;
        }

        /// <summary>
        /// ID of the top level category that the product sits in
        /// </summary>
        public Int32 TopLevelCategory
        {
            get;
            protected set;
        }

        /// <summary>
        /// Delimited string showing the path through the category hierarchy to the primary category.  
        /// This is mainly for contextual information for the translators.
        /// </summary>
        public String CategoryPath
        {
            get;
            protected set;
        }

        /// <summary>
        /// Date/time that the translation of the item is scheduled to be completed in UTC
        /// </summary>
        public DateTime DueDate
        {
            get;
            private set;
        }

        /// <summary>
        /// Contains a SKU elements
        /// </summary>
        public IList<SKU> SKUs
        {
            get;
            private set;
        }

        /// <summary>
        /// URL to retrieve the project information.
        /// </summary>
        public String ProjectURL
        {
            get;
            private set;
        }

        /// <summary>
        /// ID of the project that was used to translate this item.  If SKU was submitted multiple
        /// times, it is the most recent project.
        /// </summary>
        public Int32 ProjectID
        {
            get;
            private set;
        }

        /// <summary>
        /// The source language
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
        /// A collection of translations for the product.  The dictionary key is the language code and the value is the translation.
        /// </summary>
        private Dictionary<String, ProductTranslation> TranslationsMap
        {
            get;
            set;
        }

        /// <summary>
        /// Return the list of translations for this product
        /// </summary>
        public IEnumerable<KeyValuePair<String, ProductTranslation>> AllTranslations
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
        /// Constructor for a product
        /// </summary>
        /// <param name="title"></param>
        /// <param name="primaryCategory"></param>
        /// <param name="topLevelCategory"></param>
        /// <param name="categoryPath"></param>
        /// <param name="skus"></param>
        /// <param name="description"></param>
        public Product(String title, Int32 primaryCategory, Int32 topLevelCategory, String categoryPath, SKU[] skus, ProductDescription description)
            : this()
        {
            this.SKUs = skus;
            this.Description = description;
            this.Title = title;
            this.PrimaryCategory = primaryCategory;
            this.TopLevelCategory = topLevelCategory;
            this.CategoryPath = categoryPath;
        }

        /// <summary>
        /// Constructor from XML
        /// </summary>
        /// <param name="element"></param>
        /// <param name="client"></param>
        public Product(XElement element, IContentAPI client)
            : this()
        {

            this.Client = client;

            this.DueDate = element.GetChildValueAsDateTime("DueDate");

            this.Title = element.GetChildValue("Title");
            this.PrimaryCategory = element.GetChildValueAsInt32("PrimaryCategory");
            this.TopLevelCategory = element.GetChildValueAsInt32("TopLevelCategory");
            this.CategoryPath = element.GetChildValue("CategoryPath");

            XElement description = element.Element("Description");
            if (description != null)
            {
                this.Description = new ProductDescription(description);
            }

            this.AssetID = element.GetChildValueAsInt32("AssetID");
            this.ProjectURL = element.GetChildValue("ProjectURL");
            this.ProjectID = element.GetChildValueAsInt32("ProjectID");

            if (element.Element("SourceLanguage") != null)
            {
                this.SourceLanguage = new SourceLanguage(element.Element(XName.Get("SourceLanguage")));
            }

            this.SKUs = SKU.CreateEnumerable(element.Element("SKUs")).ToList<SKU>();

            XElement targetLanguages = element.Element(XName.Get("TargetLanguages"));
            if (targetLanguages != null)
            {
                this.TargetLanguages = TargetLanguage.CreateEnumerable(targetLanguages).ToArray<TargetLanguage>();
            }
        }

        /// <summary>
        /// Private constructor
        /// </summary>
        private Product()
        {
            this.SKUs = new SKU[0];
            this.Description = new ProductDescription();
            this.TargetLanguages = new TargetLanguage[0];
            this.TranslationsMap = new Dictionary<String, ProductTranslation>();
        }

        #endregion


        #region Static Methods

        /// <summary>
        /// Create an Enumerable list of Product from XML
        /// </summary>
        /// <param name="element"></param>
        /// <param name="client"></param>
        /// <returns></returns>
        internal static IEnumerable<Product> CreateEnumerable(XElement element, IContentAPI client)
        {
            List<Product> result = new List<Product>();

            foreach (XElement product in element.Elements("Product"))
            {
                result.Add(new Product(product, client));
            }

            return result;
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

            builder.Append("<Product>");

            if (this.AssetID > 0)
            {
                builder.Append("<AssetID>" + this.AssetID + "</AssetID>");
            }

            builder.Append(new XElement("Title", this.Title).ToString(SaveOptions.DisableFormatting));
            
            builder.Append("<PrimaryCategory>" + this.PrimaryCategory + "</PrimaryCategory>");
            builder.Append("<TopLevelCategory>" + this.TopLevelCategory + "</TopLevelCategory>");

            builder.Append(new XElement("CategoryPath", this.CategoryPath).ToString(SaveOptions.DisableFormatting));
            
            builder.Append(this.Description.ToXmlString());

            // SKUs
            builder.Append("<SKUs>");
            foreach(SKU sku in this.SKUs)
            {
                builder.Append(sku.ToXmlString());
            }
            builder.Append("</SKUs>");

            if (this.DueDate != DateTime.MinValue)
            {
                builder.Append("<DueDate>" + this.DueDate.ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ssK") + "</DueDate>");
            }

            builder.Append("</Product>");

            return builder.ToString();
        }

        /// <summary>
        /// Seralize the class data to XML in an onDemand specific way without all the Product's properties
        /// </summary>
        /// <returns>The XML string which can be passed to onDemand</returns>
        public string ToXmlStringSimple()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("<Product>");

            if (this.AssetID > 0)
            {
                builder.Append("<AssetID>" + this.AssetID + "</AssetID>");
            }

            // SKUs
            builder.Append("<SKUs>");
            foreach (SKU sku in this.SKUs)
            {
                builder.Append(sku.ToXmlString());
            }
            builder.Append("</SKUs>");

            if (this.DueDate != DateTime.MinValue)
            {
                builder.Append("<DueDate>" + this.DueDate.ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ssK") + "</DueDate>");
            }

            builder.Append("</Product>");

            return builder.ToString();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Returns the translation for the given language code
        /// </summary>
        /// <param name="languageCode">A language code</param>
        /// <returns></returns>
        private ProductTranslation GetTranslation(String languageCode)
        {
            TargetLanguage targetLanguage = this.TargetLanguages.SingleOrDefault(p => p.LanguageCode == languageCode);
            if (targetLanguage == null)
            {
                throw new ArgumentException(string.Format("LanguageCode {0} is not one of the target languages for this product", languageCode), "languageCode");
            }

            ProductTranslation translation = null;
            if (this.TranslationsMap.ContainsKey(languageCode))
            {
                translation = this.TranslationsMap[languageCode];
            }
            ////TODO Fix this
            //else if (targetLanguage.Status == TranslatedFileStatus.Complete) //FileStatus.Translated
            else
            {
                if (this.Client == null)
                {
                    throw new InvalidOperationException("The product does not have an APIClient to communicate with");
                }

                translation = this.Client.GetProductTranslation(this.AssetID, languageCode);
                this.TranslationsMap.Add(languageCode, translation);
            }
            return translation;
        }

        #endregion
    }
}
