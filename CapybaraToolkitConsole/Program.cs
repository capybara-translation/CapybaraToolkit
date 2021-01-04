using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System;
using System.Linq;
using CapybaraToolkit.Filter;

namespace CapybaraToolkitConsole
{
    class Program
    {
        private static bool ValidateArguments(string inFile, string outFile)
        {
            if (inFile == null || outFile == null)
            {
                Console.WriteLine("Invalid arguments");
                return false;
            }
            string[] validExts = { ".mxliff", ".sdlxliff" };
            var ext = Path.GetExtension(inFile);
            if (!validExts.Contains(ext))
            {
                Console.WriteLine($"Invalid extension: {ext}");
                return false;
            }
            if (!File.Exists(inFile))
            {
                Console.WriteLine($"File not found: {inFile}");
                return false;
            }
            return true;
        }

        static void Main(string[] args)
        {
            string inFile = null;
            string outFile = null;

            if (args.Length < 1)
            {
                Console.WriteLine("Invalid number of arguments");
                return;
            }
            if (args.Length == 1)
            {
                outFile = inFile = args[0];
            }
            if (args.Length > 1)
            {
                inFile = args[0];
                outFile = args[1];
            }
            if (!ValidateArguments(inFile, outFile))
            {
                return;
            }

            var ext = Path.GetExtension(inFile);
            if (ext == ".mxliff")
            {
                var filter = new MxliffFilter();
                var xliff = filter.Parse(inFile);
                xliff.Save(outFile);
            }
            else if (ext == ".sdlxliff")
            {
                var filter = new SdlxliffFilter();
                var xliff = filter.Parse(inFile);
                xliff.Save(outFile);
            }
        }
    }
}
