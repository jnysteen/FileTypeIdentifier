using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using JNysteen.FileTypeIdentifier.MagicNumbers;

namespace JNysteen.FileTypeIdentifier.Tests.IntegrationTests.Helpers
{
    public static class IntegrationTestsHelper
    {
        /// <summary>
        ///     Gets the path and extension of every test file in the test files directory.
        /// </summary>
        public static IEnumerable<(string filePath, string fileExtension)> GetTestFiles()
        {
            var testFilesDirectory = GetTestFilesDirectory();

            foreach (var testFile in Directory.GetFiles(testFilesDirectory))
            {
                var fileExtension =
                    Path.GetExtension(testFile).Substring(1); // Substring -> skip the dot in the extension

                yield return (testFile, fileExtension);
            }
        }

        private static string GetTestDirectory()
        {
            var codeBaseUrl = new Uri(Assembly.GetExecutingAssembly().CodeBase);
            var codeBasePath = Uri.UnescapeDataString(codeBaseUrl.AbsolutePath);
            var dirPath = Path.GetDirectoryName(codeBasePath);
            return dirPath;
        }

        private static string GetTestFilesDirectory()
        {
            return Path.Combine(GetTestDirectory(), "IntegrationTests/TestFiles");
        }

        public static MagicNumberMapping GetAllMappings()
        {
            var magicNumberMapping = new MagicNumberMapping();

            foreach (var magicNumber in ImageMagicNumbers.BMPMagicNumbers)
                magicNumberMapping.AddMagicNumber(magicNumber, ImageMagicNumbers.BMP);

            foreach (var magicNumber in ImageMagicNumbers.GIFMagicNumbers)
                magicNumberMapping.AddMagicNumber(magicNumber, ImageMagicNumbers.GIF);

            foreach (var magicNumber in ImageMagicNumbers.JPEGMagicNumbers)
                magicNumberMapping.AddMagicNumber(magicNumber, ImageMagicNumbers.JPEG);

            foreach (var magicNumber in ImageMagicNumbers.PNGMagicNumbers)
                magicNumberMapping.AddMagicNumber(magicNumber, ImageMagicNumbers.PNG);

            foreach (var magicNumber in DocumentMagicNumbers.PDFMagicNumbers)
                magicNumberMapping.AddMagicNumber(magicNumber, DocumentMagicNumbers.PDF);

            return magicNumberMapping;
        }
    }
}