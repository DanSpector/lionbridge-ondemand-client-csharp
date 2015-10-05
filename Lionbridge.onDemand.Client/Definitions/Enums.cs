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
    /// Reasons why a Quote was rejected
    /// </summary>
    public enum RejectQuoteStatus
    {
        /// <summary>
        /// Successful request
        /// </summary>
        Success	= 200,

        /// <summary>
        /// 
        /// </summary>
        BadRequest = 400,
 
        /// <summary>
        /// The request did not pass authentication or the customer is not a member of an enterprise site.
        /// </summary>
        Unauthorized = 401,
	
        /// <summary>
        /// This quote cannot be rejected at this time. This is probably because the projects have already been started.
        /// </summary>
        Conflict = 409,

        /// <summary>
        /// An error occured when attempting to communicate with the server (no response code received).
        /// </summary>
        CommunicationError = 0,

        /// <summary>
        /// An unknown response code was received from the server, even though a successful HTTP response (OK) code was detected.
        /// </summary>
        Unknown = -1
    }

    /// <summary>
    /// File Statuses
    /// </summary>
    public enum FileStatus
    {
        /// <summary>
        /// The file is not yet created and/or has no state.
        /// </summary>
        None = 0,

        /// <summary>
        /// The file has been recently uploaded and onDemand is still in the process of analyzing it. At this point,
        /// the file can be added to a quote but the quote cannot be authorized yet.
        /// </summary>
        Analyzing = 1,

        /// <summary>
        /// The file has been analyzed and onDemand can now generate a price for it. Quotes containing this file can be authorized.
        /// </summary>
        Analyzed = 2,

        /// <summary>
        /// onDemand could not parse the file and it cannot be used in a project. Quotes containing this file cannot be authorized.
        /// </summary>
        AnalysisFailed = 3,

        /// <summary>
        /// This file has been added to a project and the project is being worked on. This file cannot be added to other quotes or projects.
        /// </summary>
        InTranslation = 4,

        /// <summary>
        /// This file has been added to a project and the project is complete. This file cannot be added to other quotes or projects.
        /// </summary>
        Translated = 5,

        /// <summary>
        /// 
        /// </summary>
        New = 6
    }

    /// <summary>
    /// Translated File Statuses
    /// </summary>
    public enum TranslatedFileStatus
    {
        /// <summary>
        /// The file is not yet created and/or has no state.
        /// </summary>
        None = 0,

        /// <summary>
        /// The file translation has begun but is not yet complete.
        /// </summary>
        Started = 1,

        /// <summary>
        /// The file translation is complete.
        /// </summary>
        Complete = 2
    }
}
