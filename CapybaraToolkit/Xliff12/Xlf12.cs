using System;
using System.Xml.Linq;

namespace CapybaraToolkit.Xliff12
{
    public static class Xlf12
    {
        public static XNamespace xlf = @"urn:oasis:names:tc:xliff:document:1.2";
        public static XName xliff = xlf + "xliff";
        public static XName file = xlf + "file";
        public static XName header = xlf + "header";
        public static XName body = xlf + "body";
        public static XName group = xlf + "group";
        public static XName transUnit = xlf + "trans-unit";
        public static XName source = xlf + "source";
        public static XName target = xlf + "target";
        public static XName mrk = xlf + "mrk";
        public static XName segSource = xlf + "seg-source";
        public static XName altTrans = xlf + "alt-trans";
        public static XName note = xlf + "note";
        public static XName contextGroup = xlf + "context-group";
        public static XName context = xlf + "context";
    }
}
