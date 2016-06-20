using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Lionbridge.onDemand.Client
{
    public class FileRejection : IXmlSerializable
    {
        /// <summary>
        /// Integer representing the reason number of rejecting translated file.
        /// </summary>
        public Int32 ReasonCode
        {
            get;
            set;
        }

        /// <summary>
        /// String representing the description of rejecting translated file.
        /// </summary>
        public String ReasonDescription
        {
            get;
            set;
        }

        /// <summary>
        /// Create File Rejection information to pass to onDemand
        /// </summary>
        /// <param name="reasonCode">Integer representing the reason number of rejecting translated file.</param>
        /// <param name="reasonDescription">String representing the description of rejecting translated file.</param>
        public FileRejection(Int32 reasonCode, String reasonDescription)
        {
            this.ReasonCode = reasonCode;
            this.ReasonDescription = reasonDescription;
        }

        /// <summary>
        /// Seralize the class data to XML in an onDemand specific way
        /// </summary>
        /// <returns>The XML string which can be passed to onDemand</returns>
        public string ToXmlString()
        {
            var root = new XElement("RejectFile",
                new XElement("ReasonCode", this.ReasonCode),
                new XElement("ReasonDescription", this.ReasonDescription));

            return root.ToString();
        }
    }
}
