using System.Linq;
using System.Xml.Linq;
using System;
using System.Collections.Generic;
using CapybaraToolkit.Xliff12;

namespace CapybaraToolkit.Capyxliff
{
    public class CapyBody
    {
        public List<CapyGroup> Groups { get; set; } = new List<CapyGroup>();

        public static CapyBody FromElement(XElement element)
        {
            var capyBody = new CapyBody
            {
                Groups = element.Elements(Xlf12.group).Select(CapyGroup.FromElement).ToList()
            };
            return capyBody;
        }

        public XElement ToElement()
        {
            var root = new XElement(Xlf12.body);
            root.Add(Groups.Select(x => x.ToElement()));
            return root;
        }

    }
}
