namespace Bondski.QvdLib
{
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    /// This class is the starting point for users of this library. Each instance will deal with reading information and data from
    /// one file.
    /// </summary>
    public class QvdReader
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QvdReader"/> class.
        /// </summary>
        /// <param name="path">Path to the QVD file.</param>
        public QvdReader(string path)
        {
            using (var input = File.OpenRead(path))
            {
                var headerXml = new HeaderExtractor(input).ReadHeader();
                this.Header = new HeaderParser(headerXml).Header;

                //Skip 1 byte
                input.Seek(1, SeekOrigin.Current);

                foreach (var field in this.Header.Fields)
                {
                    this.Values.Add(field, ValueReader.ReadValues(field, input));
                }
            }
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
