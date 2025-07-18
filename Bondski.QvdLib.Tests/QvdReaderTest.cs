namespace Bondski.QvdLib.Tests
{
    using System;
    using System.IO;
    using System.Reflection;
    using Xunit;

    public class QvdReaderTest
    {
        private static string TestFilePath => new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName + "/Resources/HeaderReaderTest/Test.qvd";

        [Fact]
        public void ReadHeader()
        {
            QvdReader reader = new QvdReader(TestFilePath);
            Assert.NotNull(reader.Header);
            Assert.Equal("Test", reader.Header.TableName);
        }

        [Fact]
        public void NextRow_ReturnsTrueAndFalseCorrectly()
        {
            using var reader = new QvdReader(TestFilePath);
            for(int i = 0; i < reader.Header.NoOfRecords; i++)
            {
                Assert.True(reader.NextRow());
            }
            Assert.False(reader.NextRow()); // Annahme: Test.qvd hat nur eine Zeile
        }

        [Fact]
        public void GetValues_ReturnsCurrentRow()
        {
            using var reader = new QvdReader(TestFilePath);
            reader.NextRow();
            var values = reader.GetValues();
            Assert.NotNull(values);
            Assert.Equal(reader.Header.Fields.Length, values.Length);
        }

        [Fact]
        public void Indexer_ByIntAndString_ReturnsCorrectValue()
        {
            using var reader = new QvdReader(TestFilePath);
            reader.NextRow();
            var valueByInt = reader[0];
            var valueByName = reader[reader.Header.Fields[0].Name];
            Assert.Equal(valueByInt, valueByName);
        }

        [Fact]
        public void Indexer_ThrowsException_IfNoRowRead()
        {
            using var reader = new QvdReader(TestFilePath);
            Assert.Throws<InvalidOperationException>(() => { var v = reader[0]; });
            Assert.Throws<InvalidOperationException>(() => { var v = reader["Unbekannt"]; });
        }

        [Fact]
        public void Indexer_ThrowsException_IfFieldNameInvalid()
        {
            using var reader = new QvdReader(TestFilePath);
            reader.NextRow();
            Assert.Throws<ArgumentException>(() => { var v = reader["Unbekannt"]; });
        }

        [Fact]
        public void Dispose_ClosesStream()
        {
            var reader = new QvdReader(TestFilePath);
            reader.Dispose();
            Assert.Throws<ObjectDisposedException>(() => reader.NextRow());
        }
    }
}
