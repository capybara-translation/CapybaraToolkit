using System.Linq;
using System.Xml.Linq;
using System;
using System.Collections.Generic;
using CapybaraToolkit.Xliff12;

namespace CapybaraToolkit.Capyxliff
{
    public class CapyGroup
    {
        public string Id { get; set; }

        public string OriginalId { get; set; }

        public CapyContextGroup ContextGroup { get; set; }

        public List<CapyTransUnit> TransUnits { get; set; } = new List<CapyTransUnit>();


        public static CapyGroup FromElement(XElement element)
        {
            var capyGroup = new CapyGroup
            {
                Id = (string)element.Attribute("id"),
                OriginalId = (string)element.Attribute(Capy.capy + "original-id"),
                ContextGroup = CapyContextGroup.FromElement(element.Element(Xlf12.contextGroup)),
                TransUnits = element.Elements(Xlf12.transUnit).Select(CapyTransUnit.FromElement).ToList(),
            };
            return capyGroup;
        }

        public XElement ToElement()
        {
            var root = new XElement(Xlf12.group);
            root.Add(new XAttribute("id", Id));
            root.Add(new XAttribute(Capy.capy + "original-id", OriginalId));
            if (ContextGroup != null)
            {
                root.Add(ContextGroup.ToElement());
            }
            root.Add(TransUnits.Select(x => x.ToElement()));
            return root;
        }

    }
}
