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
    [TestClass]
    public class SKUTest
    {
        [TestMethod]
        public void SKUConstructorTest()
        {
            String skuNumber = "23";
            Dictionary<String, String> itemSpecifics = new Dictionary<string,string>();

            itemSpecifics.Add("key", "value");
            itemSpecifics.Add("key2", "value2");

            SKU sku = new SKU(skuNumber, itemSpecifics);

            Assert.AreEqual(sku.SKUNumber, skuNumber);
            Assert.AreEqual(sku.ItemSpecifics.Count, itemSpecifics.Count);
            Assert.AreEqual(sku.ItemSpecifics["key"], itemSpecifics["key"]);
            Assert.AreEqual(sku.ItemSpecifics["key2"], itemSpecifics["key2"]);
        }
    }
}
