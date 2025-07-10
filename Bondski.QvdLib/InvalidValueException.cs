// <copyright file="InvalidValueException.cs" company="Matthias Kersting">
// Copyright (c) Matthias Kersting. All rights reserved.
// </copyright>

namespace Bondski.QvdLib
{
    using System;

    /// <summary>
    /// Exception thrown when the a value in the value section of the QVD is invalid.
    /// </summary>
    public class InvalidValueException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidValueException"/> class.
        /// </summary>
        public InvalidValueException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidValueException"/> class.
        /// </summary>
        /// <param name="message">Message for the exception.</param>
        public InvalidValueException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidValueException"/> class.
        /// </summary>
        /// <param name="message">Message for the exception.</param>
        /// <param name="inner">Inner exception.</param>
        public InvalidValueException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
