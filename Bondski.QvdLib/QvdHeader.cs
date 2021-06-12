// <copyright file="QvdHeader.cs" company="Matthias Kersting">
// Copyright (c) Matthias Kersting. All rights reserved.
// </copyright>

namespace Bondski.QvdLib
{
    using System.Linq;
    using System.Xml.Linq;
    using System.Xml.XPath;

    /// <summary>
    /// Holds the information from a QvdTableHeader element.
    /// </summary>
    public class QvdHeader
    {
        private FieldInfo[] fields;

        /// <summary>
        /// Initializes a new instance of the <see cref="QvdHeader"/> class.
        /// </summary>
        /// <param name="qvdTableHeaderDoc">XDocument containing the QvdTableHeader element.</param>
        public QvdHeader(XDocument qvdTableHeaderDoc)
            : this(qvdTableHeaderDoc.Root)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QvdHeader"/> class.
        /// </summary>
        /// <param name="qvdTableHeaderElement">QvdTableHeader element.</param>
        public QvdHeader(XElement? qvdTableHeaderElement)
        {
            if (qvdTableHeaderElement == null)
            {
                throw new InvalidHeaderException("Header element is null.");
            }

            if (qvdTableHeaderElement.Name != "QvdTableHeader")
            {
                throw new InvalidHeaderException("XML element not QvdTableHeader.");
            }

            this.fields = GetFields(qvdTableHeaderElement);
        }

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

        private static FieldInfo GetFieldInfo(XElement qvdFieldHeaderElement)
        {
            return new FieldInfo(name: GetValue(qvdFieldHeaderElement, "FieldName"));
        }

        private static string GetValue(XElement qvdFieldHeaderElement, string elementName)
        {
            XElement? xElement = qvdFieldHeaderElement.Element(elementName);
            if (xElement == null)
            {
                throw new InvalidHeaderException("FieldHeader is missing element " + elementName);
            }

            return xElement.Value;
        }
    }
}
