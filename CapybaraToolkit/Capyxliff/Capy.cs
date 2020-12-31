using System;
using System.Xml.Linq;

namespace CapybaraToolkit.Capyxliff
{
    public static class Capy
    {
        public static XNamespace capy = @"http://capybaratranslation.com/capyxliff/1.0";
        public static XName content = capy + "content";
        public static XName tag = capy + "tag";
        public static XName sourceProps = capy + "source-props";
        public static XName targetProps = capy + "target-props";

    }
}
