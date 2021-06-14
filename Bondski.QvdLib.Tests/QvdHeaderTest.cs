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

        private string docWithoutFieldsXml = @"<QvdTableHeader></QvdTableHeader>";
        private XDocument docWithoutFields => XDocument.Parse(this.docWithoutFieldsXml);

        [Fact]
        public void Constructor_NoFieldsInQvdTableHeader()
        {
            _ = Assert.Throws<InvalidHeaderException>(() => { QvdHeader header = new QvdHeader(this.docWithoutFields.Root); } );
        }

        private string fieldNameXml = @"<QvdTableHeader><Fields><QvdFieldHeader><FieldName>Test</FieldName></QvdFieldHeader></Fields></QvdTableHeader>";
        private XDocument docFieldName => XDocument.Parse(this.fieldNameXml);

        private XDocument GetDoc(string xml)
        {
            return XDocument.Parse(xml);
        }

        [Fact]
        public void Fields_FieldName()
        {
            var header = new QvdHeader(this.docFieldName);
            Assert.Equal("Test", header.Fields[0].name);
        }

        [Fact]
        public void Fields_ThrowsIfNoName()
        {
            string xml = @"<QvdTableHeader><Fields><QvdFieldHeader></QvdFieldHeader></Fields></QvdTableHeader>";
            _ = Assert.Throws<InvalidHeaderException>(() => { QvdHeader header = new QvdHeader(GetDoc(xml)); });
        }

        [Fact]
        public void Fields_ThrowsIfInvalidValue()
        {
            string xml = @"<QvdTableHeader><Fields><QvdFieldHeader><FieldName>Test</FieldName><BitOffset>A</BitOffset></QvdFieldHeader></Fields></QvdTableHeader>";
            _ = Assert.Throws<InvalidHeaderException>(() => { QvdHeader header = new QvdHeader(GetDoc(xml)); });
        }

        [Fact]
        public void Fields_ParsesNumericValue()
        {
            string xml = @"<QvdTableHeader><Fields><QvdFieldHeader><FieldName>Test</FieldName><BitOffset>4</BitOffset></QvdFieldHeader></Fields></QvdTableHeader>";
            QvdHeader header = new QvdHeader(GetDoc(xml));
            Assert.Equal(4, header.Fields[0].bitOffset);
        }
    }
}
