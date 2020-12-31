using System.Linq;
using System.Xml.Linq;
using System;
using CapybaraToolkit.Capyxliff;
using CapybaraToolkit.Xliff12;
using CapybaraToolkit.Util;
using CapybaraToolkit.Mxliff;

namespace CapybaraToolkit.Filter
{
    public class MxliffFilter
    {
        /// <summary>
        /// Converts mxliff into capyxliff.
        /// </summary>
        /// <param name="inputFile">mxliff file</param>
        /// <returns>CapyXliff object</returns>
        public CapyXliff Parse(string inputFile)
        {
            var root = XElement.Load(inputFile, LoadOptions.PreserveWhitespace);
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
            foreach (var (idx, group) in body.Descendants(Xlf12.group).Enumerate())
            {
                var capyGroup = ParseGroup(idx, group);
                capyBody.Groups.Add(capyGroup);
            }
            return capyBody;
        }

        private CapyGroup ParseGroup(int groupId, XElement group)
        {
            var capyGroup = new CapyGroup
            {
                Id = groupId.ToString(),
                OriginalId = (string)group.Attribute("id"),
                ContextGroup = ParseContextGroup(group.Element(Xlf12.contextGroup)),
                TransUnits = group.Elements(Xlf12.transUnit).Select(ParseTransUnit).ToList()
            };
            return capyGroup;
        }

        private CapyTransUnit ParseTransUnit(XElement transUnit)
        {
            var capyTransUnit = new CapyTransUnit
            {
                Id = Guid.NewGuid().ToString(),
                OriginalId = (string)transUnit.Attribute("id"),
                Translate = !(((string)transUnit.Attribute(Mxlf.m + "locked")) == "true"),
                Source = ParseSource(transUnit.Element(Xlf12.source)),
                Target = ParseTarget(transUnit.Element(Xlf12.target)),
                AltTranslations = transUnit.Elements(Xlf12.altTrans).Select(ParseAltTrans).ToList(),
                SourceProps = ParseTunitMetadata(transUnit.Element(Mxlf.tunitMetadata)),
                TargetProps = ParseTunitTargetMetadata(transUnit.Element(Mxlf.tunitTargetMetadata))
            };

            return capyTransUnit;
        }

        private CapySourceProps ParseTunitMetadata(XElement tunitMetadata)
        {
            if (tunitMetadata == null)
            {
                return new CapySourceProps();
            }
            return new CapySourceProps
            {
                Tags = tunitMetadata.Elements(Mxlf.mark).Select(ParseMark).ToList()
            };
        }

        private CapyTargetProps ParseTunitTargetMetadata(XElement tunitTargetMetadata)
        {
            if (tunitTargetMetadata == null)
            {
                return new CapyTargetProps();
            }
            return new CapyTargetProps
            {
                Tags = tunitTargetMetadata.Elements(Mxlf.mark).Select(ParseMark).ToList(),
            };
        }

        private CapyTag ParseMark(XElement mark)
        {
            return new CapyTag
            {
                Id = (string)mark.Attribute("id"),
                Content = ParseContent(mark.Element(Mxlf.content)),
            };
        }

        private CapyContent ParseContent(XElement content)
        {
            return new CapyContent
            {
                Value = content.Value
            };
        }

        private CapyAltTrans ParseAltTrans(XElement altTrans)
        {
            return new CapyAltTrans
            {
                Origin = (string)altTrans.Attribute("origin"),
                Target = ParseTarget(altTrans.Element(Xlf12.target))
            };
        }

        private CapySource ParseSource(XElement source)
        {
            return new CapySource
            {
                Text = source.Value
            };
        }

        private CapyTarget ParseTarget(XElement target)
        {
            int confirmeValue;
            if (!int.TryParse((string)target.Attribute(Mxlf.m + "confirmed"), out confirmeValue))
            {
                confirmeValue = -1;
            }
            var state = confirmeValue switch
            {
                -1 => State.None,
                0 => State.New,
                1 => State.Translated,
                int i when i >= 2 => State.Final,
                _ => throw new ArgumentOutOfRangeException(confirmeValue.ToString())
            };

            return new CapyTarget
            {
                Text = target.Value,
                State = state
            };
        }

        private CapyContextGroup ParseContextGroup(XElement contextGroup)
        {
            if (contextGroup == null)
            {
                return null;
            }
            var capyContextGroup = new CapyContextGroup();
            foreach (var context in contextGroup.Elements(Xlf12.context))
            {
                var capyContext = new CapyContext
                {
                    ContextType = (string)context.Attribute("context-type"),
                    Value = context.Value
                };
                capyContextGroup.Contexts.Add(capyContext);
            }

            return capyContextGroup;
        }


    }
}
