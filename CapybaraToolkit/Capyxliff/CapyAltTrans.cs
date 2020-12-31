using System.Linq;
using System.ComponentModel.Design.Serialization;
using System.Xml.Linq;
using System;
using CapybaraToolkit.Xliff12;

namespace CapybaraToolkit.Capyxliff
{
    public class CapyAltTrans
    {
        public string Origin { get; set; }
        public CapyTarget Target { get; set; }

        public static CapyAltTrans FromElement(XElement element)
        {
            return new CapyAltTrans
            {
                Origin = (string)element.Attribute("origin"),
                Target = CapyTarget.FromElement(element.Element(Xlf12.target))
            };
        }

        public XElement ToElement()
        {
            var root = new XElement(Xlf12.altTrans);
            root.Add(new XAttribute("origin", Origin));
            root.Add(Target.ToElement());
            return root;
        }
    }
}
