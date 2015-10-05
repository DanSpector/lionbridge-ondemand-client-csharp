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
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lionbridge.onDemand.Client.Tests
{
    [TestClass]
    public class PaymentTest
    {
        [TestMethod]
        public void ConstructorFromXmlTest()
        {
            string xml = @"<?xml version='1.0' encoding='UTF-8'?>
                <Payment>
                    <PaymentType>PayPal</PaymentType>
                    <PaymentDescription>PayPal charge to buyer@example.com</PaymentDescription>
                    <PaymentAmount>10.00</PaymentAmount>
                    <PaymentCurrency>EUR</PaymentCurrency>
                </Payment>
            ";

            XDocument document = XDocument.Parse(xml);

            var payment = new Payment(document.Element("Payment"));

            Assert.AreEqual("PayPal", payment.Type);
            Assert.AreEqual("PayPal charge to buyer@example.com", payment.Description);
            Assert.AreEqual(10.00m, payment.Amount);
            Assert.AreEqual("EUR", payment.Currency);
        }

        [TestMethod]
        public void CreateEnumerableTest()
        {
            string xml = @"<?xml version='1.0' encoding='UTF-8'?>
                <Payments>
                    <Payment>
                        <PaymentType>PayPal</PaymentType>
                        <PaymentDescription>PayPal charge to buyer@example.com</PaymentDescription>
                        <PaymentAmount>10.00</PaymentAmount>
                        <PaymentCurrency>EUR</PaymentCurrency>
                    </Payment>
                    <Payment>
                        <PaymentType>PayPal</PaymentType>
                        <PaymentDescription>PayPal charge to buyer@example.com</PaymentDescription>
                        <PaymentAmount>10.00</PaymentAmount>
                        <PaymentCurrency>EUR</PaymentCurrency>
                    </Payment>
                </Payments>
            ";

            XDocument document = XDocument.Parse(xml);

            IEnumerable<Payment> payments = Payment.CreateEnumerable(document.Element("Payments"));

            Assert.IsNotNull(payments);
            Assert.AreEqual(2, payments.Count());

            var payment = payments.FirstOrDefault();

            Assert.AreEqual("PayPal", payment.Type);
            Assert.AreEqual("PayPal charge to buyer@example.com", payment.Description);
            Assert.AreEqual(10.00m, payment.Amount);
            Assert.AreEqual("EUR", payment.Currency);
        }
    }
}
