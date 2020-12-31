using System.Xml.Linq;
using System;
using CapybaraToolkit.Xliff12;

namespace CapybaraToolkit.Capyxliff
{
    public class CapyContext
    {
        public string ContextType { get; set; }
        public string Value { get; set; } = "";

        public static CapyContext FromElement(XElement element)
        {
            return new CapyContext
            {
                ContextType = (string)element.Attribute("context-type"),
                Value = element.Value
            };
        }

        public XElement ToElement()
        {
            var root = new XElement(Xlf12.context);
            root.Add(new XAttribute("context-type", ContextType));
            root.SetValue(Value);
            return root;
        }

    }
}
