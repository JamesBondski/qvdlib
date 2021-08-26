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
        /// Returns the integer value if the Value is of Type Int or DualInt. No conversions are done.
        /// </summary>
        /// <returns>Returns the integer value.</returns>
        /// <exception cref="InvalidValueException">Thrown if the Value is not of Type Int or DualInt.</exception>
        public int? AsInt()
        {
            if (this.Type == ValueType.Int || this.Type == ValueType.DualInt)
            {
                return this.Int;
            }

            if (this.Type == ValueType.Null)
            {
                return null;
            }

            throw new InvalidValueException("Value is not an Integer.");
        }

        /// <summary>
        /// Returns the double value if the Value is of Type Double or DualDouble. No conversions are done.
        /// </summary>
        /// <returns>Returns the double value.</returns>
        /// <exception cref="InvalidValueException">Thrown if the Value is not of Type Double or DualDouble.</exception>
        public double? AsDouble()
        {
            if (this.Type == ValueType.Double || this.Type == ValueType.DualDouble)
            {
                return this.Double;
            }

            if (this.Type == ValueType.Null)
            {
                return null;
            }

            throw new InvalidValueException("Value is not a Double.");
        }

        /// <summary>
        /// Returns the string value if the Value is of Type String, DualInt or DualDouble. No conversions are done.
        /// </summary>
        /// <returns>Returns the string value.</returns>
        /// <exception cref="InvalidValueException">Thrown if the Value is not of Type String, DualInt or DualDouble.</exception>
        public string? AsString()
        {
            if (this.Type == ValueType.String || this.Type == ValueType.DualInt || this.Type == ValueType.DualDouble)
            {
                return this.String;
            }

            if (this.Type == ValueType.Null)
            {
                return null;
            }

            throw new InvalidValueException("Value does not have a string representation.");
        }

        /// <summary>
        /// Returns the value as an integer. This will attempt to convert from
        /// double or string if possible using System.Convert.ToInt32. If the value
        /// is a dual value, the numeric representation will be used.
        /// </summary>
        /// <param name="convertFromString">Indicates whether string values should be converted to integers.</param>
        /// <returns>Returns the integer value which may have been converted.</returns>
        /// <exception cref="InvalidValueException">Thrown if convertFromString is false and the value is a string or if the value has an unhandled type.</exception>
        public int? ToInt(bool convertFromString = false)
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
                    if (convertFromString)
                    {
                        return Convert.ToInt32(this.String);
                    }
                    else
                    {
                        throw new InvalidValueException("Value '" + this.String + "' could not be converted to integer.");
                    }

                case ValueType.Null:
                    return null;
                default:
                    throw new InvalidValueException("Value has an unknown Type.");
            }
        }

        /// <summary>
        /// Returns the value as a double. This will attempt to convert from
        /// int or string if possible using System.Convert.ToDouble. If the value
        /// is a dual value, the numeric representation will be used.
        /// </summary>
        /// <param name="convertFromString">Indicates whether string values should be converted to doubles.</param>
        /// <returns>Returns the double value.</returns>
        /// <exception cref="InvalidValueException">Thrown if convertFromString is false and the value is a string or if the value has an unhandled type.</exception>
        public double? ToDouble(bool convertFromString = false)
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
                    if (convertFromString)
                    {
                        return Convert.ToDouble(this.String);
                    }
                    else
                    {
                        throw new InvalidValueException("Value '" + this.String + "' could not be converted to integer.");
                    }

                case ValueType.Null:
                    return null;
                default:
                    throw new InvalidValueException("Value has an unknown Type.");
            }
        }

        /// <summary>
        /// Gets the string representation of the value. If it is a dual, the string
        /// representation will be used.
        /// </summary>
        /// <returns>The value as a string.</returns>
        public override string? ToString()
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
                case ValueType.Null:
                    return null;
                default:
                    throw new InvalidValueException("Value has an unknown Type.");
            }
        }
    }
}
