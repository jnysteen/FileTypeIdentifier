using System;
using System.Collections;
using System.IO;
using System.Linq;
using FluentAssertions;
using JNysteen.FileTypeIdentifier.Tests.IntegrationTests.Helpers;
using NUnit.Framework;

namespace JNysteen.FileTypeIdentifier.Tests.IntegrationTests
{
    public class FileTypeIdentificationTests
    {
        public static IEnumerable TestFiles => IntegrationTestsHelper.GetTestFiles()
            .Select(t => new object[] {(t.filePath, t.fileExtension)});
        
        [Theory]
        public void InputHeaderIsNull_Negative()
        {
            var fileMagicNumberMapping = new MagicNumberMapping();
            var magicNumberMatcher = new MagicNumberMatcher(fileMagicNumberMapping);
            var fileTypeIdentifier = new FileTypeIdentifier(magicNumberMatcher);

            var magicNumber = new byte?[] {1, 2, 3};
            var fileType = "TEST";
            fileMagicNumberMapping.AddMagicNumber(magicNumber, fileType);

            byte[] testFileContents = null;

            var identifiedFileType = fileTypeIdentifier.GetFileType(testFileContents);
            identifiedFileType.Should().BeNull("no file type should have been identified");
        }
        
        [Theory]
        [TestCaseSource(nameof(TestFiles))]
        public void CorrectlyIdentifiesTestFiles_StreamAsInput((string filePath, string actualFileType) filePathAndActualFileType)
        {
            // Arrange
            var fileTypeIdentifier = CreateFileTypeIdentifier();
            var (filePath, actualFileType) = filePathAndActualFileType;

            string identifiedFileType;
            using (var fileStream = File.OpenRead(filePath))
            {
                // Act
                identifiedFileType = fileTypeIdentifier.GetFileType(fileStream);
            }
            
            // Assert
            AssertOnIdentifiedFileType(identifiedFileType, actualFileType);
        }

        [Theory]
        [TestCaseSource(nameof(TestFiles))]
        public void CorrectlyIdentifiesTestFiles_ByteEnumerableAsInput((string filePath, string actualFileType) filePathAndActualFileType)
        {
            // Arrange
            var fileTypeIdentifier = CreateFileTypeIdentifier();
            var (filePath, actualFileType) = filePathAndActualFileType;
            var fileBytesAsEnumerable = File.ReadAllBytes(filePath).AsEnumerable();

            // Act
            var identifiedFileType = fileTypeIdentifier.GetFileType(fileBytesAsEnumerable);

            // Assert
            AssertOnIdentifiedFileType(identifiedFileType, actualFileType);
        }
        
        private static FileTypeIdentifier CreateFileTypeIdentifier()
        {
            var fileMagicNumberMapping = IntegrationTestsHelper.GetAllMappings();
            var magicNumberMatcher = new MagicNumberMatcher(fileMagicNumberMapping);
            var fileTypeIdentifier = new FileTypeIdentifier(magicNumberMatcher);
            return fileTypeIdentifier;
        }
        
        private static void AssertOnIdentifiedFileType(string identifiedFileType, string actualFileType)
        {
            identifiedFileType.Should().NotBeNull("the file type should have been identified");
            var identifiedFileTypeLower = identifiedFileType.ToLower();
            var actualFileTypeLower = actualFileType.ToLower();
            identifiedFileTypeLower.Should().Be(actualFileTypeLower);
        }
    }
}