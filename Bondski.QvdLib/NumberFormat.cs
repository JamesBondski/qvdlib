// <copyright file="NumberFormat.cs" company="Matthias Kersting">
// Copyright (c) Matthias Kersting. All rights reserved.
// </copyright>

namespace Bondski.QvdLib
{
    public record NumberFormat
    {
        /// <summary>
        /// Gets the type of the field.
        /// </summary>
        public string? Type { get; init; }

        /// <summary>
        /// Gets the number of Decimals for the field.
        /// </summary>
        public byte? NDec { get; init; }

        /// <summary>
        /// Gets whether it uses thousand separator (?).
        /// </summary>
        public byte? UseThou { get; init; }

        /// <summary>
        /// Gets the number format string.
        /// </summary>
        public string? Fmt { get; init; }

        /// <summary>
        /// Gets the decimal symbol.
        /// </summary>
        public string? Dec { get; init; }

        /// <summary>
        /// Gets the thousand symbol.
        /// </summary>
        public string? Thou { get; init; }
    }
}
