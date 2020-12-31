using System.Linq;
using System.Xml.Linq;
using System;
using System.Collections.Generic;

namespace CapybaraToolkit.Capyxliff
{
    public class CapySourceProps
    {
        public List<CapyTag> Tags { get; set; } = new List<CapyTag>();

        public static CapySourceProps FromElement(XElement element)
        {
            var capySourceProps = new CapySourceProps
            {
                Tags = element.Elements(Capy.tag).Select(CapyTag.FromElement).ToList()
            };
            return capySourceProps;
        }

        public XElement ToElement()
        {
            var root = new XElement(Capy.sourceProps);
            root.Add(Tags.Select(x => x.ToElement()));
            return root;
        }
    }
}
