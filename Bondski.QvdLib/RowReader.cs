﻿// <copyright file="RowReader.cs" company="Matthias Kersting">
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
        private readonly List<Value[]> values;
        private readonly byte[] buffer;

        /// <summary>
        /// Initializes a new instance of the <see cref="RowReader"/> class. The given dictionary
        /// must be sorted by BitOffset.
        /// </summary>
        /// <param name="header">QVD header for the file.</param>
        /// <param name="values">A dictionary with all values in the file.</param>
        public RowReader(QvdHeader header, Dictionary<FieldInfo, Value[]> values)
        {
            this.header = header;
            this.buffer = new byte[this.header.RecordByteSize];
            this.values = values.Select(kv => kv.Value).ToList();
        }

        /// <summary>
        /// Reads a row from the given stream. The Stream must be pointing to the beginning
        /// of a record.
        /// </summary>
        /// <param name="input">Stream to read from.</param>
        /// <returns>An array containing the values of the next row.</returns>
        public Value[] ReadRow(Stream input)
        {
            input.Read(this.buffer);
            Value[] rowData = new Value[this.values.Count];
            int fieldIndex = 0;
            int byteIndex = 0;
            int bitIndex = 0;

            for(int i = 0; i < this.header.Fields.Length; i++)
            {
                int bitsLeft = this.header..Fields[i].BitWidth;
                int bitsRead = 0;
                int valueIndex = 0;

                while (bitsLeft > 0)
                {
                    int bitsToRead = Math.Min(8 - bitIndex, bitsLeft);
                    int bitValue = this.buffer[byteIndex] & BitMasks[bitsToRead];
                    valueIndex = valueIndex | (bitValue << bitsRead);
                    bitsLeft -= bitsToRead;
                    bitsRead += bitsToRead;

                    bitIndex += bitsToRead;
                    if (bitIndex == 8)
                    {
                        byteIndex++;
                        bitIndex = 0;
                    }
                    else
                    {
                        this.buffer[byteIndex] = (byte)(this.buffer[byteIndex] >> bitsToRead);
                    }
                }

                valueIndex += (int)this.header.Fields[i].Bias;
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
