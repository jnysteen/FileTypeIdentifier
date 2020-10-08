using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using JNysteen.FileTypeIdentifier.MagicNumbers;

namespace JNysteen.FileTypeIdentifier.Tests.IntegrationTests.Helpers
{
    internal static class IntegrationTestsHelper
    {
        /// <summary>
        ///     Gets the path and extension of every test file in the test files directory.
        /// </summary>
        public static IEnumerable<(string filePath, string fileExtension)> GetTestFiles()
        {
            var testFilesDirectory = GetTestFilesDirectory();

            foreach (var testFile in Directory.GetFiles(testFilesDirectory))
                yield return (testFile, Path.GetExtension(testFile));
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

        public static List<FileMagicNumberDefinition> GetAllMappings()
        {
            var magicNumberMapping = new List<FileMagicNumberDefinition>
            {
                ImageMagicNumbers.BMP,
                ImageMagicNumbers.GIF,
                ImageMagicNumbers.JPEG,
                ImageMagicNumbers.PNG,
                DocumentMagicNumbers.PDF
            };

            return magicNumberMapping;
        }
    }
}