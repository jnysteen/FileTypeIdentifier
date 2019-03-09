using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using JNysteen.FileTypeIdentifier.FileSignatures;

namespace JNysteen.FileTypeIdentifier.Tests.IntegrationTests.Helpers
{
    public static class IntegrationTestsHelper
    {
        /// <summary>
        ///     Gets the path and extension of every test file in the test files directory.
        /// </summary>
        public static IEnumerable<(string, string)> GetTestFiles()
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

        public static FileSignatureMapping GetAllMappings()
        {
            var fileSignatureMapping = new FileSignatureMapping();

            foreach (var signature in ImageSignatures.BMPSignatures)
                fileSignatureMapping.AddFileSignature(signature, ImageSignatures.BMP);

            foreach (var signature in ImageSignatures.GIFSignatures)
                fileSignatureMapping.AddFileSignature(signature, ImageSignatures.GIF);

            foreach (var signature in ImageSignatures.JPEGSignatures)
                fileSignatureMapping.AddFileSignature(signature, ImageSignatures.JPEG);

            foreach (var signature in ImageSignatures.PNGSignatures)
                fileSignatureMapping.AddFileSignature(signature, ImageSignatures.PNG);

            foreach (var signature in DocumentSignatures.PDFSignatures)
                fileSignatureMapping.AddFileSignature(signature, DocumentSignatures.PDF);

            return fileSignatureMapping;
        }
    }
}