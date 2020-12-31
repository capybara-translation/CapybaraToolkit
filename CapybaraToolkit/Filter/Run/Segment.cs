using System;
using System.Collections.Generic;
using System.Linq;
using CapybaraToolkit.Capyxliff;

namespace CapybaraToolkit.Filter.Run
{
    internal class Segment
    {
        public Segment()
        {
            Runs = new List<IRunMarker>();
        }

        public List<IRunMarker> Runs { get; set; }

        public List<CapyTag> GetCapyTags()
        {
            return Runs.OfType<TagRun>().Where(x => x.TagType != TagType.Close).Select(x => new CapyTag
            {
                Id = x.Id,
                Content = new CapyContent
                {
                    Value = x.Value
                }
            }).ToList();
        }

        public override string ToString()
        {
            return string.Join("", Runs.Select(x => x.SimpleValue));
        }

        public TagRun GetPairedTag(TagRun tag)
        {
            if (tag.TagType == TagType.Empty)
            {
                return null;
            }
            var tagType = tag.TagType == TagType.Open ? TagType.Close : TagType.Open;

            return Runs.OfType<TagRun>().Where(x => x.TagType == tagType).FirstOrDefault(x => x.Id == tag.Id);
        }

    }
}
