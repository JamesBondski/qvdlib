using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Bondski.QvdLib;
using Xunit;

namespace Bondski.QvdLib.Tests;
/// <summary>
/// Tests for the RowReader class.
/// </summary>
/// <remarks>
/// These tests cover the functionality of reading rows from a QVD file and ensure that the values are correctly interpreted.
/// </remarks>

public class RowReaderTest
{
    [Fact]
    public void GetFieldIndices_ReturnsSortedIndices()
    {
        var fields = new[]
        {
            new FieldInfo { Name = "B", BitOffset = 8, BitWidth = 8, Bias = 0 },
            new FieldInfo { Name = "A", BitOffset = 0, BitWidth = 8, Bias = 0 }
        };
        var header = new QvdHeader { Fields = fields, RecordByteSize = 2 };
        var values = new Dictionary<FieldInfo, IList<Value>>
        {
            [fields[0]] = new List<Value> { new Value() },
            [fields[1]] = new List<Value> { new Value() }
        };

        var reader = new RowReader(header, values);
        var indices = reader.GetFieldIndices();

        Assert.Equal(0, indices["A"]);
        Assert.Equal(1, indices["B"]);
    }

    [Fact]
    public void ReadRow_ReturnsCorrectValues()
    {
        var field = new FieldInfo { Name = "A", BitOffset = 0, BitWidth = 8, Bias = 0 };
        var header = new QvdHeader { Fields = new[] { field }, RecordByteSize = 1 };
        var valueList = new List<Value>
        {
            new Value { Type = Bondski.QvdLib.ValueType.Int, Int = 42 }
        };
        var values = new Dictionary<FieldInfo, IList<Value>>
        {
            [field] = valueList
        };

        var reader = new RowReader(header, values);

        // Buffer contains 0b00000000, so valueIndex = 0 + Bias = 0
        using var stream = new MemoryStream(new byte[] { 0x00 });
        var row = reader.ReadRow(stream);

        Assert.Single(row);
        Assert.Equal(Bondski.QvdLib.ValueType.Int, row[0].Type);
        Assert.Equal(42, row[0].Int);
    }

    [Fact]
    public void ReadRow_ReturnsNullValue_WhenValueIndexIsMinusTwo()
    {
        var field = new FieldInfo { Name = "A", BitOffset = 0, BitWidth = 8, Bias = -2 };
        var header = new QvdHeader { Fields = new[] { field }, RecordByteSize = 1 };
        var valueList = new List<Value>
        {
            new Value { Type = Bondski.QvdLib.ValueType.Int, Int = 42 }
        };
        var values = new Dictionary<FieldInfo, IList<Value>>
        {
            [field] = valueList
        };

        var reader = new RowReader(header, values);

        // Buffer contains 0b00000000, so valueIndex = 0 + Bias = -2
        using var stream = new MemoryStream(new byte[] { 0x00 });
        var row = reader.ReadRow(stream);

        Assert.Single(row);
        Assert.Equal(Bondski.QvdLib.ValueType.Null, row[0].Type);
    }
}