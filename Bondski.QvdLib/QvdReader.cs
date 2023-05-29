// <copyright file="QvdReader.cs" company="Matthias Kersting">
// Copyright (c) Matthias Kersting. All rights reserved.
// </copyright>

namespace Bondski.QvdLib
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// This class is the starting point for users of this library. Each instance will deal with reading information and data from
    /// one file.
    /// </summary>
    public class QvdReader : IDisposable
    {
        private Value[]? currentRow = null;
        private RowReader reader;
        private FileStream input;
        private Dictionary<string, int> fieldIndices = new Dictionary<string, int>();

        /// <summary>
        /// Initializes a new instance of the <see cref="QvdReader"/> class.
        /// </summary>
        /// <param name="path">Path to the QVD file.</param>
        public QvdReader(string path)
        {
            this.input = File.OpenRead(path);
            var headerXml = new HeaderExtractor(this.input).ReadHeader();
            this.Header = new HeaderParser(headerXml).Header;

            // Populate field indices for faster lookup in the order given by the header.
            int fieldIndex = 0;
            foreach (FieldInfo field in this.Header.Fields)
            {
                this.fieldIndices.Add(field.Name, fieldIndex++);
            }

            // Skip 1 byte
            this.input.Seek(1, SeekOrigin.Current);

            // Read the values (in the order of the fields in the header)
            foreach (var field in this.Header.Fields)
            {
                this.Values.Add(field, ValueReader.ReadValues(field, this.input));
            }

            this.reader = new RowReader(this.Header, this.Values);
        }

        /// <summary>
        /// Gets the Header information from the read qvd.
        /// </summary>
        public QvdHeader Header { get; init; }

        /// <summary>
        /// Gets the values from the value section of the qvd.
        /// </summary>
        public Dictionary<FieldInfo, IList<Value>> Values { get; init; } = new Dictionary<FieldInfo, IList<Value>>();

        /// <summary>
        /// Gets the value at the specified index in the current row.
        /// </summary>
        /// <param name="fieldIndex">Index of the field.</param>
        /// <returns>Value at the specified index.</returns>
        public Value this[int fieldIndex]
        {
            get
            {
                if (this.currentRow == null)
                {
                    throw new InvalidOperationException("No row has been read yet.");
                }

                return this.currentRow[fieldIndex];
            }
        }

        /// <summary>
        /// Gets the value of the field with the specified name.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <returns>Value of the field with the specified name.</returns>
        public Value this[string fieldName]
        {
            get
            {
                if (this.currentRow == null)
                {
                    throw new InvalidOperationException("No row has been read yet.");
                }

                if (!this.fieldIndices.ContainsKey(fieldName))
                {
                    throw new ArgumentException("Field " + fieldName + " not found.");
                }

                return this.currentRow[this.fieldIndices[fieldName]];
            }
        }

        /// <summary>
        /// Reads the next row.
        /// </summary>
        /// <returns>Returns true, if there are more records, otherwise false.</returns>
        public bool NextRow()
        {
            if (this.input.Position == this.input.Length)
            {
                return false;
            }
            else
            {
                this.currentRow = this.reader.ReadRow(this.input);
                return true;
            }
        }

        /// <summary>
        /// Gets the raw values of the current row.
        /// </summary>
        /// <returns>An array containing the values in the current row.</returns>
        /// <exception cref="InvalidOperationException">Thrown if no row has been read yet.</exception>
        public Value[] GetValues()
        {
            if (this.currentRow == null)
            {
                throw new InvalidOperationException("No row has been read yet.");
            }

            return this.currentRow;
        }

        /// <summary>
        /// Disposes the object.
        /// </summary>
        public void Dispose()
        {
            this.input.Dispose();
        }
    }
}
