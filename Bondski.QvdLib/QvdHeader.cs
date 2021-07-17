using System;

namespace Bondski.QvdLib
{
    public record QvdHeader
    {
        /// <summary>
        /// Gets the field information from the QVD header.
        /// </summary>
        public FieldInfo[] Fields { get; init; } = Array.Empty<FieldInfo>();

        /// <summary>
        /// Gets the build number of the Qlik engine used to produce the QVD.
        /// </summary>
        public string? QvBuildNo { get; init; }

        /// <summary>
        /// Gets the document from which the QVD was created.
        /// </summary>
        public string? CreatorDoc { get; init; }

        /// <summary>
        /// Gets the time of Creation
        /// </summary>
        public DateTime? CreateTime { get; init; }

        /// <summary>
        /// Gets the source creation time.
        /// </summary>
        public DateTime? SourceCreateTime { get; init; }

        /// <summary>
        /// Gets the file time of the source file.
        /// </summary>
        public DateTime? SourceFileTime { get; init; }
        
        /// <summary>
        /// Gets the size of the source file.
        /// </summary>
        public long? SourceFileSize { get; init; }

        /// <summary>
        /// Gets the stale Utc Time (?).
        /// </summary>
        public DateTime? StaleUtcTime { get; init; }

        /// <summary>
        /// Gets the name of the table that was stored in the QVD.
        /// </summary>
        public string? TableName { get; init; }

        /// <summary>
        /// Gets information about the compression.
        /// </summary>
        public string? Compression { get; init; }

        /// <summary>
        /// Gets the size of each record in bytes.
        /// </summary>
        public int RecordByteSize { get; init; }

        /// <summary>
        /// Gets the number of records in the file.
        /// </summary>
        public int NoOfRecords { get; init; }

        /// <summary>
        /// Gets the offset.
        /// </summary>
        public int Offset { get; init; }

        /// <summary>
        /// Gets the length of the data.
        /// </summary>
        public long Length { get; init; }

        /// <summary>
        /// Gets the lineage information.
        /// </summary>
        public string? Lineage { get; init; }

        /// <summary>
        /// Gets the comment for the table.
        /// </summary>
        public string? Comment { get; init; }

        /// <summary>
        /// Gets information about the encryption.
        /// </summary>
        public string? EncryptionInfo { get; init; }
    }
}
