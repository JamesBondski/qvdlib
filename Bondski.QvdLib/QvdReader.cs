namespace Bondski.QvdLib
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    /// This class is the starting point for users of this library. Each instance will deal with reading information and data from
    /// one file.
    /// </summary>
    public class QvdReader : IDisposable
    {
        private Value[]? currentRow = null;
        private RowReader reader;
        private FileStream input;

        /// <summary>
        /// Initializes a new instance of the <see cref="QvdReader"/> class.
        /// </summary>
        /// <param name="path">Path to the QVD file.</param>
        public QvdReader(string path)
        {
            this.input = File.OpenRead(path);
            var headerXml = new HeaderExtractor(this.input).ReadHeader();
            this.Header = new HeaderParser(headerXml).Header;

            // Skip 1 byte
            this.input.Seek(1, SeekOrigin.Current);

            foreach (var field in this.Header.Fields)
            {
                this.Values.Add(field, ValueReader.ReadValues(field, this.input));
            }

            this.reader = new RowReader(this.Header, this.Values);
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
        public Value[] GetValues()
        {
            return this.currentRow;
        }

        /// <summary>
        /// Disposes the object.
        /// </summary>
        public void Dispose()
        {
            this.input.Dispose();
        }

        /// <summary>
        /// Gets the Header information from the read qvd.
        /// </summary>
        public QvdHeader Header { get; init; }

        /// <summary>
        /// Gets the values from the value section of the qvd.
        /// </summary>
        public Dictionary<FieldInfo, Value[]> Values { get; init; } = new Dictionary<FieldInfo, Value[]>();
    }
}
