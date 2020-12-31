using System.Runtime.CompilerServices;
using System.Xml;
using System.Xml.Linq;
using System;
using CapybaraToolkit.Capyxliff;
using System.Collections.Generic;
using System.Linq;
using CapybaraToolkit.Filter.Run;
using CapybaraToolkit.Util;
using CapybaraToolkit.Xliff12;
using CapybaraToolkit.Sdlxliff;

[assembly: InternalsVisibleToAttribute("CapybaraToolkitTests")]

namespace CapybaraToolkit.Filter
{
    public class SdlxliffFilter
    {
        /// <summary>
        /// Converts sdlxliff into capyxliff.
        /// </summary>
        /// <param name="inputFile">sdlxliff file</param>
        /// <returns>CapyXliff object</returns>
        public CapyXliff Parse(string inputFile)
        {
            var root = XmlUtil.LoadInvalidXmlDocument(inputFile).Root;
            var capyXliff = new CapyXliff();
            foreach (var file in root.Elements(Xlf12.file))
            {
                var capyFile = ParseFile(file);
                capyXliff.Files.Add(capyFile);
            }
            return capyXliff;
        }

        private CapyFile ParseFile(XElement file)
        {
            var capyFile = new CapyFile
            {
                SourceLanguage = (string)file.Attribute("source-language"),
                TargetLanguage = (string)file.Attribute("target-language"),
                Original = (string)file.Attribute("original"),
                DataType = (string)file.Attribute("datatype"),
                Body = ParseBody(file.Element(Xlf12.body))
            };
            return capyFile;
        }

        private CapyBody ParseBody(XElement body)
        {
            var capyBody = new CapyBody();
            foreach (var (idx, transUnit) in body.Descendants(Xlf12.transUnit).Enumerate())
            {
                var capyGroup = ParseTransUnit(idx, transUnit);
                capyBody.Groups.Add(capyGroup);
            }
            return capyBody;
        }

        private CapyGroup ParseTransUnit(int groupId, XElement transUnit)
        {
            var capyGroup = new CapyGroup
            {
                Id = groupId.ToString(),
                OriginalId = (string)transUnit.Attribute("id"),
            };

            var segSource = transUnit.Element(Xlf12.segSource);
            var target = transUnit.Element(Xlf12.target);
            if (segSource == null || target == null)
            {
                return capyGroup;
            }

            var translate = ((string)transUnit.Attribute("translate")) == "yes";
            var segmentDefinitions = ParseSegmentDefinitions(transUnit.Element(Sdl.segDefs));

            var srcMrks = segSource.Elements(Xlf12.mrk).Where(x => ((string)x.Attribute("mtype")) == "seg");
            var tgtMrks = target.Elements(Xlf12.mrk).Where(x => ((string)x.Attribute("mtype")) == "seg").ToDictionary(selector => (string)selector.Attribute("mid"));
            foreach (var srcMrk in srcMrks)
            {
                var mid = (string)srcMrk.Attribute("mid");
                if (!tgtMrks.ContainsKey(mid))
                {
                    continue;
                }
                var tgtMrk = tgtMrks[mid];
                var segmentDefinition = segmentDefinitions[mid.Replace("_x0020_", " ")];
                var capyTransUnit = CreateTransUnit(srcMrk, tgtMrk);
                capyGroup.TransUnits.Add(capyTransUnit);
            }

            return capyGroup;
        }

        private class SegmentDefinition
        {
            public string Id { get; set; }
            public bool Locked { get; set; }
            public string Conf { get; set; }
            public string Origin { get; set; }
        }

        private Dictionary<string, SegmentDefinition> ParseSegmentDefinitions(XElement segDefsElem)
        {
            var segDefs = new Dictionary<string, SegmentDefinition>();
            foreach (var seg in segDefsElem.Elements(Sdl.seg))
            {
                var id = (string)seg.Attribute("id");
                segDefs.Add(id, new SegmentDefinition
                {
                    Id = id,
                    Locked = ((string)seg.Attribute("locked")) == "true",
                    Conf = (string)seg.Attribute("conf"),
                    Origin = (string)seg.Attribute("origin")
                });
            }
            return segDefs;
        }


        /// <summary>
        /// Creates a CapyTransUnit object from a pair of source and target mrk elements.
        /// </summary>
        /// <param name="srcMrk">Source mrk</param>
        /// <param name="tgtMrk">Target mrk</param>
        /// <returns>CapyTransUnit object</returns>
        internal CapyTransUnit CreateTransUnit(XElement srcMrk, XElement tgtMrk)
        {
            Segment sourceSegment;
            using (var reader = srcMrk.CreateReader())
            {
                reader.MoveToContent();
                sourceSegment = CreateSegment(reader);
            }
            Segment targetSegment;
            using (var reader = tgtMrk.CreateReader())
            {
                reader.MoveToContent();
                targetSegment = CreateSegment(reader);
            }

            var srcTags = sourceSegment.Runs.OfType<TagRun>().Where(x => x.TagType != TagType.Close).ToList();
            var tgtTags = targetSegment.Runs.OfType<TagRun>().Where(x => x.TagType != TagType.Close).ToList();
            if (srcTags.Count > 0 && tgtTags.Count > 0)
            {
                // Modify the ids of the tags in targetSegment.
                // Tags shared between source and target, (tags with the same content), should have the same ids.
                foreach (var tgtTag in tgtTags)
                {
                    var srcTag = srcTags.FirstOrDefault(x => x.Value == tgtTag.Value);
                    if (srcTag != null)
                    {
                        tgtTag.Id = srcTag.Id;
                        tgtTag.Shared = true;
                        if (tgtTag.PairedTag != null) // tgtTag has a paired close tag.
                        {
                            tgtTag.PairedTag.Id = srcTag.Id;
                        }
                    }
                }

                // Re-number unshared tags (tags that appear only in targetSegment)
                // The first id should be modified to the number after the last id of the tags in sourceSegment.
                var lastId = srcTags.Select(x => int.Parse(x.Id)).OrderByDescending(id => id).First();
                foreach (var tgtTag in tgtTags.Where(x => !x.Shared))
                {
                    lastId++;
                    tgtTag.Id = lastId.ToString();
                    if (tgtTag.PairedTag != null)
                    {
                        tgtTag.PairedTag.Id = tgtTag.Id;
                    }
                }
            }

            return new CapyTransUnit
            {
                Id = Guid.NewGuid().ToString(),
                OriginalId = (string)srcMrk.Attribute("mid"),
                Source = new CapySource
                {
                    Text = sourceSegment.ToString()
                },
                Target = new CapyTarget
                {
                    Text = targetSegment.ToString(),
                    State = Xliff12.State.None
                },
                SourceProps = new CapySourceProps
                {
                    Tags = sourceSegment.GetCapyTags()
                },
                TargetProps = new CapyTargetProps
                {
                    Tags = targetSegment.GetCapyTags()
                },
                Translate = true
            };
        }

        private Segment CreateSegment(XmlReader reader)
        {
            var depth = reader.Depth;
            var rootName = reader.Name;
            var segment = new Segment();
            var id = 0;
            var idStack = new Stack<int>();
            while (reader.Read())
            {
                if (reader.Depth == depth && reader.Name == rootName && reader.NodeType == XmlNodeType.EndElement)
                {
                    // Finish processing at the close tag of the root element.
                    break;
                }
                if (reader.IsEmptyElement)
                {
                    id++;
                    var tag = new TagRun(id.ToString(), reader.Name, TagType.Empty, GetAttributes(reader));
                    segment.Runs.Add(tag);
                }
                else if (reader.NodeType == XmlNodeType.Element)
                {
                    id++;
                    idStack.Push(id);
                    var tag = new TagRun(id.ToString(), reader.Name, TagType.Open, GetAttributes(reader));
                    segment.Runs.Add(tag);
                }
                else if (reader.NodeType == XmlNodeType.EndElement)
                {
                    id = idStack.Pop();
                    var tag = new TagRun(id.ToString(), reader.Name, TagType.Close, GetAttributes(reader));
                    segment.Runs.Add(tag);
                    tag.PairedTag = segment.GetPairedTag(tag);
                    tag.PairedTag.PairedTag = tag;
                }
                else if (reader.NodeType == XmlNodeType.Text ||
                reader.NodeType == XmlNodeType.Whitespace ||
                reader.NodeType == XmlNodeType.SignificantWhitespace)
                {
                    var text = new TextRun(reader.Value);
                    segment.Runs.Add(text);
                }
            }

            return segment;
        }

        private Dictionary<string, string> GetAttributes(XmlReader reader)
        {
            var attrs = new Dictionary<string, string>();
            while (reader.MoveToNextAttribute())
            {
                attrs.Add(reader.Name, reader.Value);
            }

            return attrs;
        }

    }
}
