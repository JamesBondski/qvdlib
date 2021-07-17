// <copyright file="FieldInfo.cs" company="Matthias Kersting">
// Copyright (c) Matthias Kersting. All rights reserved.
// </copyright>

namespace Bondski.QvdLib
{
    /// <summary>
    /// Holds information about a field in the QVD file.
    /// </summary>
    public record FieldInfo
    {
        /// <summary>
        /// Gets the name of the field.
        /// </summary>
        public string Name { get; init; } = string.Empty;

        /// <summary>
        /// Gets the offset in bits.
        /// </summary>
        public int BitOffset { get; init; }

        /// <summary>
        /// Gets the number of bits that represent one value.
        /// </summary>
        public int BitWidth { get; init; }

        /// <summary>
        /// Gets the bias (?).
        /// </summary>
        public int Bias { get; init; } = 0;

        /// <summary>
        /// Gets information about the number format.
        /// </summary>
        public NumberFormat? NumberFormat { get; init; }

        /// <summary>
        /// Gets the number of symbols for this field.
        /// </summary>
        public int NoOfSymbols { get; init; }

        /// <summary>
        /// Gets the offset.
        /// </summary>
        public int? Offset { get; init; }

        /// <summary>
        /// Gets the length (?).
        /// </summary>
        public int? Length { get; init; }

        /// <summary>
        /// Gets the comment for the field.
        /// </summary>
        public string? Comment { get; init; }

        public string[]? Tags { get; init; }
    }
}

