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
using System.Net;
using System.Text;
using System.Xml.Linq;

namespace Lionbridge.onDemand.Client
{
    /// <summary>
    /// This class represents an error condition
    /// </summary>
    internal class Error
    {
        #region Properties

        /// <summary>
        /// The HTTP web response status code. 
        /// </summary>
        /// <remarks>Do not confuse this property with Reason Code!</remarks>
        public HttpStatusCode HttpStatusCode
        {
            get;
            internal set;
        }

        /// <summary>
        /// The reason code. Reason code is intended to streamline client error handling logic. 
        /// </summary>
        /// <remarks>Do not confuse this property with the HTTP Status Code!</remarks>
        public Int32 ReasonCode
        {
            get;
            internal set;
        }

        /// <summary>
        /// SimpleMessage is recommended for the end user.
        /// </summary>
        public String SimpleMessage
        {
            get;
            internal set;
        }

        /// <summary>
        /// DetailedMessage can be used for troubleshooting.
        /// </summary>
        public String DetailedMessage
        {
            get;
            internal set;
        }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create an Error instance from an XML element
        /// </summary>
        /// <param name="statusCode">The status code from the HTTP web response</param>
        /// <param name="errorElement">An XML Error element</param>
        public Error(HttpStatusCode statusCode, XElement errorElement)
        {
            /* The error element should have a structure similar to the one shown here:
            <Error>
                <ReasonCode>403</ReasonCode>
                <SimpleMessage>Already exists.</SimpleMessage>
                <DetailedMessage>A user with this email address already exists.</DetailedMessage>
            </Error>
            */

            this.HttpStatusCode = statusCode;

            if (errorElement != null)
            {
                this.ReasonCode = errorElement.GetChildValueAsInt32("ReasonCode");
                this.SimpleMessage = errorElement.GetChildValue("SimpleMessage");
                this.DetailedMessage = errorElement.GetChildValue("DetailedMessage");

                if (string.IsNullOrEmpty(this.SimpleMessage) && string.IsNullOrEmpty(this.DetailedMessage))
                {
                    this.SimpleMessage = errorElement.Value;
                }
            }
            else
            {
                this.SimpleMessage = this.HttpStatusCode.ToString();
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Generate an OnDemandClientException based on the data in the Error instance.
        /// </summary>
        /// <returns>An OnDemandClientException</returns>
        public Exception GenerateException()
        {
            OnDemandClientException odce = new OnDemandClientException(this.SimpleMessage, this.DetailedMessage, this.ReasonCode, this.HttpStatusCode);
            return odce;
        }

        #endregion

       
    }
}
