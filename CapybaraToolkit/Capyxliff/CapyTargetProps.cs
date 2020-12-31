using System.Linq;
using System.Xml.Linq;
using System;
using System.Collections.Generic;

namespace CapybaraToolkit.Capyxliff
{
    public class CapyTargetProps
    {
        public List<CapyTag> Tags { get; set; } = new List<CapyTag>();

        public static CapyTargetProps FromElement(XElement element)
        {
            var capyTargetProps = new CapyTargetProps
            {
                Tags = element.Elements(Capy.tag).Select(CapyTag.FromElement).ToList()
            };
            return capyTargetProps;
        }

        public XElement ToElement()
        {
            var root = new XElement(Capy.targetProps);
            root.Add(Tags.Select(x => x.ToElement()));
            return root;
        }
    }
}
