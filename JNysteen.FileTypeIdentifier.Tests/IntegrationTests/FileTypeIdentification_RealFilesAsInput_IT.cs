using System;
using System.Collections;
using System.IO;
using System.Linq;
using FluentAssertions;
using JNysteen.FileTypeIdentifier.Tests.IntegrationTests.Helpers;
using NUnit.Framework;

namespace JNysteen.FileTypeIdentifier.Tests.IntegrationTests
{
    public class FileTypeIdentification_RealFilesAsInput_IT
    {
        public static IEnumerable TestFiles => IntegrationTestsHelper.GetTestFiles()
            .Select(t => new object[] {(t.filePath, t.fileExtension)});
        
        [Theory]
        [TestCaseSource(nameof(TestFiles))]
        public void CorrectlyIdentifiesTestFiles_StreamAsInput((string filePath, string actualFileType) filePathAndActualFileType)
        {
            // Arrange
            var fileMagicNumberMapping = IntegrationTestsHelper.GetAllMappings();
            var magicNumberMatcher = new MagicNumberMatcher(fileMagicNumberMapping);
            var fileTypeIdentifier = new FileTypeIdentifier(magicNumberMatcher);

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
        public void CorrectlyIdentifiesTestFiles_FilePathAsInput((string filePath, string actualFileType) filePathAndActualFileType)
        {
            // Arrange
            var fileMagicNumberMapping = IntegrationTestsHelper.GetAllMappings();
            var magicNumberMatcher = new MagicNumberMatcher(fileMagicNumberMapping);
            var fileTypeIdentifier = new FileTypeIdentifier(magicNumberMatcher);

            var (filePath, actualFileType) = filePathAndActualFileType;

            // Act
            var identifiedFileType = fileTypeIdentifier.GetFileType(filePath);
            
            // Assert
            AssertOnIdentifiedFileType(identifiedFileType, actualFileType);
        }
        
        [Theory]
        [TestCaseSource(nameof(TestFiles))]
        public void CorrectlyIdentifiesTestFiles_ByteEnumerableAsInput((string filePath, string actualFileType) filePathAndActualFileType)
        {
            // Arrange
            var fileMagicNumberMapping = IntegrationTestsHelper.GetAllMappings();
            var magicNumberMatcher = new MagicNumberMatcher(fileMagicNumberMapping);
            var fileTypeIdentifier = new FileTypeIdentifier(magicNumberMatcher);

            var (filePath, actualFileType) = filePathAndActualFileType;

            var fileBytesAsEnumerable = File.ReadAllBytes(filePath).AsEnumerable();

            // Act
            var identifiedFileType = fileTypeIdentifier.GetFileType(fileBytesAsEnumerable);

            // Assert
            AssertOnIdentifiedFileType(identifiedFileType, actualFileType);
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