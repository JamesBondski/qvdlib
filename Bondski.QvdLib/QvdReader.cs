namespace Bondski.QvdLib
{
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
            }
        }

        /// <summary>
        /// Gets the Header information from the read qvd.
        /// </summary>
        public QvdHeader Header { get; init; }
    }
}
