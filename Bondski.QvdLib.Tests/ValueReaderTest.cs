using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Bondski.QvdLib.Tests
{
    public class ValueReaderTest
    {
        private static string TestFilePath => new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName + "/Resources/HeaderReaderTest/Test.qvd";

        [Fact]
        public void ReadTest()
        {
            QvdReader reader = new QvdReader(TestFilePath);

        }
    }
}
