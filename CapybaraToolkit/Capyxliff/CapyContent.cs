using System.Xml.Linq;
using System;

namespace CapybaraToolkit.Capyxliff
{
    public class CapyContent
    {
        public string Value { get; set; } = "";

        public override string ToString()
        {
            return Value;
        }

        public static CapyContent FromElement(XElement element)
        {
            return new CapyContent
            {
                Value = element.Value
            };
        }

        public XElement ToElement()
        {
            return new XElement(Capy.content, Value);
        }
    }
}
