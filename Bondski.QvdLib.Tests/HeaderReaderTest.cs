using System;
using System.IO;
using Xunit;

namespace Bondski.QvdLib.Tests
{
    public class HeaderReaderTest
    {
        [Fact]
        public void NormalRead()
        {
            var reader = new HeaderReader(File.OpenRead("Resources\\HeaderReaderTest\\Test.qvd"));
            var doc = reader.ReadHeader();
            Assert.NotNull(doc);
            Assert.Equal("QvdTableHeader", doc.Root.Name);
        }

        [Fact]
        public void IsRead()
        {
            var reader = new HeaderReader(File.OpenRead("Resources\\HeaderReaderTest\\Test.qvd"));
            Assert.False(reader.IsRead);
            var doc = reader.ReadHeader();
            Assert.True(reader.IsRead);
        }

        [Fact]
        public void HeaderDocument_ShouldThrowIfNotRead()
        {
            var reader = new HeaderReader(File.OpenRead("Resources\\HeaderReaderTest\\Test.qvd"));
            Assert.Throws<InvalidOperationException>(() => _ = reader.HeaderDocument);
        }

        [Fact]
        public void HeaderDocument_ShouldBeSameAsReturnedWhenReading()
        {
            var reader = new HeaderReader(File.OpenRead("Resources\\HeaderReaderTest\\Test.qvd"));
            var doc = reader.ReadHeader();
            Assert.Same(doc, reader.HeaderDocument);
        }
    }
}
