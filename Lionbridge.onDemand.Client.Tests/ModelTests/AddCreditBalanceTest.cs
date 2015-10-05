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
    public class AddCreditBalanceTest
    {
        #region Constants

        private Decimal amount = new Decimal(100.12);
        private const string currency = "USD";

        #endregion

        #region Test Methods

        /// <summary>
        /// Test the default constructor
        /// </summary>
        [TestMethod]
        public void AddCreditBalanceConstructorTest()
        {
            AddCreditBalance acb = new AddCreditBalance(amount, currency);

            Assert.AreEqual(amount, acb.Amount);
            Assert.AreEqual(currency, acb.Currency);
        }

        /// <summary>
        /// Test the ToXmlString method
        /// </summary>
        [TestMethod]
        public void CreateAccountToXmlStringTest()
        {
            AddCreditBalance acb = new AddCreditBalance(amount, currency);

            String expected = "<AddCreditBalance><Amount>100.12</Amount><Currency>USD</Currency></AddCreditBalance>";
            String actual = acb.ToXmlString();
            Assert.AreEqual(expected, actual);
        }

        #endregion
    }

}
