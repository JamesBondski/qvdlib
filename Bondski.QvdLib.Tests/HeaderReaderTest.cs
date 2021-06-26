using System;
using System.IO;
using System.Reflection;
using Xunit;

namespace Bondski.QvdLib.Tests
{
    public class HeaderReaderTest
    {
        private static string TestFilePath => new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName + "/Resources/HeaderReaderTest/Test.qvd";

        [Fact]
        public void NormalRead()
        {
            var reader = new HeaderReader(File.OpenRead(TestFilePath));
            var doc = reader.ReadHeader();
            Assert.NotNull(doc);
            Assert.Equal("QvdTableHeader", doc.Root.Name);
        }

        [Fact]
        public void IsRead()
        {
            var reader = new HeaderReader(File.OpenRead(TestFilePath));
            Assert.False(reader.IsRead);
            var doc = reader.ReadHeader();
            Assert.True(reader.IsRead);
        }

        [Fact]
        public void HeaderDocument_ShouldThrowIfNotRead()
        {
            var reader = new HeaderReader(File.OpenRead(TestFilePath));
            Assert.Throws<InvalidOperationException>(() => _ = reader.HeaderDocument);
        }

        [Fact]
        public void HeaderDocument_ShouldBeSameAsReturnedWhenReading()
        {
            var reader = new HeaderReader(File.OpenRead(TestFilePath));
            var doc = reader.ReadHeader();
            Assert.Same(doc, reader.HeaderDocument);
        }
    }
}
