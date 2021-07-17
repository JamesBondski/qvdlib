// <copyright file="ValueReader.cs" company="Matthias Kersting">
// Copyright (c) Matthias Kersting. All rights reserved.
// </copyright>

namespace Bondski.QvdLib
{
    using System;
    using System.IO;
    using System.Text;

    /// <summary>
    /// Class for reading the value segment for one field in the QVD.
    /// </summary>
    internal static class ValueReader
    {
        private static readonly int MaxBufferSize = 1048576;

        /// <summary>
        /// Reads the values of one field from the QVD. The given field must correspond to the next data, there is no way to verify that.
        /// </summary>
        /// <param name="field">Information of the field to read.</param>
        /// <param name="data">Stream to the QVD file.</param>
        /// <returns>An array with the values read from the QVD.</returns>
        public static Value[] ReadValues(FieldInfo field, Stream data)
        {
            Value[] values = new Value[field.NoOfSymbols];

            using (BinaryReader reader = new BinaryReader(data, Encoding.UTF8, true))
            {
                for (int i = 0; i < field.NoOfSymbols; i++)
                {
                    values[i] = ReadValue(reader);
                }
            }
            return values;
        }

        private static Value ReadValue(BinaryReader reader)
        {
            ValueType type = (ValueType)reader.ReadByte(); //(ValueType)Enum.ToObject(typeof(ValueType), reader.ReadByte());

            switch (type)
            {
                case ValueType.Int:
                    return new Value()
                    {
                        Int = reader.ReadInt32(),
                        Type = type,
                    };
                case ValueType.Double:
                    return new Value()
                    {
                        Double = reader.ReadDouble(),
                        Type = type,
                    };
                case ValueType.String:
                    return new Value()
                    {
                        String = ReadString(reader),
                        Type = type,
                    };
                case ValueType.DualInt:
                    return new Value()
                    {
                        Int = reader.ReadInt32(),
                        String = ReadString(reader),
                        Type = type,
                    };
                case ValueType.DualDouble:
                    return new Value()
                    {
                        Double = reader.ReadDouble(),
                        String = ReadString(reader),
                        Type = type,
                    };
                default:
                    throw new InvalidValueException("Unknown value type " + type);
            }
        }

        private static byte[] buffer = new byte[64];

        private static string ReadString(BinaryReader reader)
        {
            //byte[] buffer = new byte[64];
            int pos = 0;
            byte nextByte = reader.ReadByte();

            // Read bytes into buffer until we find a 0x00
            while (nextByte != 0)
            {
                buffer[pos++] = nextByte;
                if (pos == buffer.Length)
                {
                    if (buffer.Length <= MaxBufferSize)
                    {
                        Array.Resize(ref buffer, buffer.Length * 2);
                    }
                    else
                    {
                        throw new InvalidValueException("String is longer than max length (" + MaxBufferSize + ")");
                    }
                }
                nextByte = reader.ReadByte();
            }

            // Convert to string
            return Encoding.UTF8.GetString(buffer, 0, pos);
        }
    }
}
