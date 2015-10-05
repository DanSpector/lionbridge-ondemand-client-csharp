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
    /// Class used to represent a project to be added and passed to onDemand
    /// </summary>
    internal class AddProject : GenerateQuote
    {
        /// <summary>
        /// User supplied name for the project
        /// </summary>
        public String ProjectName
        {
            get;
            private set;
        }

        /// <summary>
        /// Empty Constructor
        /// </summary>
        public AddProject() : base()
        {

        }

        /// <summary>
        /// Constructor when a project consists of Products to be translated
        /// </summary>
        /// <param name="name"></param>
        /// <param name="products"></param>
        /// <param name="translationOptions"></param>
        /// <param name="referenceFiles"></param>
        public AddProject(String name, IEnumerable<Product> products, TranslationOptions translationOptions, IEnumerable<File> referenceFiles = null)
            : base(products, translationOptions, referenceFiles)
        {
            this.ProjectName = name;
        }

        /// <summary>
        /// Constructor when a project consists of Files to be translated
        /// </summary>
        /// <param name="name"></param>
        /// <param name="files"></param>
        /// <param name="translationOptions"></param>
        /// <param name="referenceFiles"></param>
        public AddProject(String name, IEnumerable<File> files, TranslationOptions translationOptions, IEnumerable<File> referenceFiles = null)
            : base(files, translationOptions, referenceFiles)
        {
            this.ProjectName = name;
        }

        /// <summary>
        /// Seralize the class data to XML in an onDemand specific way
        /// </summary>
        /// <returns>The XML string which can be passed to onDemand</returns>
        public override string ToXmlString()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("<AddProject>");

            builder.Append(new XElement("ProjectName", this.ProjectName).ToString(SaveOptions.DisableFormatting));

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

            builder.Append("</AddProject>");

            return builder.ToString();
        }
    }
}
