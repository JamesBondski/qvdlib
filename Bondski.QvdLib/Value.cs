// <copyright file="Value.cs" company="Matthias Kersting">
// Copyright (c) Matthias Kersting. All rights reserved.
// </copyright>

namespace Bondski.QvdLib
{
    using System;

    /// <summary>
    /// Contains one value from the value table.
    /// </summary>
    public readonly struct Value
    {
        /// <summary>
        /// Gets the Integer representaton of the value if applicable.
        /// </summary>
        public readonly int Int { get; init; }

        /// <summary>
        /// Gets the Double representation of the value if applicable.
        /// </summary>
        public readonly double Double { get; init; }

        /// <summary>
        /// Gets the string representation of the value if applicable.
        /// </summary>
        public readonly string String { get; init; }

        /// <summary>
        /// Gets the Qlik type of this value.
        /// </summary>
        public readonly ValueType Type { get; init; }

        /// <summary>
        /// Returns the value as an integer. This will attempt to convert from
        /// double or string if possible using System.Convert.ToInt32. If the value
        /// is a dual value, the numeric representation will be used.
        /// </summary>
        /// <returns>Returns the integer value.</returns>
        public int ToInt()
        {
            switch (this.Type)
            {
                case ValueType.Int:
                case ValueType.DualInt:
                    return this.Int;
                case ValueType.Double:
                case ValueType.DualDouble:
                    return Convert.ToInt32(this.Double);
                case ValueType.String:
                    return Convert.ToInt32(this.String);
                default:
                    throw new InvalidValueException("Value has an unknown Type.");
            }
        }

        /// <summary>
        /// Returns the value as a double. This will attempt to convert from
        /// int or string if possible using System.Convert.ToDouble. If the value
        /// is a dual value, the numeric representation will be used.
        /// </summary>
        /// <returns>Returns the double value.</returns>
        public double ToDouble()
        {
            switch (this.Type)
            {
                case ValueType.Double:
                case ValueType.DualDouble:
                    return this.Double;
                case ValueType.Int:
                case ValueType.DualInt:
                    return Convert.ToDouble(this.Int);
                case ValueType.String:
                    return Convert.ToDouble(this.String);
                default:
                    throw new InvalidValueException("Value has an unknown Type.");
            }
        }

        /// <summary>
        /// Gets the string representation of the value. If it is a dual, the string
        /// representation will be used.
        /// </summary>
        /// <returns>The value as a string.</returns>
        public override string ToString()
        {
            switch (this.Type)
            {
                case ValueType.String:
                case ValueType.DualDouble:
                case ValueType.DualInt:
                    return this.String;
                case ValueType.Int:
                    return this.Int.ToString();
                case ValueType.Double:
                    return this.Double.ToString();
                default:
                    throw new InvalidValueException("Value has an unknown Type.");
            }
        }
    }
}
