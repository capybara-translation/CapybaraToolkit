using System.Xml.Linq;
using System;
using CapybaraToolkit.Xliff12;

namespace CapybaraToolkit.Capyxliff
{
    public class CapyFile
    {
        public string Original { get; set; }
        public string SourceLanguage { get; set; }
        public string TargetLanguage { get; set; }
        public string DataType { get; set; }

        public CapyBody Body { get; set; }

        public static CapyFile FromElement(XElement element)
        {
            var capyFile = new CapyFile
            {
                Original = (string)element.Attribute("original"),
                SourceLanguage = (string)element.Attribute("source-language"),
                TargetLanguage = (string)element.Attribute("target-language"),
                DataType = (string)element.Attribute("datatype"),
                Body = CapyBody.FromElement(element.Element(Xlf12.body))
            };

            return capyFile;
        }

        public XElement ToElement()
        {
            var root = new XElement(Xlf12.file);
            root.Add(new XAttribute("original", Original));
            root.Add(new XAttribute("source-language", SourceLanguage));
            root.Add(new XAttribute("target-language", TargetLanguage));
            root.Add(new XAttribute("datatype", DataType));
            root.Add(Body.ToElement());
            return root;
        }

    }
}
