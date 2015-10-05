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
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lionbridge.onDemand.Client.Tests
{
    /// <summary>
    /// This class contains unit tests for the Utility class.
    /// </summary>
    [TestClass]
    public class UtilityTest
    {

        [TestMethod]
        public void GetContentTypeHeaderTest()
        {
            Dictionary<String, String> contentTypes = new Dictionary<string,string>();

            contentTypes.Add("csv", "text/csv");
            contentTypes.Add("docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document");
            contentTypes.Add("flv", "video/x-flv");
            contentTypes.Add("htm", "text/html");
            contentTypes.Add("html", "text/html");
            contentTypes.Add("idml", "application/xml");
            contentTypes.Add("ini", "text/plain");
            contentTypes.Add("inx", "application/xml");
            contentTypes.Add("json", "application/json");
            contentTypes.Add("m4v", "video/x-m4v");
            contentTypes.Add("mif", "application/vnd.mif");
            contentTypes.Add("mov", "video/quicktime");
            contentTypes.Add("mp4", "video/mp4");
            contentTypes.Add("pdf", "application/pdf");
            contentTypes.Add("po", "text/plain");
            contentTypes.Add("pptx", "application/vnd.openxmlformats-officedocument.presentationml.presentation");
            contentTypes.Add("properties", "text/plain");
            contentTypes.Add("psd", "image/vnd.adobe.photoshop");
            contentTypes.Add("resjson", "application/json");
            contentTypes.Add("resw", "application/xml");
            contentTypes.Add("resx", "application/xml");
            contentTypes.Add("rtf", "application/rtf");
            contentTypes.Add("srt", "text/plain");
            contentTypes.Add("strings", "text/plain");
            contentTypes.Add("txt", "text/plain");
            contentTypes.Add("vtt", "text/plain");
            contentTypes.Add("wmv", "video/x-ms-wmv");
            contentTypes.Add("xlf", "application/xml");
            contentTypes.Add("xliff", "application/xml");
            contentTypes.Add("xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            contentTypes.Add("xml", "application/xml");
            contentTypes.Add("yml", "text/yaml");
            contentTypes.Add("yaml", "text/yaml");

            TestContentTypes(contentTypes);
        }


        /// <summary>
        /// This is a helper method to test content type headers
        /// </summary>
        /// <param name="contentTypes">A collection of file extensions and their expected content type headers</param>
        private void TestContentTypes(Dictionary<String, String> contentTypes)
        {
            foreach (String extension in contentTypes.Keys)
            {
                String expected = contentTypes[extension];
                String actual = Utility.GetContentTypeHeader("Testing." + extension);
                String errorMessage = "Extension: " + extension + ". Expected: " + expected + " Actual: " + actual;
                Assert.AreEqual(expected, actual, errorMessage);
            }
        }
    }
}
