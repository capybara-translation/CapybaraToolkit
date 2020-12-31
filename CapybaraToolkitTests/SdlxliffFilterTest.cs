using System.ComponentModel.Design.Serialization;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using CapybaraToolkit.Filter;
using Xunit;
using CapybaraToolkit.Xliff12;
using System.Xml;
using Xunit.Abstractions;

namespace CapybaraToolkitTests
{
    public class SdlxliffFilterTest
    {
        private readonly ITestOutputHelper _output;
        public SdlxliffFilterTest(ITestOutputHelper output)
        {
            _output = output;
        }


        [Fact]
        public void TestCreateTransUnit()
        {
            var filter = new SdlxliffFilter();
            var doc1 = @"<?xml version=""1.0"" encoding=""utf-8""?>
            <xliff xmlns:sdl=""http://sdl.com/FileTypes/SdlXliff/1.0"" xmlns=""urn:oasis:names:tc:xliff:document:1.2"" version=""1.2"" sdl:version=""1.0"">
            <mrk mtype=""seg"" mid=""2""><g id=""5"">aaa</g> bbb</mrk>
            <mrk mtype=""seg"" mid=""2""><g id=""5"">aaa</g> <g id=""Bold"">bbb</g> ccc</mrk>
            </xliff>
            ";
            var options = LoadOptions.PreserveWhitespace;
            var xlf1 = XDocument.Parse(doc1, options).Root;
            var srcMrk1 = xlf1.Elements().ElementAt(0);
            var tgtMrk1 = xlf1.Elements().ElementAt(1);
            var actual1 = filter.CreateTransUnit(srcMrk1, tgtMrk1);
            Assert.Equal("{1>aaa<1} bbb", actual1.Source.Text);
            Assert.Equal("{1>aaa<1} {2>bbb<2} ccc", actual1.Target.Text);

            var doc2 = @"<?xml version=""1.0"" encoding=""utf-8""?>
            <xliff xmlns:sdl=""http://sdl.com/FileTypes/SdlXliff/1.0"" xmlns=""urn:oasis:names:tc:xliff:document:1.2"" version=""1.2"" sdl:version=""1.0"">
            <mrk mtype=""seg"" mid=""2"">bbb<g ts=""xxx"" id=""5"">aaa</g> bbb</mrk>
            <mrk mtype=""seg"" mid=""2""><g id=""5"" ts=""xxx"">aaa</g> <g id=""Bold"">bbb</g> ccc</mrk>
            </xliff>
            ";
            var xlf2 = XDocument.Parse(doc2, options).Root;
            var srcMrk2 = xlf2.Elements().ElementAt(0);
            var tgtMrk2 = xlf2.Elements().ElementAt(1);
            var actual2 = filter.CreateTransUnit(srcMrk2, tgtMrk2);
            Assert.Equal("bbb{1>aaa<1} bbb", actual2.Source.Text);
            Assert.Equal("{1>aaa<1} {2>bbb<2} ccc", actual2.Target.Text);


            var doc3 = @"<?xml version=""1.0"" encoding=""utf-8""?>
            <xliff xmlns:sdl=""http://sdl.com/FileTypes/SdlXliff/1.0"" xmlns=""urn:oasis:names:tc:xliff:document:1.2"" version=""1.2"" sdl:version=""1.0"">
            <mrk mtype=""seg"" mid=""2"">bbb<g ts=""xxx"" id=""5"">aaa</g> bbb</mrk>
            <mrk mtype=""seg"" mid=""2""><g id=""5"" ts=""xxx"">aaa</g> <g id=""Bold"">bbb</g> <g id=""Bold"">ccc</g></mrk>
            </xliff>
            ";
            var xlf3 = XDocument.Parse(doc3, options).Root;
            var srcMrk3 = xlf3.Elements().ElementAt(0);
            var tgtMrk3 = xlf3.Elements().ElementAt(1);
            var actual3 = filter.CreateTransUnit(srcMrk3, tgtMrk3);
            Assert.Equal("bbb{1>aaa<1} bbb", actual3.Source.Text);
            Assert.Equal("{1>aaa<1} {2>bbb<2} {3>ccc<3}", actual3.Target.Text);

            var doc4 = @"<?xml version=""1.0"" encoding=""utf-8""?>
            <xliff xmlns:sdl=""http://sdl.com/FileTypes/SdlXliff/1.0"" xmlns=""urn:oasis:names:tc:xliff:document:1.2"" version=""1.2"" sdl:version=""1.0"">
            <mrk mtype=""seg"" mid=""2"">aaa</mrk>
            <mrk mtype=""seg"" mid=""2""><g id=""5"" ts=""xxx"">aaa</g> <g id=""Bold"">bbb</g> <g id=""Bold"">ccc</g></mrk>
            </xliff>
            ";
            var xlf4 = XDocument.Parse(doc4, options).Root;
            var srcMrk4 = xlf4.Elements().ElementAt(0);
            var tgtMrk4 = xlf4.Elements().ElementAt(1);
            var actual4 = filter.CreateTransUnit(srcMrk4, tgtMrk4);
            Assert.Equal("aaa", actual4.Source.Text);
            Assert.Equal("{1>aaa<1} {2>bbb<2} {3>ccc<3}", actual4.Target.Text);

            var doc5 = @"<?xml version=""1.0"" encoding=""utf-8""?>
            <xliff xmlns:sdl=""http://sdl.com/FileTypes/SdlXliff/1.0"" xmlns=""urn:oasis:names:tc:xliff:document:1.2"" version=""1.2"" sdl:version=""1.0"">
            <mrk mtype=""seg"" mid=""2""><mrk mtype=""x-sdl-location"" mid=""f88807f7-10d7-4b08-8868-09829b78bb0c""/></mrk>
            <mrk mtype=""seg"" mid=""2""><mrk mtype=""x-sdl-location"" mid=""f88807f7-10d7-4b08-8868-09829b78bb0c""/></mrk>
            </xliff>
            ";
            var xlf5 = XDocument.Parse(doc5, options).Root;
            var srcMrk5 = xlf5.Elements().ElementAt(0);
            var tgtMrk5 = xlf5.Elements().ElementAt(1);
            var actual5 = filter.CreateTransUnit(srcMrk5, tgtMrk5);
            Assert.Equal("{1}", actual5.Source.Text);
            Assert.Equal("{1}", actual5.Target.Text);

            var doc6 = @"<?xml version=""1.0"" encoding=""utf-8""?>
            <xliff xmlns:sdl=""http://sdl.com/FileTypes/SdlXliff/1.0"" xmlns=""urn:oasis:names:tc:xliff:document:1.2"" version=""1.2"" sdl:version=""1.0"">
            <mrk mtype=""seg"" mid=""2""><mrk mtype=""x-sdl-location"" mid=""f88807f7-10d7-4b08-8868-09829b78bb0c""/></mrk>
            <mrk mtype=""seg"" mid=""2""><mrk mtype=""x-sdl-location"" mid=""f88807f7-10d7-4b08-8868-09829b78bb0c""/><mrk mtype=""x-sdl-comment"" sdl:cid=""229d3377-d1c8-4419-a506-4f7ad7bf9d60""><mrk mtype=""x-sdl-comment"" sdl:cid=""7990a264-9bb4-4c5e-8906-8d234f8497d8""><mrk mtype=""x-sdl-comment"" sdl:cid=""e5011b5a-1970-473b-b94d-303dbad8058b"">With videos</mrk>, you can clearly articulate</mrk></mrk> what you want to convey.</mrk>
            </xliff>
            ";
            var xlf6 = XDocument.Parse(doc6, options).Root;
            var srcMrk6 = xlf6.Elements().ElementAt(0);
            var tgtMrk6 = xlf6.Elements().ElementAt(1);
            var actual6 = filter.CreateTransUnit(srcMrk6, tgtMrk6);
            Assert.Equal("{1}", actual6.Source.Text);
            Assert.Equal("{1}{2>{3>{4>With videos<4}, you can clearly articulate<3}<2} what you want to convey.", actual6.Target.Text);


            var doc7 = @"<?xml version=""1.0"" encoding=""utf-8""?>
            <xliff xmlns:sdl=""http://sdl.com/FileTypes/SdlXliff/1.0"" xmlns=""urn:oasis:names:tc:xliff:document:1.2"" version=""1.2"" sdl:version=""1.0"">
            <mrk mtype=""seg"" mid=""2""><g ts=""xxx"" id=""5"">aaa</g>bbb<g ts=""xxx"" id=""5"">ccc</g><x/> <x/></mrk>
            <mrk mtype=""seg"" mid=""2""><g id=""5"" ts=""xxx"">aaa</g> <g id=""Bold"">bbb</g> <g id=""Bold"">ccc</g></mrk>
            </xliff>
            ";
            var xlf7 = XDocument.Parse(doc7, options).Root;
            var srcMrk7 = xlf7.Elements().ElementAt(0);
            var tgtMrk7 = xlf7.Elements().ElementAt(1);
            var actual7 = filter.CreateTransUnit(srcMrk7, tgtMrk7);
            Assert.Equal("{1>aaa<1}bbb{2>ccc<2}{3} {4}", actual7.Source.Text);
            Assert.Equal("{1>aaa<1} {5>bbb<5} {6>ccc<6}", actual7.Target.Text);

            var doc8 = @"<?xml version=""1.0"" encoding=""utf-8""?>
            <xliff xmlns:sdl=""http://sdl.com/FileTypes/SdlXliff/1.0"" xmlns=""urn:oasis:names:tc:xliff:document:1.2"" version=""1.2"" sdl:version=""1.0"">
            <mrk mtype=""seg"" mid=""2""></mrk>
            <mrk mtype=""seg"" mid=""2""></mrk>
            </xliff>
            ";
            var xlf8 = XDocument.Parse(doc8, options).Root;
            var srcMrk8 = xlf8.Elements().ElementAt(0);
            var tgtMrk8 = xlf8.Elements().ElementAt(1);
            var actual8 = filter.CreateTransUnit(srcMrk8, tgtMrk8);
            Assert.Equal("", actual8.Source.Text);
            Assert.Equal("", actual8.Target.Text);

            var doc9 = @"<?xml version=""1.0"" encoding=""utf-8""?>
            <xliff xmlns:sdl=""http://sdl.com/FileTypes/SdlXliff/1.0"" xmlns=""urn:oasis:names:tc:xliff:document:1.2"" version=""1.2"" sdl:version=""1.0"">
            <mrk mtype=""seg"" mid=""2""></mrk>
            <mrk mtype=""seg"" mid=""2""><x id=""1"" /><g id=""1"">aaa</g><g id=""1"">bbb</g></mrk>
            </xliff>
            ";
            var xlf9 = XDocument.Parse(doc9, options).Root;
            var srcMrk9 = xlf9.Elements().ElementAt(0);
            var tgtMrk9 = xlf9.Elements().ElementAt(1);
            var actual9 = filter.CreateTransUnit(srcMrk9, tgtMrk9);
            Assert.Equal("", actual9.Source.Text);
            Assert.Equal("{1}{2>aaa<2}{3>bbb<3}", actual9.Target.Text);
        }


        [Theory]
        [InlineData("T1")]
        public void TestParse(string tid)
        {
            var dataFolder = TestUtil.GetTestDataFolder();
            var sourceFile = Path.Combine(dataFolder, tid + ".input.sdlxliff");
            var filter = new SdlxliffFilter();
            var capyxliff = filter.Parse(sourceFile);
            var fileActual = Path.Combine(dataFolder, tid + ".output.capyxliff");
            capyxliff.Save(fileActual);

            var contentActual = TestUtil.LoadContentButIgnoreIds(fileActual);
            var fileExpected = Path.Combine(dataFolder, tid + ".expected.capyxliff");
            var contentExpected = TestUtil.LoadContentButIgnoreIds(fileExpected);
            Assert.Equal(contentExpected, contentActual);
        }

        [Fact]
        public void TestRestoreElementFromString1()
        {
            var str = @"<mrk mtype=""x-sdl-comment"" sdl:cid=""a3718ebe-6490-49fc-b832-a4bb1664d048""/>";
            XmlNamespaceManager mngr = new XmlNamespaceManager(new NameTable());
            mngr.AddNamespace("", "urn:oasis:names:tc:xliff:document:1.2");
            mngr.AddNamespace("sdl", "http://sdl.com/FileTypes/SdlXliff/1.0");
            XmlParserContext parserContext = new XmlParserContext(null, mngr, null, XmlSpace.Preserve);
            XmlReader reader = XmlReader.Create(new StringReader(str), null, parserContext);
            var mrk = XElement.Load(reader, LoadOptions.PreserveWhitespace);

            var doc = @"<?xml version=""1.0"" encoding=""utf-8""?>
            <xliff xmlns:sdl=""http://sdl.com/FileTypes/SdlXliff/1.0"" xmlns=""urn:oasis:names:tc:xliff:document:1.2"" version=""1.2"" sdl:version=""1.0"">
            <mrk mtype=""seg"" mid=""2""></mrk>
            </xliff>
            ";
            var xlf = XDocument.Parse(doc, LoadOptions.PreserveWhitespace);
            var root = xlf.Root;
            root.Element(Xlf12.mrk).AddFirst("abc", mrk, "def");
            var dataFolder = TestUtil.GetTestDataFolder();
            var dest = Path.Combine(dataFolder, "T99-1.output.sdlxliff");
            root.Save(dest, SaveOptions.OmitDuplicateNamespaces);
        }

        [Fact]
        public void TestRestoreElementFromString2()
        {
            var str = @"<mrk xmlns:sdl=""http://sdl.com/FileTypes/SdlXliff/1.0"" mtype=""x-sdl-comment"" sdl:cid=""a3718ebe-6490-49fc-b832-a4bb1664d048""/>";
            XmlNamespaceManager mngr = new XmlNamespaceManager(new NameTable());
            mngr.AddNamespace("", "urn:oasis:names:tc:xliff:document:1.2");
            mngr.AddNamespace("sdl", "http://sdl.com/FileTypes/SdlXliff/1.0");
            XmlParserContext parserContext = new XmlParserContext(null, mngr, null, XmlSpace.Preserve);
            XmlReader reader = XmlReader.Create(new StringReader(str), null, parserContext);
            var mrk = XElement.Load(reader, LoadOptions.PreserveWhitespace);

            var doc = @"<?xml version=""1.0"" encoding=""utf-8""?>
            <xliff xmlns:sdl=""http://sdl.com/FileTypes/SdlXliff/1.0"" xmlns=""urn:oasis:names:tc:xliff:document:1.2"" version=""1.2"" sdl:version=""1.0"">
            <mrk mtype=""seg"" mid=""2""></mrk>
            </xliff>
            ";
            var xlf = XDocument.Parse(doc, LoadOptions.PreserveWhitespace);
            var root = xlf.Root;
            root.Element(Xlf12.mrk).AddFirst("abc", mrk, "def");
            var dataFolder = TestUtil.GetTestDataFolder();
            var dest = Path.Combine(dataFolder, "T99-2.output.sdlxliff");
            root.Save(dest, SaveOptions.OmitDuplicateNamespaces);
        }
    }
}
