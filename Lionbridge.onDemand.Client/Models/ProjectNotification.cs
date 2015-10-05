﻿/* Copyright (c) 2015 Lionbridge Technologies, Inc.
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
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Lionbridge.onDemand.Client
{
    /// <summary>
    /// A notification about a project
    /// </summary>
    public class ProjectNotification
    {
        /// <summary>
        /// The ID of the project.  Generated by onDemand.
        /// </summary>
        public Int32 ProjectID
        {
            get;
            private set;
        }

        /// <summary>
        /// Status of the project
        /// </summary>
        public String Status
        {
            get;
            private set;
        }

        /// <summary>
        /// The URL where the project can be downloaded.  See GetProject.
        /// </summary>
        public Uri URL
        {
            get;
            private set;
        }

        /// <summary>
        /// DateTime when the project was created
        /// </summary>
        public DateTime CreationDate
        {
            get;
            private set;
        }

        /// <summary>
        /// DateTime when the project was due
        /// </summary>
        public DateTime DueDate
        {
            get;
            private set;
        }

        /// <summary>
        /// DateTime when the project was completed
        /// </summary>
        public DateTime CompletionDate
        {
            get;
            private set;
        }

        /// <summary>
        /// List of Error strings that occurred when the project was created
        /// </summary>
        public IEnumerable<String> Errors
        {
            get
            {
                return this.ErrorsList;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public String SourceLanguage
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<String> TargetLanguages
        {
            get
            {
                return this.TargetLanguagesList;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<Product> Products
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<File> Files
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        private List<String> ErrorsList
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        private List<String> TargetLanguagesList
        {
            get;
            set;
        }

        /// <summary>
        /// Empty Constructor
        /// </summary>
        public ProjectNotification()
        {
            this.ErrorsList = new List<string>();
            this.TargetLanguagesList = new List<string>();
            this.Products = new List<Product>();
            this.Files = new List<File>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        public ProjectNotification(Stream stream)
            : this(ElementFromStream(stream, "Project"), null)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmlString"></param>
        public ProjectNotification(String xmlString)
            : this(ElementFromString(xmlString, "Project"), null)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <param name="client"></param>
        public ProjectNotification(XElement element, ContentAPI client)
            : this()
        {
            if (element != null)
            {
                this.ProjectID = element.GetChildValueAsInt32("ProjectID");

                this.Status = element.GetChildValue("Status");

                this.URL = new Uri(element.GetChildValue("URL"));

                this.CreationDate = element.GetChildValueAsDateTime("CreationDate");

                this.DueDate = element.GetChildValueAsDateTime("DueDate");

                this.CompletionDate = element.GetChildValueAsDateTime("CompletionDate");

                XElement errors = element.Element("Errors");

                if (errors != null)
                {
                    foreach (XElement error in errors.Elements("Error"))
                    {
                        this.ErrorsList.Add(error.Value);
                    }
                }

                XElement sourceLanguage = element.Element("SourceLanguage");

                if (sourceLanguage != null)
                {
                    this.SourceLanguage = sourceLanguage.GetChildValue("LanguageCode");
                }

                XElement targetLanguages = element.Element("TargetLanguages");

                if (targetLanguages != null)
                {
                    foreach (XElement targetLanguage in targetLanguages.Descendants("LanguageCode"))
                    {
                        this.TargetLanguagesList.Add(targetLanguage.Value);
                    }
                }

                XElement products = element.Element("Products");

                if (products != null)
                {
                    this.Products = Product.CreateEnumerable(products, client);
                }

                XElement files = element.Element("Files");

                if (files != null)
                {
                    this.Files = File.CreateEnumerable(files, client);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="elementName"></param>
        /// <returns></returns>
        private static XElement ElementFromStream(Stream stream, String elementName)
        {
            XElement result = null;
            using (StreamReader reader = new StreamReader(stream))
            {
                result = XDocument.Load(reader).Element(elementName);
            }

            return result;
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="elementName"></param>
        /// <returns></returns>
        private static XElement ElementFromString(String str, String elementName)
        {
            XElement result = null;
            using (StringReader reader = new StringReader(str))
            {
                result = XDocument.Load(reader).Element(elementName);
            }

            return result;

        }

    }
}
