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
    /// A collection of utility methods that support the onDemand client
    /// </summary>
    public static class Utility
    {
        /// <summary>
        /// Return the web request content type header text given a file name
        /// </summary>
        /// <param name="fileName">The file name</param>
        /// <returns>The content type header text for the given file name</returns>
        public static string GetContentTypeHeader(String fileName)
        {
            int extensionPos = fileName.LastIndexOf(".");
            String extension = fileName.Substring(extensionPos + 1);

            String mimeType;

            switch (extension)
            {
                case "csv":
                    mimeType = "text/csv";
                    break;
                case "docx":
                    mimeType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                    break;
                case "flv":
                    mimeType = "video/x-flv";
                    break;
                case "htm":
                    mimeType = "text/html";
                    break;
                case "html":
                    mimeType = "text/html";
                    break;
                case "idml":
                    mimeType = "application/xml";
                    break;
                case "ini":
                    mimeType = "text/plain";
                    break;
                case "inx":
                    mimeType = "application/xml";
                    break;
                case "json":
                    mimeType = "application/json";
                    break;
                case "m4v":
                    mimeType = "video/x-m4v";
                    break;
                case "mif":
                    mimeType = "application/vnd.mif";
                    break;
                case "mov":
                    mimeType = "video/quicktime";
                    break;
                case "mp4":
                    mimeType = "video/mp4";
                    break;
                case "pdf":
                    mimeType = "application/pdf";
                    break;
                case "po":
                    mimeType = "text/plain";
                    break;
                case "pptx":
                    mimeType = "application/vnd.openxmlformats-officedocument.presentationml.presentation";
                    break;
                case "properties":
                    mimeType = "text/plain";
                    break;
                case "psd":
                    mimeType = "image/vnd.adobe.photoshop";
                    break;
                case "resjson":
                    mimeType = "application/json";
                    break;
                case "resw":
                    mimeType = "application/xml";
                    break;
                case "resx":
                    mimeType = "application/xml";
                    break;
                case "rtf":
                    mimeType = "application/rtf";
                    break;
                case "srt":
                    mimeType = "text/plain";
                    break;
                case "strings":
                    mimeType = "text/plain";
                    break;
                case "txt":
                    mimeType = "text/plain";
                    break;
                case "vtt":
                    mimeType = "text/plain";
                    break;
                case "wmv":
                    mimeType = "video/x-ms-wmv";
                    break;
                case "xlf":
                    mimeType = "application/xml";
                    break;
                case "xliff":
                    mimeType = "application/xml";
                    break;
                case "xlsx":
                    mimeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    break;
                case "xml":
                    mimeType = "application/xml";
                    break;
                case "yml":
                    mimeType = "text/yaml";
                    break;
                case "yaml":
                    mimeType = "text/yaml";
                    break;
                default:
                    // Not sure if this is the best default
                    mimeType = "text/plain";
                    break;
            }
            return mimeType;
        }

    }
}
