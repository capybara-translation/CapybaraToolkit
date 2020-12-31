using System.Xml.Linq;
using System;

namespace CapybaraToolkit.Capyxliff
{
    public class CapyTag
    {
        public string Id { get; set; }

        public CapyContent Content { get; set; }

        public override string ToString()
        {
            return $"{{{Id}}} {Content}";
        }

        public static CapyTag FromElement(XElement element)
        {
            var capyTag = new CapyTag
            {
                Id = (string)element.Attribute("id"),
                Content = CapyContent.FromElement(element.Element(Capy.content))
            };
            return capyTag;
        }

        public XElement ToElement()
        {
            var root = new XElement(Capy.tag);
            root.Add(new XAttribute("id", Id));
            root.Add(Content.ToElement());
            return root;
        }
    }
}
