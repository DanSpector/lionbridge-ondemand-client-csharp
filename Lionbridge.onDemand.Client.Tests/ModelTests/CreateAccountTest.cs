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
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lionbridge.onDemand.Client.Tests
{
    [TestClass]
    public class CreateAccountTest
    {
        #region Constants

        private const string merchantID = "123233244";
        private const string email = "example@example.com";
        private const string firstName = "Joe";
        private const string lastName = "BLoggs";
        private const string companyName = "ACME Coyote Products, Ltd.";
        private const string country = "GB";
        private const string vatID = "12334455544";

        #endregion

        #region Test Methods

        [TestMethod]
        public void CreateAccountConstructorTest()
        {
            CreateAccount account = new CreateAccount(merchantID, email, firstName, lastName, companyName, country, vatID);

            Assert.AreEqual(merchantID, account.MerchantID, "MerchantID");
            Assert.AreEqual(email, account.Email, "Email");
            Assert.AreEqual(companyName, account.CompanyName, "CompanyName");
            Assert.AreEqual(country, account.Country, "Country");
            Assert.AreEqual(firstName, account.FirstName, "FirstName");
            Assert.AreEqual(lastName, account.LastName, "LastName");
            Assert.AreEqual(vatID, account.VATID, "VATID");
        }


        [TestMethod]
        public void CreateAccountToXmlStringTest()
        {
            CreateAccount account = new CreateAccount(merchantID, email, firstName, lastName, companyName, country, vatID);

            String expected = CreateExpected();
            String actual = account.ToXmlString();
            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Private Methods

        private String CreateExpected()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("<CreateAccount>");
            this.AddXmlElementToString(builder, "MerchantID", merchantID);
            this.AddXmlElementToString(builder, "EmailAddress", email);
            this.AddXmlElementToString(builder, "FirstName", firstName);
            this.AddXmlElementToString(builder, "LastName", lastName);
            this.AddXmlElementToString(builder, "CompanyName", companyName);
            this.AddXmlElementToString(builder, "Country", country);
            this.AddXmlElementToString(builder, "VATID", vatID);
            builder.Append("</CreateAccount>");

            return builder.ToString();
        }


        private void AddXmlElementToString(StringBuilder builder, String elementName, String value)
        {
            builder.Append("<" + elementName + ">");
            builder.Append(value ?? "");
            builder.Append("</" + elementName + ">");
        }

        #endregion
    }
}
