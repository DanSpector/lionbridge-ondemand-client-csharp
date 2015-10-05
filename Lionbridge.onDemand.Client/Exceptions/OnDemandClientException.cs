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
using System.Net;

namespace Lionbridge.onDemand.Client
{
    /// <summary>
    /// This class represents an error condition that occurs when calling a method in the onDemandClient API
    /// </summary>
    [Serializable]
    public class OnDemandClientException : SystemException
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
        /// Create an empty OnDemandClientException
        /// </summary>
        public OnDemandClientException()
            : base()
        {
            //
        }

        /// <summary>
        /// Create an OnDemandClientException
        /// </summary>
        /// <param name="message">The exception message</param>
        public OnDemandClientException(String message)
            : base(message)
        {
            //
        }

        /// <summary>
        /// Create an OnDemandClientException
        /// </summary>
        /// <param name="message">The exception message</param>
        /// <param name="innerException">An inner exception</param>
        public OnDemandClientException(String message, Exception innerException)
            : base(message, innerException)
        {
            //
        }

        /// <summary>
        /// Create an OnDemandClientException
        /// </summary>
        /// <param name="simpleMessage">The error message</param>
        /// <param name="detailedMessage"></param>
        /// <param name="reasonCode"></param>
        /// <param name="statusCode"></param>
        public OnDemandClientException(String simpleMessage, String detailedMessage, Int32 reasonCode, HttpStatusCode statusCode) 
            : base (simpleMessage + " " + detailedMessage)
        {
            this.SimpleMessage = simpleMessage;
            this.DetailedMessage = detailedMessage;
            this.ReasonCode = reasonCode;
            this.HttpStatusCode = statusCode;
        }

        #endregion

    }
}
