using System;
using System.Xml.Linq;
using Xunit;

namespace Bondski.QvdLib.Tests
{
    public class HeaderParserTest
    {
        private string invalidDocXml = @"<TableHeader></TableHeader>";
        private XDocument invalidDoc => XDocument.Parse(this.invalidDocXml);

        [Fact]
        public void Constructor_XDocNotQvdTableHeader()
        {
            _ = Assert.Throws<InvalidHeaderException>(() => { HeaderParser header = new HeaderParser(this.invalidDoc); });
        }

        [Fact]
        public void Constructor_XElementNotQvdTableHeader()
        {
            _ = Assert.Throws<InvalidHeaderException>(() => { HeaderParser header = new HeaderParser(this.invalidDoc.Root); });
        }

        private string docWithoutFieldsXml = @"<QvdTableHeader></QvdTableHeader>";
        private XDocument docWithoutFields => XDocument.Parse(this.docWithoutFieldsXml);

        [Fact]
        public void Constructor_NoFieldsInQvdTableHeader()
        {
            _ = Assert.Throws<InvalidHeaderException>(() => { HeaderParser header = new HeaderParser(this.docWithoutFields.Root); } );
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
            var parser = new HeaderParser(this.docFieldName);
            Assert.Equal("Test", parser.Header.Fields[0].name);
        }

        [Fact]
        public void Fields_ThrowsIfNoName()
        {
            string xml = @"<QvdTableHeader><Fields><QvdFieldHeader></QvdFieldHeader></Fields></QvdTableHeader>";
            _ = Assert.Throws<InvalidHeaderException>(() => { HeaderParser header = new HeaderParser(GetDoc(xml)); });
        }

        [Fact]
        public void Fields_ThrowsIfInvalidValue()
        {
            string xml = @"<QvdTableHeader><Fields><QvdFieldHeader><FieldName>Test</FieldName><BitOffset>A</BitOffset></QvdFieldHeader></Fields></QvdTableHeader>";
            _ = Assert.Throws<InvalidHeaderException>(() => { HeaderParser header = new HeaderParser(GetDoc(xml)); });
        }

        [Fact]
        public void Fields_ParsesNumericValue()
        {
            string xml = @"<QvdTableHeader><Fields><QvdFieldHeader><FieldName>Test</FieldName><BitOffset>4</BitOffset></QvdFieldHeader></Fields></QvdTableHeader>";
            HeaderParser parser = new HeaderParser(GetDoc(xml));
            Assert.Equal(4, parser.Header.Fields[0].bitOffset);
        }

        private record HeaderFieldTest(string XmlTagName, string content, string PropertyName, object value);

        private static HeaderFieldTest[] HeaderFields = new HeaderFieldTest[]
        {
            new HeaderFieldTest("QvBuildNo", "1", "QvBuildNo", "1"),
            new HeaderFieldTest("CreatorDoc", "Test.qvw", "CreatorDoc", "Test.qvw"),
            new HeaderFieldTest("CreateUtcTime", "2020-06-12 19:20:22", "CreateTime", DateTime.Parse("2020-06-12 19:20:22")),
            new HeaderFieldTest("SourceCreateUtcTime", "", "SourceCreateTime", null),
        };

        [Fact]
        public void ParsesHeaderFields()
        {
            foreach(HeaderFieldTest test in HeaderFields)
            {
                string xml = String.Format(
                    @"<QvdTableHeader><{0}>{1}</{0}><Fields><QvdFieldHeader><FieldName>Test</FieldName><BitOffset>4</BitOffset></QvdFieldHeader></Fields></QvdTableHeader>",
                    test.XmlTagName, 
                    test.content
                    );
                HeaderParser parser = new HeaderParser(GetDoc(xml));
                object actualValue = typeof(QvdHeader).GetProperty(test.PropertyName).GetValue(parser.Header);
                Assert.Equal(test.value, actualValue);
            }
        }
    }
}
