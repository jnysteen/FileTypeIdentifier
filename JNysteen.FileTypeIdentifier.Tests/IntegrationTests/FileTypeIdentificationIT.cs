using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using JNysteen.FileTypeIdentifier.Tests.IntegrationTests.Helpers;
using Xunit;

namespace JNysteen.FileTypeIdentifier.Tests.IntegrationTests
{
    public class FileTypeIdentificationIT
    {
        [Theory]
        [MemberData(nameof(TestFiles))]
        public void CorrectlyIdentifiesTestFiles(Tuple<string, string> filePathAndActualFileType)
        {
            var fileSignatureMapping = IntegrationTestsHelper.GetAllMappings();
            var fileSignatureMatcher = new FileSignatureMatcher(fileSignatureMapping);
            var fileTypeIdentifier = new FileTypeIdentifier(fileSignatureMatcher);

            var (filePath, actualFileType) = filePathAndActualFileType;

            using (var fileStream = File.OpenRead(filePath))
            {
                var identifiedFileType = fileTypeIdentifier.GetFileType(fileStream);
                Assert.NotNull(identifiedFileType);

                var identifiedFileTypeLower = identifiedFileType.ToLower();
                var actualFileTypeLower = actualFileType.ToLower();
                Assert.Equal(actualFileTypeLower, identifiedFileTypeLower);
            }
        }

        public static IEnumerable<object[]> TestFiles => IntegrationTestsHelper.GetTestFiles()
            .Select(t => new object[] {new Tuple<string, string>(t.Item1, t.Item2)});

        [Fact]
        public void CanGetFileType_FileContentsAsLongAsSignature_Positive()
        {
            var fileSignatureMapping = new FileSignatureMapping();
            var fileSignatureMatcher = new FileSignatureMatcher(fileSignatureMapping);
            var fileTypeIdentifier = new FileTypeIdentifier(fileSignatureMatcher);

            var fileSignature = new byte?[] {1, 2, 3, 4, 5};
            var fileType = "TEST";
            fileSignatureMapping.AddFileSignature(fileSignature, fileType);

            var testFileContents = new byte[] {1, 2, 3, 4, 5};

            var identifiedFileType = fileTypeIdentifier.GetFileType(testFileContents);
            Assert.NotNull(identifiedFileType);
            Assert.Equal(fileType, identifiedFileType);
        }

        [Fact]
        public void CanGetFileType_FileContentsLongerThanSignature_Positive()
        {
            var fileSignatureMapping = new FileSignatureMapping();
            var fileSignatureMatcher = new FileSignatureMatcher(fileSignatureMapping);
            var fileTypeIdentifier = new FileTypeIdentifier(fileSignatureMatcher);

            var fileSignature = new byte?[] {1, 2, 3};
            var fileType = "TEST";
            fileSignatureMapping.AddFileSignature(fileSignature, fileType);

            var testFileContents = new byte[] {1, 2, 3, 4, 5};

            var identifiedFileType = fileTypeIdentifier.GetFileType(testFileContents);
            Assert.NotNull(identifiedFileType);
            Assert.Equal(fileType, identifiedFileType);
        }

        [Fact]
        public void CanGetFileType_SignatureLongerThanInput_Positive()
        {
            var fileSignatureMapping = new FileSignatureMapping();
            var fileSignatureMatcher = new FileSignatureMatcher(fileSignatureMapping);
            var fileTypeIdentifier = new FileTypeIdentifier(fileSignatureMatcher);

            var fileSignature = new byte?[] {1, 2, 3, 4, 5};
            var fileType = "TEST";
            fileSignatureMapping.AddFileSignature(fileSignature, fileType);

            var testFileContents = new byte[] {1, 2, 3};

            var identifiedFileType = fileTypeIdentifier.GetFileType(testFileContents);
            Assert.Null(identifiedFileType);
        }

        [Fact]
        public void CanGetFileType_UnknownFileSignature_Positive()
        {
            var fileSignatureMapping = new FileSignatureMapping();
            var fileSignatureMatcher = new FileSignatureMatcher(fileSignatureMapping);
            var fileTypeIdentifier = new FileTypeIdentifier(fileSignatureMatcher);

            var fileSignature = new byte?[] {1, 2, 3};
            var fileType = "TEST";
            fileSignatureMapping.AddFileSignature(fileSignature, fileType);

            var testFileContents = new byte[] {2, 3, 4, 5};

            var identifiedFileType = fileTypeIdentifier.GetFileType(testFileContents);
            Assert.Null(identifiedFileType);
        }

        [Fact]
        public void InputHeaderIsNull_Negative()
        {
            var fileSignatureMapping = new FileSignatureMapping();
            var fileSignatureMatcher = new FileSignatureMatcher(fileSignatureMapping);
            var fileTypeIdentifier = new FileTypeIdentifier(fileSignatureMatcher);

            var fileSignature = new byte?[] {1, 2, 3};
            var fileType = "TEST";
            fileSignatureMapping.AddFileSignature(fileSignature, fileType);

            byte[] testFileContents = null;

            var identifiedFileType = fileTypeIdentifier.GetFileType(testFileContents);
            Assert.Null(identifiedFileType);
        }
    }
}