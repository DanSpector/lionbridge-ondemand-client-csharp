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
using System.Xml.Linq;

namespace Lionbridge.onDemand.Client
{
    /// <summary>
    /// The Get Estimate API is used to get a rough estimate for a project based on the parameters listed below. 
    /// It is useful for client applications that are capable of counting words and that want to give the end customer a rough estimate of how much a project will cost. 
    /// Please note, the estimate will not be the same as the actual price if the onDemand word counting algorithm produces a different result than the client application.
    /// </summary>
    public class Estimate
    {
        /// <summary>
        /// Contains information about the service being quoted.
        /// </summary>
        public Service Service
        {
            get;
            private set;
        }

        /// <summary>
        /// Currency that the price is in.
        /// </summary>
        public String Currency
        {
            get;
            private set;
        }

        /// <summary>
        /// Total price that needs to be paid. Exclude translation credit.
        /// </summary>
        public Decimal TotalCost
        {
            get;
            private set;
        }

        /// <summary>
        /// String representing the DateTime that the project would be completed if purchased right now.
        /// </summary>
        public DateTime DueDate
        {
            get;
            private set;
        }

        /// <summary>
        /// Reference to the sdk client that retrieved this object
        /// </summary>
        private IContentAPI Client
        {
            get;
            set;
        }

        /// <summary>
        /// Empty private constructor
        /// </summary>
        private Estimate()
        {
        }

        /// <summary>
        /// Constructor from XML
        /// </summary>
        /// <param name="element"></param>
        /// <param name="client"></param>
        internal Estimate(XElement element, IContentAPI client)
            : this()
        {
            this.Client = client;

            if (element != null)
            {
                XElement service = element.Element("Service");

                int serviceID = service.GetChildValueAsInt32("ServiceID");

                if (this.Client != null)
                {
                    this.Service = this.Client.GetService(serviceID);
                }

                this.Currency = element.GetChildValue("Currency");

                this.TotalCost = element.GetChildValueAsDecimal("TotalCost");
            }
        }
    }
}
