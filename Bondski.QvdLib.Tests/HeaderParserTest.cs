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

        private XDocument GetDoc(string xml)
        {
            return XDocument.Parse(xml);
        }

        private static string baseXml = @"
 <QvdTableHeader>
   <QvBuildNo>50622</QvBuildNo>
   <CreatorDoc>C:\Users\wuntv\Documents\Qlik\Sense\Apps\Test.qvf</CreatorDoc>
   <CreateUtcTime>2020-06-12 19:20:22</CreateUtcTime>
   <SourceCreateUtcTime></SourceCreateUtcTime>
   <SourceFileUtcTime></SourceFileUtcTime>
   <SourceFileSize>-1</SourceFileSize>
   <StaleUtcTime></StaleUtcTime>
   <TableName>Test</TableName>
   <Fields>
     <QvdFieldHeader>
       <FieldName>StringColumn</FieldName>
       <BitOffset>0</BitOffset>
       <BitWidth>1</BitWidth>
       <Bias>0</Bias>
       <NumberFormat>
         <Type>UNKNOWN</Type>
         <nDec>0</nDec>
         <UseThou>0</UseThou>
         <Fmt></Fmt>
         <Dec></Dec>
         <Thou></Thou>
       </NumberFormat>
       <NoOfSymbols>2</NoOfSymbols>
       <Offset>0</Offset>
       <Length>6</Length>
       <Comment></Comment>
       <Tags>
         <String>$ascii</String>
         <String>$text</String>
       </Tags>
     </QvdFieldHeader>
     <QvdFieldHeader>
       <FieldName>DualColumn</FieldName>
       <BitOffset>1</BitOffset>
       <BitWidth>7</BitWidth>
       <Bias>0</Bias>
       <NumberFormat>
         <Type>UNKNOWN</Type>
         <nDec>0</nDec>
         <UseThou>0</UseThou>
         <Fmt></Fmt>
         <Dec></Dec>
         <Thou></Thou>
       </NumberFormat>
       <NoOfSymbols>2</NoOfSymbols>
       <Offset>6</Offset>
       <Length>14</Length>
       <Comment></Comment>
       <Tags>
         <String>$numeric</String>
         <String>$integer</String>
       </Tags>
     </QvdFieldHeader>
   </Fields>
   <Compression></Compression>
   <RecordByteSize>1</RecordByteSize>
   <NoOfRecords>2</NoOfRecords>
   <Offset>20</Offset>
   <Length>2</Length>
   <Lineage></Lineage>
   <Comment></Comment>
   <EncryptionInfo></EncryptionInfo>
 </QvdTableHeader>
";

        [Fact]
        public void Fields_FieldName()
        {
            var parser = new HeaderParser(XDocument.Parse(baseXml));
            Assert.Equal("StringColumn", parser.Header.Fields[0].Name);
        }

        [Fact]
        public void Fields_ThrowsIfNoName()
        {
            string xml = baseXml.Replace("<FieldName>StringColumn</FieldName>", "");
            _ = Assert.Throws<InvalidHeaderException>(() => { HeaderParser header = new HeaderParser(GetDoc(xml)); });
        }

        [Fact]
        public void Fields_ThrowsIfInvalidValue()
        {
            string xml = baseXml.Replace("<BitOffset>0</BitOffset>", "<BitOffset>A</BitOffset>");
            _ = Assert.Throws<InvalidHeaderException>(() => { HeaderParser header = new HeaderParser(GetDoc(xml)); });
        }

        [Fact]
        public void Fields_ParsesNumericValue()
        {
            string xml = baseXml;
            HeaderParser parser = new HeaderParser(GetDoc(xml));
            Assert.Equal(0, parser.Header.Fields[0].BitOffset);
        }

        private record HeaderFieldTest(string PropertyName, object value);

        private static HeaderFieldTest[] HeaderFields = new HeaderFieldTest[]
        {
            new HeaderFieldTest("QvBuildNo", "50622"),
            new HeaderFieldTest("CreatorDoc", @"C:\Users\wuntv\Documents\Qlik\Sense\Apps\Test.qvf"),
            new HeaderFieldTest("CreateTime", DateTime.Parse("2020-06-12 19:20:22")),
            new HeaderFieldTest("SourceCreateTime", null),
        };

        [Fact]
        public void ParsesHeaderFields()
        {
            foreach(HeaderFieldTest test in HeaderFields)
            {
                string xml = baseXml;
                HeaderParser parser = new HeaderParser(GetDoc(xml));
                object actualValue = typeof(QvdHeader).GetProperty(test.PropertyName).GetValue(parser.Header);
                Assert.Equal(test.value, actualValue);
            }
        }

        [Fact]
        public void Fields_TagList()
        {
            string xml = baseXml;
            HeaderParser parser = new HeaderParser(GetDoc(xml));
            Assert.Equal(2, parser.Header.Fields[0].Tags.Length);
            Assert.Equal("$ascii", parser.Header.Fields[0].Tags[0]);
            Assert.Equal("$text", parser.Header.Fields[0].Tags[1]);
        }
    }
}
