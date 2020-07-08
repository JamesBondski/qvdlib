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
        private readonly Stream input;
        private XDocument headerDocument = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="HeaderReader"/> class.
        /// </summary>
        /// <param name="input">Path to the QVD file.</param>
        public HeaderReader(Stream input)
        {
            this.input = input;
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
        public XDocument ReadHeader()
        {
            byte[] searchString = Encoding.UTF8.GetBytes("</QvdTableHeader>");
            using (MemoryStream bufferStream = new MemoryStream())
            {
                int foundBytes = 0;
                while (foundBytes < searchString.Length)
                {
                    byte nextByte = (byte)this.input.ReadByte();
                    if (nextByte == searchString[foundBytes])
                    {
                        foundBytes++;
                    }
                    else
                    {
                        foundBytes = 0;
                    }

                    bufferStream.WriteByte(nextByte);

                    if (this.input.Position == this.input.Length)
                    {
                        throw new InvalidDataException("Could not find QVD XML table header.");
                    }
                }

                bufferStream.Seek(0, SeekOrigin.Begin);
                this.headerDocument = XDocument.Load(bufferStream);
                this.IsRead = true;
                return this.headerDocument;
            }
        }
    }
}