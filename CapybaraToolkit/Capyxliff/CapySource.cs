using System.ComponentModel.Design.Serialization;
using System.Xml.Linq;
using System;
using CapybaraToolkit.Xliff12;

namespace CapybaraToolkit.Capyxliff
{
    public class CapySource
    {
        public string Text { get; set; } = "";

        public override string ToString()
        {
            return Text;
        }

        public static CapySource FromElement(XElement element)
        {
            var capySource = new CapySource
            {
                Text = element.Value
            };
            return capySource;
        }

        public XElement ToElement()
        {
            var root = new XElement(Xlf12.source, Text);
            return root;
        }
    }
}
