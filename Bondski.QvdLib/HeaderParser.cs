// <copyright file="HeaderParser.cs" company="Matthias Kersting">
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
    internal class HeaderParser
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

            this.Header = new QvdHeader()
            {
                Fields = GetFields(qvdTableHeaderElement),
                QvBuildNo = Get<string>(qvdTableHeaderElement, "QvBuildNo"),
                CreatorDoc = Get<string>(qvdTableHeaderElement, "CreatorDoc"),
                CreateTime = GetDateTime(qvdTableHeaderElement, "CreateUtcTime"),
                SourceCreateTime = GetDateTime(qvdTableHeaderElement, "SourceCreateUtcTime"),
                SourceFileTime = GetDateTime(qvdTableHeaderElement, "SourceFileUtcTime"),
                SourceFileSize = Get<long>(qvdTableHeaderElement, "SourceFileSize"),
                StaleUtcTime = GetDateTime(qvdTableHeaderElement, "StaleUtcTime"),
                TableName = Get<string>(qvdTableHeaderElement, "TableName"),
                Compression = Get<string>(qvdTableHeaderElement, "Compression"),
                RecordByteSize = GetRequired<int>(qvdTableHeaderElement, "RecordByteSize"),
                NoOfRecords = GetRequired<int>(qvdTableHeaderElement, "NoOfRecords"),
                Offset = GetRequired<int>(qvdTableHeaderElement, "Offset"),
                Length = GetRequired<long>(qvdTableHeaderElement, "Length"),
                Lineage = Get<string>(qvdTableHeaderElement, "Lineage"),
                Comment = Get<string>(qvdTableHeaderElement, "Comment"),
                EncryptionInfo = Get<string>(qvdTableHeaderElement, "EncryptionInfo"),
            };
        }

        /// <summary>
        /// Gets the information from the parsed XML document.
        /// </summary>
        public QvdHeader Header { get; }

        private static FieldInfo[] GetFields(XElement qvdTableHeaderElement)
        {
            var fields = qvdTableHeaderElement
                .XPathSelectElements("Fields/QvdFieldHeader")
                .Select(qvdFieldHeaderElement => GetFieldInfo(qvdFieldHeaderElement));

            if (!fields.Any())
            {
                throw new InvalidHeaderException("QVD header needs to have at least 1 field.");
            }

            return fields.OrderBy(f => f.BitOffset).ToArray();
        }

        private static FieldInfo GetFieldInfo(XElement element)
        {
            return new FieldInfo()
            {
                Name = GetRequired<string>(element, "FieldName"),
                BitOffset = Get<int>(element, "BitOffset"),
                BitWidth = Get<int>(element, "BitWidth"),
                Bias = Get<int>(element, "Bias"),
                NumberFormat = GetNumberFormat(element.Element("NumberFormat")),
                NoOfSymbols = Get<int>(element, "NoOfSymbols"),
                Offset = Get<int>(element, "Offset"),
                Length = Get<int>(element, "Length"),
                Comment = Get<string>(element, "Comment"),
                Tags = GetStringList(element, "Tags", "String"),
            };
        }

        private static NumberFormat GetNumberFormat(XElement? element)
        {
            if (element == null)
            {
                return new NumberFormat();
            }

            return new NumberFormat()
            {
                Type = Get<string>(element, "Type"),
                NDec = Get<byte>(element, "nDec"),
                UseThou = Get<byte>(element, "UseThou"),
                Fmt = Get<string>(element, "Fmt"),
                Dec = Get<string>(element, "Dec"),
                Thou = Get<string>(element, "Thou"),
            };
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

        private static DateTime? GetDateTime(XElement element, string elementName)
        {
            DateTime? result = Get<DateTime>(element, elementName);
            if (result == default(DateTime))
            {
                return null;
            }

            return result;
        }

        private static string[]? GetStringList(XElement element, string elementName, string subElementName)
        {
            XElement? xElement = element.Element(elementName);
            if (xElement == null || string.IsNullOrEmpty(xElement.Value))
            {
                return null;
            }

            var childElements = xElement.Elements(subElementName);
            return childElements.Select(ce => ce.Value).ToArray();
        }

        private static T? GetValue<T>(XElement element, string elementName, out bool missing)
        {
            XElement? xElement = element.Element(elementName);
            if (xElement == null || string.IsNullOrEmpty(xElement.Value))
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
