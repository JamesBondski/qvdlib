using System;
using System.Xml.Linq;
using Xunit;

namespace Bondski.QvdLib.Tests
{
    public class QvdHeaderTest
    {
        private string invalidDocXml = @"<TableHeader></TableHeader>";
        private XDocument invalidDoc => XDocument.Parse(this.invalidDocXml);

        [Fact]
        public void Constructor_XDocNotQvdTableHeader()
        {
            _ = Assert.Throws<InvalidHeaderException>(() => { QvdHeader header = new QvdHeader(this.invalidDoc); });
        }

        [Fact]
        public void Constructor_XElementNotQvdTableHeader()
        {
            _ = Assert.Throws<InvalidHeaderException>(() => { QvdHeader header = new QvdHeader(this.invalidDoc.Root); });
        }
    }
}
