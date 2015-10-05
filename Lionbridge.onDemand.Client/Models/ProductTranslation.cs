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
    /// Translation of a Product
    /// </summary>
    public class ProductTranslation
    {
        #region Public Properties

        /// <summary>
        /// The Asset ID
        /// </summary>
        public Int32 AssetID
        {
            get;
            protected set;
        }

        /// <summary>
        /// The SKUs of the source text
        /// </summary>
        public SKU[] SourceSKUs
        {
            get;
            protected set;
        }

        /// <summary>
        /// The source title
        /// </summary>
        public String SourceTitle
        {
            get;
            protected set;
        }

        /// <summary>
        /// The Service number
        /// </summary>
        public Int32 Service
        {
            get;
            protected set;
        }

        /// <summary>
        /// The source language
        /// </summary>
        public String Language
        {
            get;
            protected set;
        }

        /// <summary>
        /// 
        /// </summary>
        public String Title
        {
            get;
            protected set;
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
        /// 
        /// </summary>
        public Int32 PrimaryCategory
        {
            get;
            protected set;
        }

        /// <summary>
        /// The SKUs of the translated text
        /// </summary>
        public SKU[] SKUs
        {
            get;
            protected set;
        }

        #endregion


        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        public ProductTranslation()
        {
            this.SourceSKUs = new SKU[0];
            this.SKUs = new SKU[0];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        public ProductTranslation(XElement element)
            : this()
        {
            // Source SKUs
            XElement skus = element.Element("SKUs");
            if (skus != null)
            {
                this.SourceSKUs = SKU.CreateEnumerable(skus).ToArray<SKU>();
            }
            
            this.AssetID = element.GetChildValueAsInt32("AssetID");
            this.SourceTitle = element.GetChildValue("SourceTitle");
            this.Service = element.GetChildValueAsInt32("Service");
            this.Language = element.GetChildValue("Language");

            //Translated Fields
            XElement translatedFields = element.Element("TranslatedFields");
            if (translatedFields != null)
            {
                this.Title = translatedFields.GetChildValue("Title");
                this.PrimaryCategory = translatedFields.GetChildValueAsInt32("PrimaryCategory");

                XElement description = translatedFields.Element("Description");
                if (description != null)
                {
                    this.Description = new ProductDescription(description); 
                }

                skus = translatedFields.Element("SKUs");
                if (skus != null)
                {
                    this.SKUs = SKU.CreateEnumerable(skus).ToArray<SKU>();
                }
            }
        }

        #endregion

        /// <summary>
        /// Get the TranslatedFields portion of this Product Translation as an XML string
        /// </summary>
        /// <returns></returns>
        public String GetTranslatedFields()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("<TranslatedFields>");

            builder.Append(new XElement("Title", this.Title).ToString(SaveOptions.DisableFormatting));

            builder.Append(this.Description.ToXmlString());

            builder.Append("<PrimaryCategory>" + this.PrimaryCategory + "</PrimaryCategory>");

            builder.Append("<SKUs>");

            foreach (SKU sku in this.SKUs)
            {
                builder.Append(sku.ToXmlString());
            }

            builder.Append("</SKUs>");
            builder.Append("</TranslatedFields>");

            return builder.ToString();
        }
    }
}
