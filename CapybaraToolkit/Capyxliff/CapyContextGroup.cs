using System.Linq;
using System.Xml.Linq;
using System;
using System.Collections.Generic;
using CapybaraToolkit.Xliff12;

namespace CapybaraToolkit.Capyxliff
{
    public class CapyContextGroup
    {
        public List<CapyContext> Contexts { get; set; } = new List<CapyContext>();

        public static CapyContextGroup FromElement(XElement element)
        {
            if (element == null)
            {
                return null;
            }
            return new CapyContextGroup
            {
                Contexts = element.Elements(Xlf12.context).Select(CapyContext.FromElement).ToList()
            };
        }

        public XElement ToElement()
        {
            var root = new XElement(Xlf12.contextGroup);
            root.Add(Contexts.Select(x => x.ToElement()));
            return root;
        }

    }
}
