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
    /// Class used to represent a quote to be added and passed to onDemand
    /// </summary>
    internal class GenerateQuote : IXmlSerializable
    {
        /// <summary>
        /// 
        /// </summary>
        internal TranslationOptions TranslationOptions
        {
            get;
            private set;
        }

        /// <summary>
        /// The Products if this is for a Quote based on Products
        /// </summary>
        internal List<Product> Products
        {
            get;
            private set;
        }

        /// <summary>
        /// The Files if this is for a Quote based on Files
        /// </summary>
        internal List<File> Files
        {
            get;
            private set;
        }

        /// <summary>
        /// Any additional files to be passed with this quote.
        /// </summary>
        /// <remarks>These files will NOT be translated</remarks>
        internal List<File> ReferenceFiles
        {
            get;
            private set;
        }

        /// <summary>
        /// The Projects if this is for a Quote based on Projects
        /// </summary>
        internal List<Project> Projects
        {
            get;
            private set;
        }

        /// <summary>
        /// Empty Constructor
        /// </summary>
        public GenerateQuote()
        {
            this.Products = new List<Product>();
            this.Files = new List<File>();
            this.Projects = new List<Project>();
            this.ReferenceFiles = new List<File>();
        }

        /// <summary>
        /// Constructor for a quote based on Products to be translated
        /// </summary>
        /// <param name="products"></param>
        /// <param name="translationOptions"></param>
        /// <param name="referenceFiles"></param>
        public GenerateQuote(IEnumerable<Product> products, TranslationOptions translationOptions, IEnumerable<File> referenceFiles = null)
            : this()
        {
            this.Products.AddRange(products);
            this.TranslationOptions = translationOptions;

            if (referenceFiles != null)
            {
                this.ReferenceFiles.AddRange(referenceFiles);
            }
        }

        /// <summary>
        /// Constructor for a quote based on Files to be translated
        /// </summary>
        /// <param name="files"></param>
        /// <param name="translationOptions"></param>
        /// <param name="referenceFiles"></param>
        public GenerateQuote(IEnumerable<File> files, TranslationOptions translationOptions, IEnumerable<File> referenceFiles = null)
            : this()
        {
            this.Files.AddRange(files);
            this.TranslationOptions = translationOptions;

            if (referenceFiles != null)
            {
                this.ReferenceFiles.AddRange(referenceFiles);
            }
        }

        /// <summary>
        /// Constructor for a quote given already created Projects
        /// </summary>
        /// <param name="projects"></param>
        /// <param name="translationOptions"></param>
        public GenerateQuote(IEnumerable<Project> projects, TranslationOptions translationOptions)
            : this()
        {
            this.Projects.AddRange(projects);
            this.TranslationOptions = translationOptions;
        }

        /// <summary>
        /// Seralize the class data to XML in an onDemand specific way
        /// </summary>
        /// <returns>The XML string which can be passed to onDemand</returns>
        public virtual string ToXmlString()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("<GenerateQuote>");

            builder.Append(this.TranslationOptions.ToXmlString());

            if (this.Products != null && this.Products.Count > 0)
            {
                builder.Append("<Products>");
                foreach (Product product in this.Products)
                {
                    builder.Append(product.ToXmlString());
                }
                builder.Append("</Products>");
            }

            if (this.Files != null && this.Files.Count > 0)
            {
                builder.Append("<Files>");
                foreach (File file in this.Files)
                {
                    builder.Append(file.ToXmlString());
                }
                builder.Append("</Files>");
            }

            if (this.ReferenceFiles != null && this.ReferenceFiles.Count > 0)
            {
                builder.Append("<ReferenceFiles>");
                foreach (File referenceFile in this.ReferenceFiles)
                {
                    builder.Append(referenceFile.ToXmlStringExtended());
                }
                builder.Append("</ReferenceFiles>");
            }

            if (this.Projects != null && this.Projects.Count > 0)
            {
                builder.Append("<Projects>");
                foreach (Project project in this.Projects)
                {
                    builder.Append(project.ToXmlStringSimple());
                }
                builder.Append("</Projects>");
            }

            builder.Append("</GenerateQuote>");

            return builder.ToString();
        }

    }
}
