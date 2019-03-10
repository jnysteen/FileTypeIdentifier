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
            var fileMagicNumberMapping = IntegrationTestsHelper.GetAllMappings();
            var magicNumberMatcher = new MagicNumberMatcher(fileMagicNumberMapping);
            var fileTypeIdentifier = new FileTypeIdentifier(magicNumberMatcher);

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
        public void CanGetFileType_FileContentsAsLongAsMagicNumber_Positive()
        {
            var fileMagicNumberMapping = new MagicNumberMapping();
            var magicNumberMatcher = new MagicNumberMatcher(fileMagicNumberMapping);
            var fileTypeIdentifier = new FileTypeIdentifier(magicNumberMatcher);

            var magicNumber = new byte?[] {1, 2, 3, 4, 5};
            var fileType = "TEST";
            fileMagicNumberMapping.AddMagicNumber(magicNumber, fileType);

            var testFileContents = new byte[] {1, 2, 3, 4, 5};

            var identifiedFileType = fileTypeIdentifier.GetFileType(testFileContents);
            Assert.NotNull(identifiedFileType);
            Assert.Equal(fileType, identifiedFileType);
        }

        [Fact]
        public void CanGetFileType_FileContentsLongerThanMagicNumber_Positive()
        {
            var fileMagicNumberMapping = new MagicNumberMapping();
            var magicNumberMatcher = new MagicNumberMatcher(fileMagicNumberMapping);
            var fileTypeIdentifier = new FileTypeIdentifier(magicNumberMatcher);

            var magicNumber = new byte?[] {1, 2, 3};
            var fileType = "TEST";
            fileMagicNumberMapping.AddMagicNumber(magicNumber, fileType);

            var testFileContents = new byte[] {1, 2, 3, 4, 5};

            var identifiedFileType = fileTypeIdentifier.GetFileType(testFileContents);
            Assert.NotNull(identifiedFileType);
            Assert.Equal(fileType, identifiedFileType);
        }

        [Fact]
        public void CanGetFileType_MagicNumberLongerThanInput_Positive()
        {
            var fileMagicNumberMapping = new MagicNumberMapping();
            var magicNumberMatcher = new MagicNumberMatcher(fileMagicNumberMapping);
            var fileTypeIdentifier = new FileTypeIdentifier(magicNumberMatcher);

            var magicNumber = new byte?[] {1, 2, 3, 4, 5};
            var fileType = "TEST";
            fileMagicNumberMapping.AddMagicNumber(magicNumber, fileType);

            var testFileContents = new byte[] {1, 2, 3};

            var identifiedFileType = fileTypeIdentifier.GetFileType(testFileContents);
            Assert.Null(identifiedFileType);
        }

        [Fact]
        public void CanGetFileType_UnknownMagicNumber_Positive()
        {
            var fileMagicNumberMapping = new MagicNumberMapping();
            var magicNumberMatcher = new MagicNumberMatcher(fileMagicNumberMapping);
            var fileTypeIdentifier = new FileTypeIdentifier(magicNumberMatcher);

            var magicNumber = new byte?[] {1, 2, 3};
            var fileType = "TEST";
            fileMagicNumberMapping.AddMagicNumber(magicNumber, fileType);

            var testFileContents = new byte[] {2, 3, 4, 5};

            var identifiedFileType = fileTypeIdentifier.GetFileType(testFileContents);
            Assert.Null(identifiedFileType);
        }

        [Fact]
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
            Assert.Null(identifiedFileType);
        }
    }
}