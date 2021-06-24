using System;

namespace Bondski.QvdLib
{
    public record QvdHeader
    {
        /// <summary>
        /// Gets the field information from the QVD header.
        /// </summary>
        public FieldInfo[]? Fields { get; init; }

        /// <summary>
        /// Gets the build number of the Qlik engine used to produce the QVD.
        /// </summary>
        public string? QvBuildNo { get; init; }

        /// <summary>
        /// Gets the document from which the QVD was created.
        /// </summary>
        public string? CreatorDoc { get; init; }
    }
}
