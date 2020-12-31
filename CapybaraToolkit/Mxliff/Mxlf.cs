using System;
using System.Xml.Linq;

namespace CapybaraToolkit.Mxliff
{
    public static class Mxlf
    {
        public static string TagPattern = @"(\{[biu_^]+?>|<[biu_^]+?\}|\{[0-9]{1,2}>|<[0-9]{1,2}\}|\{[0-9]{1,2}\}|\{j\})";
        public static XNamespace m = @"http://www.memsource.com/mxlf/2.0";
        public static XName tunitMetadata = m + "tunit-metadata";
        public static XName tunitTargetMetadata = m + "tunit-target-metadata";
        public static XName mark = m + "mark";
        public static XName type = m + "type";
        public static XName content = m + "content";
        public static XName comment = m + "comment";
    }
}
