using System;

namespace CapybaraToolkit.Filter.Run
{
    internal class TextRun : IRunMarker
    {
        public TextRun(string value)
        {
            Value = value;
        }
        public string SimpleValue
        {
            get => Value;
        }

        public string Value { get; }

        public override string ToString()
        {
            return Value;
        }

    }
}
