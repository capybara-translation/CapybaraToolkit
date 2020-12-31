using System.Xml.Linq;
using System;
using CapybaraToolkit.Xliff12;

namespace CapybaraToolkit.Capyxliff
{
    public class CapyTarget
    {
        public string Text { get; set; } = "";

        public State State { get; set; } = State.None;

        public override string ToString()
        {
            return Text;
        }

        public static CapyTarget FromElement(XElement element)
        {
            var capyTarget = new CapyTarget
            {
                Text = element.Value,
                State = ((string)element.Attribute("state")).StringToState()
            };
            return capyTarget;
        }

        public XElement ToElement()
        {
            var root = new XElement(Xlf12.target, Text);
            if (State != State.None)
            {
                root.Add(new XAttribute("state", State.StateToString()));
            }
            return root;

        }
    }
}
