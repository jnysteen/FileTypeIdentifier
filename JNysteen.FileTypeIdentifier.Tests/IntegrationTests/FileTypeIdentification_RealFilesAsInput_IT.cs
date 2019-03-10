using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using JNysteen.FileTypeIdentifier.Tests.IntegrationTests.Helpers;
using Xunit;

namespace JNysteen.FileTypeIdentifier.Tests.IntegrationTests
{
    public class FileTypeIdentification_RealFilesAsInput_IT
    {
        public static IEnumerable<object[]> TestFiles => IntegrationTestsHelper.GetTestFiles()
            .Select(t => new object[] {new Tuple<string, string>(t.Item1, t.Item2)});
        
        [Theory]
        [MemberData(nameof(TestFiles))]
        public void CorrectlyIdentifiesTestFiles_StreamAsInput(Tuple<string, string> filePathAndActualFileType)
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
        
        [Theory]
        [MemberData(nameof(TestFiles))]
        public void CorrectlyIdentifiesTestFiles_FilePathAsInput(Tuple<string, string> filePathAndActualFileType)
        {
            var fileMagicNumberMapping = IntegrationTestsHelper.GetAllMappings();
            var magicNumberMatcher = new MagicNumberMatcher(fileMagicNumberMapping);
            var fileTypeIdentifier = new FileTypeIdentifier(magicNumberMatcher);

            var (filePath, actualFileType) = filePathAndActualFileType;

            var identifiedFileType = fileTypeIdentifier.GetFileType(filePath);
            Assert.NotNull(identifiedFileType);

            var identifiedFileTypeLower = identifiedFileType.ToLower();
            var actualFileTypeLower = actualFileType.ToLower();
            Assert.Equal(actualFileTypeLower, identifiedFileTypeLower);
        }
        
        [Theory]
        [MemberData(nameof(TestFiles))]
        public void CorrectlyIdentifiesTestFiles_ByteEnumerableAsInput(Tuple<string, string> filePathAndActualFileType)
        {
            var fileMagicNumberMapping = IntegrationTestsHelper.GetAllMappings();
            var magicNumberMatcher = new MagicNumberMatcher(fileMagicNumberMapping);
            var fileTypeIdentifier = new FileTypeIdentifier(magicNumberMatcher);

            var (filePath, actualFileType) = filePathAndActualFileType;

            var fileBytesAsEnumerable = File.ReadAllBytes(filePath).AsEnumerable();

            var identifiedFileType = fileTypeIdentifier.GetFileType(fileBytesAsEnumerable);
            Assert.NotNull(identifiedFileType);

            var identifiedFileTypeLower = identifiedFileType.ToLower();
            var actualFileTypeLower = actualFileType.ToLower();
            Assert.Equal(actualFileTypeLower, identifiedFileTypeLower);
        }
    }
}