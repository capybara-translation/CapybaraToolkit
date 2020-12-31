using System;
using System.Xml.Linq;

namespace CapybaraToolkit.Sdlxliff
{
    public static class Sdl
    {
        public static XNamespace sdl = @"http://sdl.com/FileTypes/SdlXliff/1.0";
        public static XName docInfo = sdl + "doc-info";
        public static XName segDefs = sdl + "seg-defs";
        public static XName seg = sdl + "seg";
        public static XName conf = sdl + "conf";
        public static XName locked = sdl + "locked";
        public static XName cmtDefs = sdl + "cmt-defs";
        public static XName cmtDef = sdl + "cmt-def";
        public static XName Comment = sdl + "Comment";
        public static XName tagDefs = sdl + "tag-defs";
        public static XName tag = sdl + "tag";
        public static XName bpt = sdl + "bpt";
        public static XName ept = sdl + "ept";
        public static XName ph = sdl + "ph";
    }
}
