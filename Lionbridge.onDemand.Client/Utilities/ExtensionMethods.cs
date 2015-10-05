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
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Lionbridge.onDemand.Client
{
    /// <summary>
    /// This class contains extension methods used by the onDemand client
    /// </summary>
    public static class ExtensionMethods
    {

        /// <summary>
        /// This method extends System.Net.WebRequest so that requests that would otherwise have caused a System.Net.WebException
        /// are instead returned as regular responses. 
        /// </summary>
        /// <param name="request">The web request</param>
        /// <returns>A WebResponse for both valid and invalid requests</returns>
        public static WebResponse GetResponseWithoutException(this WebRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            try
            {
                return request.GetResponse();
            }
            catch (WebException e)
            {
                return e.Response;
            }
        }

        /// <summary>
        /// Safely reads all bytes from a file path
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static byte[] ReadAllBytes(String path)
        {
            byte[] bytes;

            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                int index = 0;
                long fileLength = fs.Length;
                if (fileLength > Int32.MaxValue)
                {
                    throw new IOException("File too long");
                }

                int count = (int)fileLength;
                bytes = new byte[count];

                while (count > 0)
                {
                    int n = fs.Read(bytes, index, count);
                    if (n == 0)
                    {
                        throw new InvalidOperationException("End of file reached before expected");
                    }
                    index += n;
                    count -= n;
                }
            }

            return bytes;
        }
    }
}
