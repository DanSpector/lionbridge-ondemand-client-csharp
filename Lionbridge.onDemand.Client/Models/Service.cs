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
    /// 
    /// </summary>
    public class Service
    {
        /// <summary>
        /// 
        /// </summary>
        public String Name
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public String Description
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public String PriceDescription
        {
            get;
            private set;
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
        public IEnumerable<String> SourceLanguages
        {
            get
            {
                return this.SourceLanguagesList;
            }
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
        public Boolean AcceptsFiles
        {
            get
            {
                return this.ValidInputs.AcceptsFiles; 
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Boolean AcceptsProducts
        {
            get
            {
                return this.ValidInputs.AcceptsProducts;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<String> FileExtensions
        {
            get
            {
                return this.ValidInputs.FileExtensions;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private ValidInputs ValidInputs
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        private List<String> SourceLanguagesList
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
        /// 
        /// </summary>
        private IContentAPI Client
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        internal Service()
        {
            this.SourceLanguagesList = new List<String>();
            this.TargetLanguagesList = new List<String>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <param name="client"></param>
        internal Service(XElement element, IContentAPI client)
            : this()
        {

            this.Client = client;

            if (element != null)
            {
                this.ServiceID = element.GetChildValueAsInt32("ServiceID");

                this.Name = element.GetChildValue("Name");

                this.Description = element.GetChildValue("Description");

                this.PriceDescription = element.GetChildValue("PriceDescription");

                this.ValidInputs = new ValidInputs(element.Element("ValidInputs"));

                XElement sourceLanguages = element.Element("SourceLanguages");

                if (sourceLanguages != null)
                {
                    foreach (XElement sourceLanguage in sourceLanguages.Descendants("LanguageCode"))
                    {
                        this.SourceLanguagesList.Add(sourceLanguage.Value);
                    }
                }

                XElement targetLanguages = element.Element("TargetLanguages");

                if (targetLanguages != null)
                {
                    foreach (XElement targetLanguage in targetLanguages.Descendants("LanguageCode"))
                    {
                        this.TargetLanguagesList.Add(targetLanguage.Value);
                    }
                }
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <param name="client"></param>
        /// <returns></returns>
        internal static IEnumerable<Service> CreateEnumerable(XElement element, IContentAPI client)
        {
            List<Service> result = new List<Service>();

            foreach (XElement service in element.Elements("Service"))
            {
                result.Add(new Service(service, client));
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="extension">With or without the "."</param>
        /// <returns></returns>
        public Boolean AcceptsExtension(String extension)
        {
            return this.FileExtensions.Contains(extension.TrimStart('.'));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePaths"></param>
        /// <param name="options"></param>
        /// <param name="referenceFilePaths"></param>
        /// <returns></returns>
        public Quote GenerateQuote(String[] filePaths, QuoteOptions options, String[] referenceFilePaths = null)
        {
            if (this.Client == null)
            {
                throw new InvalidOperationException("The service does not have an APIClient to communicate with");
            }

            return this.Client.GenerateQuote(this, filePaths, options, referenceFilePaths);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="files"></param>
        /// <param name="options"></param>
        /// <param name="referenceFiles"></param>
        /// <returns></returns>
        public Quote GenerateQuote(IEnumerable<File> files, QuoteOptions options, IEnumerable<File> referenceFiles = null)
        {
            if (this.Client == null)
            {
                throw new InvalidOperationException("The service does not have an APIClient to communicate with");
            }

            return this.Client.GenerateQuote(this, files, options, referenceFiles);
        }

        /// <summary>
        /// Return a ProductsQuote given a list of products
        /// </summary>
        /// <param name="products">A collection of products</param>
        /// <param name="options"></param>
        /// <param name="referenceFiles"></param>
        /// <returns></returns>
        public Quote GenerateQuote(IEnumerable<Product> products, QuoteOptions options, IEnumerable<File> referenceFiles = null)
        {
            if (this.Client == null)
            {
                throw new InvalidOperationException("The service does not have an APIClient to communicate with");
            }

            return this.Client.GenerateQuote(this, products, options, referenceFiles);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="unitCount"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public Estimate GetEstimate(Int32 unitCount, EstimateOptions options)
        {
            if (this.Client == null)
            {
                throw new InvalidOperationException("The service does not have an APIClient to communicate with");
            }

            return this.Client.GetEstimate(this, unitCount, options);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectName"></param>
        /// <param name="files"></param>
        /// <param name="options"></param>
        /// <param name="referenceFiles"></param>
        /// <returns></returns>
        public Project AddProject(String projectName, IEnumerable<File> files, ProjectOptions options, IEnumerable<File> referenceFiles = null)
        {
            if (this.Client == null)
            {
                throw new InvalidOperationException("The service does not have an APIClient to communicate with");
            }

            return this.Client.AddProject(projectName, this, files, options, referenceFiles);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectName"></param>
        /// <param name="filePaths"></param>
        /// <param name="options"></param>
        /// <param name="referenceFilePaths"></param>
        /// <returns></returns>
        public Project AddProject(String projectName, String[] filePaths, ProjectOptions options, String[] referenceFilePaths = null)
        {
            if (this.Client == null)
            {
                throw new InvalidOperationException("The service does not have an APIClient to communicate with");
            }

            return this.Client.AddProject(projectName, this, filePaths, options, referenceFilePaths);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectName"></param>
        /// <param name="products"></param>
        /// <param name="options"></param>
        /// <param name="referenceFiles"></param>
        /// <returns></returns>
        public Project AddProject(String projectName, IEnumerable<Product> products, ProjectOptions options, IEnumerable<File> referenceFiles = null)
        {
            if (this.Client == null)
            {
                throw new InvalidOperationException("The service does not have an APIClient to communicate with");
            }

            return this.Client.AddProject(projectName, this, products, options, referenceFiles);
        }
    }
}
