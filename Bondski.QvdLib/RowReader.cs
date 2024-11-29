// <copyright file="RowReader.cs" company="Matthias Kersting">
// Copyright (c) Matthias Kersting. All rights reserved.
// </copyright>

namespace Bondski.QvdLib
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// Class for reading rows from an input stream.
    /// </summary>
    internal class RowReader
    {
        private static readonly byte[] BitMasks = new byte[9]
        {
            0,
            0b0000_0001,
            0b0000_0011,
            0b0000_0111,
            0b0000_1111,
            0b0001_1111,
            0b0011_1111,
            0b0111_1111,
            0b1111_1111,
        };

        private readonly QvdHeader header;
        private readonly List<IList<Value>> values;
        private readonly byte[] buffer;
        private readonly FieldInfo[] fields;

        /// <summary>
        /// Initializes a new instance of the <see cref="RowReader"/> class. The given dictionary
        /// must be sorted by BitOffset.
        /// </summary>
        /// <param name="header">QVD header for the file.</param>
        /// <param name="values">A dictionary with all values in the file.</param>
        public RowReader(QvdHeader header, Dictionary<FieldInfo, IList<Value>> values)
        {
            this.header = header;
            this.buffer = new byte[this.header.RecordByteSize];

            // Make sure both the fields and the values are sorted by the BitOffset of the field
            this.values = values.OrderBy(f => f.Key.BitOffset).Select(kv => kv.Value).ToList();
            this.fields = this.header.Fields.OrderBy(f => f.BitOffset).ToArray();
        }

        /// <summary>
        /// Returns the sorted indices
        /// </summary>
        /// <returns>A dictionary with name an postion based on the sorted fields</returns>
        public Dictionary<string, int> GetFieldIndices()
        {
            Dictionary<string, int> retVal = new Dictionary<string, int>();

            for (int i = 0; i < this.fields.Length; i++)
            {
                retVal.Add(this.fields[i].Name, i);
            }
            return retVal;
        }

        /// <summary>
        /// Reads a row from the given stream. The Stream must be pointing to the beginning
        /// of a record.
        /// </summary>
        /// <param name="input">Stream to read from.</param>
        /// <returns>An array containing the values of the next row.</returns>
        public Value[] ReadRow(Stream input)
        {
            input.ReadExactly(this.buffer);
            Value[] rowData = new Value[this.values.Count];
            int fieldIndex = 0;
            int byteIndex = 0;
            int bitIndex = 0;

            for (int i = 0; i < this.fields.Length; i++)
            {
                int bitsLeft = this.fields[i].BitWidth;
                int bitsRead = 0;
                int valueIndex = 0;

                // Read byte for byte until we have enough bits
                while (bitsLeft > 0)
                {
                    int bitsToRead = Math.Min(8 - bitIndex, bitsLeft);
                    int bitValue = this.buffer[byteIndex] & BitMasks[bitsToRead];

                    // Add read bits to value index
                    valueIndex = valueIndex | (bitValue << bitsRead);
                    bitsLeft -= bitsToRead;
                    bitsRead += bitsToRead;

                    // Advance indices for reading
                    bitIndex += bitsToRead;
                    if (bitIndex == 8)
                    {
                        // If the rest of the byte was read, simply proceed to the next one.
                        byteIndex++;
                        bitIndex = 0;
                    }
                    else
                    {
                        // Otherwise shift the value so the remaining bits will be read next iteration
                        this.buffer[byteIndex] = (byte)(this.buffer[byteIndex] >> bitsToRead);
                    }
                }

                // Adjust the read value index using the Bias from the QVD header
                // If it is a null value, the result after applying the bias is -2
                valueIndex += (int)this.fields[i].Bias;
                if (valueIndex != -2)
                {
                    rowData[fieldIndex] = this.values[fieldIndex][valueIndex];
                }
                else
                {
                    rowData[fieldIndex] = new Value()
                    {
                        Type = ValueType.Null,
                    };
                }

                fieldIndex++;
            }

            return rowData;
        }
    }
}
