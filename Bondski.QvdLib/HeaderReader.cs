// <copyright file="HeaderReader.cs" company="Matthias Kersting">
// Copyright (c) Matthias Kersting. All rights reserved.
// </copyright>

namespace Bondski.QvdLib
{
    using System;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Linq;

    /// <summary>
    /// Reads the XML header from a QVD file.
    /// </summary>
    public class HeaderReader
    {
        private readonly string path;
        private XDocument headerDocument = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="HeaderReader"/> class.
        /// </summary>
        /// <param name="path">Path to the QVD file.</param>
        public HeaderReader(string path)
        {
            this.path = path;
        }

        /// <summary>
        /// Gets a value indicating whether the XML header has been read.
        /// </summary>
        public bool IsRead { get; private set; } = false;

        /// <summary>
        /// Gets the XML Document containing the header. If the header was not read yet, this will return null.
        /// </summary>
        public XDocument HeaderDocument
        {
            get
            {
                if (!this.IsRead)
                {
                    throw new InvalidOperationException("Header has not been read yet.");
                }

                return this.headerDocument;
            }
        }

        /// <summary>
        /// Returns the XML document read from the QVD file header. If the header has not been read yet, it will be read from the file.
        /// </summary>
        /// <returns>XDocument containing the QVD file hader.</returns>
        public async Task<XDocument> ReadHeaderAsync()
        {
            using (StreamReader reader = new StreamReader(this.path))
            {
                StringBuilder xmlString = new StringBuilder();
                while (!reader.EndOfStream)
                {
                    string line = await reader.ReadLineAsync();
                    xmlString.Append(line).AppendLine();
                    if (line.Contains("</QvdTableHeader>"))
                    {
                        break;
                    }
                }

                this.IsRead = true;
                this.headerDocument = XDocument.Parse(xmlString.ToString());
                return this.headerDocument;
            }
        }
    }
}