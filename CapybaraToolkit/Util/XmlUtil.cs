using System.Text;
using System.Xml.Linq;
using System;
using System.Linq;
using System.Xml;
using System.IO;

namespace CapybaraToolkit.Util
{
    public static class XmlUtil
    {
        public static string RemoveInvalidXmlCharacters(this string s)
        {
            try
            {
                // throws exception if string contains any invalid characters.
                return XmlConvert.VerifyXmlChars(s);
            }
            catch
            {
                // Remove invalid characters.
                var validXmlChars = s.Where(ch => XmlConvert.IsXmlChar(ch)).ToArray();
                return new string(validXmlChars);
            }
        }

        public static XDocument LoadInvalidXmlDocument(string xmlFile, Encoding encoding = null)
        {
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }
            var buf = new StringBuilder();

            using (var sr = new StreamReader(xmlFile, encoding))
            {
                while (sr.Peek() > -1)
                {
                    buf.Append(sr.ReadLine().RemoveInvalidXmlCharacters());
                }
            }

            return XDocument.Parse(buf.ToString(), LoadOptions.PreserveWhitespace);
        }

    }
}
