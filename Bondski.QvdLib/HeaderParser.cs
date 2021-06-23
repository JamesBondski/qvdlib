// <copyright file="QvdHeader.cs" company="Matthias Kersting">
// Copyright (c) Matthias Kersting. All rights reserved.
// </copyright>

namespace Bondski.QvdLib
{
    using System;
    using System.Linq;
    using System.Xml.Linq;
    using System.Xml.XPath;

    /// <summary>
    /// Holds the information from a QvdTableHeader element.
    /// </summary>
    public class HeaderParser
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HeaderParser"/> class.
        /// </summary>
        /// <param name="qvdTableHeaderDoc">XDocument containing the QvdTableHeader element.</param>
        public HeaderParser(XDocument qvdTableHeaderDoc)
            : this(qvdTableHeaderDoc.Root)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HeaderParser"/> class.
        /// </summary>
        /// <param name="qvdTableHeaderElement">QvdTableHeader element.</param>
        public HeaderParser(XElement? qvdTableHeaderElement)
        {
            if (qvdTableHeaderElement == null)
            {
                throw new InvalidHeaderException("Header element is null.");
            }

            if (qvdTableHeaderElement.Name != "QvdTableHeader")
            {
                throw new InvalidHeaderException("XML element not QvdTableHeader.");
            }

            this.Fields = GetFields(qvdTableHeaderElement);
        }

        /// <summary>
        /// Gets the field descriptions from the QVD header.
        /// </summary>
        public FieldInfo[] Fields { get; }

        private static FieldInfo[] GetFields(XElement qvdTableHeaderElement)
        {
            var fields = qvdTableHeaderElement
                .XPathSelectElements("Fields/QvdFieldHeader")
                .Select(qvdFieldHeaderElement => GetFieldInfo(qvdFieldHeaderElement))
                .ToArray();

            if (fields.Length == 0)
            {
                throw new InvalidHeaderException("QVD header needs to have at least 1 field.");
            }

            return fields;
        }

        private static FieldInfo GetFieldInfo(XElement element)
        {
            return new FieldInfo(
                name: GetRequired<string>(element, "FieldName"),
                bitOffset: Get<int>(element, "BitOffset"),
                bitWidth: Get<int>(element, "BitWidth"),
                bias: Get<int>(element, "Bias"),
                noOfSymbols: Get<int>(element, "NoOfSymbols"),
                offset: Get<int>(element, "Offset"),
                length: Get<int>(element, "Length"),
                comment: Get<string>(element, "Comment"));
        }

        private static T GetRequired<T>(XElement element, string elementName)
        {
            bool isMissing = false;
            T? result = GetValue<T>(element, elementName, out isMissing);
            if (isMissing || result == null)
            {
                throw new InvalidHeaderException("QVD header is missing element " + elementName);
            }

            return result;
        }

        private static T? Get<T>(XElement element, string elementName)
        {
            bool dummy = false;
            return GetValue<T>(element, elementName, out dummy);
        }

        private static T? GetValue<T>(XElement element, string elementName, out bool missing)
        {
            XElement? xElement = element.Element(elementName);
            if (xElement == null)
            {
                missing = true;
                return default;
            }
            else
            {
                missing = false;
            }

            try
            {
                return (T)Convert.ChangeType(xElement.Value, typeof(T));
            }
            catch (Exception ex)
            {
                throw new InvalidHeaderException("Invalid field value " + xElement.Value + " in element " + elementName, ex);
            }
        }
    }
}
