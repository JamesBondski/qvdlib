// <copyright file="QvdHeader.cs" company="Matthias Kersting">
// Copyright (c) Matthias Kersting. All rights reserved.
// </copyright>

namespace Bondski.QvdLib
{
    using System;
    using System.Xml.Linq;

    /// <summary>
    /// Holds the information from a QvdTableHeader element.
    /// </summary>
    public class QvdHeader
    {
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
        /// <param name="qvdTableHaderElement">QvdTableHeader element.</param>
        public QvdHeader(XElement qvdTableHaderElement)
        {
            if (qvdTableHaderElement.Name != "QvdTableHeader")
            {
                throw new ArgumentException("XML element not QvdTableHeader.");
            }

            this.QvdTableHeaderElement = qvdTableHaderElement;
        }

        /// <summary>
        /// Gets the XML element containing all header information.
        /// </summary>
        public XElement QvdTableHeaderElement { get; }
    }
}
