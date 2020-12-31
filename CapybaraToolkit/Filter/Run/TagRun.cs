using System;
using System.Collections.Generic;
using System.Linq;

namespace CapybaraToolkit.Filter.Run
{
    internal class TagRun : IRunMarker
    {
        public TagRun(string id, string name, TagType tagType, Dictionary<string, string> attributes)
        {
            Id = id;
            Name = name;
            TagType = tagType;
            Attributes = attributes;
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public TagType TagType { get; set; }

        public TagRun PairedTag { get; set; }

        public bool Shared { get; set; }

        public Dictionary<string, string> Attributes { get; set; }

        public string SimpleValue
        {
            get
            {
                if (TagType == TagType.Open)
                {
                    return $"{{{Id}>";
                }
                if (TagType == TagType.Close)
                {
                    return $"<{Id}}}";
                }
                return $"{{{Id}}}";
            }
        }

        public string Value
        {
            get
            {
                if (TagType == TagType.Close)
                {
                    return $"</{Name}>";
                }

                var attrString = string.Join(" ", Attributes.OrderBy(selector => selector.Key).Select(selector => $"{selector.Key}=\"{selector.Value}\""));
                var content = $"<{Name} {attrString}/>";
                return content;
            }
        }

        public override string ToString()
        {
            return Value;
        }
    }
}
