using System;
using System.IO;
using System.Reflection;
using Xunit;

namespace Bondski.QvdLib.Tests
{
    public class ValueReaderTest
    {
        private static string TestFilePath => new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName + "/Resources/HeaderReaderTest/Test.qvd";
        private QvdReader reader;

        public ValueReaderTest()
        {
            this.reader = new QvdReader(TestFilePath);
            this.reader.NextRow();
        }

        [Fact]
        public void InvalidNameTest()
        {
            Assert.Throws<ArgumentException>( () => this.reader["A"] );
        }

        [Fact]
        public void ThrowsIfNotRead()
        {
            QvdReader qvdReader = new QvdReader(TestFilePath);
            Assert.Throws<InvalidOperationException>(() => qvdReader["StringColumn"]);
        }

        [Fact]
        public void ReadStringTest()
        {
            Value val = reader["StringColumn"];
            Assert.Equal(ValueType.String, val.Type);
            Assert.Equal("A", val.String);
            Assert.Equal(0, val.Double);
            Assert.Equal(0, val.Int);
        }

        [Fact]
        public void ReadDualIntTest()
        {
            Value val = reader["DualIntColumn"];
            Assert.Equal(ValueType.DualInt, val.Type);
            Assert.Equal("A", val.String);
            Assert.Equal(0, val.Double);
            Assert.Equal(1, val.Int);
        }

        [Fact]
        public void ReadDualDoubleTest()
        {
            Value val = reader["DualDoubleColumn"];
            Assert.Equal(ValueType.DualDouble, val.Type);
            Assert.Equal("A", val.String);
            Assert.Equal(1.1, val.Double);
            Assert.Equal(0, val.Int);
        }

        [Fact]
        public void ReadNullTest()
        {
            // Skip to third row
            reader.NextRow();
            reader.NextRow();

            Value val = reader["StringColumn"];
            Assert.Equal(ValueType.Null, val.Type);
            Assert.Null(val.String);
            Assert.Null(val.ToString());
        }
    }
}
