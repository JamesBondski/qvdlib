using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Bondski.QvdLib
{
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
        private readonly Dictionary<FieldInfo, Value[]> values;
        private byte[] buffer;

        public RowReader(QvdHeader header, Dictionary<FieldInfo, Value[]> values)
        {
            this.header = header;
            this.values = values;
            this.buffer = new byte[header.RecordByteSize];
        }

        public Value[] ReadRow(Stream input)
        {
            input.Read(this.buffer);
            Value[] rowData = new Value[this.values.Count];
            int fieldIndex = 0;
            int byteIndex = 0;
            int bitIndex = 0;

            for(int i = 0; i < header.Fields.Length; i++)
            {
                int bitsLeft = header.Fields[i].BitWidth;
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
                        buffer[byteIndex] = (byte)(buffer[byteIndex] >> bitsToRead);
                    }
                }

                valueIndex += (int) header.Fields[i].Bias;
                if(valueIndex != -2)
                {
                    rowData[fieldIndex++] = this.values[header.Fields[i]][valueIndex];
                }
                else
                {
                    rowData[fieldIndex++] = new Value()
                    {
                        Type = ValueType.Null,
                    };
                }
            }
            return rowData;
        }
    }
}
