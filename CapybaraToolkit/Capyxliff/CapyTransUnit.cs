using System.Linq;
using System.Xml.Linq;
using System;
using System.Collections.Generic;
using CapybaraToolkit.Xliff12;

namespace CapybaraToolkit.Capyxliff
{
    public class CapyTransUnit
    {
        public string Id { get; set; }
        public string OriginalId { get; set; }
        public bool Translate { get; set; } = true;

        public CapySource Source { get; set; }
        public CapyTarget Target { get; set; }

        public CapySourceProps SourceProps { get; set; }

        public CapyTargetProps TargetProps { get; set; }

        public List<CapyAltTrans> AltTranslations { get; set; } = new List<CapyAltTrans>();

        public override string ToString()
        {
            return $"{Source} | ${Target}";
        }

        public static CapyTransUnit FromElement(XElement element)
        {
            var capyTransUnit = new CapyTransUnit
            {
                Id = (string)element.Attribute("id"),
                OriginalId = (string)element.Attribute(Capy.capy + "original-id"),
                Translate = ((string)element.Attribute("translate")) == "yes",
                Source = CapySource.FromElement(element.Element(Xlf12.source)),
                Target = CapyTarget.FromElement(element.Element(Xlf12.target)),
                SourceProps = CapySourceProps.FromElement(element.Element(Capy.sourceProps)),
                TargetProps = CapyTargetProps.FromElement(element.Element(Capy.targetProps)),
                AltTranslations = element.Elements(Xlf12.altTrans).Select(CapyAltTrans.FromElement).ToList(),
            };
            return capyTransUnit;
        }

        public XElement ToElement()
        {
            var root = new XElement(Xlf12.transUnit);
            root.Add(new XAttribute("id", Id));
            root.Add(new XAttribute(Capy.capy + "original-id", OriginalId));
            root.Add(new XAttribute("translate", Translate ? "yes" : "no"));
            root.Add(Source.ToElement());
            root.Add(Target.ToElement());
            root.Add(AltTranslations.Select(x => x.ToElement()));
            root.Add(SourceProps.ToElement());
            root.Add(TargetProps.ToElement());
            return root;
        }
    }
}
