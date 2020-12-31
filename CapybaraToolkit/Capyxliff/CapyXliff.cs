using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;
using CapybaraToolkit.Xliff12;

namespace CapybaraToolkit.Capyxliff
{
    public class CapyXliff
    {
        public string Version { get; set; } = "1.2";
        public string CapyVersion { get; set; } = "1.0";

        public List<CapyFile> Files { get; set; } = new List<CapyFile>();

        public static CapyXliff FromElement(XElement element)
        {
            var capyXliff = new CapyXliff
            {
                Version = (string)element.Attribute("version"),
                CapyVersion = (string)element.Attribute(Capy.capy + "version"),
                Files = element.Elements(Xlf12.file).Select(CapyFile.FromElement).ToList()
            };
            return capyXliff;
        }

        public static CapyXliff Load(string inputFile)
        {
            var root = XElement.Load(inputFile, LoadOptions.PreserveWhitespace);
            return FromElement(root);
        }

        public XElement ToElement()
        {
            var root = new XElement(Xlf12.xliff,
                new XAttribute("xmlns", Xlf12.xlf.NamespaceName),
                new XAttribute(XNamespace.Xmlns + "capy", Capy.capy.NamespaceName));
            root.Add(new XAttribute("version", Version));
            root.Add(new XAttribute(Capy.capy + "version", CapyVersion));
            root.Add(Files.Select(x => x.ToElement()));
            return root;
        }

        public void Save(string destination)
        {
            var doc = new XDocument(new XDeclaration("1.0", "utf-8", null));
            doc.Add(ToElement());
            doc.Save(destination);
        }
    }
}
