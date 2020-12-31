using System;
using System.IO;
using System.Xml.Linq;
using CapybaraToolkit.Xliff12;

namespace CapybaraToolkitTests
{
    public static class TestUtil
    {
        public static string GetTestDataFolder()
        {
            return new DirectoryInfo(Path.Combine("../../../", "data")).FullName;
        }

        public static string LoadContentButIgnoreIds(string file)
        {
            var doc = XDocument.Load(file, LoadOptions.PreserveWhitespace).Root;
            foreach (var transUnit in doc.Descendants(Xlf12.transUnit))
            {
                var attr = transUnit.Attribute("id");
                attr.Remove();
            }
            return doc.ToString();
        }
    }
}
