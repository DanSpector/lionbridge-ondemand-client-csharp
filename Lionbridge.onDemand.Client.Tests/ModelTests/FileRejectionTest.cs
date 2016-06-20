/* Copyright (c) 2016 Lionbridge Technologies, Inc.
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
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lionbridge.onDemand.Client.Tests
{
    [TestClass]
    public class FileRejectionTest
    {
        [TestMethod]
        public void FileRejectionConstructorTest()
        {
            Int32 reasonCode = 5000;
            String reasonDescription = "Something bad happened";

            var fileRejection = new FileRejection(reasonCode, reasonDescription);

            Assert.AreEqual(reasonCode, fileRejection.ReasonCode);
            Assert.AreEqual(reasonDescription, fileRejection.ReasonDescription);
        }

        [TestMethod]
        public void ToXmlStringTest()
        {
            Int32 reasonCode = 5000;
            String reasonDescription = @"Failed .po file validation";

            var fileRejection = new FileRejection(reasonCode, reasonDescription);

            string xml = @"
                <RejectFile>
	                <ReasonCode>5000</ReasonCode>
	                <ReasonDescription>Failed .po file validation</ReasonDescription>
                </RejectFile>
            ";

            Assert.AreEqual(XElement.Parse(xml).ToString(), XElement.Parse(fileRejection.ToXmlString()).ToString());
        }
    }
}
