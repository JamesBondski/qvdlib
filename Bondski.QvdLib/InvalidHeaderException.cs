// <copyright file="InvalidHeaderException.cs" company="Matthias Kersting">
// Copyright (c) Matthias Kersting. All rights reserved.
// </copyright>

namespace Bondski.QvdLib
{
    using System;

    /// <summary>
    /// Exception thrown when the header of the qvd file is invalid.
    /// </summary>
    [Serializable]
    public class InvalidHeaderException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidHeaderException"/> class.
        /// </summary>
        public InvalidHeaderException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidHeaderException"/> class.
        /// </summary>
        /// <param name="message">Message for the exception.</param>
        public InvalidHeaderException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidHeaderException"/> class.
        /// </summary>
        /// <param name="message">Message for the exception.</param>
        /// <param name="inner">Inner exception.</param>
        public InvalidHeaderException(string message, Exception inner)
            : base(message, inner)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidHeaderException"/> class.
        /// </summary>
        /// <param name="info">SerializationInfo object.</param>
        /// <param name="context">StreamingContext object.</param>
        protected InvalidHeaderException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
        }
    }
}
