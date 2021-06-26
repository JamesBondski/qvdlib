namespace Bondski.QvdLib.Tests
{
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
    }
}
